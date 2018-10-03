using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareSoft.Repository.Models
{
   public class HospitalDoctorEntity
    {
        public long Id { get; set; }
        [Display(Name = "Doctors")]
        public long DoctorId { get; set; }
        public long HospitalId { get; set; }
        [Display(Name = "Department")]
        public long DepartmentId { get; set; }
    }
}
