using System.ComponentModel.DataAnnotations;

namespace Tutorial_6.DTOs;

public class ReservationDTO
{
    public int RoomId { get; set; }

    [Required]
    public string OrganizerName { get; set; }

    [Required]
    public string Topic { get; set; }

    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    [Required]
    public string Status { get; set; }
}
