#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBook.Data;
using FPTBook.Models;
using Microsoft.AspNetCore.Identity;
using FPTBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace FPTBook.Controllers
{
    public class CartsController : Controller
    {
        private readonly FPTBookContext _context;
        private readonly UserManager<FPTBookUser> _userManager;

        public CartsController(FPTBookContext context,UserManager<FPTBookUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Customer")]
        // GET: Carts
        public async Task<IActionResult> Index()
        {
            string thisUserId = _userManager.GetUserId(HttpContext.User);
            var fPTBookContext = _context.Cart.Where(c => c.UId == thisUserId).Include(c => c.Book).Include(c => c.User);
            return View(await fPTBookContext.ToListAsync());

        }

        // GET: Carts/Details/5
       public async Task<IActionResult> UpdateCart(string isbn)
        {
            string thisUserId = _userManager.GetUserId(HttpContext.User);
            Cart fromDb = _context.Cart.FirstOrDefault(c => c.UId == thisUserId && c.BookIsbn == isbn);
            fromDb.Quantity++;
            _context.Update(fromDb);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> removeItem(string isbn)
        {
            string thisUserId = _userManager?.GetUserId(HttpContext.User);
            Cart fromDb = _context.Cart.FirstOrDefault(c => c.UId == thisUserId && c.BookIsbn == isbn);
            fromDb.Quantity--;
            while (fromDb.Quantity != 0)
            {
                _context.Update(fromDb);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(string isbn)
        {
            string thisUserId = _userManager.GetUserId(HttpContext.User);
            Cart fromDb = _context.Cart.FirstOrDefault(c => c.UId == thisUserId && c.BookIsbn == isbn);
            _context.Remove(fromDb);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        private bool CartExists(string id)
        {
            return _context.Cart.Any(e => e.UId == id);
        }
    }
}
