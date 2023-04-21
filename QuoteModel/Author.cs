using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuoteModel;

[Table("Author")]
public partial class Author
{
    [Key]
    [Column("id")]
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

    [InverseProperty("Author")]
    public virtual ICollection<Quote> Quotes { get; } = new List<Quote>();
}
