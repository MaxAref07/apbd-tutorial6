namespace Tutorial_6.Models;

public class RoomModel
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string BuildingCode { get; set; }
    public int Floor { get; set; }
    public int Capacity { get; set; }
    public bool HasProjector { get; set; }
    public bool IsActive { get; set; }

    public RoomModel(int id, string name, string buildingCode, int floor, int capacity, bool hasProjector, bool isActive)
    {
        Id = id;
        Name = name;
        BuildingCode = buildingCode;
        Floor = floor;
        Capacity = capacity;
        HasProjector = hasProjector;
        IsActive = isActive;
    }
}
