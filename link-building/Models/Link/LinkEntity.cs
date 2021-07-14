using link_building.Models.LinkCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace link_building.Models.Link
{
    public class LinkEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string SubCategoriesSelected { get; set; }
        public List<LinkCategoryEntity> LinkCategories { get; set; }
    }
}
