using FlashCard.API.Data;
using FlashCard.API.DTOs;
using FlashCard.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.API.Controllers
{
    

    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

         // GET: api/test
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestDto>>> GetTests()
        {
            var tests = await _context.Tests
                .Include(t => t.TestQuestions)
                    .ThenInclude(tq => tq.Question)
                        .ThenInclude(q => q.Subject)
                .Include(t => t.TestQuestions)
                    .ThenInclude(tq => tq.Question)
                        .ThenInclude(q => q.LevelOfDifficulty)
                .Select(t => new TestDto
                {
                    TestId = t.TestId,
                    Questions = t.TestQuestions.Select(tq => new QuestionDto
                    {
                        QuestionId = tq.Question.QuestionId,
                        QuestionText = tq.Question.QuestionText,
                        Answer = tq.Question.Answer,
                        Explanation = tq.Question.Explanation,
                        SubjectId = tq.Question.SubjectId,
                        SubjectName = tq.Question.Subject.Name,
                        LevelOfDifficultyId = tq.Question.LevelOfDifficultyId,
                        LevelOfDifficultyName = tq.Question.LevelOfDifficulty.Name
                    }).ToList()
                })
                .ToListAsync();

            return Ok(tests);
        }

        // GET: api/test/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TestDto>> GetTestById(int id)
        {
            var test = await _context.Tests
                .Include(t => t.TestQuestions)
                    .ThenInclude(tq => tq.Question)
                        .ThenInclude(q => q.Subject)
                .Include(t => t.TestQuestions)
                    .ThenInclude(tq => tq.Question)
                        .ThenInclude(q => q.LevelOfDifficulty)
                .Where(t => t.TestId == id)
                .Select(t => new TestDto
                {
                    TestId = t.TestId,
                    Questions = t.TestQuestions.Select(tq => new QuestionDto
                    {
                        QuestionId = tq.Question.QuestionId,
                        QuestionText = tq.Question.QuestionText,
                        Answer = tq.Question.Answer,
                        Explanation = tq.Question.Explanation,
                        SubjectId = tq.Question.SubjectId,
                        SubjectName = tq.Question.Subject.Name,
                        LevelOfDifficultyId = tq.Question.LevelOfDifficultyId,
                        LevelOfDifficultyName = tq.Question.LevelOfDifficulty.Name
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (test == null) return NotFound();

            return Ok(test);
        }

        
        // POST: api/test
        [HttpPost]
        public async Task<ActionResult<TestDto>> CreateTest(CreateTestDto dto)
        {
            if (dto == null || dto.QuestionIds == null || !dto.QuestionIds.Any())
                return BadRequest("You must provide a list of question IDs.");

            // Ensure all question IDs exist
            var validQuestions = await _context.Questions
                .Where(q => dto.QuestionIds.Contains(q.QuestionId))
                .Select(q => q.QuestionId)
                .ToListAsync();

            if (!validQuestions.Any())
                return BadRequest("None of the provided QuestionIds are valid.");

            // Create the Test
            var test = new Test();
            _context.Tests.Add(test);
            await _context.SaveChangesAsync();

            // Map QuestionIds to TestQuestions
            foreach (var questionId in validQuestions)
            {
                _context.TestQuestions.Add(new TestQuestion
                {
                    TestId = test.TestId,
                    QuestionId = questionId
                });
            }

            await _context.SaveChangesAsync();

            // Map entity to DTO for response
            var createdTest = await _context.Tests
                .Include(t => t.TestQuestions)
                    .ThenInclude(tq => tq.Question)
                        .ThenInclude(q => q.Subject)
                .Include(t => t.TestQuestions)
                    .ThenInclude(tq => tq.Question)
                        .ThenInclude(q => q.LevelOfDifficulty)
                .Where(t => t.TestId == test.TestId)
                .Select(t => new TestDto
                {
                    TestId = t.TestId,
                    Questions = t.TestQuestions.Select(tq => new QuestionDto
                    {
                        QuestionId = tq.Question.QuestionId,
                        QuestionText = tq.Question.QuestionText,
                        Answer = tq.Question.Answer,
                        Explanation = tq.Question.Explanation,
                        SubjectId = tq.Question.SubjectId,
                        SubjectName = tq.Question.Subject.Name,
                        LevelOfDifficultyId = tq.Question.LevelOfDifficultyId,
                        LevelOfDifficultyName = tq.Question.LevelOfDifficulty.Name
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return CreatedAtAction(nameof(GetTestById), new { id = test.TestId }, createdTest);
        }


        // PUT: api/tests/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTestQuestions(int id, UpdateTestDto dto)
        {
            // Find the test including its current questions
            var test = await _context.Tests
                .Include(t => t.TestQuestions)
                .FirstOrDefaultAsync(t => t.TestId == id);

            if (test == null)
                return NotFound();

            if (dto.QuestionIds == null || !dto.QuestionIds.Any())
                return BadRequest("QuestionIds cannot be empty.");

            // Remove old TestQuestions
            _context.TestQuestions.RemoveRange(test.TestQuestions);

            // Add new TestQuestions
            test.TestQuestions = dto.QuestionIds.Select(qId => new TestQuestion
            {
                TestId = test.TestId,
                QuestionId = qId
            }).ToList();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/tests/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTest(int id)
        {
            var test = await _context.Tests
                .Include(t => t.TestQuestions) // Include related questions to handle cascade or cleanup
                .FirstOrDefaultAsync(t => t.TestId == id);

            if (test == null)
                return NotFound();

            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }

}