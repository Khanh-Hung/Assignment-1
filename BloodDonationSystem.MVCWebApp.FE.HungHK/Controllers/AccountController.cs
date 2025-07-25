﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BloodDonationSystem.Repositories.HungHK.ModelExtensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationSystem.MVCWebApp.FE.HungHK.Controllers
{
    public class AccountController : Controller
    {
        private string APIEndPoint = "https://localhost:7016/api/";
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(APIEndPoint + "SystemUserAccounts/Login", login))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var tokenString = await response.Content.ReadAsStringAsync();

                            var tokenHandler = new JwtSecurityTokenHandler();
                            var jwtToken = tokenHandler.ReadToken(tokenString) as JwtSecurityToken;

                            if (jwtToken != null)
                            {
                                var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                                var roleId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                                var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, userName),
                                new Claim(ClaimTypes.Role, roleId),
                            };

                                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                                Response.Cookies.Append("UserName", userName);
                                Response.Cookies.Append("Role", roleId);
                                Response.Cookies.Append("TokenString", tokenString);

                                return RedirectToAction("Index", "ReportsHungHks");
                                //return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ModelState.AddModelError("", "Login failure");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Cookies.Delete("UserName");
            Response.Cookies.Delete("Role");
            Response.Cookies.Delete("TokenString");

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Forbidden()
        {
            return View();
        }
    }
}
