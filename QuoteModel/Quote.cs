using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuoteModel;

[Table("Quote")]
public partial class Quote
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("text")]
    [StringLength(150)]
    public string Text { get; set; } = null!;

    [Column("date_published", TypeName = "date")]
    public DateTime DatePublished { get; set; }

    [Column("author_id")]
    public int AuthorId { get; set; }

    [ForeignKey("AuthorId")]
    [InverseProperty("Quotes")]
    public virtual Author Author { get; set; } = null!;
}
