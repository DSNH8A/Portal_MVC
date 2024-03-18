namespace MVC.ViewModel 
{
    public class CreateCourseViewModel 
    {
        public int Id { get; set; } 
        public string Name { get; set; }    
        public string Description { get; set; }
        public IFormFile Image { get; set; }   
    }
}
