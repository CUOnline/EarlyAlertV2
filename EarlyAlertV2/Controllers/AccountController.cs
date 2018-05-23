using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using EarlyAlertV2.ViewModels.AccountViewModels;
using EarlyAlertV2.Services;
using EarlyAlertV2.Models;
using EarlyAlertV2.Helpers;

namespace EarlyAlertV2.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [AuthorizeRoles(UserRoles.OITAdminRole, UserRoles.AdminRole)]
        public IActionResult UserAccounts()
        {
            var model = new UserAccountsViewModel();
            model.Users = _userManager.Users
                                        .Where(x => x.Email != "oitadmin@ucdenver.edu")
                                        .OrderByDescending(x => x.LockoutEnabled)
                                        .ThenBy(x => x.FirstName).ToList();
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(UserRoles.OITAdminRole, UserRoles.AdminRole)]
        public IActionResult SearchUserAccounts(UserAccountsViewModel model)
        {
            if(!string.IsNullOrWhiteSpace(model.SearchFirstName) && !string.IsNullOrWhiteSpace(model.SearchLastName))
            {
                model.Users = _userManager.Users.OrderBy(x => x.FirstName).Where(x => x.FirstName.Contains(model.SearchFirstName) && x.LastName.Contains(model.SearchLastName));
            }
            else if (!string.IsNullOrWhiteSpace(model.SearchFirstName))
            {
                model.Users = _userManager.Users.OrderBy(x => x.FirstName).Where(x => x.FirstName.Contains(model.SearchFirstName));
            }
            else if (!string.IsNullOrWhiteSpace(model.SearchLastName))
            {
                model.Users = _userManager.Users.OrderBy(x => x.FirstName).Where(x => x.LastName.Contains(model.SearchLastName));
            }
            
            return View(model);
        }

        public async Task<IActionResult> AddEditUser(string id)
        {
            var model = new AddEditUserViewModel();

            if (!string.IsNullOrWhiteSpace(id))
            {
                var user = await _userManager.FindByIdAsync(id);
                model = new AddEditUserViewModel()
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };

                var roles = _userManager.GetRolesAsync(user);

                if (roles.Result.Contains(UserRoles.AdminRole)) model.IsAdminRole = true;
                if (roles.Result.Contains(UserRoles.ReportManagerRole)) model.IsReportManagerRole = true;
            }

            return PartialView("~/Views/Account/_AddEditUser.cshtml", model);
        }

        [HttpPost]
        [AuthorizeRoles(UserRoles.OITAdminRole, UserRoles.AdminRole)]
        public async Task<IActionResult> AddEditUser(AddEditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.UserId))
                {
                    var user = await _userManager.FindByIdAsync(model.UserId);

                    // Strip roles from user
                    var roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles);
                    
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;

                    await _userManager.UpdateAsync(user);

                    // Add selected roles
                    if (model.IsAdminRole) await _userManager.AddToRoleAsync(user, UserRoles.AdminRole);
                    if (model.IsReportManagerRole) await _userManager.AddToRoleAsync(user, UserRoles.ReportManagerRole);

                    return RedirectToAction("UserAccounts");
                }
                else
                {
                    var user = new ApplicationUser()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserName = model.Email
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        user = await _userManager.FindByEmailAsync(user.Email);

                        if (model.IsAdminRole) await _userManager.AddToRoleAsync(user, UserRoles.AdminRole);
                        if (model.IsReportManagerRole) await _userManager.AddToRoleAsync(user, UserRoles.ReportManagerRole);

                        return RedirectToAction("UserAccounts");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }

            // Todo: Problem here.... We cannot just send the users to the UserAccounts view....
            return RedirectToAction("UserAccounts", model);
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Check for account.  If exists, create external signin.
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if(user != null && !(await _userManager.GetLoginsAsync(user)).Any())
            {
                await _userManager.AddLoginAsync(user, info);
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                return RedirectToAction(nameof(AccessDenied));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
