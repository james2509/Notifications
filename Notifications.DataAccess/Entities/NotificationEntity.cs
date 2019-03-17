using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Notifications.Common.Models;

namespace Notifications.DataAccess.Entities
{
    public class NotificationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public NotificationTypeEntity NotificationType { get; set; }

        [Required]
        public NotificationUser User { get; set; }

        [Required]
        public string NotificationData { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
    }
}
