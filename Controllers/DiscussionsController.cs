using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HamsterForum.Data;
using HamsterForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HamsterForum.Controllers {
    [Authorize]
    public class DiscussionsController : Controller
    {
        private readonly HamsterForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

       
        public DiscussionsController(HamsterForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
      
        // GET: Discussions
        public async Task<IActionResult> Index()
        {

           
            var userId = _userManager.GetUserId(User);
            var discussions = await _context.Discussion
                .Where(d => d.ApplicationUserId == userId)
                .Include(d => d.Comments)
                .Include(d => d.ApplicationUser) // Add this line
                .OrderByDescending(d => d.CreateDate)
                .ToListAsync();



            return View(discussions);
        }

        //// GET: Discussions/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
            
        //    var discussion = await _context.Discussion
        //        .Include(d=>d.Comments)
        //        .OrderByDescending(d => d.CreateDate)
        //        .FirstOrDefaultAsync(m => m.DiscussionId == id);
        //    if (discussion == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(discussion);
        //}

        // GET: Discussions/Create
        public IActionResult Create()
        {
          

             var discussion = new Discussion();
               
          

            return View(discussion);
        }

        // POST: Discussions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscussionId,Title,Content,CreateDate, ImageFile, Comments")] Discussion discussion)
        {
            // rename the uploaded file to a guid (unique filename). Set before photo saved in database.
            discussion.ImageFilename = Guid.NewGuid().ToString() + Path.GetExtension(discussion.ImageFile?.FileName);
            Console.WriteLine(discussion.ImageFilename);
            discussion.ApplicationUserId = _userManager.GetUserId(User);
            
            

            if (ModelState.IsValid) {
                _context.Add(discussion);
                await _context.SaveChangesAsync();

                // Save the uploaded file after the photo is saved in the database.
                if (discussion.ImageFile != null) {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images",discussion.ImageFilename);
                    using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                        await discussion.ImageFile.CopyToAsync(fileStream);
                    }
                }

                return RedirectToAction("Index", "Home");
            }

            return View(discussion);
        }

        // GET: Discussions/Edit/id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var discussion = await _context.Discussion
                .Where(d => d.ApplicationUserId == userId)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            
            if (discussion == null)
            {
                return NotFound();
            }
            return View(discussion);
        }

        // POST: Discussions/Edit/id
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content,ImageFilename,CreateDate,Comments, ApplicationUserId")] Discussion discussion)
        {
            if (id != discussion.DiscussionId)
            {
                return NotFound();
            }

           
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscussionExists(discussion.DiscussionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }

        // GET: Discussions/Delete/id
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            var discussion = await _context.Discussion
                .Where(d => d.ApplicationUserId == userId)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // POST: Discussions/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var userId = _userManager.GetUserId(User);
            var discussion = await _context.Discussion
                .Where(d => d.ApplicationUserId == userId)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion != null)
            {
                _context.Discussion.Remove(discussion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool DiscussionExists(int id)
        {
            return _context.Discussion.Any(e => e.DiscussionId == id);
        }
    }
}
