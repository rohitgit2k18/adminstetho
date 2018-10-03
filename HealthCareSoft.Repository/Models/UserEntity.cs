using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareSoft.Repository.Models
{
   public class UserEntity
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please Enter FirstName!")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddelName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Enter PostalCode!")]
        [Display(Name = "Postal Code")]
       // [StringLength(6, ErrorMessage = "The PostalCode must contains 6 characters!", MinimumLength = 6)]
        public Nullable<int> PostalCode { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile No")]
        [Display(Name = "Mobile No.")]
        [StringLength(15, ErrorMessage = "The Mobile must contains 10 characters!", MinimumLength = 10)]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address!")]
        [Display(Name = "Email ID")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please Enter Correct Email Address format!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Password!")]
        [Display(Name = "Password")]
        [StringLength(50, ErrorMessage = "The Password must contains 8 characters!", MinimumLength = 8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter Gender!")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Profile Image")]
        public string ProfilePicture { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Role")]
        public int RoleId { get; set; }

        public long CreatedBy { get; set; }
        [Display(Name = "Remember Me?")]
        public bool Rememberme { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long Otp { get; set; }
        public Nullable<int> UserPin { get; set; }
        public Nullable<long> BloodGroupId { get; set; }
        public Nullable<long> SpecializationId { get; set; }

        [NotMapped]
        public string DePassword { get; set; }
    }
}
