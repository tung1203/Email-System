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

            User userLoged = dbContext.User.FirstOrDefault(user => user.UserId == userId);
            ViewBag.user = userLoged;

            var emailQuery = from de in dbContext.DetailEmail
                             join e in dbContext.Email
                             on de.EmailId equals e.EmailId
                             join u in dbContext.User
                             on de.IdRecevier equals u.UserId
                             where de.IdSender == userLoged.UserId
                             select new EmailView
                             {
                                 EmailVm = e,
                                 UserVm = u

                             };

            ViewBag.ListEmails = emailQuery.ToList();

            return View();
        }

        public IActionResult ViewEmailSent(int idEmail)
        {
            var EmailSent = dbContext.Email.FromSql("select * from email where emailId =" + idEmail).FirstOrDefault();
            ViewBag.email = EmailSent;
            return View();
        }


    }
}