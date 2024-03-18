using Microsoft.Build.Framework;
using MVC.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Skill : Entity 
    {
        public int Id { get; set; }

        public int? CourseId { get; set; }

        public int? OwnerId { get; set; }

        public int SkillLevel { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Name { get; set; }

    }
}
