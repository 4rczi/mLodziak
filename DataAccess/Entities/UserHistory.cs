﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class UserHistory : BaseEntity
    {
        public int PhysicalLocationId { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public PhysicalLocation? PhysicalLocation { get; set; }


    }
}
