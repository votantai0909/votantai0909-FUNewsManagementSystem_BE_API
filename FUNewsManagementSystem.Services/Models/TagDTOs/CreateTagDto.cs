using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.Models.TagDTOs
{
    public class CreateTagDto
    {
        public string? TagName { get; set; }
        public string? Note { get; set; }
    }
}
