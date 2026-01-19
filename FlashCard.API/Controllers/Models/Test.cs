namespace FlashCard.API.Models;

public class Test
{
    public int TestId { get; set; }
    public DateTime DateTaken { get; set; } = DateTime.UtcNow;
    public int Score { get; set; }
    public ICollection<TestQuestion> TestQuestions { get; set; } = new List<TestQuestion>();
}