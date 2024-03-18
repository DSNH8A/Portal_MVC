using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Interface;
using MVC.Models;

namespace MVC.Repository
{
    public class MaterialRepository : IMaterialRepository
    {

        private readonly MVC_Context _context;

        public MaterialRepository(MVC_Context context) 
        {
            _context = context; 
        }

        public void Add(Material material)
        {
            List<Material> materials = _context.Materials.ToList();

            foreach (var item in materials)
            {
                if (item.Name == material.Name)
                {
                    return;
                }
            }

            _context.Materials.Add(material);
            _context.SaveChanges();
        }

        public void Delete(Material material)
        {
            _context.Materials.Remove(material);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Material>> GetAll()
        {
            return await _context.Materials.ToListAsync();
        }

        public List<Material> GetAllUnique() 
        {
            List<Material> materials = _context.Materials.ToList();

            for (int i = 0; i < materials.Count; i++)
            {
                for (int j = i+1; j < materials.Count; j++)
                {
                    if (materials[i].Name == materials[j].Name)
                    {
                        materials.Remove(materials[j]);    
                    }
                }
            }
            
            return materials;
        }

        public List<Material> GetAllList()
        {
            return _context.Materials.ToList();    
        }

        public async Task<Material> GetBeyIdAsync(int id)
        {
            return await _context.Materials.Where(m => m.Id == id).FirstOrDefaultAsync();
        }

        public Material GetById(int id)
        {
            return _context.Materials.Where(m => m.Id == id).FirstOrDefault();        
        }

        public async Task<IEnumerable<Material>> GetMaterialsOfCourse(Course course)
        {
            //return await _context.Materials.Where(m => m.CourseId == course.Id).ToListAsync();
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Material>> GetMaterialsOfUser(User user)
        {
            //return await _context.Materials.Where(m => m.UserID == user.Id).ToListAsync();
            throw new NotImplementedException();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Material material)
        {
            throw new NotImplementedException();
        }
    }
}
