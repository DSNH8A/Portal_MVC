using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModel
{
    public class UserViewModel
    {
        [Display(Name = "username")]
        [Required(ErrorMessage = "username is required")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }    
    }
}
