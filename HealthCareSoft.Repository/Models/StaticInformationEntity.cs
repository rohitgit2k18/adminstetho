using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareSoft.Repository.Models
{
   public class StaticInformationEntity
    {
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Content")]
        public string Content { get; set; }
    }
}
