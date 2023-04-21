using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuoteModel;
using System.Diagnostics.Metrics;

namespace QuoteGenius_Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuoteController : ControllerBase
{

    private readonly QuoteGeniusContext _context;

    public QuoteController(QuoteGeniusContext context)
    {
        _context = context;
    }

    // GET: api/Quotes
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
    {
        return await _context.Quotes.ToListAsync();
    }

    // GET: api/Quotes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Quote>> GetQuote(int id)
    {
        Quote? quote = await _context.Quotes.FindAsync(id);
        return quote == null ? NotFound() : quote;
    }

    // PUT: api/Quotes/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutQuote(int id, Quote quote)
    {
        if (id != quote.Id)
        {
            return BadRequest();
        }

        _context.Entry(quote).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!QuoteExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/Quotes
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Quote>> PostQuote(Quote quote)
    {
        _context.Quotes.Add(quote);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetQuote", new { id = quote.Id }, quote);
    }

    // DELETE: api/Quotes/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteQuote(int id)
    {
        Quote? quote = await _context.Quotes.FindAsync(id);
        if (quote == null)
        {
            return NotFound();
        }

        _context.Quotes.Remove(quote);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool QuoteExists(int id) => _context.Quotes.Any(e => e.Id == id);
}

