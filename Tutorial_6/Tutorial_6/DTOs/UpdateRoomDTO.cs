using System.ComponentModel.DataAnnotations;

namespace Tutorial_6.DTOs;

public class UpdateRoomDTO
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string BuildingCode { get; set; }

    public int Floor { get; set; }

    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }

    public bool HasProjector { get; set; }
    public bool IsActive { get; set; }
}
