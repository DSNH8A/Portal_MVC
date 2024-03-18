using Microsoft.AspNetCore.Mvc;
using MVC.Data;
using MVC.Interface;
using MVC.Migrations;
using MVC.Models;
using MVC.ViewModel;
using MVC.Wrapper;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(MVC_Context context, IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            var users = _userRepository.GetAllList();

            if (ModelState.IsValid)
            {
                return View(users);   
            }
           
            return View();
        }

        public IActionResult Login(User user)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            User storedUsers = _userRepository.GetAllList().Where(u => u.Password == user.Password).FirstOrDefault();
            return RedirectToAction("Create");
        }

        public IActionResult Create(User user) 
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            foreach (var item in _userRepository.GetAllList()) 
            {
                if (item.Password == user.Password)
                {
                    Console.WriteLine();    
                }

                if (item.UserName == user.UserName)
                {
                    Console.WriteLine();
                }
            }

            _userRepository.Add(user);
            MySession session = new MySession(HttpContext.Session);
            session.SetUser(user);  
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            User user = await _userRepository.GetByIdAsync(id);
            return View(user);
        }
    }
}
