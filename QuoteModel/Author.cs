using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuoteModel;

[Table("Author")]
public partial class Author
{
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("birthday", TypeName = "date")]
    public DateTime Birthday { get; set; }

    [Column("race")]
    [StringLength(50)]
    public string Race { get; set; } = null!;

    [Column("gender")]
    [StringLength(10)]
    public string Gender { get; set; } = null!;

    public virtual ICollection<Quote> Quotes { get; } = new List<Quote>();
}
