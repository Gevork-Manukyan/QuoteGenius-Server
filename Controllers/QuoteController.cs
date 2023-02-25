using Microsoft.AspNetCore.Mvc;

namespace QuoteGenius_Server.Controllers;

[ApiController]
[Route("[controller]")]
public class QuoteController : ControllerBase
{
    private static readonly string[] Quotes = new[]
    {
        "Be the change that you wish to see in the world.",
        "The only way to do great work is to love what you do.",
        "In three words I can sum up everything I've learned about life: it goes on.",
        "I have not failed. I've just found 10,000 ways that won't work.",
        "Believe you can and you're halfway there.",
        "The best way to predict the future is to invent it.",
        "If you want to live a happy life, tie it to a goal, not to people or things.",
        "Success is not final, failure is not fatal: it is the courage to continue that counts.",
        "To be yourself in a world that is constantly trying to make you something else is the greatest accomplishment.",
        "I am not a product of my circumstances. I am a product of my decisions." 
    };

    private static readonly string[] Authors = new[]
    {
        "Mahatma Gandhi",
        "Steve Jobs",
        "Robert Frost",
        "Thomas Edison",
        "Theodore Roosevelt",
        "Alan Kay",
        "Albert Einstein",
        "Winston Churchill",
        "Ralph Waldo Emerson",
        "Stephen Covey"
    };

    private readonly ILogger<QuoteController> _logger;

    public QuoteController(ILogger<QuoteController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetQuote")]
    public IEnumerable<Quote> Get()
    {


        return Enumerable.Range(1, 5).Select(index => new Quote
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),

            Text = Quotes[Random.Shared.Next(Quotes.Length)],

            Author = Authors[Random.Shared.Next(Authors.Length)]

            //TemperatureC = Random.Shared.Next(-20, 55),
        })
        .ToArray();
    }
}

