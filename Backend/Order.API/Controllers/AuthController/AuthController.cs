using System.Security.Claims;
using System.Text;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using NuGet.Protocol;
using Order.API.Context;
using Order.API.Controllers.AuthController.Models;
using Order.API.Util;
using Order.API.Util.Configuration;
using Order.API.Util.Store;

namespace Order.API.Controllers.AuthController
{
    /// <summary>
    /// The controller to handle endpoints associated with authentication and authorization.
    /// </summary>
    [Route("auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly CustomSignInManager _signInManager;
        private readonly CustomUserManager _userManager;
        private readonly CustomUserStore _userStore;
        private readonly MailSender _sender;
        private readonly UrlConfiguration _urlConfiguration;

        /// <summary>
        /// Initialize a new <see cref="AuthController"/> with the required information.
        /// </summary>
        /// <param name="signInManager">
        /// The <see cref="CustomSignInManager"/> used to handle the login and logout of users.
        /// </param>
        /// <param name="userManager">
        /// The <see cref="CustomUserManager"/> used to manage the information of a user. 
        /// </param>
        /// <param name="userStore">
        /// The <see cref="CustomUserStore"/> used to access the present users.
        /// </param>
        /// <param name="sender">
        /// The <see cref="MailSender"/> used to send emails to users.
        /// </param>
        /// <param name="urlConfiguration">
        /// The <see cref="UrlConfiguration"/> used to access the base urls that are send to the users for the different emails. 
        /// </param>
        /// <param name="orderContext">
        /// The <see cref="OrderContext"/> used to handle database access.
        /// </param>
        public AuthController(
            CustomSignInManager signInManager,
            CustomUserManager userManager,
            CustomUserStore userStore,
            MailSender sender,
            UrlConfiguration urlConfiguration,
            OrderContext orderContext
        ) : base(orderContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _sender = sender;
            _urlConfiguration = urlConfiguration;
        }

        /// <summary>
        /// Check if the current user is logged in or not.
        /// </summary>
        /// <returns>
        /// An <see cref="OkObjectResult"/> with if the user is logged in or not.
        /// </returns>
        [EnableCors(Cors.AllowFrontend)]
        [AllowAnonymous]
        [HttpGet("logged-in")]
        public IActionResult LoggedIn()
        {
            return Ok(new IsLoggedIn {
                LoggedIn = User.Identity?.IsAuthenticated ?? false
            });
        }

        /// <summary>
        /// Get the account information of the current user.
        /// </summary>
        /// <returns>
        /// An <see cref="OkObjectResult"/> with the account information,
        /// or an <see cref="ObjectResult"/> with a 403 status code if the the user does not exists or is not logged in.
        /// </returns>
        [EnableCors(Cors.AllowFrontend)]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Account()
        {
            var (category, user) = await GetCurrentUser();
            if (category == UserState.NotExist || user == null)
                return NewStatusCode(403);
            return Ok(new Account {
                Name = user.Name,
                Address = user.Address,
                Email = user.Email
            });
        }

        /// <summary>
        /// Register a new employee account.
        /// </summary>
        /// <param name="registerEmployee">
        /// The account information for the new employee.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the account creations was successful,
        /// an <see cref="ObjectResult"/> with a 403 status code if the current user is not logged in or has not the right permissions,
        /// or an <see cref="ObjectResult"/> with a 500 status code if something went wrong.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [Authorize(Policy = Claims.AccountCreateEmployeeClaim)]
        [HttpPost("register/employee")]
        public async Task<IActionResult> RegisterEmployee([FromBody] Register registerEmployee)
        {
            return await Register(registerEmployee, Role.Employee.Name, _urlConfiguration.Admin);
        }

        /// <summary>
        /// Register a new end user account.
        /// </summary>
        /// <param name="registerEndUser">
        /// The account information for the new user.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the account creations was successful,
        /// or an <see cref="ObjectResult"/> with a 500 status code if something went wrong.
        /// </returns>
        [EnableCors(Cors.AllowEndUser)]
        [AllowAnonymous]
        [HttpPost("register/user")]
        public async Task<IActionResult> RegisterEndUser([FromBody] Register registerEndUser)
        {
            return await Register(registerEndUser, Role.User.Name, _urlConfiguration.EndUser);
        }

        /// <summary>
        /// Verify the email of a user.
        /// </summary>
        /// <param name="id">
        /// The id of the account for which to verify the email.
        /// </param>
        /// <param name="code">
        /// The email verification code of the user.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the email verification was successful,
        /// an <see cref="ObjectResult"/> with a 401 status code if the user does not exists or the code is invalid.
        /// </returns>
        [EnableCors(Cors.AllowFrontend)]
        [AllowAnonymous]
        [HttpPost("verify/{id:int}/{code}")]
        public async Task<IActionResult> Verify([FromRoute] int id, [FromRoute] string code)
        {
            var (category, user) = await GetUserById(id);
            if (category == UserState.NotExist || user == null || !IsValidBase64(code))
                return NewStatusCode(401);
            
            code = Decode(code);
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
                return Ok();
            return NewStatusCode(401);
        }

        /// <summary>
        /// Log an employee in.
        /// </summary>
        /// <param name="login">
        /// The credentials of the employee.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the login was successful,
        /// or an <see cref="ObjectResult"/> with a 401 status code and a string representing what went wrong when the login failed.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [AllowAnonymous]
        [HttpPost("login/employee")]
        public async Task<IActionResult> SignInEmployee([FromBody] Login login)
        {
            if ((await GetUserByEmail(login.Email)).Item1 != UserState.Employee)
                return StatusCode(401, new ErrorMessage {Error = ErrorType.Invalid.ToString()});
            return await BaseSignIn(login);
        }

        /// <summary>
        /// Log an user in.
        /// </summary>
        /// <param name="login">
        /// The credentials of the user.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the login was successful,
        /// or an <see cref="ObjectResult"/> with a 401 status code and a string representing what went wrong when the login failed.
        /// </returns>
        [EnableCors(Cors.AllowEndUser)]
        [AllowAnonymous]
        [HttpPost("login/user")]
        public async Task<IActionResult> SignInUser([FromBody] Login login)
        {
            if ((await GetUserByEmail(login.Email)).Item1 != UserState.User)
                return StatusCode(401, new ErrorMessage {Error = ErrorType.Invalid.ToString()});
            return await BaseSignIn(login);
        }

        /// <summary>
        /// Sign the current user out.
        /// </summary>
        /// <returns>
        /// An <see cref="OkResult"/> if the log out was successful,
        /// or an <see cref="ObjectResult"/> with a 403 status code if the user is not logged in.
        /// </returns>
        [EnableCors(Cors.AllowFrontend)]
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> SignOutAll()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        /// <summary>
        /// Request an email to reset the password of the employee for which the email is given.
        /// </summary>
        /// <param name="forgotPassword">
        /// The email of the employee.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> no matter the success, to stop users knowing which emails are already used.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [AllowAnonymous]
        [HttpPost("forgot-password/employee")]
        public async Task<IActionResult> ForgotPasswordEmployee([FromBody] ForgotPassword forgotPassword)
        {
            var (category, user) = await GetUserByEmail(forgotPassword.Email);
            if (category != UserState.Employee || user == null)
                return Ok(new{a=category.ToString()});

            return await DefaultForgotPassword(user, _urlConfiguration.Admin);
        }

        /// <summary>
        /// Request an email to reset the password of the user for which the email is given.
        /// </summary>
        /// <param name="forgotPassword">
        /// The email of the user.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> no matter the success, to stop users knowing which emails are already used.
        /// </returns>
        [EnableCors(Cors.AllowEndUser)]
        [AllowAnonymous]
        [HttpPost("forgot-password/user")]
        public async Task<IActionResult> ForgotPasswordUser([FromBody] ForgotPassword forgotPassword)
        {
            var (category, user) = await GetUserByEmail(forgotPassword.Email);
            if (category != UserState.User || user == null)
                return Ok();

            return await DefaultForgotPassword(user, _urlConfiguration.EndUser);
        }

        /// <summary>
        /// Request an email to reset the password of the employee who is currently logged in.
        /// </summary>
        /// <returns>
        /// An <see cref="OkResult"/> if the email was send,
        /// or an <see cref="ObjectResult"/> with a 403 status code if the employee is not logged in.
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [HttpPost("forgot-password/current/employee")]
        public async Task<IActionResult> ForgotPasswordCurrentEmployee()
        {
            var (category, user) = await GetCurrentUser();
            if (category != UserState.Employee || user == null)
                return NewStatusCode(403);

            return await DefaultForgotPassword(user, _urlConfiguration.Admin);
        }

        /// <summary>
        /// Request an email to reset the password of the employee who is currently logged in.
        /// </summary>
        /// <returns>
        /// An <see cref="OkResult"/> if the email was send,
        /// or an <see cref="ObjectResult"/> with a 403 status code if the employee is not logged in.
        /// </returns>
        [EnableCors(Cors.AllowEndUser)]
        [Authorize(Policy = Claims.NotEmployeeClaim)]
        [HttpPost("forgot-password/current/user")]
        public async Task<IActionResult> ForgotPasswordCurrentUser()
        {
            var (category, user) = await GetCurrentUser();
            if (category != UserState.User || user == null)
                return NewStatusCode(403);

            return await DefaultForgotPassword(user, _urlConfiguration.EndUser);
        }

        /// <summary>
        /// Change the password of the user referenced by <see cref="id"/>.
        /// </summary>
        /// <param name="id">
        /// The id of the user for which to update the password.
        /// </param>
        /// <param name="code">
        /// The password change code.
        /// </param>
        /// <param name="updatePassword">
        /// The new password information.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the update was successful,
        /// or an <see cref="ObjectResult"/> with a 401 status code if the <see cref="id"/> or <see cref="code"/> are invalid.
        /// </returns>
        [EnableCors(Cors.AllowFrontend)]
        [AllowAnonymous]
        [HttpPost("change-password/{id:int}/{code}")]
        public async Task<IActionResult> ChangePassword([FromRoute] int id, [FromRoute] string code, [FromBody] UpdatePassword updatePassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || !IsValidBase64(code))
                return NewStatusCode(401);

            var result = await _userManager.ResetPasswordAsync(user, Decode(code), updatePassword.Password);
            if (!result.Succeeded)
                return NewStatusCode(401);
            var currentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var authenticated = User.Identity?.IsAuthenticated ?? false;
            if (int.TryParse(currentId, out _) && authenticated)
                await _signInManager.SignOutAsync();
            return Ok();
        }

        /// <summary>
        /// Update the account information of an employee.
        /// </summary>
        /// <param name="updateAccount">
        /// The new account information.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the account is updated,
        /// an <see cref="ObjectResult"/> with a 401 if the password is incorrect,
        /// an <see cref="ObjectResult"/> with a 403 status code if the user does not have the right permissions,
        /// or an <see cref="ObjectResult"/> with a 500 status code if something went wrong. 
        /// </returns>
        [EnableCors(Cors.AllowAdmin)]
        [Authorize(Policy = Claims.AccountEditClaim)]
        [Authorize(Policy = Claims.EmployeeClaim)]
        [HttpPost("employee")]
        public async Task<IActionResult> UpdateAccountEmployee([FromBody] UpdateAccountEmployee updateAccount)
        {
            var (category, user) = await GetCurrentUser();
            if (category != UserState.Employee || user == null)
                return NewStatusCode(403);

            return await DefaultUpdateAccount(user, updateAccount);
        }

        /// <summary>
        /// Update the account information of an employee.
        /// </summary>
        /// <param name="updateAccount">
        /// The new account information.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the account is updated,
        /// an <see cref="ObjectResult"/> with a 401 if the password is incorrect,
        /// an <see cref="ObjectResult"/> with a 403 status code if the user does not have the right permissions,
        /// or an <see cref="ObjectResult"/> with a 500 status code if something went wrong. 
        /// </returns>
        [EnableCors(Cors.AllowEndUser)]
        [Authorize(Policy = Claims.AccountEditClaim)]
        [Authorize(Policy = Claims.NotEmployeeClaim)]
        [HttpPost("user")]
        public async Task<IActionResult> UpdateAccountUser([FromBody] UpdateAccountEndUser updateAccount)
        {
            var (category, user) = await GetCurrentUser();
            if (category != UserState.User || user == null)
                return NewStatusCode(403);

            if (user.Email != updateAccount.Email)
            {
                if (_userManager.Users.Any(user =>
                        user.Email == updateAccount.Email ||
                        user.TempEmail == updateAccount.Email
                    )
                )
                    return NewStatusCode(409);
                user.TempEmail = updateAccount.Email;
            }
            var result = await DefaultUpdateAccount(user, new UpdateAccountEmployee
            {
                Name = updateAccount.Name,
                Address = updateAccount.Address,
                Password = updateAccount.Password
            });

            if ((result is not OkResult && result is not OkObjectResult) || user.Email == updateAccount.Email)
                return result;

            var code = await _userManager.GenerateChangeEmailTokenAsync(user, user.TempEmail);
            await SendEmail(user, _urlConfiguration.EndUser, "email", code, true);
            return result;
        }

        /// <summary>
        /// Update the email with the new email address.
        /// </summary>
        /// <param name="id">
        /// The id of the user for which to update the account.
        /// </param>
        /// <param name="code">
        /// The email change code of the user.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the email verification was successful,
        /// an <see cref="ObjectResult"/> with a 401 status code if the user does not exists or the code is invalid.
        /// </returns>
        [EnableCors(Cors.AllowFrontend)]
        [Authorize(Policy = Claims.AccountEditClaim)]
        [HttpPost("email/{id:int}/{code}")]
        public async Task<IActionResult> UpdateEmail([FromRoute] int id, [FromRoute] string code)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null || !IsValidBase64(code))
                return NewStatusCode(401);

