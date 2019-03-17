using System;
using System.Collections.Generic;
using System.Text;

namespace Notifications.Common.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }
    }
}
