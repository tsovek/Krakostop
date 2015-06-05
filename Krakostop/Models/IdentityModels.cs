using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using Krakostop.Models.dbModels;

namespace Krakostop.Models
{
    public class KrakostopUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<KrakostopUser> manager)
        {
            var userIdentity = 
                await manager.CreateIdentityAsync(this, 
                    DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
        private List<Notifications> notifs = new List<Notifications>();
        public virtual Pair Pair { get; set; }
        public virtual Pair_Joiner Joiner { get; set; }
        public virtual ICollection<Notifications> Notifs 
        {
            get { return notifs; }
            set { notifs = new List<Notifications>(value); }
        }
    }

    public class KrakostopDbContext : IdentityDbContext<KrakostopUser>
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Pair> Pairs { get; set; }
        public DbSet<Pair_Joiner> Pair_Joiners { get; set; }
        public DbSet<Content> Contents { get; set; }
        
        public KrakostopDbContext()
            : base("Website", throwIfV1Schema: false)
        {
        }

        public static KrakostopDbContext Create()
        {
            return new KrakostopDbContext();
        }

        protected override void OnModelCreating(
            System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<KrakostopUser>()
                        .ToTable("Users")
                        .Property(p => p.Id)
                        .HasColumnName("UserId");
            modelBuilder.Entity<IdentityUserRole>()
                        .ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>()
                        .ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>()
                        .ToTable("UserClaims")
                        .Property(pc => pc.Id)
                        .HasColumnName("UserClaimsId");
            modelBuilder.Entity<IdentityRole>()
                        .ToTable("Roles")
                        .Property(pc => pc.Id)
                        .HasColumnName("RoleId");
            // One-To-One: Pairs :: Users
            modelBuilder.Entity<Pair>()
                        .HasKey(e => e.ID);
            modelBuilder.Entity<KrakostopUser>()
                        .HasOptional(u => u.Pair)
                        .WithRequired(person => person.User);
            // One-To-Many: People :: Pairs
            modelBuilder.Entity<Person>()
                        .HasRequired<Pair>(pers => pers.Pair)
                        .WithMany(pair => pair.Persons)
                        .HasForeignKey(pers => pers.PairID);
            // One-To-One: Pair_Joiners :: User 
            modelBuilder.Entity<Pair_Joiner>()
                        .HasKey(e => e.ID);
            modelBuilder.Entity<KrakostopUser>()
                        .HasOptional(p => p.Joiner)
                        .WithRequired(joiner => joiner.User);
        }
    }
}