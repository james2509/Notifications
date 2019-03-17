using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notifications.DataAccess.Entities
{
    public class NotificationTypeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int16 Id { get; set; }

        [Required]
        public string Description { get; set; }

        public List<NotificationEntity> Notifications { get; set; }

        public NotificationTemplateEntity NotificationTemplate { get; set; }
    }
}
