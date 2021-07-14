using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace link_building.Dtos.User
{
    public class UserRoleUpdateDto
    {
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public string NewRole { get; set; }

    }
}
