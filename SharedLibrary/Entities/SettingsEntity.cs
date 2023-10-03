
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Entities;

public class SettingsEntity
{
    [Key]
    public string ConnectionString { get; set; } = null!;
}
