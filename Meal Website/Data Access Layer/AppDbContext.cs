using Meal_Website.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meal_Website.Data_Access_Layer
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options ):base(options)
        {

        }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
