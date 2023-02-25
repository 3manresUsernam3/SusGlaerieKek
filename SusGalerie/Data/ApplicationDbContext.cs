using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using SusGalerie.Models;

namespace SusGalerie.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<string>, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ImageCollection> Galleries { get; set; }
        public DbSet<StoredFile> Files { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Thumbnail>().HasKey(t => new { t.FileId, t.Type });

            this.SeedUsers(builder);
            this.SeedRoles(builder);
            this.SeedUserRoles(builder, UserId, RoleId);
        }
        private void SeedUsers(ModelBuilder builder)
        {
            User user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = "Susmogus@BroWhat.cz",
                NormalizedEmail = "SUSMOGUS@BROWHAT.CZ",
                EmailConfirmed = true,
                UserName = "Susmogus@BroWhat.cz",
                NormalizedUserName = "SUSMOGUS@BROWHAT.CZ",
                PhoneNumber = "911",
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            UserId = user.Id;
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, "Susmogus!");

            builder.Entity<User>().HasData(user);
        }
        private void SeedRoles(ModelBuilder builder)
        {
            IdentityRole role = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin",
                ConcurrencyStamp = "1",
                NormalizedName = "Admin"
            };
            RoleId = role.Id;

            builder.Entity<IdentityRole>().HasData(
                role,
                new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = "Member", ConcurrencyStamp = "2", NormalizedName = "Member" }
                );
        }

        private void SeedUserRoles(ModelBuilder builder, string userid, string roleid)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = roleid, UserId = userid }
                );
        }
    }
}