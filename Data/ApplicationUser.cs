﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HamsterForum.Models;
using Humanizer;
using Microsoft.AspNetCore.Identity;

namespace HamsterForum.Data {
    public class ApplicationUser : IdentityUser {

        [Required(ErrorMessage = "A Username is required")]
        public required string Name { get; set; }

        public string? Location { get; set; }

        public string? ImageFileName { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        public List<Discussion> DiscussionList { get; set; } = [];

    }
}
