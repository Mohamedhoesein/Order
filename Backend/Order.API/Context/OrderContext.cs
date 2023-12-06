using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Order.API.Context
{
    /// <summary>
    /// The context used for database access.
    /// </summary>
    public class OrderContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        /// <summary>
        /// Initialize a new <see cref="OrderContext"/> with just the options.
        /// </summary>
        /// <param name="options">
        /// The options sued by <see cref="DbContext"/>.
        /// </param>
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) {}

        /// <summary>
        /// Configure the schema for the database.
        /// </summary>
        /// <param name="builder">
        /// The builder used to create the schema.
        /// </param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasKey(user => user.Id);
            builder.Entity<Role>()
                .HasKey(role => role.Id);
            builder.Entity<RoleClaim>()
                .HasKey(roleClaim => roleClaim.Id);
            builder.Entity<UserClaim>()
                .HasKey(userClaim => userClaim.Id);
            builder.Entity<UserRole>()
                .HasKey(userRole => new {userRole.RoleId, userRole.UserId});
            builder.Entity<User>()
                .HasData(User.GetUsers());
            builder.Entity<Role>()
                .HasData(Role.GetRoles());
            builder.Entity<RoleClaim>()
                .HasData(RoleClaim.GetRoleClaim());
            builder.Entity<UserRole>()
                .HasData(UserRole.GetUserRoles());
            base.OnModelCreating(builder);
        }
    }
}

