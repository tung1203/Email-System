using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LoginSystem.Filters;
using LoginSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginSystem.Controllers
{
    [Authentication]
    public class SentEmailController : Controller
    {
        private MyDbContext dbContext;
        public SentEmailController(MyDbContext context)
        {
            this.dbContext = context;
            dbContext.Database.EnsureCreated();
        }

        public IActionResult Index()
        {

            int? userId = HttpContext.Session.GetInt32("userId");

            User userLoged = dbContext.User.FirstOrDefault(user => user.userId == userId);
            ViewBag.user = userLoged;

            List<Message> Messages = new List<Message>();
            // List<Email> Emails = new List<Email>();
            // List<User> Users = new List<User>();
            
            string query1 = "select m.messageId, m.messageTitle, m.messageContent, m.messageSenderId, m.messageSendDate from message m inner join inbox i on m.messageId = i.messageId inner join outbox o on m.messageId = o.messageId where m.messageSenderId =" + 1 + " group by m.messageId;";
            Messages = dbContext.Message.FromSql(query1).ToList();




            // string query2 = "select messageId, GROUP_CONCAT(DISTINCT userName SEPARATOR ', ') as receiverNames from inbox join user on receiverId = userId where messageId in (select m.messageId from message as m join outbox as o on m.messageId = o.messageId where o.senderId = " + 1 + " ) group by messageId;";
            // var Users = dbContext.User.FromSql(query2).ToList();
            



            ViewBag.ListEmails = Messages;
            // ViewBag.ListUsers = Users;
            // foreach (var item in DetailEmails)
            // {

            //     User user = dbContext.User.FirstOrDefault(x => x.UserId == item.IdRecevier);
            //     Email email = dbContext.Email.FirstOrDefault(x => x.EmailId == item.EmailId);
            //     Emails.Add(email);
            //     Users.Add(user);

            // }

            // ------------------


            // foreach (var item in Emails)
            // {
            //     foreach (var b in item.DetailEmails)
            //     {
            //         User user = dbContext.User.FirstOrDefault(x => x.UserId == b.IdRecevier);
            //         b.User = user;
            //     }
            // }

            var emailQuery = from o in dbContext.Outbox
                             join m in dbContext.Message
                             on o.messageId equals m.messageId
                             join i in dbContext.Inbox
                             on o.messageId equals i.messageId
                             where m.messageSenderId == userLoged.userId
                             select new
                             {
                                 messageId = m.messageId,
                                 messageContent = m.messageContent,
                                 receiverId = i.receiverId
                             };

            Console.WriteLine(emailQuery.ToList());
            // ViewBag.ListEmails = emailQuery.ToList();



            // ---------------------

            // List<DetailEmail> DetailEmails = new List<DetailEmail>();
            // List<Email> Emails = new List<Email>();
            // List<User> Users = new List<User>();
            // Emails = dbContext.Email.FromSql("select * from Email").ToList();


            // foreach (var item in Emails)
            // {

            //     DetailEmail detailEmail = dbContext.DetailEmail.FirstOrDefault(x => x.EmailId == item.EmailId);
            //     item.DetailEmails.Add(detailEmail);
            //     // foreach (var user in  item.DetailEmails)
            //     // {
            //     //     User newuser = dbContext.User.FirstOrDefault(x => x.UserId == user.IdRecevier);
            //     //     user.User = newuser;
            //     // }
            // }
            // foreach (var item in Emails)
            // {


            //     foreach (var user in  item.DetailEmails)
            //     {
            //         User newuser = dbContext.User.FirstOrDefault(x => x.UserId == user.IdRecevier);
            //         user.User = newuser;
            //     }
            // }
            //  foreach (var item in Emails)
            // {


            //     foreach (var user in  item.DetailEmails)
            //     {
            //        Console.WriteLine(user.User.UserName);

            //     }
            // }


            // ViewBag.ListEmails = DetailEmails;
            // ViewBag.ListEmails = Emails;
            return View();
        }

        // public IActionResult ViewEmailSent(int idEmail)
        // {
        //     var EmailSent = dbContext.Email.FromSql("select * from email where emailId =" + idEmail).FirstOrDefault();
        //     ViewBag.email = EmailSent;
        //     return View();
        // }


    }
}