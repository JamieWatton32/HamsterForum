﻿using HamsterForum.Data;
using HamsterForum.Models;

namespace HamsterForum.Models {
    public class Comment {   //primary key
        public int CommentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; } = DateTime.Now;

        //foreign key
        public int DiscussionId { get; set; }

        // Foreign key (AspNetUsers table)
        public string ApplicationUserId { get; set; } = string.Empty;
        // Navigation property
        public ApplicationUser? ApplicationUser { get; set; } // nullable!!!

    }

}

