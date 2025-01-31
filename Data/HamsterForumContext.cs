using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HamsterForum.Models;

namespace HamsterForum.Data
{
    public class HamsterForumContext : DbContext
    {
        public HamsterForumContext (DbContextOptions<HamsterForumContext> options)
            : base(options)
        {
        }

        public DbSet<HamsterForum.Models.Discussion> Discussion { get; set; } = default!;
        public DbSet<HamsterForum.Models.Comment> Comment { get; set; } = default!;
    }
}
