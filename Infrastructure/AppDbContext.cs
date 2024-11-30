using Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>().ToTable("UsersAccount", "security");
            builder.Entity<IdentityRole>().ToTable("Roles", "security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole", "security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim", "security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin", "security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim", "security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken", "security");
        }

        public DbSet<Attendance> attendences { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }
        public DbSet<feedBack> feedBack { get; set; }
        public DbSet<Materails> Materails { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<SolveTask> SolveTask { get; set; }
        public DbSet<TypeJop> TypeJop { get; set; }
        public DbSet<Grades> Grades { get; set; }
        public DbSet<Student_Course> StudentCourse { get; set; }
        public DbSet<TeacherCourses> TeacherCourses { get; set; }

    }
}
