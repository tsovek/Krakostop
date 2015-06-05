using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Krakostop.Models;

namespace Krakostop
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class KrakostopUserManager : UserManager<KrakostopUser>
    {
        public KrakostopUserManager(IUserStore<KrakostopUser> store)
            : base(store)
        {
        }

        public async Task<KrakostopUser> FindByNameOrEmailAsync
            (string usernameOrEmail, string password)
        {
            var username = usernameOrEmail;
            if (usernameOrEmail.Contains("@"))
            {
                var userForEmail = await base.FindByEmailAsync(usernameOrEmail);
                if (userForEmail != null)
                {
                    username = userForEmail.UserName;
                }
                else
                { 
                    using (KrakostopDbContext db = new KrakostopDbContext())
                    { 
                        var q = db.People.FirstOrDefault(p => p.Email == usernameOrEmail);
                        if (q != null)
                        { 
                            username = q.Pair.User.UserName;
                        }
                    }
                }
            }
            return await base.FindAsync(username, password);
        }

        public static KrakostopUserManager Create(IdentityFactoryOptions<KrakostopUserManager> options, IOwinContext context) 
        {
            var manager = new KrakostopUserManager(new UserStore<KrakostopUser>(context.Get<KrakostopDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<KrakostopUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true,
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = false,
            };
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<KrakostopUser>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<KrakostopUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is: {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<KrakostopUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
