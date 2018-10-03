using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareSoft.Repository.Models
{
  public  class DepartmentEntity
    {
        public long Id { get; set; }
        [Display(Name = "Department Name")]
        public string Name { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }
}