            var changeEmailResult = await _userManager.ChangeEmailAsync(user, user.TempEmail, Decode(code));
            if (!changeEmailResult.Succeeded)
                return NewStatusCode(401);

            var changeUsernameResult = await _userManager.SetUserNameAsync(user, user.TempEmail);
            if (!changeUsernameResult.Succeeded)
                return NewStatusCode(401);

            await _signInManager.SignOutAsync();
            transaction.Complete();
            return Ok();
        }

        /// <summary>
        /// Delete the currently logged in user.
        /// </summary>
        /// <param name="deleteAccount">
        /// The password of the user about to be deleted.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the deletion was a success,
        /// an <see cref="ObjectResult"/> with a 401 status code if the user does not exist,
        /// or an <see cref="ObjectResult"/> with a 500 status code if something went wrong.
        /// </returns>
        [EnableCors(Cors.AllowFrontend)]
        [Authorize(Policy = Claims.AccountDeleteOwnClaim)]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteAccount deleteAccount)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(id, out var intId))
                return NewStatusCode(500);

            var user = await _userManager.FindByIdAsync(intId);
            if (user == null)
                return NewStatusCode(401);

            if (!await _userManager.CheckPasswordAsync(user, deleteAccount.Password))
                return NewStatusCode(401);

            var result = await _userStore.DeleteAsync(user);
            if (!result.Succeeded)
                return NewStatusCode(500);
            await _signInManager.SignOutAsync();

            return Ok();
        }

        /// <summary>
        /// Register an user with the specific information, and role.
        /// </summary>
        /// <param name="register">
        /// The account information.
        /// </param>
        /// <param name="role">
        /// The role to assign the user to.
        /// </param>
        /// <param name="url">
        /// The base url to redirect the user to in the verification email.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the account creations was successful,
        /// or an <see cref="ObjectResult"/> with a 500 status code if something went wrong.
        /// </returns>
        private async Task<IActionResult> Register(Register register, string role, string url)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var user = new User
            {
                Name = register.Name,
                Address = register.Address,
                UserName = register.Email,
                NormalizedUserName = _userManager.NormalizeName(register.Email),
                Email = register.Email,
                NormalizedEmail = _userManager.NormalizeEmail(register.Email)
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
                return StatusCode(500, result.Errors);

            result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
                return NewStatusCode(500);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmail(user, url, "verify", code);

            transaction.Complete();
            return Ok();
        }

        /// <summary>
        /// Log an user in.
        /// </summary>
        /// <param name="login">
        /// The credentials of the user.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the login was successful,
        /// or an <see cref="ObjectResult"/> with a 401 status code and a string representing what went wrong when the login failed.
        /// </returns>
        private async Task<IActionResult> BaseSignIn(Login login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, true, true);
            if (result.Succeeded)
                return Ok();
            if (result.IsLockedOut)
                return StatusCode(401, new ErrorMessage {Error = ErrorType.Locked.ToString()});
            if (result.RequiresTwoFactor)
                return StatusCode(401, new ErrorMessage {Error = ErrorType.TwoFactor.ToString()});
            if (!result.IsNotAllowed)
                return StatusCode(401, new ErrorMessage {Error = ErrorType.Invalid.ToString()});
            return StatusCode(401, new ErrorMessage {Error = result.ToJson()});
        }

        /// <summary>
        /// Send the forgot password email.
        /// </summary>
        /// <param name="user">
        /// The user for which to send the email.
        /// </param>
        /// <param name="url">
        /// The base url to redirect the user to in the email.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/>.
        /// </returns>
        private async Task<IActionResult> DefaultForgotPassword(User user, string url)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            await SendEmail(user, url, "change-password", code);
            return Ok();
        }

        /// <summary>
        /// Update the user with the base information.
        /// </summary>
        /// <param name="user">
        /// The user to update.
        /// </param>
        /// <param name="updateAccount">
        /// The new information.
        /// </param>
        /// <returns>
        /// An <see cref="OkResult"/> if the account is updated,
        /// an <see cref="ObjectResult"/> with a 401 if the password is incorrect,
        /// or an <see cref="ObjectResult"/> with a 500 status code if something went wrong. 
        /// </returns>
        private async Task<IActionResult> DefaultUpdateAccount(User user, UpdateAccountEmployee updateAccount)
        {
            if (!await _userManager.CheckPasswordAsync(user, updateAccount.Password))
                return NewStatusCode(401);

            user.Address = updateAccount.Address;
            user.Name = updateAccount.Name;
            
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return NewStatusCode(500);

            return Ok();
        }

        /// <summary>
        /// Get the currently logged in user.
        /// </summary>
        /// <returns>
        /// Returns a tuple of the success of the retrieval and the user itself.
        /// If the user state is <see cref="UserState.NotExist"/> then the user is null.
        /// </returns>
        private async Task<(UserState, User?)> GetCurrentUser()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(id, out var intId))
                return (UserState.NotExist, null);
            return await GetUserById(intId);
        }

        /// <summary>
        /// Get the user associated with the id.
        /// </summary>
        /// <param name="id">
        /// The id of the user.
        /// </param>
        /// <returns>
        /// Returns a tuple of the success of the retrieval and the user itself.
        /// If the user state is <see cref="UserState.NotExist"/> then the user is null.
        /// </returns>
        private async Task<(UserState, User?)> GetUserById(int id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return (UserState.NotExist, null);
            if (await IsEmployee(user))
                return (UserState.Employee, user);
            return (UserState.User, user);
        }

        /// <summary>
        /// Get the user associated with the email.
        /// </summary>
        /// <param name="email">
        /// The email of the user.
        /// </param>
        /// <returns>
        /// Returns a tuple of the success of the retrieval and the user itself.
        /// If the user state is <see cref="UserState.NotExist"/> then the user is null.
        /// </returns>
        private async Task<(UserState, User?)> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(_userManager.NormalizeEmail(email));
            if (user == null)
                return (UserState.NotExist, user);
            if (await IsEmployee(user))
                return (UserState.Employee, user);
            return (UserState.User, user);
        }

        /// <summary>
        /// Check if a user is an employee or not.
        /// By checking if he has the <see cref="Claims.EmployeeClaim"/> value for one claim.
        /// </summary>
        /// <param name="user">
        /// The user to check.
        /// </param>
        /// <returns>
        /// If a user is an employee or not.
        /// </returns>
        private async Task<bool> IsEmployee(User user)
        {
            var claims = await _userManager.GetAllClaims(user);
            return claims.Any(claim => claim is {Type: Claims.Type, Value: Claims.EmployeeClaim});
        }

        /// <summary>
        /// Send a email to a user with a url setup as follows "baseUrl/path/user.id/code".
        /// </summary>
        /// <param name="user">
        /// The user to which to send the email.
        /// </param>
        /// <param name="baseUrl">
        /// The base url to redirect the user to.
        /// </param>
        /// <param name="path">
        /// The path to send the user to.
        /// </param>
        /// <param name="code">
        /// The code to put in the url.
        /// </param>
        /// <param name="useTempEmail">
        /// If the <see cref="User.TempEmail"/> needs to be used.
        /// </param>
        private async Task SendEmail(User user, string baseUrl, string path, string code, bool useTempEmail = false)
        {
            var email = (useTempEmail ? user.TempEmail : user.Email) ?? user.Email;
            code = Encode(code);
            var callbackUrl = $"{baseUrl}/{path}/{user.Id}/{code}";
            var message = $"To verify please click on the following link: {callbackUrl}";
            await _sender.Send(user.Name, email, message);
        }

        /// <summary>
        /// Encode a string to use it in a url.
        /// </summary>
        /// <param name="code">
        /// The string to encode.
        /// </param>
        /// <returns>
        /// The encoded string.
        /// </returns>
        private static string Encode(string code)
        {
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        }

        /// <summary>
        /// Decode a string encoded by <see cref="Encode"/>.
        /// </summary>
        /// <param name="code">
        /// The string to decode.
        /// </param>
        /// <returns>
        /// The decode string.
        /// </returns>
        private static string Decode(string code)
        {
            return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }

        /// <summary>
        /// Check if a string is a valid base64 string.
        /// </summary>
        /// <param name="base64">
        /// The string to check.
        /// </param>
        private static bool IsValidBase64(string base64)
        {
            try
            {
                WebEncoders.Base64UrlDecode(base64);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}