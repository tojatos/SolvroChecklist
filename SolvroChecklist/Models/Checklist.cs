using System.Collections.Generic;

namespace SolvroChecklist.Models
{
    public class Checklist
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Item> Items { get; set; }
    }
}