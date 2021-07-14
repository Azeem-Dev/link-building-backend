using AutoMapper;
using link_building.Dtos;
using link_building.Dtos.Category;
using link_building.Dtos.Link;
using link_building.Dtos.SubCategory;
using link_building.Ef_Core;
using link_building.Models.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace link_building.Controllers
{
    [Authorize(Roles ="Admin,admin")]
    [ApiController]
    [Route("api/[Controller]")]
    public class CategoryController: ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public CategoryController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpPost("CreateCategory")]
        public async Task<ServiceResponse<int>> CreateCategory(CategoryEntityDto request)
        {
            var mapped = _mapper.Map<CategoryEntity>(request);
            await _context.Categories.AddAsync(mapped);
            await _context.SaveChangesAsync();


            return new ServiceResponse<int>
            {
                Data = mapped.Id,
                Message = "Category added Successfully",
                Success = true

            };
        }
        [AllowAnonymous]
        [HttpGet("GetTopTrending")]
        public async Task<ServiceResponse<List<LinkResponseDto>>> GetTopTrending()
        {
            var response = new ServiceResponse<List<LinkResponseDto>>
            {
                Data = _mapper.Map<List<LinkResponseDto>>(_context.Links.Include(c=>c.LinkCategories).ThenInclude(c=>c.Category).ToList())

            };
            response.Data.ForEach(c => c.Content = c.Description);
            return response;
        }
        [AllowAnonymous]
        [HttpPost("GetSubCategories")]
        public async Task<SubCategoryResponseDto> GetSubCategories(LinkEntityDto category)
        {
            var categoryId = Convert.ToInt32(category.Id);
            var cat = await _context.Categories.Include(c=>c.SubCategories).Include(c=>c.LinkCategories).FirstOrDefaultAsync(c => c.Id == categoryId);
            var linkCatg=_context.LinkCategories.Where(c => c.CategoryId == cat.Id).Include(c=>c.Link).ToList();
            var list = new List<SubCategoriesFromCategory>();
            var subcat = new List<SubCategoriesDto>();
            
            foreach (var item in cat.SubCategories)
            {
                subcat.Add(new SubCategoriesDto
                {
                    Id=item.Id,
                    Name=item.Name
                });
            }
            foreach(var item in linkCatg)
            {
                var linkInfo=_context.Links.FirstOrDefault(c => c.Id == item.LinkId);

                list.Add(new SubCategoriesFromCategory
                {
                    Title = linkInfo.Title,
                    Description = linkInfo.Description,
                    Link = linkInfo.Link,
                    SubCategories = subcat,
                    Id = linkInfo.Id
                });
            }

            return new SubCategoryResponseDto
            {
                Category = cat.Name,
                List = list
            };
        }
        [AllowAnonymous]
        [HttpPost("SearchLink")]
        public async Task<ServiceResponse<List<LinkResponseDto>>> SearchLink(LinkSearchDto request)
        {
            var response = new ServiceResponse<List<LinkResponseDto>>
            {
                Data = _mapper.Map<List<LinkResponseDto>>(_context.Links.Include(c => c.LinkCategories).ThenInclude(c => c.Category).Where(c => c.Title.Contains(request.Keyword) || c.Description.Contains(request.Keyword)).ToList())

            };
            return response;
        }
        [AllowAnonymous]
        [HttpGet("GetAllCategories")]
        public async Task<List<CategoryAllResponseDto>> GetAllCategories()
        {
            var response = new List<CategoryAllResponseDto>();
            var categories = _context.Categories.Include(c => c.SubCategories).Include(c => c.LinkCategories).ToList();

            foreach(var category in categories)
            {
                response.Add(new CategoryAllResponseDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    SubCategories = _mapper.Map<List<LinkCreateSubCategoryResponseDto>>(_context.SubCategories.Where(d => d.CategoryId == category.Id).ToList())
                });
            }
            return response;

        }

        [HttpPost("DeleteCategory")]
        public async Task<bool> DeleteCategory(CategoryDeleteDto request)
        {
            var categoryToDelete=_context.Categories.FirstOrDefault(c=>c.Name==request.CategoryName);
            var linksRelatedToCategories=_context.LinkCategories.Where(c => c.CategoryId == categoryToDelete.Id).Include(c=>c.Link).ToList();
            foreach(var item in linksRelatedToCategories)
            {
                _context.Links.Remove(item.Link);
            }
            _context.Categories.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
