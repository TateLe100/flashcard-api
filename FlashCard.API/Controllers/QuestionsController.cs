using FlashCard.API.Data;
using FlashCard.API.DTOs;
using FlashCard.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FlashCard.API.Controllers
{
    

    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuestionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions()
        {
        var questions = await _context.Questions
            .Include(q => q.Subject)
            .Include(q => q.LevelOfDifficulty)
            .Select(q => new QuestionDto
            {
                QuestionId = q.QuestionId,
                QuestionText = q.QuestionText,
                Answer = q.Answer,
                Explanation = q.Explanation,

                SubjectId = q.SubjectId,
                SubjectName = q.Subject.Name,

                LevelOfDifficultyId = q.LevelOfDifficultyId,
                LevelOfDifficultyName = q.LevelOfDifficulty.Name
            })
            .ToListAsync();

            return Ok(questions);
        }

        // GET: api/questions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestionById(int id)
        {
            var question = await _context.Questions
                .Include(q => q.Subject)
                .Include(q => q.LevelOfDifficulty)
                .FirstOrDefaultAsync(q => q.QuestionId == id);

            if (question == null) 
                return NotFound();

            // Map entity → DTO
            var dto = new QuestionDto
            {
                QuestionId = question.QuestionId,
                QuestionText = question.QuestionText,
                Answer = question.Answer,
                Explanation = question.Explanation,
                SubjectId = question.SubjectId,
                SubjectName = question.Subject.Name,
                LevelOfDifficultyId = question.LevelOfDifficultyId,
                LevelOfDifficultyName = question.LevelOfDifficulty.Name
            };

            return Ok(dto);
        }

        // POST: api/questions
        [HttpPost]
        public async Task<IActionResult> Create(CreateQuestionDto dto)
        {
            var question = new Question
            {
                QuestionText = dto.QuestionText,
                Answer = dto.Answer,
                Explanation = dto.Explanation,
                SubjectId = dto.SubjectId,
                LevelOfDifficultyId = dto.LevelOfDifficultyId
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return Ok(question);
        }


        // PUT: api/questions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, UpdateQuestionDto dto)
        {
            var question = await _context.Questions.Where(q => q.QuestionId == id).FirstOrDefaultAsync();

            if (question == null)
                return NotFound();

            question.QuestionText = dto.QuestionText;
            question.Answer = dto.Answer;
            question.Explanation = dto.Explanation;
            question.SubjectId = dto.SubjectId;
            question.LevelOfDifficultyId = dto.LevelOfDifficultyId;

            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        // DELETE: api/questions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return NotFound();

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}