using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuoteModel;

namespace QuoteGenius_Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly QuoteGeniusContext _context;

    public AuthorController(QuoteGeniusContext context)
    {
        _context = context;
    }

    // GET: api/Author
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
    {
        return await _context.Authors.ToListAsync();
    }

    // GET: api/Author/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetAuthor(int id)
    {
        Author? author = await _context.Authors.FindAsync(id);
        return author == null ? NotFound() : author;
    }

    // POST: api/Author
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Author>> PostAuthor(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
    }

    // PUT: api/Author/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutAuthor(int id, Author author)
    {
        if (id != author.Id)
        {
            return BadRequest();
        }

        _context.Entry(author).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuthorExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // DELETE: api/Author/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        Author? author = await _context.Authors.FindAsync(id);
        if (author == null)
        {
            return NotFound();
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AuthorExists(int id) => _context.Authors.Any(e => e.Id == id);
}
