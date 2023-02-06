using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Furniture.Data.FurnitureContext;
using Furniture.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace Furniture.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AuthDB _context;

        public ProductController(AuthDB context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return _context.Product != null ?
                        View(await _context.Product.ToListAsync()) :
                        Problem("Entity set 'AuthDB.Product'  is null.");
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,file")] ProdInput productInp)
        {
            Product product = new Product();
            try
            {
                string filename = productInp.file.FileName;
                filename = Path.GetFileName(filename);
                string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", filename);
                var stream = new FileStream(uploadpath, FileMode.Create);
                productInp.file.CopyToAsync(stream);
                ViewBag.message = "Data uploaded successfully!";

                product.Id = productInp.Id;
                product.Name = productInp.Name;
                product.Description = productInp.Description;
                product.Price = productInp.Price;
                product.filename = filename;

                if (ModelState.IsValid)
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;
            }


            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,file")] ProdInput productInp)
        {
            Product product = new Product();
            try
            {
                string filename = productInp.file.FileName;
                filename = Path.GetFileName(filename);
                string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", filename);
                var stream = new FileStream(uploadpath, FileMode.Create);
                productInp.file.CopyToAsync(stream);
                ViewBag.message = "Data edited successfully!";

                product.Id = productInp.Id;
                product.Name = productInp.Name;
                product.Description = productInp.Description;
                product.Price = productInp.Price;
                product.filename = filename;

                if (id != product.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                        ViewBag.message = ex.Message;
                        throw;

                    }
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;
            }
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'AuthDB.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        /*[HttpPost]
        *//*public IActionResult OnAddCart(Product o)
        {
           
        }*/
    }
}
