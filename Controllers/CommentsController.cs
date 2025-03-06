using Microsoft.AspNetCore.Mvc;
using HamsterForum.Data;
using HamsterForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace HamsterForum.Controllers {
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly HamsterForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(HamsterForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Comments/Create
        public IActionResult Create(int? id) {
            if (id ==null) {
                return NotFound();
            }

            var comment = new Comment {
                DiscussionId = (int)id
            };
            

            return View(comment); 
        }


        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,Content,CreateDate,DiscussionId,ApplicationUserId")] Comment comment) {

            if (ModelState.IsValid) {


                var userId = _userManager.GetUserId(User);
                comment.ApplicationUserId = userId;

                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetDiscussion", "Home", new {id = comment.DiscussionId});
            }

            return View(comment);
        }
    }
}
