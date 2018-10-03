using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareSoft.Repository.Models
{
   public class EnquiryEntity
    {
        public long Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }

        [Display(Name = "Status")]
        public bool IsAnswered { get; set; }
        public string Answer { get; set; }
    }
}
