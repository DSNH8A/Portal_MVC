using MVC.Interface;
using MVC.Models;

namespace MVC.Interface
{
    public interface ISkillRepository
    {
        public Task<IEnumerable<Skill>> GetAll();

        public List<Skill> GetAllList();

        public Task<List<Skill>> GetAllSkillsAsync();

        public List<Skill> Skills();

        public List<Skill> GetAllUnique();

        public Task<Skill> GetByIdAsync(int id);

        public Skill GetById(int id);

        public Task<IEnumerable<Skill>> GetSkillsOfCourse(Course course);

        public Task<IEnumerable<Skill>> GetSkillsOfUser(User user);

        public void Add(Skill skill);

        public void Delete(Skill skill);
        public void Update(Skill skill);
        public void Save();

    }
}
