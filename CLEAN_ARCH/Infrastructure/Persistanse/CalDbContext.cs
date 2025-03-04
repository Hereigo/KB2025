﻿using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistanse
{
    public class CalDbContext : IdentityDbContext<IdentityUser>
    {
        public CalDbContext(DbContextOptions<CalDbContext> options) 
            : base(options) { }

        public DbSet<CalEvent> CalEvents { get; set; } = null!;

        public DbSet<CalEventCategory> CalEventCategories { get; set; } = null!;

        public DbSet<RequestHeader> RequestsHeaders { get; set; } = null!;
    }
}

//public class CalDbContext : IdentityDbContext<IdentityUser>
//{
//    public CalDbContext(DbContextOptions<CalDbContext> options)
//        : base(options)
//    {
//    }

//    protected override void OnModelCreating(ModelBuilder builder)
//    {
//        base.OnModelCreating(builder);
//        // Customize the ASP.NET Identity model and override the defaults if needed.
//        // For example, you can rename the ASP.NET Identity table names and more.
//        // Add your customizations after calling base.OnModelCreating(builder);
//    }
//}