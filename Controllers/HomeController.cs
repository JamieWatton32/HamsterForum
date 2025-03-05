using System.Diagnostics;
using HamsterForum.Data;
using HamsterForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace HamsterForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly HamsterForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(HamsterForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() {

            var discussions = await _context.Discussion
                 .Include(d => d.Comments) // Load comments count
                 .Include(d => d.ApplicationUser)
                 .OrderByDescending(d => d.CreateDate)
                 .ToListAsync();

       

            return View(discussions);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //GET: /Home/Profile/id
        public async Task<IActionResult> Profile(string? id) {
            var userId = _userManager.GetUserId(User);
            var profile = await _context.Discussion
                .Include(d => d.ApplicationUser)
                .FirstOrDefaultAsync(d => d.ApplicationUserId == id);
            var discussion = await _context.Discussion
               .Include(d => d.Comments) // Include related comments
               .ToListAsync();

            foreach(var item in discussion) {
                profile?.ApplicationUser?.DiscussionList.Add(item);
            }
           

            return View(profile);

        }
       
        //Get a single discussion
        public async Task<IActionResult> GetDiscussion(int? id) {
            if (id == null) {
                return NotFound();
            }
            var discussion = await _context.Discussion
                .Include(d => d.Comments) // Include related comments
                    .ThenInclude(c => c.ApplicationUser) // Include the user associated with each comment
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