using link_building.Dtos.SubCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace link_building.Dtos.Category
{
    public class CategoryEntityDto
    {
        public string Name { get; set; }
        public List<SubCategoryEntityDto> SubCategories { get; set; }
    }
}
