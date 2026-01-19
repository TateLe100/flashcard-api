namespace FlashCard.API.Models;

public class Subject
{
    public int SubjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}