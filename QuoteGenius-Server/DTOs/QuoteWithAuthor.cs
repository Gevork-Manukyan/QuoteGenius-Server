namespace QuoteGenius_Server.DTOs
{
    public class QuoteWithAuthor
    {
        public int QuoteId { get; set; }
        public string? QuoteText { get; set; }
        public DateTime DatePublished { get; set; }
        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public DateTime AuthorBirthday { get; set; }
        public string? AuthorRace { get; set; }
        public string? AuthorGender { get; set; }
    }
}
