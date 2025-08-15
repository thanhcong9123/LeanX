using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Data.EF
{
    public class LearnXDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public LearnXDbContext(DbContextOptions<LearnXDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Định cấu hình các quy tắc cho các entity và các quan hệ

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Enrollments)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserID)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete on User deletion

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Enrollments)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete on Course deletion

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Lessons)
                .WithOne(l => l.Course)
                .HasForeignKey(l => l.CourseID)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete Lessons when a Course is deleted

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Reviews)
                .WithOne(r => r.Course)
                .HasForeignKey(r => r.CourseID)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete Reviews when a Course is deleted

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(u => u.Courses)
                .HasForeignKey(c => c.InstructorID)
                .OnDelete(DeleteBehavior.Restrict); // Restrict cascade delete for Instructor

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Courses)
                .WithOne(co => co.Category)
                .HasForeignKey(co => co.CategoryID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete on Category deletion

            modelBuilder.Entity<Exercise>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Exercises)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete Exercises when Course is deleted

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Exercise)
                .WithMany(e => e.Questions)
                .HasForeignKey(q => q.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete Questions when Exercise is deleted

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete Answers when Question is deleted

            modelBuilder.Entity<EssaySubmissions>()
                .HasOne(es => es.User)
                .WithMany()
                .HasForeignKey(es => es.IdUser)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete when User is deleted

            modelBuilder.Entity<EssaySubmissions>()
                .HasOne(es => es.Exercise)
                .WithMany()
                .HasForeignKey(es => es.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete when Exercise is deleted
            modelBuilder.Entity<Lesson>()
                .HasMany(l => l.Resources)
                .WithOne(r => r.Lesson)
                .HasForeignKey(r => r.LessonID)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete Resources when Lesson is deleted
            
            modelBuilder.Entity<EBook>()
                .HasMany(e => e.EvaluateBooks)
                .WithOne(eb => eb.Book)
                .HasForeignKey(eb => eb.BookId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete EvaluateBooks when EBook is deleted

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<AppUser> AppUsers { get; set; } = default!;
        public DbSet<AppRole> AppRoles { get; set; } = default!;
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Scores> Scores { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<EBook> EBooks { get; set; }
        public DbSet<EssaySubmissions> EssaySubmissions { get; set; }
        public DbSet<ResourcesLesson> ResourcesLessons { get; set; } = default!;
        public DbSet<EvaluateBook> EvaluateBooks { get; set; } = default!;

    }
}
