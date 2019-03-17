using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Notifications.DataAccess.Entities;

namespace Notifications.DataAccess
{
    public class NotificationsDbContext : DbContext
    {
        public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
            : base(options)
        { }

        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<NotificationTypeEntity> NotificationTypes { get; set; }
        public DbSet<NotificationTemplateEntity> NotificationTemplates { get; set; }
        public DbSet<NotificationUser> NotificationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationEntity>()
                .HasOne(n => n.NotificationType)
                .WithMany(n => n.Notifications);

            modelBuilder.Entity<NotificationEntity>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications);

            modelBuilder.Entity<NotificationTypeEntity>()
                .HasOne(nt => nt.NotificationTemplate)
                .WithOne(nt => nt.NotificationType)
                .HasForeignKey<NotificationTemplateEntity>(nt => nt.NotificationTypeId);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NotificationTypeEntity>().HasData(
                new NotificationTypeEntity { Description = "Appointment Cancelled", Id = 1 }
            );

            modelBuilder.Entity<NotificationTemplateEntity>().HasData(
                new NotificationTemplateEntity { Id = Guid.Parse("d9a2c131-202d-4bfb-b2ef-0b09a26d55d2"), NotificationTypeId = 1,
                    Body = "Hi {Firstname}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.",
                    Title = "Appointment Cancelled",
                    BuilderClass = "AppointmentCancelledNotificationBuilder"
                }
            );

            modelBuilder.Entity<NotificationUser>().HasData(
                new NotificationUser { Email = "james@test.com", Id = Guid.Parse("512ac9b0-8a26-4666-9e9a-bda84f67b782"), FirstName = "James" },
                new NotificationUser { Email = "natalia@test.com", Id = Guid.Parse("95c34d6e-06c5-41d8-a623-835c5ae39032"), FirstName = "Natalia"}
            );
        }
    }
}
