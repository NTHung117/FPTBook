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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FPTBook.Areas.Identity.Data;

namespace FPTBook.Controllers
{
    
    public class OrderDetailsController : Controller
    {
        private readonly FPTBookContext _context;
        private readonly UserManager<FPTBookUser> _userManager;

        public OrderDetailsController(FPTBookContext context, UserManager<FPTBookUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Customer")]
        // GET: OrderDetails
        public async Task<IActionResult> CustomerManager(int id)
        {
            string thisUsers = _userManager.GetUserId(HttpContext.User);
            var bookContext = _context.OrderDetail.Where(o => o.Order.UId == thisUsers&& o.OrderId == id).Include(o => o.Book).Include(o => o.Order).Include(o => o.Order.User);
            return View(await bookContext.ToListAsync());
        }
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> SellerManager(int id)
        {

            var orderManager = _context.OrderDetail.Where(o=>o.OrderId==id).Include(o => o.Book).Include(o => o.Order).Include(o => o.Order.User);
            return View(await orderManager.ToListAsync());
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetail.Any(e => e.OrderId == id);
        }
    }
}
