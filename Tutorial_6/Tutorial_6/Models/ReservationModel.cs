namespace Tutorial_6.Models;

public class ReservationModel
{
    public int Id { get; init; }
    public int RoomId { get; set; }
    public string OrganizerName { get; set; }
    public string Topic { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Status { get; set; }

    public ReservationModel(int id, int roomId, string organizerName, string topic,
        DateTime date, TimeSpan startTime, TimeSpan endTime, string status)
    {
        Id = id;
        RoomId = roomId;
        OrganizerName = organizerName;
        Topic = topic;
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
    }
}
