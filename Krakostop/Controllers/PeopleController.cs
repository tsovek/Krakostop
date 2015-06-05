using Krakostop.Models;
using Krakostop.Models.dbModels;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Krakostop.Controllers
{
    [Authorize]
    public class PeopleController : Controller
    {
        private KrakostopDbContext db = new KrakostopDbContext();

        public bool HasClaims(int? id)
        {
            return (id.HasValue && User.Identity.Name ==
                db.People.Find(id.Value).Pair.User.UserName) ||
                User.IsInRole("Admin");
        }

        [Authorize(Roles="Admin")]
        public async Task<ActionResult> Index()
        {
            var people = db.People.Include(p => p.Pair);
            return PartialView(await people.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null || !HasClaims(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = await db.People.FindAsync(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.PairID = new SelectList(db.Pairs, "ID", "ID", person.PairID);
            ViewBag.ID = new SelectList(db.Pair_Joiners, "PersonID", "Description", person.ID);
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Account");
            }
            ViewBag.PairID = new SelectList(db.Pairs, "ID", "ID", person.PairID);
            ViewBag.ID = new SelectList(db.Pair_Joiners, "PersonID", "Description", person.ID);
            return View(person);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = await db.People.FindAsync(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Person person = await db.People.FindAsync(id);
            db.People.Remove(person);
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
            Person person = await db.People.FindAsync(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        [HttpPost, ActionName("Payments")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PaymentsConfirmed(int id, bool isConfirmed)
        {
            Person person = await db.People.FindAsync(id);
            if (person.Autocar)
            { 
                person.AutocarPayments = isConfirmed;
                if (isConfirmed)
                {
                    var notif = new Notifications()
                    {
                        Desc = "Otrzymaliśmy Wpłatę za autokar, dzięki!",
                        NotifType = NotificationType.PaymentPair,
                        User = "(" + person.Name + " " + person.Surname + ")"
                    };
                    person.Pair.User.Notifs.Add(notif);
                }
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
