using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuoteGenius_Server.DTOs;
using QuoteModel;
using System.Threading.Tasks;


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

    // GET: api/Quotes/WithAuthors
    [HttpGet("WithAuthors")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<QuoteWithAuthor>>> GetQuotesWithAuthors()
    {
        var quotesWithAuthors = await _context.Quotes
            .Include(q => q.Author)
            .Select(q => new QuoteWithAuthor
            {
                QuoteId = q.Id,
                QuoteText = q.Text,
                DatePublished = q.DatePublished,
                AuthorId = q.Author.Id,
                AuthorName = q.Author.Name,
                AuthorBirthday = q.Author.Birthday,
                AuthorRace = q.Author.Race,
                AuthorGender = q.Author.Gender
            })
            .ToListAsync();

        return quotesWithAuthors;
    }

    // GET: api/Quotes/WithAuthor/{id}
    [HttpGet("WithAuthor/{id}")]
    [Authorize]
    public async Task<ActionResult<QuoteWithAuthor>> GetQuoteWithAuthor(int id)
    {
        var quoteWithAuthor = await _context.Quotes
            .Include(q => q.Author)
            .Where(q => q.Id == id)
            .Select(q => new QuoteWithAuthor
            {
                QuoteId = q.Id,
                QuoteText = q.Text,
                DatePublished = q.DatePublished,
                AuthorId = q.Author.Id,
                AuthorName = q.Author.Name,
                AuthorBirthday = q.Author.Birthday,
                AuthorRace = q.Author.Race,
                AuthorGender = q.Author.Gender
            })
            .SingleOrDefaultAsync();

        if (quoteWithAuthor == null)
        {
            return NotFound();
        }

        return quoteWithAuthor;
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

    // PUT: api/Quotes/WithAuthor/{id}
    [HttpPut("WithAuthor/{id}")]
    [Authorize]
    public async Task<IActionResult> PutQuoteWithAuthor(int id, QuoteWithAuthor quoteWithAuthor)
    {
        Console.WriteLine("CHECK POINT 1");

        if (id != quoteWithAuthor.QuoteId)
        {
            return BadRequest();
        }

        Console.WriteLine("CHECK POINT 2");

        var quote = await _context.Quotes.FindAsync(id);

        if (quote == null)
        {
            return NotFound();
        }

        Console.WriteLine("CHECK POINT 3");

        var author = await _context.Authors.FindAsync(quoteWithAuthor.AuthorId);

        if (author == null)
        {
            return NotFound();
        }

        Console.WriteLine("CHECK POINT 4");

        // Update the quote properties
        quote.Text = quoteWithAuthor.QuoteText;
        quote.DatePublished = quoteWithAuthor.DatePublished;

        // Update the author properties
        author.Name = quoteWithAuthor.AuthorName;
        author.Birthday = quoteWithAuthor.AuthorBirthday;
        author.Race = quoteWithAuthor.AuthorRace;
        author.Gender = quoteWithAuthor.AuthorGender;

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

