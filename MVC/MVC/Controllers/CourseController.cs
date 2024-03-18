using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MVC.Data;
using MVC.Interface;
using MVC.Models;
using MVC.Repository;
using MVC.Services;
using MVC.Wrapper;
using Newtonsoft.Json.Linq;
using MVC.ViewModel;

namespace MVC.Controllers
{
    public partial class CourseController : Controller 
    {

        private readonly ICourseRepository _courseRepository;
        private readonly MVC_Context _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IPhotoService _photoService;

        public CourseController(MVC_Context context, ICourseRepository courseRepository, IMemoryCache memoryCache, IPhotoService photoService)
        {
            _courseRepository = courseRepository;
            _context = context;
            _memoryCache = memoryCache;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Course> courses = _memoryCache.Get<IEnumerable<Course>>("courses"); //await _courseRepository.GetAll();

            if (courses == null)
            {
                courses = await _courseRepository.GetAll();
                _memoryCache.Set("courses", courses);
            }

            if (ModelState.IsValid)
            {
                return View(courses);
            }

            else
            {
                return View("empty");
            }
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            Course course = await _courseRepository.GetByIdAsync(id);
            return View(course);
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateCourseViewModel course, List<Course> courses)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var result = await _photoService.AddPhotsAsync(course.Image);

            var newCourse = new Course
            {
                Name = course.Name,
                Description = course.Description,
                Image = result.Url.ToString()
            };

            _courseRepository.Add(newCourse);
            return View("Detail", newCourse);
            //return RedirectToAction()
        }

        public async Task<IActionResult> AddSkill(int id, int course)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            _courseRepository.AddSkillToCourse(id, course);
            Course currentCourse = await _courseRepository.GetByIdAsync(course);
            return RedirectToAction("Edit", currentCourse);
        }

        [HttpPost]
        public async Task<IActionResult> AddMaterial(int id, int course)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            _courseRepository.AddMaterialToCourse(id, course);
            Course currentCourse = await _courseRepository.GetByIdAsync(course);
            return RedirectToAction("Addmaterial", currentCourse);
        }

        [HttpGet]
        public IActionResult AddMaterial(Course course)
        {
            Console.WriteLine("devilsId: " + course.Id);
            //Course course = _courseRepository.GetById(ViewBag.integet);
            return RedirectToAction("Edit", course);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Course course = _courseRepository.GetById(id);  
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Course course)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            Console.WriteLine("Sanyis id: " + course.Id);
            //Course course = await _courseRepository.GetByIdAsync(course.Id);

            if (course == null)
            {
                Console.WriteLine("Szopottgombóc");
            }

            return RedirectToAction("Edit");
            //return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Nodelstate nem valid");
                return View("Index");
            }

            Course course = await _courseRepository.GetByIdAsync(id);
            _courseRepository.Delete(course);
            return RedirectToAction("Detail", "Home");
        }

        public async Task<IActionResult> Save(int id, CreateCourseViewModel newCourse)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var result = await _photoService.AddPhotsAsync(newCourse.Image);
            Course course = _courseRepository.GetAllList().Where(s => s.Id == id).FirstOrDefault();

            var name = newCourse.Name;
            var number = newCourse.Description;

            course.Description = number;
            course.Name = name;
            course.Image = result.Url.ToString(); 
            _courseRepository.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Finish(int id)
        {
            Console.WriteLine("Feriekeeeddd");

            MySession session = new MySession(HttpContext.Session);
            User user = session.GetUser();
            _courseRepository.FinishCourse(id, user);

           Course course =  _courseRepository.GetById(id);

            return RedirectToAction("Finish", user.Id);
        }

        [HttpGet]
        public IActionResult Finish()
        {
            Console.WriteLine("sanyikadddd");
            return RedirectToAction("Detail", "Home");
        }
    }

    public partial class CourseController : Controller 
    {
        /// <summary>
        /// Gets the list of all Courses.
        /// </summary>
        /// <returns>The list of Courses.</returns>
        [HttpGet]
        public async Task<IEnumerable<Course>> GetApi()
        {
            IEnumerable<Course> courses = await _courseRepository.GetAll();
            return courses;
        }
        /// <summary>
        /// Gets the course associated with the given id.
        /// </summary>
        /// <returns>The course specified.</returns>
        // GET: api/Course
        [HttpGet("id")]
        public async Task<Course> Get(int id)
        {
            Course course = await _courseRepository.GetByIdAsync(id);
            return course;
        }

        /// <summary>
        /// Creates a new course object.
        /// </summary>
        /// <returns>The course that was created.</returns>
        // GET: api/Course
        [HttpPost("course")]
        public Course AddApi(Course course)
        {
            _courseRepository.Add(course);
            return course;
        }

        /// <summary>
        /// Associates a skill object with a course object based on the courses id.
        /// </summary>
        /// <returns>The skills associated with the chosen course.</returns>
        // PATCH: api/Course
        [HttpPatch("skillId, course")]
        public List<Skill> AddSkillApi(int skillId, int course)
        {
            _courseRepository.AddSkillToCourse(skillId, course);
            Course currentCourse = _courseRepository.GetById(course);
            return _context.Skills.Where(s => s.CourseId == currentCourse.Id).ToList();
        }

        /// <summary>
        /// Associates a material object with a course object based on the courses id.
        /// </summary>
        /// <returns>The materials associated with the chosen course.</returns>
        // PATCH: api/Course
        [HttpPatch("materialId, course")]
        public List<Material> AddMaterialApi(int materialId, int course)
        {
            _courseRepository.AddMaterialToCourse(materialId, course);
            Course currentCourse = _courseRepository.GetById(course);
            return _context.Materials.Where(m => m.CourseId == currentCourse.Id).ToList();
        }

        /// <summary>
        /// Gets the course associated with the given id and saves any changes made.
        /// </summary>
        /// <returns>The course that was changed.</returns>
        // PATCH: api/Course
        [HttpPatch("id")]
        public async Task<Course> EditApi(int id, string name, string description)
        {
            Course course = await _courseRepository.GetByIdAsync(id);
            course.Name = name;
            course.Description = description;
            _courseRepository.Save();
            return course;
        }

        /// <summary>
        /// Sets the chosen courses progression property to 100.
        /// </summary>
        /// <returns>The finshed course.</returns>
        // PATCH: api/Course
        [HttpPatch("id, currentUser")]
        public Course FinishCourseApi(int id, User currentUser)
        {
            _courseRepository.FinishCourse(id, currentUser);
            return _context.courses.Where(c => c.Id == id).FirstOrDefault();
        }
    }
}