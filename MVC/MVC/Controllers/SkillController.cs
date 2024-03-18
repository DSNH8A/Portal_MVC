using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MVC.Data;
using MVC.Interface;
using MVC.Models;
using MVC.Repository;
using Newtonsoft.Json.Linq;

namespace MVC.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public partial class SkillController : Controller
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IMemoryCache _memoryCache;

        public SkillController(MVC_Context context, ISkillRepository skillRepository, IMemoryCache memoryCache) 
        {
            _skillRepository = skillRepository;
            _memoryCache = memoryCache;
        }

        public IEnumerable<Skill> GetAll()
        {
            List<Skill> skills;
            //return  _skillRepository.GetAllList();
           skills =  _memoryCache.Get<List<Skill>>("skills");

            if (skills == null)
            {
                skills = _skillRepository.GetAllList();
                _memoryCache.Set("skills", skills);
            }
          
            return skills;
        }

        public List<Skill> Skills()
        {
            List<Skill> skills;

            skills = _memoryCache.Get<List<Skill>>("skills");

            if (skills == null)
            {
                skills = _skillRepository.GetAllList();
                _memoryCache.Set("skills", skills);
            }
            return skills;
        }

        public async Task<IActionResult> Index() //Controller
        {
            IEnumerable<Skill> skills = await _skillRepository.GetAll(); //Model

            if (ModelState.IsValid)
            {
                return View(skills);  //view
            }

            else 
            {
                return View("Empty");    
            }
        }

        public IActionResult Add(Skill skill)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");    
            }
            _skillRepository.Add(skill);
            _skillRepository.Save();
            return View("Index");
        }

        public IActionResult Empty()
        {
            return View("Empty");    
        }

        public IActionResult GoToCreate()
        {
            return View("Create");    
        }

       
        public async Task<IActionResult> Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            Skill skill = await _skillRepository.GetByIdAsync(id);
            return View(skill);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");    
            }

            Skill skill = await _skillRepository.GetByIdAsync(id);
            return View(skill);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Skill skill)
        {
            _skillRepository.Delete(skill);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Save(int id, Skill newSkill)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");    
            }

            Skill skill = await _skillRepository.GetByIdAsync(id);

            var name = newSkill.Name;
            var number = newSkill.SkillLevel;

            skill.SkillLevel = number;
            skill.Name = name;
            //_skillRepository.Save();

            return RedirectToAction("Index");
        }
        //[HttpPost]
        //public async Task<ActionResult<Skill>> Valami(Skill skill)
        //{
        //        return CreatedAtAction(nameof(Get), new { id = skill.Id}, skill);  
        //}

        //[HttpGet("/${id}", Name = "GetProduct")]
        //[ProducesResponseType(typeof(Skill), 200)]
        //public IActionResult Get([FromRoute] int id)
        //{
        //    // some code...
        //    return Ok(200);
        //}
    }

    public partial class SkillController 
    {
        /// <summary>
        /// Gets all skills.
        /// </summary>
        /// <returns>Returns a page with all the skills displayed.</returns>
        // GET: api/Skill
        [HttpGet("allSkills")]
        public IEnumerable<Skill> GetAllApi()
        {
            return _skillRepository.GetAllList();
        }

        /// <summary>
        /// Adds a skill object to the skills list.
        /// </summary>
        /// <returns>Redirects to the page where every skill is dsiplayed.</returns>
        // POST: api/Skill
        [HttpPost("skill")]
        [Produces("application/skill.json")]
        public Skill AddApi([FromBody] Skill skill)
        {
            _skillRepository.Add(skill);
            _skillRepository.Save();
            return skill;
        }

        /// <summary>
        /// Edits a skill object.
        /// </summary>
        /// <returns>The edited skill object.</returns>
        // PATCH: api/Skill
        [HttpPatch("skillid")]
        public Skill EditApi(int skillId)
        {
            Skill skill = _skillRepository.GetById(skillId);
            return skill;
        }

        /// <summary>
        /// Deletes the skill associated with the given id.
        /// </summary>
        /// <returns>The list of existing skills.</returns>
        // DELETE: api/Skill/5
        [HttpDelete("{skilllid}")]
        public List<Skill> DeleteApi(int skillId)
        {

            Skill skill = _skillRepository.GetById(skillId);
            _skillRepository.Delete(skill);
            return  _skillRepository.GetAllList();
        }
    }
}
