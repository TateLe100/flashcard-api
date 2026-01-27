using FlashCard.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.API.Data.Seeders;

public static class SeedData
{
    public static void Seed(AppDbContext context)
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // Seed Subjects

        if (!context.Subjects.Any())
        {
            context.Subjects.AddRange(
                new Subject { Name = "Math" },
                new Subject { Name = "History" },
                new Subject { Name = "Science" }
            );
            context.SaveChanges();
        }

        // Seed LevelOfDifficulty
        if (!context.LevelOfDifficulties.Any())
        {
            context.LevelOfDifficulties.AddRange(
                new LevelOfDifficulty { Name = "Easy" },
                new LevelOfDifficulty { Name = "Medium" },
                new LevelOfDifficulty { Name = "Hard" }
            );
            context.SaveChanges();
        }

        // Seed Questions
        if (!context.Questions.Any())
        {
            var math = context.Subjects.First(s => s.Name == "History");
            var easy = context.LevelOfDifficulties.First(l => l.Name == "Easy");

            context.Questions.Add(new Question
            {
                QuestionText = "What is 2 + 2?",
                Answer = "4",
                SubjectId = math.SubjectId,
                LevelOfDifficultyId = easy.LevelOfDifficultyId
            });

            context.Questions.Add(new Question
            {
                QuestionText = "What is 10 - 3?",
                Answer = "7",
                SubjectId = math.SubjectId,
                LevelOfDifficultyId = easy.LevelOfDifficultyId
            });

            context.SaveChanges();
        }
    }
}
