using System.Security.Claims;

namespace Order.API.Context
{
    /// <summary>
    /// A class to contain information about different permissions.
    /// </summary>
    public class Claims
    {
        /// <summary>
        /// The claim type for permissions associated with a role or user.
        /// </summary>
        public const string Type = "Permission";
        /// <summary>
        /// The name of the authorization policy for having the Employee permission.
        /// </summary>
        public const string EmployeeClaim = "Employee";
        /// <summary>
        /// The name of the authorization policy for not having the Employee permission.
        /// </summary>
        public const string NotEmployeeClaim = $"!{EmployeeClaim}";
        /// <summary>
        /// The name of the authorization policy for having the Account.Edit permission.
        /// </summary>
        public const string AccountEditClaim = "Account.Edit";
        /// <summary>
        /// The name of the authorization policy for not having the Account.Edit permission.
        /// </summary>
        public const string NotAccountEditClaim = $"!{AccountEditClaim}";
        /// <summary>
        /// The name of the authorization policy for having the Account.Delete.Own permission.
        /// </summary>
        public const string AccountDeleteOwnClaim = "Account.Delete.Own";
        /// <summary>
        /// The name of the authorization policy for not having the Account.Delete.Own permission.
        /// </summary>
        public const string NotAccountDeleteOwnClaim = $"!{AccountDeleteOwnClaim}";
        /// <summary>
        /// The name of the authorization policy for having the Account.Delete.User permission.
        /// </summary>
        public const string AccountDeleteUserClaim = "Account.Delete.User";
        /// <summary>
        /// The name of the authorization policy for not having the Account.Delete.User permission.
        /// </summary>
        public const string NotAccountDeleteUserClaim = $"!{AccountDeleteUserClaim}";
        /// <summary>
        /// The name of the authorization policy for having the Account.Create.Employee permission.
        /// </summary>
        public const string AccountCreateEmployeeClaim = "Account.Create.Employee";
        /// <summary>
        /// The name of the authorization policy for not having the Account.Create.Employee permission.
        /// </summary>
        public const string NotAccountCreateEmployeeClaim = $"!{AccountCreateEmployeeClaim}";
        /// <summary>
        /// The name of the authorization policy for having the Controller.Manage permission. 
        /// </summary>
        public const string CategoryManageClaim = "Controller.Manage";
        /// <summary>
        /// The name of the authorization policy for not having the Controller.Manage permission. 
        /// </summary>
        public const string NotCategoryManageClaim = $"!{CategoryManageClaim}";

        /// <summary>
        /// The claim for the Employee permission.
        /// </summary>
        public static Claim Employee { get; } = new(Type, EmployeeClaim);
        /// <summary>
        /// The claim for the Account.Edit permission.
        /// </summary>
        public static Claim AccountEdit { get; } = new(Type, AccountEditClaim);
        /// <summary>
        /// The claim for the Account.Delete.Own permission.
        /// </summary>
        public static Claim AccountDeleteOwn { get; } = new(Type, AccountDeleteOwnClaim);
        /// <summary>
        /// The claim for the Account.Delete.User permission.
        /// </summary>
        public static Claim AccountDeleteUser { get; } = new(Type, AccountDeleteUserClaim);
        /// <summary>
        /// The claim for the Account.Create.Employee permission.
        /// </summary>
        public static Claim AccountCreateEmployee { get; } = new(Type, AccountCreateEmployeeClaim);
        /// <summary>
        /// The claim for the Controller.Manage permission.
        /// </summary>
        public static Claim CategoryManage { get; } = new(Type, CategoryManageClaim);
    }
}