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
             Message message = new Message();
            // email.emailId = 1;
            // email.emailTitle = "o9k";
            // email.emailContent = "a";
            // dbContext.Email.Add(email);
            // dbContext.SaveChanges();
            // Console.WriteLine(message.messageContent);
            int? userId = HttpContext.Session.GetInt32("userId");
            ViewBag.user = dbContext.User.FirstOrDefault(user => user.userId == userId);
            return View();
        }
        
        public IActionResult ReceiveEmail(string titleEmail, string contentEmail, string reciverName)
        {

            // Console.WriteLine("username: " + reciverName);
            string[] userNames = reciverName.Split(',');
            
            // foreach (var item in userNames)
            // {
            //     Console.WriteLine(item + "\n");
            // }
            User reciver = dbContext.User.FirstOrDefault(user => user.userName == reciverName);
            // Console.WriteLine(reciverName);
            // Console.WriteLine(contentEmail);
            Message message = new Message();
            int? userId = HttpContext.Session.GetInt32("userId");
            message.messageTitle = titleEmail;
            message.messageContent = contentEmail;
            message.messageSenderId = userId;
            message.messageSendDate = DateTime.Now;
            dbContext.Message.Add(message);
            dbContext.SaveChanges();

            Outbox outbox = new Outbox();
            outbox.messageId = message.messageId;
            outbox.senderId = userId;
            outbox.isDeleted = 0;
            dbContext.Outbox.Add(outbox);
            dbContext.SaveChanges();

           
             foreach (var userName in userNames)
            {
                Console.WriteLine(userName + ",");
                User userReciever = dbContext.User.FirstOrDefault(user => user.userName == userName);
                // Console.WriteLine(userReciever.userName);
                Inbox inbox = new Inbox();
                inbox.messageId = message.messageId;
                inbox.receiverId = userReciever.userId;
                outbox.isDeleted = 0;
                dbContext.Inbox.Add(inbox);
                dbContext.SaveChanges();
            }
            // foreach (var item in userNames)
            // {
            //     if (item == "")
            //     {
            //         break;
            //     }
            //     User user = dbContext.User.FirstOrDefault(x => x.UserName == item);
            //     Console.WriteLine(user.UserName);
            //     DetailEmail detailEmail = new DetailEmail();
            //     int? userId = HttpContext.Session.GetInt32("userId");
            //     detailEmail.IdSender = userId;
            //     detailEmail.IdRecevier = user.UserId;
            //     detailEmail.EmailId = email.EmailId;
            //     dbContext.DetailEmail.Add(detailEmail);
            //     dbContext.SaveChanges();
            // }

            return Redirect("/");
        }
    }
}