using FlashCard.API.Data;
using FlashCard.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SubjectsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/subjects
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects()
    {
        var subjects = await _context.Subjects
            .Select(s => new SubjectDto
            {
                SubjectId = s.SubjectId,
                Name = s.Name
            })
            .ToListAsync();

        return Ok(subjects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubjectDto>> GetSubjectById(int id)
    {
        var subject = await _context.Subjects
            .FirstOrDefaultAsync(s => s.SubjectId == id);

        if (subject == null)
            return NotFound();

        return Ok(new SubjectDto
        {
            SubjectId = subject.SubjectId,
            Name = subject.Name
        });
    }



    // POST: api/subjects
    [HttpPost]
    public async Task<ActionResult<SubjectDto>> CreateSubject(CreateSubjectDto dto)
    {
        var subject = new Subject
        {
            Name = dto.Name
        };

        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();

        var resultDto = new SubjectDto
        {
            SubjectId = subject.SubjectId,
            Name = subject.Name
        };

        return CreatedAtAction(
            nameof(GetSubjectById),
            new { id = subject.SubjectId },
            resultDto
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubject(int id, CreateSubjectDto dto)
    {
        var subject = await _context.Subjects.FindAsync(id);

        if (subject == null)
            return NotFound();

        subject.Name = dto.Name;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    
    // DELETE: api/subjects/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);

        if (subject == null)
            return NotFound();

        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}