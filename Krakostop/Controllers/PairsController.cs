using Krakostop.Mails;
using Krakostop.Models;
using Krakostop.Models.dbModels;

using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Krakostop.Controllers
{
    [Authorize]
    public class PairsController : Controller
    {
        private KrakostopDbContext db = new KrakostopDbContext();

        public bool HasClaims(int? id)
        {
            return (id.HasValue && User.Identity.Name == 
                db.Pairs.Find(id.Value).User.UserName) || 
                User.IsInRole("Admin"); 
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Pairs.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pair pair = await db.Pairs.FindAsync(id);
            if (pair == null)
            {
                return HttpNotFound();
            }
            return View(pair);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pair pair = await db.Pairs.FindAsync(id);
            db.Pairs.Remove(pair);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Payments(int? id, bool isConfirmed)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pair pair = await db.Pairs.FindAsync(id);
            if (pair == null)
            {
                return HttpNotFound();
            }
            return View(pair);
        }

        [HttpPost, ActionName("Payments")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PaymentsConfirmed(int id, bool isConfirmed)
        {
            if (isConfirmed)
            {
                Pair pair = await db.Pairs.FindAsync(id);
                pair.Payments = isConfirmed;
                var notif = new Notifications()
                {
                    Desc = "Otrzymaliśmy Waszą wpłatę, dzięki!",
                    NotifType = NotificationType.PaymentPair,
                };
                pair.User.Notifs.Add(notif);
                Mailing.SendEmail(pair.User.Email, Text.RegisterTitle,
                    Text.PaymentPairBody(pair.User.UserName));
                await db.SaveChangesAsync();
            }
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
