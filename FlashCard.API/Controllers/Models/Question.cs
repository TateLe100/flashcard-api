namespace FlashCard.API.Models;

public class Question
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public string? Explanation { get; set; }

    public int SubjectId { get; set; }
    public int LevelOfDifficultyId { get; set; }

    public Subject Subject { get; set; } = null!;
    public LevelOfDifficulty LevelOfDifficulty { get; set; } = null!;
}