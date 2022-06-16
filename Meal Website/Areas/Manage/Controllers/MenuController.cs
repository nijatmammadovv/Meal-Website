using Meal_Website.Data_Access_Layer;
using Meal_Website.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Meal_Website.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class MenuController : Controller
    {
        private AppDbContext _context { get; }
        private IWebHostEnvironment _env { get; }
        public MenuController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Menus.Include(x=>x.Category).ToList());
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Menu menu)
        {
            if (!ModelState.IsValid) return View(menu);
            string filename = menu.ImageUrl.FileName;
            if(filename.Length > 64)
            {
                filename.Substring(filename.Length - 64, 64);
            }
            string newFileName = Guid.NewGuid().ToString() + filename;
            string path = Path.Combine(_env.WebRootPath, "assets", "images", newFileName);
            using (FileStream fs = new FileStream(path,FileMode.Create))
            {
                menu.ImageUrl.CopyTo(fs);
            }
            menu.Image = newFileName;
            await _context.Menus.AddAsync(menu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Menu menu = _context.Menus.Find(id);
            _context.Menus.Remove(menu);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Menu menu = _context.Menus.FirstOrDefault(c => c.Id == id);
            if (menu == null) return NotFound();
            return View(menu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Menu menu)
        {
            Menu menu1 = _context.Menus.FirstOrDefault(C => C.Id == menu.Id);
            if (menu1 == null) return NotFound();
            menu1.Name = menu.Name;
            menu1.Price = menu.Price;
            menu1.Image = menu.Image;
            menu1.Description = menu.Description;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
