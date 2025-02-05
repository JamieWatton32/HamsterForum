using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HamsterForum.Models {
    public class Discussion {   //primary key
        public int DiscussionId { get; set; }
        [Required(ErrorMessage = "A Title is required")]
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ImageFilename { get; set; } = string.Empty;
        [Display(Name = "Created")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        // Property for file upload, not mapped in EF
        [NotMapped]
        [Display(Name = "Photograph")]
        public IFormFile? ImageFile { get; set; } // nullable!!!

        public List<Comment>? Comments { get; set; }
    }

}
