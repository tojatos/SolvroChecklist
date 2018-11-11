using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using SolvroChecklist.Models;

namespace SolvroChecklist.Controllers
{
    [Produces("application/json")] 
    public class ChecklistController : Controller
    {
        private readonly ChecklistContext _context;

        public ChecklistController(ChecklistContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Returns list of checklists' names. 
        /// </summary>
        /// <response code="200">New checklist inserted.</response>
        [Route("lists")]
        [HttpGet]
        [ProducesResponseType(200)]
        public List<string> GetListNames()
        {
            return _context.Checklists.Select(c => c.Name).ToList();
        }
        
        /// <summary>
        /// Inserts new checklist with a unique name. 
        /// </summary>
        /// <response code="201">New checklist inserted.</response>
        /// <response code="409">Checklist of given name already exists.</response>
        [Route("lists")]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        public IActionResult Create([FromBody] string checklistName)
        {
            if(_context.Checklists.Any(c => c.Name == checklistName)) return StatusCode(409);
            long newId = !_context.Checklists.Any() ? 1 : _context.Checklists.Last().Id + 1;
            _context.Checklists.Add(new Checklist
            {
                Id = newId,
                Name = checklistName,
                Items = new List<Item>(),
            });
            _context.SaveChanges();

            return StatusCode(201);
        }

        /// <summary>
        /// Remove checklist of given name.
        /// </summary>
        /// <response code="200">OK.</response>
        /// <response code="404">Checklist of given ID does not exist.</response>
//        [Route("lists/{name}")]
        [HttpDelete("lists/{name}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Delete(string name)
        {
            var checklist = _context.Checklists.FirstOrDefault(c => c.Name == name);
            if (checklist == null) return NotFound();

            _context.Checklists.Remove(checklist);
            _context.SaveChanges();
            return Ok();

        }
    }
}