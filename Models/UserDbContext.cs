using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LoginSystem.Models
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Email> Email { get; set; }
        public DbSet<DetailEmail> DetailEmail { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=127.0.0.1;uid=root;pwd=thanhtung1203;database=loginsystem");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserName);
                entity.Property(e => e.UserPassword);


            });
            modelBuilder.Entity<Email>(entity =>
            {
                entity.HasKey(e => e.EmailId);
                entity.Property(e => e.EmailTitle);
                entity.Property(e => e.EmailContent);

            });
            modelBuilder.Entity<DetailEmail>(entity =>
            {
                entity.HasKey(e=>e.IdDetailEmail);
                entity.Property(e => e.EmailId);
                entity.Property(e => e.IdSender);
                entity.Property(e => e.IdRecevier);
              

            });
            modelBuilder.Entity<DetailEmail>()
                .HasOne<Email>(s => s.Email)
                .WithMany(e =>e.DetailEmails).HasForeignKey(s => s.EmailId);
        }
    }
    public class User
    {
        public User() { }
        public User(string username, string password)
        {
            UserName = username;
            UserPassword = password;
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }

    }
    public class Email
    {
        public Email() { }
        public int EmailId { get; set; }
        public string EmailTitle { get; set; }
        public string EmailContent { get; set; }
        
        public List<DetailEmail> DetailEmails { get; set; }

    }
    public class DetailEmail
    {
        public DetailEmail() { }
        public int IdDetailEmail { get; set; }
        public int EmailId { get; set; }
        public int? IdSender { get; set; }

        public int IdRecevier { get; set; }

        public Email Email { get; set; }
    }

}