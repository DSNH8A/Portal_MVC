using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Interface;
using MVC.Models;
using MVC.Wrapper;

namespace MVC.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly MVC_Context _context;
        private MySession session;

        public CourseRepository(MVC_Context context) 
        {
            _context = context; 
        }

        public void Add(Course course)
        {
            _context.courses.Add(course);
            _context.SaveChanges();
        }

        public void Delete(Course course)
        {
            _context.Remove(course);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Course>> GetAll()
        {
            return await _context.courses.ToListAsync();
        }

        public List<Course> GetAllList()
        {
            return _context.courses.ToList();    
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _context.courses.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public Course GetById(int id)
        {
            return _context.courses.Where(c => c.Id == id).FirstOrDefault();
        }

        public async Task<IEnumerable<Course>> GetCoursesByUser(int id)
        {
            return await _context.courses.Where(c => c.UserID == id).ToListAsync(); 
        }

        public async Task AddSkillToCourse(int id, int courseId)
        {
            Skill skill = _context.Skills.Where(s => s.Id == id).FirstOrDefault();
            List<Skill> skills = _context.Skills.ToList();

            foreach (var item in skills)
            {
                if (item.CourseId == courseId && item.Name == skill.Name)
                {
                    Console.WriteLine("You already have this skill.");
                    return;    
                }
            }

            if (skill.CourseId != null)
            {
                Skill newSkill = new Skill()
                {
                    Name = skill.Name,
                    SkillLevel = 0,
                    CourseId = courseId,
                };

                _context.Skills.Add(newSkill);
                _context.SaveChanges();
            }

            else
            {
                skill.CourseId = courseId;
            }
            _context.SaveChanges();
        }

        public async Task AddMaterialToCourse(int id, int courseId)
        {
            Material material = _context.Materials.Where(m => m.Id == id).FirstOrDefault();
            List<Material> materials = _context.Materials.ToList();

            foreach (var item in materials)
            {
                if (item.CourseId == courseId && item.Name == material.Name)
                {
                    Console.WriteLine("You already have thois item.");
                    return;    
                }
            }
            if (material.CourseId != null)
            {
                Material newMaterial = new Material()
                {
                    Name = material.Name,
                    CourseId = courseId,
                    Author = material.Author,
                    DateOfPublication = material.DateOfPublication,
                    Duration = material.Duration,
                    Format = material.Format,
                    NumberOgpages = material.NumberOgpages,
                    Quality = material.Quality,
                    TypeOfDatacarrier = material.TypeOfDatacarrier, 
                    YearOfPublication = material.YearOfPublication,
                };

                _context.Materials.Add(newMaterial);
                _context.SaveChanges();
            }

            else
            {
                material.CourseId = courseId;
            }
            _context.SaveChanges();

        }

        public async Task<Skill> AddSkill(int id, Course course)
        {
          return await _context.Skills.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Course course)
        {
            throw new NotImplementedException();
        }

        public List<Material> GetCoursesMaterials(int courseId)
        {
            List<Material> materials = _context.Materials.ToList();
            List<Material> coursesMaterials = new List<Material>();

            for (int i = 0; i < materials.Count; i++)
            {
                if (materials[i].CourseId == courseId)
                {
                    coursesMaterials.Add(materials[i]);
                }
            }

            return coursesMaterials;   
        }

        public List<Skill> GetCoursesSkills(int courseId)
        {
            List<Skill> skills = _context.Skills.ToList();
            List<Skill> coursesSkills = new List<Skill>();

            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].CourseId == courseId)
                {
                    coursesSkills.Add(skills[i]);
                }
            }

            return coursesSkills;
        }

        public void FinishCourse(int courseId, User user)
        {
            Console.WriteLine("CourseID: " + courseId);
            Console.WriteLine("UserID: " + user.Id);
            Course course = _context.courses.Where(c => c.Id == courseId).FirstOrDefault();
            course.ProgressInPercentage = 100.00;

            List<Skill> skills = GetCoursesSkills(courseId);
            Console.WriteLine("skillsNumber: " + skills.Count);
            List<Material> materials = GetCoursesMaterials(courseId);
            Console.WriteLine("materialsNumber: " + materials.Count);

            List<Skill> newSkills = new List<Skill>();

            int userid = user.Id;

            for (int i = 0; i < skills.Count; i++)
            {
                Skill skill = new Skill()
                {
                    Name = skills[i].Name,
                };

                skill.OwnerId = userid;
                newSkills.Add(skill);
            }

            _context.Skills.AddRange(newSkills);
            _context.SaveChanges();
           
            foreach (var item in materials)
            {
                _context.Materials.Add(new Material
                {
                    Name = item.Name,
                    NumberOgpages = item.NumberOgpages,
                    DateOfPublication = item.DateOfPublication,
                    Duration = item.Duration,
                    TypeOfDatacarrier = item.TypeOfDatacarrier,
                    Author = item.Author,
                    Quality = item.Quality,
                    Format = item.Format,
                    YearOfPublication = item.YearOfPublication,
                    OwnerId = userid
                });
                _context.SaveChanges();
            }

            _context.SaveChanges();
        }
    }
}