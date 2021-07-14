using link_building.Dtos.Category;
using link_building.Dtos.SubCategory;
using link_building.Models.LinkCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace link_building.Dtos.Link
{
    public class CreateLinkDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public List<int> Categories { get; set; }
        public List<SubCategories> SubCategories { get; set; }

    }
    public class SubCategories
    {
        public int CategoryId { get; set; }
        public List<int> subCategories { get; set; }
    }
}
