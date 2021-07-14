using link_building.Models.Category;
using link_building.Models.Link;
using link_building.Models.LinkCategory;
using link_building.Models.SubCategory;
using link_building.Models.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace link_building.Ef_Core
{
    public class DataContext : DbContext
    {
        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<LinkEntity> Links { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<LinkCategoryEntity> LinkCategories { get; set; }
        public DbSet<SubCategoryEntity> SubCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
