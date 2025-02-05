using Microsoft.AspNetCore.Mvc;
using HamsterForum.Data;
using HamsterForum.Models;


namespace HamsterForum.Controllers
{
    public class CommentsController : Controller
    {
        private readonly HamsterForumContext _context;

        public CommentsController(HamsterForumContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create([Bind("CommentId,Content,CreateDate,DiscussionId")] Comment comment) {

            if (ModelState.IsValid) {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetDiscussion", "Home", new {id = comment.DiscussionId});
            }

            return View(comment);
        }
    }
}
