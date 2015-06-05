using Krakostop.Models;
using Krakostop.Models.dbModels;

using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Krakostop.Controllers
{
    [Authorize]
    public class PairJoinerController : Controller
    {
        private KrakostopDbContext db = new KrakostopDbContext();

        public bool HasClaims(int? id)
        {
            return (id.HasValue && User.Identity.Name ==
                db.Pair_Joiners.Find(id.Value).User.UserName) ||
                User.IsInRole("Admin");
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return View(await db.Pair_Joiners.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pair_Joiner pair_Joiner = await db.Pair_Joiners.FindAsync(id);
            if (pair_Joiner == null)
            {
                return HttpNotFound();
            }
            return View(pair_Joiner);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Mail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pair_Joiner pair_Joiner = await db.Pair_Joiners.FindAsync(id);
            ViewBag.Name = pair_Joiner.Name;
            ViewBag.Email = pair_Joiner.User.Email;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Mail(Mail model)
        {
            if (ModelState.IsValid)
            { 
                var b = Mails.Text.GenerateJoinerEmail(model.Name,
                    model.From, model.Body);
                Mails.Mailing.SendEmail(
                    model.From, model.To, model.Title, 
                    b, false, model.Name);
                return RedirectToAction("MailSend");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult MailSend()
        { 
            return View();
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateJoinerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Joiner.IsActual = true;
                var user = new KrakostopUser()
                {
                    Email = viewModel.Email,
                    UserName = viewModel.Email,  
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
                viewModel.Joiner.User = user;
                db.Pair_Joiners.Add(viewModel.Joiner);
                TempData["Us"] = user;
                return RedirectToAction("UserRegister", "Account",
                    new { userKrako = user,
                        pw = viewModel.Password, 
                    role = "Joiner" });
            }

            return View(viewModel);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null || !HasClaims(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pair_Joiner pair_Joiner = await db.Pair_Joiners.FindAsync(id);
            if (pair_Joiner == null)
            {
                return HttpNotFound();
            }
            return View(pair_Joiner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Pair_Joiner joiner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(joiner).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(joiner);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pair_Joiner pair_Joiner = await db.Pair_Joiners.FindAsync(id);
            if (pair_Joiner == null)
            {
                return HttpNotFound();
            }
            return View(pair_Joiner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pair_Joiner pair_Joiner = await db.Pair_Joiners.FindAsync(id);
            db.Pair_Joiners.Remove(pair_Joiner);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
