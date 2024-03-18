using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MVC.Data;
using MVC.Interface;
using MVC.Models;
using MVC.Repository;
using System.Collections;
using System.ComponentModel;

namespace MVC.Controllers
{
    public partial class MaterialController : Controller
    {
        private readonly IMaterialRepository _materialRepository;
        private ICourseRepository _courseRepository;
        private readonly IMemoryCache _memoryCache; 

        public MaterialController(MVC_Context context, IMaterialRepository materialRepository, ICourseRepository courseRepository
            , IMemoryCache memoryCache) 
        {
            _materialRepository = materialRepository;
            _courseRepository = courseRepository;
            _memoryCache = memoryCache; 
        } 

        public async Task<IActionResult> Index()
        {
            IEnumerable<Material> materials = _memoryCache.Get<IEnumerable<Material>>("materials"); //await _materialRepository.GetAll();

            if(materials == null)
            {
                materials = await _materialRepository.GetAll();
                _memoryCache.Set("materials", materials);
            }

            if (ModelState.IsValid)
            {
                return View(materials);
            }

            else 
            {
                return RedirectToAction("Empty");    
            }
        }

        public IActionResult Empty()
        {
            return RedirectToAction("Add");    
        }

        public IActionResult Create(string type)
        {
            if (type == "ElectronicCopies")
            {
                return View("CreateElectronicCopy");
            }
            if(type == "OnlineArticle")
            {
                return View("CreateOnlineArticle"); 
            }
            if (type == "VideoMaterial")
            {
                return View("CreateVideoMaterial");
            }

            return View(Index);
        }

        public async Task<IActionResult> Add(Material material)
        {
            if (!ModelState.IsValid)
            {
                return View("Index"); 
            }

            _materialRepository.Add(material);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            Material material = await _materialRepository.GetBeyIdAsync(id);
            return View(material);    
        }
        /// <summary>
        /// Deletes the material associated with the given id.
        /// </summary>
        /// <returns>The page where the course is displayed.</returns>
        // DELETE: api/material/delete
        [HttpDelete("{id, course}")]
        public async Task<IActionResult> Delete(int id, int course)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");     
            }

            Material material = await _materialRepository.GetBeyIdAsync(id);
            Course c = await _courseRepository.GetByIdAsync(course);
            _materialRepository.Delete(material);
            return RedirectToAction("Index", course);
        }

        public async Task<IActionResult> Update(int id)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");     
            }

            _materialRepository.Save();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");     
            }

            Material material = await _materialRepository.GetBeyIdAsync(id);
            return View(material);
        }

        public async Task<IActionResult> Save(int id, Material newMaterial)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");     
            }

            Material material = await _materialRepository.GetBeyIdAsync(id);

            var name = newMaterial.Name;
            var aouthor = newMaterial.Author;
            var numberOfpages = newMaterial.NumberOgpages;
            var format = newMaterial.Format;    
            var yearOfPublication = newMaterial.YearOfPublication;
            var dateOfPublication = newMaterial.DateOfPublication;
            var typeOfDataCarrier = newMaterial.TypeOfDatacarrier;
            var duration = newMaterial.Duration;
            var Quality = newMaterial.Quality;

            material.Name = name;
            material.Author = aouthor;
            material.NumberOgpages = numberOfpages;
            material.Format = format;
            material.YearOfPublication = yearOfPublication;
            material.DateOfPublication = dateOfPublication; 
            material.TypeOfDatacarrier = typeOfDataCarrier;
            material.Duration = duration;   
            material.Quality = Quality;
            _materialRepository.Save();
            return RedirectToAction("Index");
        }
    }

    public partial class MaterialController 
    {
        /// <summary>
        /// Gets all material object.
        /// </summary>
        /// <returns>All materials.</returns>
        // GET: api/Course
        [HttpGet("materialId")]
        public Material GetMaterialApi(int materialId)
        {
            return _materialRepository.GetById(materialId);
        }

        /// <summary>
        /// Gets all material object.
        /// </summary>
        /// <returns>All materials.</returns>
        // GET: api/Course/feri
        [HttpGet("allname")]
        public IEnumerable<Material> GetAllApi()
        {
            return _materialRepository.GetAllList();
        }

        /// <summary>
        /// Creates a new ElectronicCopies.
        /// </summary>
        /// <returns>The created object.</returns>
        // POST: api/Course
        [HttpPost("author, numberOfPages, format, yearOfPublication")]
        public Material CreateElectronicCopyApi(string author, int numberOfPages, string format, DateTime yearOfPublication)
        {
            return new ElectronicCopies
            {
                Author = author,
                NumberOgpages = numberOfPages,
                Format = format,
                YearOfPublication = yearOfPublication
            };
        }

        /// <summary>
        /// Creates a new OnlineArticle object.
        /// </summary>
        /// <returns>The created object.</returns>
        // POST: api/Course
        [HttpPost("dateOfPublication, typeOfDateCarrier")]
        public Material CreateOnlineArticleApi(DateTime dateOfPublication, string typeOfDateCarrier)
        {
            return new OnlineArticle
            {
                DateOfPublication = dateOfPublication,
                TypeOfDatacarrier = typeOfDateCarrier
            };
        }

        /// <summary>
        /// Creates a new VideoMaterial object.
        /// </summary>
        /// <returns>The created object.</returns>
        // POST: api/Course
        [HttpPost("duration, quality")]
        public Material CreateVideoMaterialApi(float duration, string quality)
        {
            return new VideoMaterial
            {
                Duration = duration,
                Quality = quality
            };
        }

        /// <summary>
        /// Adds a new Material to the list.
        /// </summary>
        /// <returns>The created object.</returns>
        // POST: api/Course
        [HttpPost("material")]
        public Material AddApi(Material material)
        {
            _materialRepository.Add(material);
            return material;
        }

        /// <summary>
        /// Updates the Material object associated with th given id.
        /// </summary>
        /// <returns>The updated object.</returns>
        // PATCH: api/Course
        [HttpPatch("id, newMaterial")]
        public Material SaveApi(int id, Material newMaterial)
        {

            Material material = _materialRepository.GetById(id);

            var name = newMaterial.Name;
            var aouthor = newMaterial.Author;
            var numberOfpages = newMaterial.NumberOgpages;
            var format = newMaterial.Format;
            var yearOfPublication = newMaterial.YearOfPublication;
            var dateOfPublication = newMaterial.DateOfPublication;
            var typeOfDataCarrier = newMaterial.TypeOfDatacarrier;
            var duration = newMaterial.Duration;
            var Quality = newMaterial.Quality;

            material.Name = name;
            material.Author = aouthor;
            material.NumberOgpages = numberOfpages;
            material.Format = format;
            material.YearOfPublication = yearOfPublication;
            material.DateOfPublication = dateOfPublication;
            material.TypeOfDatacarrier = typeOfDataCarrier;
            material.Duration = duration;
            material.Quality = Quality;
            _materialRepository.Save();
            return material;
        }
    }
}