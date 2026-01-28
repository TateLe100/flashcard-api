using FlashCard.API.DTOs;

public class TestDto
{
    public int TestId { get; set; }

    // List of questions in the test
    public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
}