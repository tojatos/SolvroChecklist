using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SolvroChecklist.Models;

namespace SolvroChecklist.Controllers
{
//    [Route("api/[controller]")]
    public class ChecklistController : Controller
    {
        private readonly ChecklistContext _context;

        public ChecklistController(ChecklistContext context)
        {
            _context = context;
//            _context.Checklists.Add(new Checklist{Name = "test"});
//            _context.SaveChanges();
        }

        [Route("lists")]
        [HttpGet]
        public List<string> GetListNames()
        {
            return _context.Checklists.Select(c => c.Name).ToList();
        }
    }
}