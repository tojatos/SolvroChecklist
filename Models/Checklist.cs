using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SolvroChecklist.Models
{
    public class Checklist
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Item> Items { get; set; }
    }
}