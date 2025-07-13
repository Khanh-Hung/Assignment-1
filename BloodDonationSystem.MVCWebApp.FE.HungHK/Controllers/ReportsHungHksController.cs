using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreGeneratedDocument;
using BloodDonationSystem.Repositories.HungHK.DBContext;
using BloodDonationSystem.Repositories.HungHK.ModelExtensions;
using BloodDonationSystem.Repositories.HungHK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BloodDonationSystem.MVCWebApp.FE.HungHK.Controllers
{
    public class ReportsHungHksController : Controller
    {
        private string APIEndPoint = "https://localhost:7016/api/";
        public ReportsHungHksController() { }
        public async Task<IActionResult> Index(int? totalUsers, int? generatedBy, string? mostNeededBloodType, int currentPage = 1, int pageSize = 6)
        {
            var request = new SearchReportsHungHkRequest
            {
                TotalUsers = totalUsers,
                GeneratedBy = generatedBy,
                MostNeededBloodType = mostNeededBloodType,
                CurrentPage = currentPage,
                PageSize = pageSize
            };

            using (var httpClient = new HttpClient())
            {
                var tokenString = HttpContext.Request.Cookies["TokenString"];
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                // Gọi API lấy danh sách user để đổ vào dropdown
                var usersResponse = await httpClient.GetAsync(APIEndPoint + "SystemUserAccounts");
                if (usersResponse.IsSuccessStatusCode)
                {
                    var usersJson = await usersResponse.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<List<SystemUserAccount>>(usersJson);
                    ViewBag.UserList = new SelectList(users, "UserAccountId", "UserName", generatedBy);
                }
                else
                {
                    ViewBag.UserList = new SelectList(new List<SystemUserAccount>(), "UserAccountId", "UserName");
                }

                // Gọi API tìm kiếm báo cáo có phân trang
                var response = await httpClient.PostAsJsonAsync(APIEndPoint + "ReportsHungHk/SearchWithPaging", request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<PaginationResult<List<ReportsHungHk>>>(content);

                    ViewBag.TotalUsers = totalUsers;
                    ViewBag.GeneratedBy = generatedBy;
                    ViewBag.MostNeededBloodType = mostNeededBloodType;

                    return View(result);
                }
            }

            // Trường hợp lỗi thì trả về kết quả rỗng
            ViewBag.UserList = new SelectList(new List<SystemUserAccount>(), "UserAccountId", "UserName");
            ViewBag.TotalUsers = totalUsers;
            ViewBag.GeneratedBy = generatedBy;
            ViewBag.MostNeededBloodType = mostNeededBloodType;

            // Nếu lỗi xảy ra, trả về view trống
            var empty = new PaginationResult<List<ReportsHungHk>>
            {
                Items = new List<ReportsHungHk>(),
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = 0,
                TotalPage = 0
            };

            return View(empty);
        }


        // GET: ReportsHungHks/Create
        public async Task<IActionResult> Create()
        {
            var report = new ReportsHungHk
            {
                ReportDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var (userId, userName) = GetCurrentUserFromToken();
            ViewBag.CurrentUserId = userId;
            ViewBag.CurrentUserName = userName;

            var reportTypes = await this.GetReportTypes();
            ViewData["ReportTypeId"] = new SelectList(reportTypes, "ReportTypeHungHkid", "TypeName");

            return View(report);
        }

        // POST: ReportsHungHks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReportsHungHk reportsHungHk)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                    using (var response = await httpClient.PostAsJsonAsync(APIEndPoint + "ReportsHungHk", reportsHungHk))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<int>(content);

                            if (result > 0)
                            {
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
            }

            var reportTypes = await this.GetReportTypes();
            ViewData["ReportTypeId"] = new SelectList(reportTypes, "ReportTypeHungHkid", "TypeName", reportsHungHk.ReportTypeId);
            return View(reportsHungHk);
        }

        // GET: ReportsHungHks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var (userId, userName) = GetCurrentUserFromToken();
            ViewBag.CurrentUserId = userId;
            ViewBag.CurrentUserName = userName;

            var reportsHungHk = await this.GetReportById(id.Value);
            if (reportsHungHk == null)
            {
                return NotFound();
            }

            if (reportsHungHk.GeneratedBy != userId)
                return Forbid();

            var reportTypes = await this.GetReportTypes();
            ViewData["ReportTypeId"] = new SelectList(reportTypes, "ReportTypeHungHkid", "TypeName", reportsHungHk.ReportTypeId);
            return View(reportsHungHk);
        }

        // POST: ReportsHungHks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReportsHungHk reportsHungHk)
        {

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                    using (var response = await httpClient.PutAsJsonAsync(APIEndPoint + "ReportsHungHk", reportsHungHk))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<int>(content);

                            if (result > 0)
                            {
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
            }
            var reportTypes = await this.GetReportTypes();
            ViewData["ReportTypeId"] = new SelectList(reportTypes, "ReportTypeHungHkid", "TypeName", reportsHungHk.ReportTypeId);
            return View(reportsHungHk);
        }

        // GET: ReportsHungHks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportsHungHk = await this.GetReportById(id.Value);
            if (reportsHungHk == null)
            {
                return NotFound();
            }

            return View(reportsHungHk);
        }

        // GET: ReportsHungHks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportsHungHk = await this.GetReportById(id.Value);
            if (reportsHungHk == null)
            {
                return NotFound();
            }

            return View(reportsHungHk);
        }

        // POST: ReportsHungHks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reportsHungHk = await this.GetReportById(id);
            if (reportsHungHk != null)
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                    using (var response = await httpClient.DeleteAsync(APIEndPoint + "ReportsHungHk" + "/" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<bool>(content);

                            if (result)
                            {
                                TempData["SuccessMessage"] = "Delete successfully!";
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
            }
            TempData["ErrorMessage"] = "Failed to delete the report.";
            return View(reportsHungHk);
        }

        public async Task<List<ReportTypesHungHk>> GetReportTypes()
        {
            var reportTypes = new List<ReportTypesHungHk>();

            using (var httpClient = new HttpClient())
            {

                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;

                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);


                using (var response = await httpClient.GetAsync(APIEndPoint + "ReportTypesHungHk"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        reportTypes = JsonConvert.DeserializeObject<List<ReportTypesHungHk>>(content);
                    }
                }
            }

            return reportTypes;
        }
        public async Task<ReportsHungHk> GetReportById(int id)
        {
            var report = new ReportsHungHk();

            using (var httpClient = new HttpClient())
            {

                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;

                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);


                using (var response = await httpClient.GetAsync(APIEndPoint + "ReportsHungHk" + "/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        report = JsonConvert.DeserializeObject<ReportsHungHk>(content);
                    }
                }
            }

            return report;
        }
        public (int userId, string userName) GetCurrentUserFromToken()
        {
            var tokenString = HttpContext.Request.Cookies["TokenString"];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            var userName = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var userIdStr = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdStr, out int userId);

            return (userId, userName);
        }
        public async Task<List<ReportsHungHk>> GetReports()
        {
            throw new NotImplementedException();
        }
        public async Task<IActionResult> ReportTypesList()
        {
            return View();
        }

    }
}
