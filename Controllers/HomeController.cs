using System.Diagnostics;
using HamsterForum.Data;
using HamsterForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HamsterForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly HamsterForumContext _context;
        public HomeController(HamsterForumContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() {

            var discussions = await _context.Discussion
                 .Include(d => d.Comments) // Load comments count
                 .OrderByDescending(d => d.CreateDate)
                 .ToListAsync();

            return View(discussions);
        }

        public IActionResult Privacy()
        {
            return View();
        }
       
        public async Task<IActionResult> GetDiscussion(int id) {
            var discussion = await _context.Discussion
                .Include(d => d.Comments) // Ensure comments are loaded
                .FirstOrDefaultAsync(d => d.DiscussionId == id);

            if (discussion == null) {
                return NotFound();
            }

            return View(discussion);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
