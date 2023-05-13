using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuoteModel;

[Table("Quote")]
public partial class Quote
{
    public int Id { get; set; }

    [Column("text")]
    [StringLength(150)]
    public string Text { get; set; } = null!;

    [Column("date_published", TypeName = "date")]
    public DateTime DatePublished { get; set; }

    [Column("author_id")]
    public int AuthorId { get; set; }

    public virtual Author Author { get; set; } = null!;
}
