using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.Models.ArticleDTOs
{
    public class NewsArticleDto
    {
        public string NewsArticleId { get; set; }
        public string NewsTitle { get; set; }
        public string Headline { get; set; }
        public string NewsContent { get; set; }
        public string NewsSource { get; set; }
        public bool NewsStatus { get; set; }
        public int CategoryId { get; set; }
        public short CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<int> TagIds { get; set; }
    }
}
