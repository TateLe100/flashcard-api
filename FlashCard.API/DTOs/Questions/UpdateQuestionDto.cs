public class UpdateQuestionDto
{
    public string QuestionText { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public string? Explanation { get; set; }

    public int SubjectId { get; set; }
    public int LevelOfDifficultyId { get; set; }
}
