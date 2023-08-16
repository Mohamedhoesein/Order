using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Order.API.Context;

namespace Order.API.Util.Store
{
    /// <summary>
    /// A <see cref="UserManager{TUser}"/> wrapper to have a easy reference to it without the need for the generics and
    /// to add some custom methods.
    /// </summary>
    public class CustomUserManager : UserManager<User>
    {
        private readonly OrderContext _context;

        /// <summary>
        /// Initialize a new <see cref="CustomUserManager"/> with the required information.
        /// </summary>
        /// <param name="store">
        /// The <see cref="IUserStore{TUser}"/> to use.
        /// </param>
        /// <param name="optionsAccessor">
        /// The <see cref="IOptions{TOptions}"/> to use.
        /// </param>
        /// <param name="passwordHasher">
        /// The <see cref="IPasswordHasher{TUser}"/> to use.
        /// </param>
        /// <param name="userValidators">
        /// The <see cref="IUserValidator{TUser}"/> list to use.
        /// </param>
        /// <param name="passwordValidators">
        /// The <see cref="IPasswordValidator{TUser}"/> list to use.
        /// </param>
        /// <param name="keyNormalizer">
        /// The <see cref="ILookupNormalizer"/> to use.
        /// </param>
        /// <param name="errors">
        /// The <see cref="IdentityErrorDescriber"/> to use.
        /// </param>
        /// <param name="services">
        /// The <see cref="IServiceProvider"/> to use.
        /// </param>
        /// <param name="logger">
        /// A <see cref="ILogger{TCategoryName}"/> to use.
        /// </param>
        /// <param name="context">
        /// The <see cref="OrderContext"/> to use.
        /// </param>
        public CustomUserManager(
            IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger,
            OrderContext context
        ) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _context = context;
        }

        /// <summary>
        /// Find a user by their id.
        /// </summary>
        /// <param name="id">
        /// The id to use for looking for a user.
        /// </param>
        /// <returns>
        /// Either the user or null if he could not be found.
        /// </returns>
        public async Task<User?> FindByIdAsync(int id)
        {
            return await Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        /// <summary>
        /// Get all the permissions for the user. Those are the ones associated with the user directly,
        /// and those associated to the roles which are associated with the user.
        /// </summary>
        /// <param name="user">
        /// The user for which to get the permissions.
        /// </param>
        /// <returns>
        /// All the permissions for the user.
        /// </returns>
        public async Task<List<Claim>> GetAllClaims(User user)
        {
            var userClaims = await _context.UserClaims.Where(claim => claim.UserId == user.Id)
                .Select(claim => new Claim(claim.ClaimType, claim.ClaimValue))
                .ToListAsync();
            var roleClaims = await _context.UserRoles
                .Where(claim => claim.UserId == user.Id)
                .Join(
                    _context.Roles,
                    userRole => userRole.RoleId,
                    role => role.Id,
                    (userRole, role) => new {userRole.UserId, Role = role}
                )
                .Join(
                    _context.RoleClaims,
                    role => role.Role.Id,
                    roleClaim => roleClaim.RoleId,
                    (role, roleClaim) => new {role.UserId, Claim = new {roleClaim.ClaimType, roleClaim.ClaimValue}}
                )
                .Select(claim => new Claim(claim.Claim.ClaimType, claim.Claim.ClaimValue))
                .ToListAsync();
            return userClaims.Concat(roleClaims).ToList();
        }
    }
}