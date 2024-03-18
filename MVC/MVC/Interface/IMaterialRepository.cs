using MVC.Models;


namespace MVC.Interface
{
    public interface IMaterialRepository
    {
        public Task<IEnumerable<Material>> GetAll();

        public List<Material> GetAllList();

        public List<Material> GetAllUnique();

        public Task<Material> GetBeyIdAsync(int id);

        public Material GetById(int id);

        public Task<IEnumerable<Material>> GetMaterialsOfCourse(Course course);

        public Task<IEnumerable<Material>> GetMaterialsOfUser(User user);

        public void Add(Material material);

        public void Delete(Material material);

        public void Update(Material material);

        public void Save();
    }
}
