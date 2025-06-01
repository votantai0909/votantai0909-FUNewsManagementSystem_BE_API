﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.Models.AccoutDTOs
{
    public class SystemAccountDto
    {
        public short AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountPassword { get; set; }
        public string AccountEmail { get; set; }
        public string AccountRole { get; set; }
    }
}
