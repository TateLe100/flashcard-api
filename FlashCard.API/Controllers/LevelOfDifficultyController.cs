using FlashCard.API.Data;
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
        public async Task<ActionResult<IEnumerable<LevelOfDifficulty>>> GetLevelsOfDifficulty()
        {
            var levels = await _context.LevelOfDifficulties.ToListAsync();
            return Ok(levels);
        }

        // GET: api/levelofdifficulty/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LevelOfDifficulty>> GetLevelOfDifficultyById(int id)
        {
            var level = await _context.LevelOfDifficulties.FindAsync(id);

            if (level == null)
                return NotFound();

            return Ok(level);
        }

        // POST: api/levelofdifficulty
        [HttpPost]
        public async Task<ActionResult<LevelOfDifficulty>> CreateLevelOfDifficulty(LevelOfDifficulty level)
        {
            _context.LevelOfDifficulties.Add(level);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLevelOfDifficultyById), new { id = level.LevelOfDifficultyId }, level);
        }
    }
}