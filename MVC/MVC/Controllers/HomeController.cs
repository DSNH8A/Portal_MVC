using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MVC.Data;
using MVC.Extensions;
using MVC.Interface;
using MVC.Migrations;
using MVC.Models;
using MVC.Repository;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Packaging;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using MVC.Wrapper;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace MVC.Controllers
{
    public partial class HomeController : Controller 
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ICourseRepository _courseRepository;
        public MySession session;

        public HomeController(ILogger<HomeController> logger, MVC_Context context, IUserRepository userRepository, ICourseRepository courseRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
            _courseRepository = courseRepository;

        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("modelstate nem valid");
                return View(user);
            }

            var userr = await _userRepository.GetByUserNameAsync(user.UserName);
               
            session = new MySession(HttpContext.Session);
            session.SetUser(userr);

            if (userr != null)
            {
                // var passwordCheck = _userRepository.Login(userr);
                var passwordCheck = _userRepository.Login(userr);

                //we have a user
                if (passwordCheck == true)
                {
                    if (userr.isActive == false || userr.isActive == null)
                    {
                        userr.isActive = true;
                    }

                    foreach (var item in _userRepository.ActiveUsersList())
                    {
                        if (item == userr)
                        {
                            Console.WriteLine("Ugye nema aloginba megyünk?");
                            //Console.WriteLine("Aktív felhasználók száma: " + users.Count);
                            return RedirectToAction("Login");
                        }
                    }
                }

                if (passwordCheck == false)
                {
                    Console.WriteLine("Valami szar van a palcsintában.");
                    return RedirectToAction("Index");
                }
            }

            else 
            {
                return default;
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            session = new MySession(HttpContext.Session);

                if (session.GetUser() != null)
                {
                    Console.WriteLine("Detailre mwgyünk");

                    return View("Detail", session.GetUser());
                }

                else
                {
                    Console.WriteLine("nem detailre megyünk");
                    return View();
                }

            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            MySession session = new MySession(HttpContext.Session);
            User user = session.GetUser();

            Console.WriteLine("Ez visz a kezdő oldalra");

            if (user != null)
            {
                return View(user);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            return RedirectToAction("Index");
        }

        public async Task GoogleLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Result")
                });
        }

        public async Task GitLogin()
        {
            await HttpContext.ChallengeAsync("github", new AuthenticationProperties
            {
                RedirectUri = Url.Action("Result")
            });
        }

        public async Task FacebookLogin()
        {
            await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("Result")
            });
        }

        public async Task<IActionResult> Result()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var facebookAuthenticateResult = HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            var googleAuthenticationResult = HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var gitAuthenticationResult = HttpContext.AuthenticateAsync("github");

            var emailClaim = facebookAuthenticateResult.Result.Principal.FindFirst(ClaimTypes.Email);  //Principal.FindFirst(ClaimTypes.Email);
            var emailClaimGoogle = googleAuthenticationResult.Result.Principal.FindFirst(ClaimTypes.Email);
            var emailClaimGithub = gitAuthenticationResult.Result.Principal.FindFirst(ClaimTypes.Email);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            User user = await _userRepository.GetByEmail(emailClaim.Value.ToString());
            Console.WriteLine(emailClaim.Value.ToString());

            if (user == null)
            {
                User newUser = new User
                {
                    Email = emailClaim.Value.ToString()
                };

                _userRepository.Add(newUser);

                //session = new MySession(HttpContext.Session);
                //session.SetUser(newUser);
                IUserRepository.loggedInUser = newUser;
                //return Json(claims);
                Console.WriteLine(emailClaim.Value.ToString());
                return View("Detail", newUser);
            }

            IUserRepository.loggedInUser = user;
            return View("Detail", user);
        }

        public async Task<IActionResult> Detail(User u)
        {
            Console.WriteLine("Bejutottunk a detailbe");
            User user = HttpContext.Session.GetObject<User>("user");
            if (user != null)
            {
                return View(user);
            }

            else
            {
                return View(user);
                //return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public IActionResult Add(User user)
        {
            ViewBag.Action = "Add";  

            if (!ModelState.IsValid)
            {
                return View("Login");
            }

            List<User> users = _userRepository.GetAllList();

            foreach (var item in users)
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

            user.dateOfJoining = DateTime.Now;
            _userRepository.Add(user);
            session = new MySession(HttpContext.Session);
            session.SetUser(user);
            return RedirectToAction("Add");
        }

        [HttpGet]
        public IActionResult Add()
        {
            session = new MySession(HttpContext.Session);
            return View("Detail", session.GetUser());
        }

        public IActionResult Create()
        {
            return View("Create", session.GetUser());
        }

        //public IActionResult Create(User user)
        //{
        //    //if (!ModelState.IsValid)
        //    //{

        //    //}

        //    _userRepository.Add(user);
        //    return View(user);

        //    //return View("Index");
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult AddCourse()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCourse(int id)
        {
            if (!ModelState.IsValid)
            {
                return View("Index"); 
            }

            session = new MySession(HttpContext.Session);  
            User user = session.GetUser();

            _userRepository.AddCourseToUser(id, user.Id);
            return RedirectToAction("Detail", id);
        }

        public IActionResult Clear()
        {
            session = new MySession(HttpContext.Session);
            User user = session.GetUser();
            //Console.WriteLine(user.UserName);
            session.ClearSession();
            //Console.WriteLine("SessionCleared");
            return View("Login");
        }
    }

    public partial class HomeController 
    {
        /// <summary>
        /// Initializes a new user with the given data and adds it to the users list.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="eMail"></param>
        /// <returns>The vreated user object</returns>
        [HttpPost("userName, password, email")]
        public User AddApi(string userName, string password, string eMail)
        {
            User newUser = new User
            {
                UserName = userName,
                Password = password,
                Email = eMail
            };

            _userRepository.Add(newUser);
            return newUser;
        }

        /// <summary>
        /// Adds a course to a user object.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns>Thr course that was given to the user.</returns>
        [HttpPatch("id, userId")]
        public Course AddCourseToUserApi(int id, int userId)
        {
            _userRepository.AddCourseToUser(id, userId);
            List<Course> courses = _userRepository.GetCoursesList(userId);
            Course course = courses.Where(c => c.Id == id).FirstOrDefault();
            Console.WriteLine(course.UserID);
            return course;
            //return _userRepository.GetCoursesList(id);
        }

        /// <summary>
        /// Deletes the user object associated with the given id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>The user list without the deleted user.</returns>
        [HttpDelete("userId")]
        public List<User> DeleteApi(int userId)
        {
            User user = _userRepository.GetById(userId);
            _userRepository.Delete(user);

            return _userRepository.GetAllList();
        }

        [HttpGet("userId")]
        public User DetailApi(int id)
        {
            //User user = _userRepository.LoggedinUser();
            User user = _userRepository.GetById(id);
            if (user != null)
            {
                return user;
            }

            else
            {
                return default;
            }
        }

        [HttpPatch("userName, password")]
        public List<User> LoginApi(string userName, string password)
        {
            var userr = _userRepository.GetByUserName(userName);

            if (userr != null)
            {
                var passwordCheck = _userRepository.Login(userr);

                //we have a user
                if (passwordCheck == true)
                {
                    //IUserRepository.loggedInUser = userr;

                    if (userr.isActive == false || userr.isActive == null)
                    {
                        userr.isActive = true;
                    }

                    Console.WriteLine("van aktyv cucc");
                    return _userRepository.ActiveUsersList();
                }

                if (passwordCheck == false)
                {
                    Console.WriteLine("nincs aktív cucc");
                    return _userRepository.ActiveUsersList();
                }
            }

            return default;
        }
    }
}