using AutoMapper;
using link_building.Dtos;
using link_building.Dtos.Link;
using link_building.Ef_Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace link_building.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LinkController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public LinkController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpPost("SubmitLink")]
        public async Task<bool> SubmitLink(CreateLinkDto request)
        {
            var link = new Models.Link.LinkEntity {
                Link=request.Link,
                Description=request.Description,
                Title=request.Title
            };

            foreach(var item in request.SubCategories)
            {
                foreach(var subCategory in item.subCategories)
                {
                    link.SubCategoriesSelected = " " + subCategory + ",";
                }
            }
            await _context.Links.AddAsync(link);
            _context.SaveChanges();

            foreach (var categories in request.Categories)
            {
                _context.LinkCategories.Add(new Models.LinkCategory.LinkCategoryEntity
                {
                    CategoryId = categories,
                    LinkId = link.Id
                });
            }
            _context.SaveChanges();
            return true;
        }
        [HttpPost("DeleteLink")]
        public async Task<bool> DeleteLink(LinkDeleteRequest request)
        {
            var linkToDelete = _context.Links.FirstOrDefault(c => c.Link == request.LinkName);
            _context.Remove(linkToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
