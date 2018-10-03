using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareSoft.Repository.Models
{
   public class DeptDdlEntity
    {
        public long DeptId { get; set; }

        [Display(Name = "Department Name")]
        public string DeptName { get; set; }
    }
}
