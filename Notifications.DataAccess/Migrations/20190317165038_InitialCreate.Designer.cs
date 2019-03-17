﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notifications.DataAccess;

namespace Notifications.DataAccess.Migrations
{
    [DbContext(typeof(NotificationsDbContext))]
    [Migration("20190317165038_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Notifications.DataAccess.Entities.NotificationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("NotificationData")
                        .IsRequired();

                    b.Property<short>("NotificationTypeId");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("NotificationTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Notifications.DataAccess.Entities.NotificationTemplateEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<string>("BuilderClass")
                        .IsRequired();

                    b.Property<short>("NotificationTypeId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("NotificationTypeId")
                        .IsUnique();

                    b.ToTable("NotificationTemplates");

                    b.HasData(
                        new { Id = new Guid("d9a2c131-202d-4bfb-b2ef-0b09a26d55d2"), Body = "Hi {Firstname}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.", BuilderClass = "AppointmentCancelledNotificationBuilder", NotificationTypeId = (short)1, Title = "Appointment Cancelled" }
                    );
                });

            modelBuilder.Entity("Notifications.DataAccess.Entities.NotificationTypeEntity", b =>
                {
                    b.Property<short>("Id");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("NotificationTypes");

                    b.HasData(
                        new { Id = (short)1, Description = "Appointment Cancelled" }
                    );
                });

            modelBuilder.Entity("Notifications.DataAccess.Entities.NotificationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("NotificationUsers");

                    b.HasData(
                        new { Id = new Guid("512ac9b0-8a26-4666-9e9a-bda84f67b782"), Email = "james@test.com", FirstName = "James" },
                        new { Id = new Guid("95c34d6e-06c5-41d8-a623-835c5ae39032"), Email = "natalia@test.com", FirstName = "Natalia" }
                    );
                });

            modelBuilder.Entity("Notifications.DataAccess.Entities.NotificationEntity", b =>
                {
                    b.HasOne("Notifications.DataAccess.Entities.NotificationTypeEntity", "NotificationType")
                        .WithMany("Notifications")
                        .HasForeignKey("NotificationTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Notifications.DataAccess.Entities.NotificationUser", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Notifications.DataAccess.Entities.NotificationTemplateEntity", b =>
                {
                    b.HasOne("Notifications.DataAccess.Entities.NotificationTypeEntity", "NotificationType")
                        .WithOne("NotificationTemplate")
                        .HasForeignKey("Notifications.DataAccess.Entities.NotificationTemplateEntity", "NotificationTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}