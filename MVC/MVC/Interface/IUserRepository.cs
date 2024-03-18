using MVC.Models;

namespace MVC.Interface
{
    public interface IUserRepository
    {
        public static User loggedInUser;

        public Task<List<User>> ActiveUsers();

        public List<User> ActiveUsersList();
        public List<User> GetAllUsers();

        public User LoggedinUser();
        public Task<User> GetByUserNameAsync(string userName);
        public Task<User> GetByPasswordAsync(string password);
        public User GetByUserName(string userName);
        public User GetByPassword(string password);

        public Task<User> GetByEmail(string email);
        public Task<IEnumerable<Course>> GetCourses(int id);

        public Task<User> GetByIdAsync(int id);

        public User GetById(int id);

        public List<Course> GetCoursesList(int id);

        public List<Material> GetUsersMaterials(int id);

        public List<Skill> GetUsersSkills(int id);

        public Task<IEnumerable<User>> GetAll();

        public List<User> GetAllList();

        public bool Login(string userName, string password);

        public bool Login(User user);

        public void AddCourseToUser(int id, int userID);

        public List<Skill> SortSkills(List<Skill> skills);

        public List<Material> SortMaterials(List<Material> materials);

        public void Add(User user);

        public void Delete(User user);
    }
}
