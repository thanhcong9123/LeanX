using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Data.Entities.EF
{
    public class LearnXPayMentDbContext : IdentityDbContext
    {
        
         public LearnXPayMentDbContext(DbContextOptions<LearnXPayMentDbContext> options) : base(options) { }

        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Payment>()
                .HasIndex(x => x.OrderCode).IsUnique();
            b.Entity<Payment>()
                .HasIndex(x => new { x.UserId, x.Status });
            b.Entity<Payment>()
                .Property(x => x.Status).HasConversion<int>();

            // decimal mapping already set by column attribute
            base.OnModelCreating(b);
        }

    }
}