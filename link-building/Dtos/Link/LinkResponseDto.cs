using link_building.Dtos.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace link_building.Dtos.Link
{
    public class LinkResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Content { get; set; } 
        public List<CategoryResponseDto> Categories { get; set; }
    }
}
