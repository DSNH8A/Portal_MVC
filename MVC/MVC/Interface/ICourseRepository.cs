using MVC.Models;

namespace MVC.Interface
{
    public interface ICourseRepository
    {
        public Task<IEnumerable<Course>> GetAll();

        public List<Course> GetAllList();

        public Task<Course> GetByIdAsync(int id);

        public Course GetById(int id);

        public Task<IEnumerable<Course>> GetCoursesByUser(int id);

        public Task AddSkillToCourse(int id, int courseId);

        public Task AddMaterialToCourse(int id, int materialId);

        public List<Material> GetCoursesMaterials(int courseId);

        public List<Skill> GetCoursesSkills(int courseId);

        public void FinishCourse(int courseId, User user);

        public void Add(Course course);

        public void Delete(Course course);

        public void Update(Course course);

        public void Save();
    }
}
