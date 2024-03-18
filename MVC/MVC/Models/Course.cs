using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MVC.BaseEntity;

namespace MVC.Models
{
    public class Course : Entity
    {
        public int Id { get; set; }

        //[ForeignKey("User")]
        public int? UserID { get; set; }
        //public User? User { get; set; } 

        public string ?Name { get; set; }

        [StringLength(200)]
        public string ?Description { get; set; }

        public double ProgressInPercentage { get; set; }

        public string? Image { get; set; }

        //[ForeignKey("Skill")]
        public int ?SkillId { get; set; }
       // public Skill? Skill { get; set; }

       // [ForeignKey("Material")]
        public int ?MaterialId { get; set; }
        //public Skill? Material { get; set; }
    }
}
