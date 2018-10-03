using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareSoft.Repository.Models
{
   public class UserCountEntity
    {
        public Int64 TotalPatient { get; set; }
        public Int64 TotalDoctor { get; set; }
        public Int64 TotalUsers { get; set; }
    }
}
