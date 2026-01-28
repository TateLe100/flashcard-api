using FlashCard.API.Data;
using FlashCard.API.DTOs;
using FlashCard.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.API.Controllers
{
    

    [ApiController]
    [Route("api/[controller]")]
    public class LevelOfDifficultyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LevelOfDifficultyController(AppDbContext context)
        {
            _context = context;
        }


        // GET: api/levelofdifficulty
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LevelOfDifficultyDto>>> GetLevelsOfDifficulty()
        {
            var levels = await _context.LevelOfDifficulties.Select(l => new LevelOfDifficultyDto
            {
                LevelOfDifficultyId = l.LevelOfDifficultyId,
                Name = l.Name
            }).ToListAsync();
            return Ok(levels);
        }

        // GET: api/levelofdifficulty/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LevelOfDifficultyDto>> GetLevelOfDifficultyById(int id)
        {
            var level = await _context.LevelOfDifficulties.FindAsync(id);

            if (level == null)
                return NotFound();

            return Ok(new LevelOfDifficultyDto
            {
                LevelOfDifficultyId = level.LevelOfDifficultyId,
                Name = level.Name
            });
        }

        // POST: api/levelofdifficulty
        [HttpPost]
        public async Task<ActionResult<LevelOfDifficultyDto>> CreateLevelOfDifficulty(LevelOfDifficulty level)
        {
            _context.LevelOfDifficulties.Add(level);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetLevelOfDifficultyById),
                new { id = level.LevelOfDifficultyId },
                new LevelOfDifficultyDto
                {
                    LevelOfDifficultyId = level.LevelOfDifficultyId,
                    Name = level.Name
                });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLevelOfDifficulty(int id, LevelOfDifficulty lvl)
        {
            var level = await _context.LevelOfDifficulties.FindAsync(id);

            if (level == null)
                return NotFound();

            level.Name = lvl.Name;

            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        // DELETE: api/levelofdifficulty/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLevelOfDifficulty(int id)
        {
            var level = await _context.LevelOfDifficulties.FindAsync(id);

            if (level == null)
                return NotFound();

            _context.LevelOfDifficulties.Remove(level);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}