using link_building.Models.Category;
using link_building.Models.Link;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace link_building.Models.LinkCategory
{
    public class LinkCategoryEntity
    {
        public int Id { get; set; }
        public int LinkId { get; set; }
        public LinkEntity Link { get; set; }
        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
    }
}
