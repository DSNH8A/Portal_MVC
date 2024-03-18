using Microsoft.Build.Framework;
using MVC.BaseEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    public class Material : Entity
    {
        public int Id { get; set; } 

        public int ?CourseId { get; set; }

        public int ?OwnerId { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Name { get; set; }

        //Electronic copies
        public string ?Author { get; set; }

        public int ?NumberOgpages { get; set; }

        public string ?Format { get; set; }

        public DateTime ?YearOfPublication { get; set; }

        //OnlineArticles

        public DateTime ?DateOfPublication { get; set; }

        public string ?TypeOfDatacarrier { get; set; }

        //VideoMaterial
        public float ?Duration { get; set; }

        public string ?Quality { get; set; }
    }
}
