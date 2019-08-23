using System;
using System.Collections.Generic;
using System.Linq;
using LoginSystem.Filters;
using LoginSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoginSystem.Controllers
{
    [Authentication]
    public class ComposeController : Controller
    {
        private MyDbContext dbContext;
        public ComposeController(MyDbContext context)
        {
            this.dbContext = context;
            dbContext.Database.EnsureCreated();
        }
        
        public IActionResult Index()
        {
            LoginSystem.Models.Email email = new LoginSystem.Models.Email();
            // email.emailId = 1;
            // email.emailTitle = "o9k";
            // email.emailContent = "a";
            // dbContext.Email.Add(email);
            // dbContext.SaveChanges();
            Console.WriteLine(email.EmailContent);
            int? userId = HttpContext.Session.GetInt32("userId");
            ViewBag.user = dbContext.User.FirstOrDefault(user => user.UserId == userId);
            return View();
        }
        
        public IActionResult ReceiveEmail(string titleEmail, string contentEmail, string userName)
        {

            // Console.WriteLine("username: " + userName);
            string[] userNames = userName.Split(',');
            // foreach (var item in userNames)
            // {
            //     Console.WriteLine(item);
            // }
            LoginSystem.Models.Email email = new LoginSystem.Models.Email();
            email.EmailTitle = titleEmail;
            email.EmailContent = contentEmail;
            dbContext.Email.Add(email);
            dbContext.SaveChanges();

            foreach (var item in userNames)
            {
                if (item == "")
                {
                    break;
                }
                User user = dbContext.User.FirstOrDefault(x => x.UserName == item);
                Console.WriteLine(user.UserName);
                DetailEmail detailEmail = new DetailEmail();
                int? userId = HttpContext.Session.GetInt32("userId");
                detailEmail.IdSender = userId;
                detailEmail.IdRecevier = user.UserId;
                detailEmail.EmailId = email.EmailId;
                dbContext.DetailEmail.Add(detailEmail);
                dbContext.SaveChanges();
            }

            return Redirect("/");
        }
    }
}