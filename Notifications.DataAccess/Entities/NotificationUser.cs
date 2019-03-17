using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Notifications.DataAccess.Entities
{
    public class NotificationUser
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        public List<NotificationEntity> Notifications { get; set; }
    }
}
