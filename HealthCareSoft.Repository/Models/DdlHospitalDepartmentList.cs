using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareSoft.Repository.Models
{
  public  class DdlHospitalDepartmentList
    {
        public long DeptId { get; set; }

        public long HospitalId { get; set; }

        [Display(Name = "Department Name")]
        public string DeptName { get; set; }

        [Display(Name = "Hospital Name")]
        public string HospitalName { get; set; }
    }
}
