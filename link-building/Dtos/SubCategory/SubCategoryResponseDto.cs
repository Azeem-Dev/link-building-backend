using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace link_building.Dtos.SubCategory
{
    public class SubCategoryResponseDto
    {
        public string Category { get; set; }
        public List<SubCategoriesFromCategory> List { get; set; }

    }
    public class SubCategoriesFromCategory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
        public List<SubCategoriesDto> SubCategories { get; set; }

    }

    public class SubCategoriesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
