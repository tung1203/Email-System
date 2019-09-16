using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System;
namespace LoginSystem.Models
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Inbox> Inbox { get; set; }
        public DbSet<Outbox> Outbox { get; set; }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=127.0.0.1;uid=root;pwd=thanhtung1203;database=loginsystem");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.userId);
                entity.Property(e => e.userName);
                entity.Property(e => e.userPassword);


            });
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.messageId);
                entity.Property(e => e.messageTitle);
                entity.Property(e => e.messageContent);
                entity.Property(e => e.messageSenderId);
                entity.Property(e => e.messageSendDate);

            });
            modelBuilder.Entity<Inbox>(entity =>
            {
                entity.HasKey(e => e.inboxId);
                entity.Property(e => e.messageId);
                entity.Property(e => e.receiverId);
                entity.Property(e => e.isDeleted);

            });
            modelBuilder.Entity<Outbox>(entity =>
            {
                entity.HasKey(e => e.outboxId);
                entity.Property(e => e.messageId);
                entity.Property(e => e.senderId);
                entity.Property(e => e.isDeleted);
            });
            // modelBuilder.Entity<DetailEmail>(
            //     entity =>
            //     {
            //         entity.HasOne<Email>(s => s.Email)
            //         .WithMany(e => e.DetailEmails).HasForeignKey(s => s.EmailId);
            //         // entity.HasOne<User>(s => s.User)
            //         // .WithMany().HasForeignKey(s => s.IdSender).HasForeignKey(s => s.IdRecevier);
            //         // entity.HasOne<User>(s => s.User)
            //         // .WithMany().HasForeignKey(s => s.IdRecevier);
            //     }

            // );


        }
    }
    public class User
    {
        public User() { }
        public User(string username, string password)
        {
            userName = username;
            userPassword = password;
        }
        public int userId { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
       
    }
    public class Message
    {
        public Message() { } 
        public int messageId { get; set; }
        public string messageTitle { get; set; }
        public string messageContent { get; set; }
        public int? messageSenderId { get; set; }
        public DateTime messageSendDate { get; set; }

    }
    
    public class Inbox
    {
        public Inbox() { }
        public int inboxId { get; set; }
        public int messageId { get; set; }
        public int receiverId { get; set; }
        public byte isDeleted { get; set; }
       
     
    }
    public class Outbox
    {
        public Outbox() { }
        public int outboxId { get; set; }
        public int messageId { get; set; }
        public int? senderId { get; set; }
        public byte isDeleted { get; set; }
     
    }
    
}