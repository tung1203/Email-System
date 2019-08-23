using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LoginSystem.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using LoginSystem.Filters;
using Microsoft.AspNetCore.Authorization;

namespace LoginSystem.Controllers
{
    
    public class Register : Controller
    {
        private MyDbContext dbContext;

        public Register(MyDbContext context)
        {
            this.dbContext = context;
            dbContext.Database.EnsureCreated();
        }
       
        public IActionResult Index(bool err, bool wrongRepassWord)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId != null)
            {
                return Redirect("/");
            }
            if (err == true)
            {
                ViewBag.err = err;
            }
            if (wrongRepassWord == true)
            {
                ViewBag.wrongRepassWord = wrongRepassWord;
            }
            return View();
        }

        public IActionResult DoRegister(string username, string password, string reEnter)
        {

            if (password != reEnter)
            {
                return Redirect("/Register/?wrongRepassWord=true");
            }
            else
            {

                var user = new User(username, password);
                var newUser = dbContext.User.FirstOrDefault(acc => acc.UserName == username);
                if (newUser == null)
                {
                    var user1 = new User(username, password);
                    using (MD5 md5Hash = MD5.Create())
                    {
                        user1.UserPassword = GetMd5Hash(md5Hash, user1.UserPassword);
                    }
                    dbContext.User.Add(user1);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetString("username", username);
                    Console.WriteLine(HttpContext.Session.GetString("username"));
                    return Redirect("/");
                }
                else
                {
                    Console.WriteLine(HttpContext.Session.GetString("username"));
                    return Redirect("/Register/?err=true");
                }
            }


        }
        // public string Get(string key)
        // {
        //     return Request.Cookies[key];
        // }

        // public void Set(string key, string value, int? expireTime)
        // {
        //     CookieOptions option = new CookieOptions();
        //     if (expireTime.HasValue)
        //         option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
        //     else
        //         option.Expires = DateTime.Now.AddMilliseconds(10);
        //     Response.Cookies.Append(key, value, option);
        // }

        // public void Remove(string key)
        // {
        //     Response.Cookies.Delete(key);
        // }
        public string GetMd5Hash(MD5 md5Hash, string input)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
