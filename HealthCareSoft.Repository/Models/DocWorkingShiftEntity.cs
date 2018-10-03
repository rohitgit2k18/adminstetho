using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareSoft.Repository.Models
{
  public  class DocWorkingShiftEntity      
    {
        public int Id { get; set; }
        public string Weakdayname { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
 
        //public List<DocWorkingShiftEntity> GetAllWorkingShift { get; set; }
    }
   
}
