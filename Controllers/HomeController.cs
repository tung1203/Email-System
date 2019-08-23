using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginSystem.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using LoginSystem.Filters;
using Microsoft.AspNetCore.Authorization;

namespace LoginSystem.Controllers
{

    public class HomeController : Controller
    {
        private MyDbContext dbContext;

        public HomeController(MyDbContext context)
        {
            this.dbContext = context;
            dbContext.Database.EnsureCreated();
        }

        [Authentication]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            // if (userId == null)
            // {
            //     return Redirect("/Home/Login");
            // }
            User userLoged = dbContext.User.FirstOrDefault(user => user.UserId == userId);
            ViewBag.user = userLoged;

            var emailQuery = from de in dbContext.DetailEmail
                             join e in dbContext.Email
                             on de.EmailId equals e.EmailId
                             join u in dbContext.User
                             on de.IdRecevier equals u.UserId
                             where de.IdRecevier == userLoged.UserId
                             select new EmailView
                             {
                                 EmailVm = e,
                                 UserVm = u
                             };

            ViewBag.ListEmails = emailQuery.ToList();
            return View();
        }

        public IActionResult Login(bool err)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId != null)
            {
                return Redirect("/");
            }
            if (err == true)
            {
                ViewBag.err = true;
            }
            return View();
        }
        public IActionResult DeleteEmail(int idEmail)
        {
            var email = dbContext.Email.FirstOrDefault(x => x.EmailId == idEmail);
            if (email != null)
            {
                dbContext.Email.Remove(email);
                dbContext.SaveChanges();
                ViewBag.delSucess = true;
            }
            return Redirect("/");
        }
        [HttpPost]
        public IActionResult DoLogin(string username, string password)
        {
            var user = new User(username, password);
            using (MD5 md5Hash = MD5.Create())
            {
                user = dbContext.User.FirstOrDefault(acc => acc.UserName == username && VerifyMd5Hash(md5Hash, password, acc.UserPassword));
            }
            if (user == null)
            {
                return Redirect("/Home/Login?err=true");
            }
            HttpContext.Session.SetInt32("userId", user.UserId);
            return Redirect("/");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userId");
            return Redirect("/");
        }

        public string Get(string key)
        {
            return Request.Cookies[key];
        }

        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            Response.Cookies.Append(key, value, option);
        }

        public void Remove(string key)
        {
            Response.Cookies.Delete(key);
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // Verify a hash against a string.


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
