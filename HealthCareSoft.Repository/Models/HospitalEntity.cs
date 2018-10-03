using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareSoft.Repository.Models
{
  public  class HospitalEntity
    {
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Please Enter HospitalName!")]
        [Display(Name = "Hospital Name")]
        public string HospitalName { get; set; }

        [Required(ErrorMessage = "Please Enter Address!")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public bool IsAcctive { get; set; }
    }
}
