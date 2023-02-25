namespace QuoteGenius_Server;

public class Quote
{
    public DateOnly Date { get; set; }

    public string? Text { get; set; }

    public string? Author { get; set; }

    //public int TemperatureC { get; set; }
    //public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

