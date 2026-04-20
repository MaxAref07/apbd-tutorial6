using Microsoft.AspNetCore.Mvc;
using Tutorial_6.DTOs;
using Tutorial_6.Models;

namespace Tutorial_6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        public static List<ReservationModel> Reservations = new List<ReservationModel>()
        {
            new ReservationModel(1, 1, "Oleg Suchilin", "Time scheduling",
                new DateTime(2026, 5, 10), new TimeSpan(10, 0, 0), new TimeSpan(12, 0, 0), "confirmed"),
            new ReservationModel(2, 2, "Max Arefiev", "Intern Walkthrough",
                new DateTime(2026, 5, 10), new TimeSpan(9, 0, 0), new TimeSpan(10, 30, 0), "planned"),
            new ReservationModel(3, 2, "Ego Lipatov", "Consultation",
                new DateTime(2026, 5, 11), new TimeSpan(14, 0, 0), new TimeSpan(15, 0, 0), "confirmed"),
            new ReservationModel(4, 4, "Travis Scott", "Strategical Planning",
                new DateTime(2026, 5, 12), new TimeSpan(11, 0, 0), new TimeSpan(13, 0, 0), "cancelled"),
            new ReservationModel(5, 5, "Ken Carson", "Sales Strategy",
                new DateTime(2026, 5, 13), new TimeSpan(9, 0, 0), new TimeSpan(11, 0, 0), "confirmed")
        };

        [HttpGet]
        public ActionResult<List<ReservationModel>> GetAll([FromQuery] DateTime? date, [FromQuery] string? status, [FromQuery] int? roomId)
        {
            var result = Reservations.AsEnumerable();

            if (date.HasValue)
                result = result.Where(x => x.Date.Date == date.Value.Date);

            if (!string.IsNullOrEmpty(status))
                result = result.Where(x => string.Equals(x.Status, status, StringComparison.OrdinalIgnoreCase));

            if (roomId.HasValue)
                result = result.Where(x => x.RoomId == roomId.Value);

            return Ok(result.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<ReservationModel> GetById([FromRoute] int id)
        {
            var reservation = Reservations.FirstOrDefault(x => x.Id == id);

            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }

        [HttpPost]
        public ActionResult<ReservationModel> Create([FromBody] ReservationDTO createReservationDto)
        {
            if (createReservationDto.EndTime <= createReservationDto.StartTime)
                return BadRequest("EndTime must be later than StartTime");

            var room = RoomsController.Rooms.FirstOrDefault(x => x.Id == createReservationDto.RoomId);
            if (room == null)
                return NotFound();

            if (!room.IsActive)
                return BadRequest("Room is inactive");

            var overlap = Reservations.Any(x =>
                x.RoomId == createReservationDto.RoomId &&
                x.Date.Date == createReservationDto.Date.Date &&
                x.StartTime < createReservationDto.EndTime &&
                createReservationDto.StartTime < x.EndTime);

            if (overlap)
                return Conflict();

            var newId = Reservations.Any() ? Reservations.Max(x => x.Id) + 1 : 1;

            var reservation = new ReservationModel(
                newId,
                createReservationDto.RoomId,
                createReservationDto.OrganizerName,
                createReservationDto.Topic,
                createReservationDto.Date,
                createReservationDto.StartTime,
                createReservationDto.EndTime,
                createReservationDto.Status);

            Reservations.Add(reservation);

            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id}")]
        public ActionResult<ReservationModel> Update([FromRoute] int id, [FromBody] UpdateReservationDTO updateReservationDto)
        {
            var found = Reservations.FirstOrDefault(x => x.Id == id);
            if (found == null)
                return NotFound();

            if (updateReservationDto.EndTime <= updateReservationDto.StartTime)
                return BadRequest("EndTime must be later than StartTime");

            var room = RoomsController.Rooms.FirstOrDefault(x => x.Id == updateReservationDto.RoomId);
            if (room == null)
                return NotFound();

            if (!room.IsActive)
                return BadRequest("Room is inactive");

            var overlap = Reservations.Any(x =>
                x.Id != id &&
                x.RoomId == updateReservationDto.RoomId &&
                x.Date.Date == updateReservationDto.Date.Date &&
                x.StartTime < updateReservationDto.EndTime &&
                updateReservationDto.StartTime < x.EndTime);

            if (overlap)
                return Conflict();

            var updated = new ReservationModel(
                found.Id,
                updateReservationDto.RoomId,
                updateReservationDto.OrganizerName,
                updateReservationDto.Topic,
                updateReservationDto.Date,
                updateReservationDto.StartTime,
                updateReservationDto.EndTime,
                updateReservationDto.Status);

            Reservations[Reservations.IndexOf(found)] = updated;

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var found = Reservations.FirstOrDefault(x => x.Id == id);
            if (found == null)
                return NotFound();

            Reservations.Remove(found);
            return NoContent();
        }
    }
}
