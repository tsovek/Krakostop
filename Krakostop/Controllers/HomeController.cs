using Krakostop.Mails;
using Krakostop.Models;
using Rotativa;
using System.Linq;
using System.Web.Mvc;

namespace Krakostop.Controllers
{
    public class HomeController : Controller
    {
        private KrakostopDbContext db = new KrakostopDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rules()
        { 
            return View();
        }

        public ActionResult HowToKrako()
        {
            return View();
        }

        public ActionResult HowToAuto()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Mail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MailToMany(MailToMany model)
        {
            if (ModelState.IsValid)
            {
                var list = model.GetEmails(model.KindOfEmail);
                foreach (var email in list)
                {
                    Mailing.SendEmailWithoutHtml(
                        email, model.Title, model.Body);
                }
                return RedirectToAction("MailSend");
            }
            return View(model);
        }

        public ActionResult MailSend()
        {
            return View();
        }

        public ActionResult PDF()
        {
            return View(db.Pairs.ToList());
        }

        public ActionResult Print()
        {
            return new ActionAsPdf("PDF") { FileName = "Test.pdf" };
        }
    }
}