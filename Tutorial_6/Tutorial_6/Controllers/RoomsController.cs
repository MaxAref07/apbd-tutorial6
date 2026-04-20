using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial_6.DTOs;
using Tutorial_6.Models;

namespace Tutorial_6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        public static List<RoomModel> Rooms = new List<RoomModel>()
        {
            new RoomModel(1, "OlegRoom", "A", 1, 20, true, true),
            new RoomModel(2, "MaxRoom", "A", 2, 15, false, true),
            new RoomModel(3, "SocialRoom", "B", 1, 30, true, false),
            new RoomModel(4, "GreenRoom", "B", 3, 10, false, true),
            new RoomModel(5, "RedRum", "C", 2, 40, true, true)
        };

        [HttpGet]
        public ActionResult<List<RoomModel>> GetAll([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
        {
            var result = Rooms.AsEnumerable();

            if (minCapacity.HasValue)
                result = result.Where(x => x.Capacity >= minCapacity.Value);

            if (hasProjector.HasValue)
                result = result.Where(x => x.HasProjector == hasProjector.Value);

            if (activeOnly.HasValue && activeOnly.Value)
                result = result.Where(x => x.IsActive);

            return Ok(result.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<RoomModel> GetById([FromRoute] int id)
        {
            var room = Rooms.FirstOrDefault(x => x.Id == id);

            if (room == null)
                return NotFound();

            return Ok(room);
        }

        [HttpGet("building/{buildingCode}")]
        public ActionResult<List<RoomModel>> GetByBuilding([FromRoute] string buildingCode)
        {
            var rooms = Rooms.Where(x => x.BuildingCode == buildingCode).ToList();
            return Ok(rooms);
        }

        [HttpPost]
        public ActionResult<RoomModel> Create([FromBody] RoomDTO createRoomDto)
        {
            var newId = Rooms.Any() ? Rooms.Max(x => x.Id) + 1 : 1;

            var room = new RoomModel(
                newId,
                createRoomDto.Name,
                createRoomDto.BuildingCode,
                createRoomDto.Floor,
                createRoomDto.Capacity,
                createRoomDto.HasProjector,
                createRoomDto.IsActive);

            Rooms.Add(room);

            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }

        [HttpPut("{id}")]
        public ActionResult<RoomModel> Update([FromRoute] int id, [FromBody] UpdateRoomDTO updateRoomDto)
        {
            var foundRoom = Rooms.FirstOrDefault(x => x.Id == id);
            if (foundRoom == null)
                return NotFound();

            var updated = new RoomModel(
                foundRoom.Id,
                updateRoomDto.Name,
                updateRoomDto.BuildingCode,
                updateRoomDto.Floor,
                updateRoomDto.Capacity,
                updateRoomDto.HasProjector,
                updateRoomDto.IsActive);

            Rooms[Rooms.IndexOf(foundRoom)] = updated;

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var foundRoom = Rooms.FirstOrDefault(x => x.Id == id);
            if (foundRoom == null)
                return NotFound();

            if (ReservationsController.Reservations.Any(x => x.RoomId == id))
                return Conflict();

            Rooms.Remove(foundRoom);
            return NoContent();
        }
    }
}
