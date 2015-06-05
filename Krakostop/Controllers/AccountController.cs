using Krakostop.Mails;
using Krakostop.Models;
using Krakostop.Models.dbModels;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Krakostop.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private KrakostopDbContext db = new KrakostopDbContext();

        private KrakostopUserManager _userManager;

        public AccountController()
        {
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Manager()
        { 
            ViewBag.PairsCount = db.Pairs.Count();
            ViewBag.PairsPayments = db.Pairs.Where(p => p.Payments).ToList().Count;
            ViewBag.AutocarCount = db.People.Where(p => p.Autocar).ToList().Count;
            ViewBag.AutocarPayments = db.People.Where(p => p.Autocar && p.AutocarPayments).ToList().Count;
            ViewBag.XS = db.People.Where(p => p.Size == ShirtSize.XS).ToList().Count;
            ViewBag.S = db.People.Where(p => p.Size == ShirtSize.S).ToList().Count;
            ViewBag.M = db.People.Where(p => p.Size == ShirtSize.M).ToList().Count;
            ViewBag.L = db.People.Where(p => p.Size == ShirtSize.L).ToList().Count;
            ViewBag.XL = db.People.Where(p => p.Size == ShirtSize.XL).ToList().Count;
            ViewBag.XXL = db.People.Where(p => p.Size == ShirtSize.XXL).ToList().Count;
            ViewBag.JoinersCount = db.Pair_Joiners.Count();
            ViewBag.JoinersAct = db.Pair_Joiners.Where(p => p.IsActual).ToList().Count;
            ViewBag.Average = db.Pair_Joiners.Average(p => p.Age);
            return View();
        }

        public AccountController(KrakostopUserManager userManager)
        {
            UserManager = userManager;
        }

        public KrakostopUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<KrakostopUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public new HttpContextBase HttpContext
        {
            get
            {
                HttpContextWrapper context =
                    new HttpContextWrapper(System.Web.HttpContext.Current);
                return (HttpContextBase)context;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase[] files)
        {
            try
            {
                foreach (HttpPostedFileBase file in files)
                {
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    string filename = User.Identity.GetUserName() + extension.ToLower();
                    file.SaveAs(Server.MapPath("~/Images/" + filename));
                    string filepathtosave = "Images/" + filename;
                }

                ViewBag.Message = "Upload poprawny";
            }
            catch
            {
                ViewBag.Message = "Error.";
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, 
            string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameOrEmailAsync(model.Email,
                    model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", 
                        "Błędna nazwa użytkownika lub hasło");
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.CountLimit = db.Pairs.Count();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pair = new Pair();
                pair.Persons.Add(model.Person1);
                pair.Persons.Add(model.Person2);
                string userName = Mailing.UserNameFactory(
                    model.Person1, model.Person2);
                int count = db.Users.Count(u => u.UserName == userName);
                if (count > 0)
                {
                    userName += count.ToString();
                }
                var user = new KrakostopUser() 
                {
                    UserName = userName,
                    Email = model.Person1.Email,
                };
                var notif2 = new Notifications()
                {
                    Desc = "Potwierdzono i zweryfikowano!",
                    NotifType = NotificationType.Confirm,
                };
                user.Notifs.Add(notif2);
                var notif = new Notifications()
                {
                    Desc = "Dziękujemy za rejestrację!",
                    NotifType = NotificationType.Register,
                };
                user.Notifs.Add(notif);
                pair.User = user;
                db.Pairs.Add(pair);
                return await UserRegister(user, model.Password, "Pair");
            }

            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> UserRegister(
            KrakostopUser userKrako, string pw, string role)
        {
            if (userKrako == null)
            {
                userKrako = (KrakostopUser)TempData["Us"];
            }
            IdentityResult result = 
                await UserManager.CreateAsync(userKrako, pw);
            var roleResult = UserManager.AddToRoleAsync(userKrako.Id, role);
            if (result != null && result.Succeeded && 
                roleResult != null && roleResult.Result.Succeeded)
            {
                string body = role == "Pair" 
                    ? Text.RegisterBody(userKrako.UserName, userKrako.Pair.ID.ToString(), userKrako)
                    : Text.RegisterJoiner(userKrako.UserName);
                await SignInAsync(userKrako, isPersistent: false);
                Mailing.SendEmail(userKrako.Email, 
                    Text.RegisterTitle, body);
                
                return RedirectToAction("Index", "Home");
            }
            else
            {
                AddErrors(result);
            }
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null) 
            {
                return View("Error");
            }

            IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            else
            {
                AddErrors(result);
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
                    return View();
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
	
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if (code == null) 
            {
                return View("Error");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "No user found.");
                    return View();
                }
                IdentityResult result = 
                    await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Hasło zostało zmienione."
                : message == ManageMessageId.SetPasswordSuccess ? "Hasło ustawione poprawnie."
                : message == ManageMessageId.RemoveLoginSuccess ? "Login usunięty."
                : message == ManageMessageId.Error ? "Wystąpił błąd."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (User.IsInRole("Admin") || User.IsInRole("Pair"))
            {
                ViewBag.NamesToDisplay = Mailing.NamesToDisplay(
                    UserManager.FindByName(User.Identity.GetUserName()).Pair.Persons.ToList(),
                    User.Identity.GetUserName(), true);   
            }
            else
            { 
                ViewBag.NamesToDisplay = User.Identity.GetUserName();
            }
                 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManagePasswordUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = 
                        await UserManager.ChangePasswordAsync(
                            User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToAction(
                            "Manage", 
                            new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result =
                        await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(KrakostopUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(
                new AuthenticationProperties()
                {
                    IsPersistent = isPersistent
                }, 
                await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
    }

    public static class Extensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null, string id = null)
        {
            string cssClass = "active";
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];
            var ids = (string)html.ViewContext.RouteData.Values["id"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            if (String.IsNullOrEmpty(id))
                id = ids;

            return controller == currentController && action == currentAction
                && id == ids ?
                cssClass : String.Empty;
        }
    }
}