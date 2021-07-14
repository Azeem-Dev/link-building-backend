using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace link_building.Dtos.User
{
    public class UserPasswordUpdateDto
    {
        public string username { get; set; }
        public string password { get; set; }
        public string NewPassword { get; set; }
    }
}
