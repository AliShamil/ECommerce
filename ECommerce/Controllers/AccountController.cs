using ECommerce.Models;
using ECommerce.Models.ViewModels;
using ECommerce.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace ECommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    Year = model.Year
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var confirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action("ConfirmEmail", "Account", new { confirmToken, user.Email }, Request.Scheme);

                    using var message = new MailMessage()
                    {
                        Body = $@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <title>Email Confirmation</title>
</head>
<body style=""font-family: Arial, sans-serif; background-color: #f2f2f2; margin: 0; padding: 0;"">

    <!-- Header -->
    <table width=""100%"" bgcolor=""#ffffff"" style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px;"">
        <tr>
            <td>
                <img src='https://scontent.fgyd9-1.fna.fbcdn.net/v/t39.30808-6/243190031_4305656159516878_6915229837043115203_n.jpg?_nc_cat=101&amp';ccb=1-7&amp;_nc_sid=2c4854&amp;_nc_ohc=FI4AAqqdRd4AX-esGey&amp;_nc_ht=scontent.fgyd9-1.fna&amp;oh=00_AfABi_ya5iplGhhvjYrhMevzOrPUO8ogwl2dRzC6q2MaFg&amp;oe=64CFA434"" alt=""Company Logo"" style=""display: block; max-width: 200px;"">
            </td>
        </tr>
    </table>

    <!-- Main Content -->
    <table width=""100%"" bgcolor=""#ffffff"" style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; margin-top: 20px;"">
        <tr>
            <td>
                <h2 style=""color: #333333;"">Confirm Your Email Address</h2>
                <p style=""color: #333333;"">Hello {user.FullName},</p>
                <p style=""color: #333333;"">Thank you for signing up on our website. Please click the button below to confirm your email address and complete the registration process:</p>
                <p style=""text-align: center; margin-top: 30px;"">
                    <a href={url} style=""background-color: #007BFF; color: #ffffff; text-decoration: none; padding: 10px 20px; border-radius: 5px; display: inline-block;"">Confirm Email</a>
                </p>
                <p style=""color: #333333;"">If you didn't sign up on our website, you can ignore this email.</p>
            </td>
        </tr>
    </table>

    <!-- Footer -->
    <table width=""100%"" bgcolor=""#f2f2f2"" style=""max-width: 600px; margin: 0 auto; background-color: #f2f2f2; padding: 20px; margin-top: 20px;"">
        <tr>
            <td>
                <p style=""color: #777777; font-size: 12px; text-align: center;"">Company Name | Address | Contact Information</p>
            </td>
        </tr>
    </table>

</body>
</html>
",
                        Subject = "Test subject",
                        IsBodyHtml = true,
                    };
                    EmailService.SendSmtp(user.Email, message);
                    //result = await userManager.AddToRoleAsync(user, "User");
                    
                    return RedirectToAction("Login", "Account", new {area="default"});
                    
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> ConfirmEmail(string confirmToken, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                var result = await userManager.ConfirmEmailAsync(user, confirmToken);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                return View();
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                var roles = await userManager.GetRolesAsync(user);
                if (user is not null)
                {
                    if (await userManager.IsEmailConfirmedAsync(user))
                    {
                        var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                        
                        if (result.Succeeded)
                        {
                            if (roles.FirstOrDefault("Admin") is not null)
                                return Redirect("/");
                            else
                                RedirectToAction("Index", "Home", new { area = "User" });
                            //if (!string.IsNullOrWhiteSpace(returnUrl))
                            //    return Redirect(returnUrl);
                            //return Redirect("/");
                        }
                        else if (result.IsLockedOut)
                            ModelState.AddModelError("All", "Lockout");
                    }
                    else
                        ModelState.AddModelError("All", "Mail has not confired yet!!");

                }
                else
                    ModelState.AddModelError("login", "Incorrect username or password");
            }

            return View(model);
        }

    }
}
