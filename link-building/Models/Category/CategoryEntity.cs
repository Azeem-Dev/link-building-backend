using link_building.Models.LinkCategory;
using link_building.Models.SubCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace link_building.Models.Category
{
    public class CategoryEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubCategoryEntity> SubCategories { get; set; }
        public List<LinkCategoryEntity> LinkCategories { get; set; }

    }
}
