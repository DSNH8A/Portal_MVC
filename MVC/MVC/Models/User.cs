using Microsoft.Build.Framework;
using MVC.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class User : Entity
    {
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 8, ErrorMessage = "The username must be minimum 8 character long.")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "A username is required.")]
        public string ?UserName { get; set; }

        [StringLength(20, MinimumLength = 8, ErrorMessage = "The password must be minimum 8 characters long.")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "A pasword is required.")]
        public string ?Password { get; set; }

        [StringLength(30,MinimumLength =8)]
        [Display(Name = "Email address")]
        public string ?Email { get; set; }   

        public bool ?isActive;

        [DataType(DataType.Date)]
        public DateTime dateOfJoining { get; set; }

        public ICollection<Course> ?AvailableCourses { get; set; } = null!;

        public ICollection<Skill> ?UserSkills { get; set; } = null!;

        public ICollection<Material> ?UserMaterials { get; set; } = null!;

        public List<Course> courses = new List<Course>();
    }
}
