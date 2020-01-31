using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FishStore.Data;
using FishStore.Models.ViewModels;
using FishStore.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FishStore.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    [Area("Admin")]
    public class StoreItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [BindProperty]
        public StoreItemViewModel StoreItemVM { get; set; }

        public StoreItemController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            StoreItemVM = new StoreItemViewModel
            {
                Category = _db.Category,
                StoreItem = new Models.StoreItem()
            };
        }

        //GET
        public async Task<IActionResult> Index()
        {
            return View(await _db.StoreItem.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync());
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View(StoreItemVM);
        }


        //POST - CREATE
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            StoreItemVM.StoreItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                return View(StoreItemVM);
            }

            _db.StoreItem.Add(StoreItemVM.StoreItem);
            await _db.SaveChangesAsync();

            //IMAGE SAVING SECTION

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var StoreItemFromDb = await _db.StoreItem.FindAsync(StoreItemVM.StoreItem.Id);

            if (files.Count > 0)
            {
                //files has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, StoreItemVM.StoreItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                StoreItemFromDb.Image = @"\images\" + StoreItemVM.StoreItem.Id + extension;
            }
            else
            {
                //no file, use default
                var uploads = Path.Combine(webRootPath, @"images\" + SD.DefaultImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + StoreItemVM.StoreItem.Id + ".jpg");
                StoreItemFromDb.Image = @"\images\" + StoreItemVM.StoreItem.Id + ".jpg";
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            StoreItemVM.StoreItem = await _db.StoreItem.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);
            StoreItemVM.SubCategory = await _db.SubCategory.Where(s => s.CategoryId == StoreItemVM.StoreItem.CategoryId).ToListAsync();
            if (StoreItemVM.StoreItem == null)
            {
                return NotFound();
            }
            return View(StoreItemVM);
        }


        //POST - EDIT
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StoreItemVM.StoreItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                StoreItemVM.SubCategory = await _db.SubCategory.Where(s => s.CategoryId == StoreItemVM.StoreItem.CategoryId).ToListAsync();
                return View(StoreItemVM);
            }


            //IMAGE SAVING SECTION

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var StoreItemFromDb = await _db.StoreItem.FindAsync(StoreItemVM.StoreItem.Id);

            if (files.Count > 0)
            {
                //new image has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension_new = Path.GetExtension(files[0].FileName);

                //Delete the original file
                var ImagePath = Path.Combine(webRootPath, StoreItemFromDb.Image.TrimStart('\\'));

                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }
                //store new image
                using (var filesStream = new FileStream(Path.Combine(uploads, StoreItemVM.StoreItem.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                StoreItemFromDb.Image = @"\images\" + StoreItemVM.StoreItem.Id + extension_new;
            }

            StoreItemFromDb.Name = StoreItemVM.StoreItem.Name;
            StoreItemFromDb.Description = StoreItemVM.StoreItem.Description;
            StoreItemFromDb.Volume = StoreItemVM.StoreItem.Volume;
            StoreItemFromDb.Price = StoreItemVM.StoreItem.Price;
            StoreItemFromDb.Weight = StoreItemVM.StoreItem.Weight;
            StoreItemFromDb.CategoryId = StoreItemVM.StoreItem.CategoryId;
            StoreItemFromDb.SubCategoryId = StoreItemVM.StoreItem.SubCategoryId;


            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET - DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            StoreItemVM.StoreItem = await _db.StoreItem.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);
            StoreItemVM.SubCategory = await _db.SubCategory.Where(s => s.CategoryId == StoreItemVM.StoreItem.CategoryId).ToListAsync();
            if (StoreItemVM.StoreItem == null)
            {
                return NotFound();
            }
            return View(StoreItemVM);
        }

        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            StoreItemVM.StoreItem = await _db.StoreItem.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);
            StoreItemVM.SubCategory = await _db.SubCategory.Where(s => s.CategoryId == StoreItemVM.StoreItem.CategoryId).ToListAsync();
            if (StoreItemVM.StoreItem == null)
            {
                return NotFound();
            }
            return View(StoreItemVM);
        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int id)
        {
           
            string webRootPath = _hostingEnvironment.WebRootPath;
            var StoreItemFromDb = await _db.StoreItem.FindAsync(id);

            //Delete image
            var ImagePath = Path.Combine(webRootPath, StoreItemFromDb.Image.TrimStart('\\'));
            if (System.IO.File.Exists(ImagePath))
            {
                System.IO.File.Delete(ImagePath);
            }
            
            _db.StoreItem.Remove(StoreItemFromDb);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}