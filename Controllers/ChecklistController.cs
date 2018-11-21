using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
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
        public List<string> GetListNames() => _context.Checklists.Select(c => c.Name).ToList();

        /// <summary>
        /// Inserts new checklist with a unique name. 
        /// </summary>
        /// <response code="201">New checklist inserted.</response>
        /// <response code="409">Checklist of given name already exists.</response>
        [Route("lists")]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        public IActionResult CreateChecklist([FromBody] string checklistName)
        {
            if(_context.Checklists.Any(c => c.Name == checklistName)) return StatusCode(409);
            long newId = !_context.Checklists.Any() ? 1 : _context.Checklists.Last().Id + 1;
            _context.Checklists.Add(new Checklist
            {
                Id = newId,
                Name = checklistName,
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
        [HttpDelete("lists/{checklistName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteChecklist(string checklistName)
        {
            Checklist checklist = _context.Checklists.FirstOrDefault(c => c.Name == checklistName);
            if (checklist == null) return NotFound();

            _context.Checklists.Remove(checklist);
            _context.SaveChanges();
            return Ok();

        }

        /// <summary>
        /// Returns list of checklist items. 
        /// </summary>
        /// <response code="200">JSON array of checklist's items.</response>
        [HttpGet("lists/{checklistName}/items")]
        [ProducesResponseType(200)]
        public List<CItem> GetChecklistItems(string checklistName) => _context.Items.ToList().Where(i => i.ChecklistName == checklistName)
            .Select(i => new CItem{Name = i.Name, Checked = i.Checked}).ToList();

        public struct CItem
        {
            public string Name;
            public bool Checked;
        }

        /// <summary>
        /// Inserts new unchecked item to checklist and gives it unique ID.
        /// </summary>
        /// <response code="201">Returns ID of newly added item.</response>
        [HttpPost("lists/{checklistName}/items")]
        [ProducesResponseType(201)]
        public IActionResult CreateChecklistItem([FromBody] string itemName, string checklistName)
        {
            long newId = !_context.Items.Any() ? 1 : _context.Items.Last().Id + 1;
            _context.Items.Add(new Item
            {
                Id = newId,
                ChecklistName = checklistName,
                Name = itemName,
                Checked = false,
            });
            _context.SaveChanges();

            return StatusCode(201, newId);
            
        }
    }
}