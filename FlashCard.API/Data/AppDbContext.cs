using Microsoft.EntityFrameworkCore;
using FlashCard.API.Models;

namespace FlashCard.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Subject> Subjects { get; set; } = null!;
    public DbSet<LevelOfDifficulty> LevelOfDifficulties { get; set; } = null!;
    public DbSet<Test> Tests { get; set; } = null!;
    public DbSet<TestQuestion> TestQuestions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Composite key for many-to-many
    modelBuilder.Entity<TestQuestion>()
        .HasKey(tq => new { tq.TestId, tq.QuestionId });

    // TestQuestion → Test
    modelBuilder.Entity<TestQuestion>()
        .HasOne(tq => tq.Test)
        .WithMany(t => t.TestQuestions)
        .HasForeignKey(tq => tq.TestId);

    // TestQuestion → Question
    modelBuilder.Entity<TestQuestion>()
        .HasOne(tq => tq.Question)
        .WithMany(q => q.TestQuestions)
        .HasForeignKey(tq => tq.QuestionId);

    // Question → Subject
    modelBuilder.Entity<Question>()
        .HasOne(q => q.Subject)
        .WithMany(s => s.Questions)
        .HasForeignKey(q => q.SubjectId);

    // Question → LevelOfDifficulty
    modelBuilder.Entity<Question>()
        .HasOne(q => q.LevelOfDifficulty)
        .WithMany(l => l.Questions)
        .HasForeignKey(q => q.LevelOfDifficultyId);

    base.OnModelCreating(modelBuilder);
}
}
