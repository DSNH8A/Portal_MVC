using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Interface;
using MVC.Models;

namespace MVC.Repository
{
    public class SkillRepository : ISkillRepository {

        private readonly MVC_Context _context;

        public SkillRepository(MVC_Context context)
        {
            _context = context;
        }

        public void Add(Skill skill)
        {
            List<Skill> skills = _context.Skills.ToList();
            foreach (var item in skills)
            {
                if (item.Name == skill.Name)
                {
                    return;
                }
            }

            _context.Add(skill);
            _context.SaveChanges();
        }

        public void Delete(Skill skill)
        {
            _context.Skills.Remove(skill);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Skill>> GetAll()
        {
            return await _context.Skills.ToListAsync();
        }

        public List<Skill> GetAllUnique()
        {
            List<Skill> skills = _context.Skills.ToList();

            for (int i = 0; i < skills.Count; i++)
            {
                for (int j = i + 1; j < skills.Count; j++)
                {
                    if (skills[i].Name == skills[j].Name)
                    {
                        skills.Remove(skills[j]);
                    }
                }
            }

            return skills;
        }

        public List<Skill> GetAllList()
        {
            List<Skill> skills = _context.Skills.ToList();
            return skills;
        }

        public List<Skill> Skills()
        {
            return GetAllSkillsAsync().Result;
        }

        public async Task<List<Skill>> GetAllSkillsAsync()
        {
            return await _context.Skills.ToListAsync();
        }

        public async Task<Skill> GetByIdAsync(int id)
        {
            return await _context.Skills.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public Skill GetById(int id)
        {
            return _context.Skills.Where(s => s.Id == id).FirstOrDefault();
        }

        public async Task<IEnumerable<Skill>> GetSkillsOfCourse(Course course)
        {
            //return await _context.Skills.Where(s => s.CourseId == course.Id).ToListAsync();
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Skill>> GetSkillsOfUser(User user)
        {
            //return await _context.Skills.Where(s => s.UserId == user.Id).ToListAsync();
            throw new NotImplementedException();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async void SaveAsync()
        {
            await _context.SaveChangesAsync();    
        }

        public void Update(Skill skill)
        {
            throw new NotImplementedException();
        }
    }
}
