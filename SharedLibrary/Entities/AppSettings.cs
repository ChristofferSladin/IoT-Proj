
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Entities;

public class AppSettings
{
    [Key]
    public string ConnectionString { get; set; } = null!;
}
