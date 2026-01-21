namespace FlashCard.API.DTOs;

public class QuestionDto
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public string? Explanation { get; set; }

    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;

    public int LevelOfDifficultyId { get; set; }
    public string LevelOfDifficultyName { get; set; } = string.Empty;
}
