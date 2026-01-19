namespace FlashCard.API.Models;

public class LevelOfDifficulty
{
    public int LevelOfDifficultyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}