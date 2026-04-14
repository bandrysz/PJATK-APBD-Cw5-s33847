using Microsoft.AspNetCore.Mvc;
using PJATK_APBD_Cw5_s33847.Models;

namespace PJATK_APBD_Cw5_s33847.Controllers;


[ApiController ]
[Route("api/[controller]")]
public class ReservationsController :ControllerBase
{
    public static List<Reservation> reservations =
    [
        new Reservation
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "Jan Kowalski",
            Topic = "C#",
            Date = new DateTime(2026, 5, 10),
            StartTime = new TimeSpan(8, 0, 0),
            EndTime = new TimeSpan(10, 0, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 2,
            RoomId = 1,
            OrganizerName = "Anna Nowak",
            Topic = "GUI",
            Date = new DateTime(2026, 5, 10),
            StartTime = new TimeSpan(10, 30, 0),
            EndTime = new TimeSpan(12, 30, 0),
            Status = "planned"
        },
        new Reservation
        {
            Id = 3,
            RoomId = 2,
            OrganizerName = "Piotr Zieliński",
            Topic = "MAD",
            Date = new DateTime(2026, 5, 11),
            StartTime = new TimeSpan(9, 0, 0),
            EndTime = new TimeSpan(11, 0, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 4,
            RoomId = 3,
            OrganizerName = "Maria Wiśniewska",
            Topic = "SQL",
            Date = new DateTime(2026, 5, 12),
            StartTime = new TimeSpan(13, 0, 0),
            EndTime = new TimeSpan(15, 0, 0),
            Status = "cancelled"
        }

    ];
    
    // GET /api/reservations
    /*[HttpGet]
    public IActionResult GetAll()
    {
        return Ok(reservations.Select(e => new Reservation()
        {
            Id = e.Id,
            RoomId = e.RoomId,
            OrganizerName = e.OrganizerName,
            Topic = e.Topic,
            Date = e.Date,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Status = e.Status
        }));
    }*/
    // GET /api/reservations/1
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var e = reservations.FirstOrDefault(x => x.Id == id);

        if (e is null)
        {
            return NotFound($"Rezerwacja o id: {id} nie istnieje");
        }
        
        return Ok(new Reservation()
        {
            Id = e.Id,
            RoomId = e.RoomId,
            OrganizerName = e.OrganizerName,
            Topic = e.Topic,
            Date = e.Date,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Status = e.Status
            
        });
    }
    //GET /api/reservations?date=2026-05-10&status=confirmed&roomId=2
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] DateTime? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        var result = reservations.AsEnumerable();

        if (date.HasValue)
            result = result.Where(r => r.Date.Date == date.Value.Date);

        if (status!=null)
            result = result.Where(r => r.Status.Equals(status));

        if (roomId.HasValue)
            result = result.Where(r => r.RoomId==roomId.Value);

        return Ok(result);
    }
    //POST api/reserations
    [HttpPost]
    public IActionResult Post([FromBody] Reservation e)
    {
        var room = RoomsController.rooms.FirstOrDefault(r => r.ID == e.RoomId);

        if (room is null)
            return NotFound("Sala nie istnieje");

        if (!room.IsActive)
            return Conflict("Sala jest nieaktywna");
        
        var collision = reservations.Any(r =>
            r.RoomId == e.RoomId &&
            r.Date.Date == e.Date.Date &&
            e.StartTime < r.EndTime &&
            e.EndTime > r.StartTime);

        if (collision)
            return Conflict("Kolizja rezerwacji");
        
        e.Id = reservations.Max(r=>r.Id)+1;
        reservations.Add(e);
        return CreatedAtAction(nameof(GetById), new { id = e.Id }, e);
    }
    // PUT /api/reservations/1
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Reservation e)
    {
        var room = RoomsController.rooms.FirstOrDefault(r => r.ID == e.RoomId);

        if (room is null)
            return NotFound("Sala nie istnieje");

        if (!room.IsActive)
            return Conflict("Sala jest nieaktywna");
        
        var collision = reservations.Any(r =>
            r.Id !=id &&
            r.RoomId == e.RoomId &&
            r.Date.Date == e.Date.Date &&
            e.StartTime < r.EndTime &&
            e.EndTime > r.StartTime);

        if (collision)
            return Conflict("Kolizja rezerwacji");

        var reservation = reservations.FirstOrDefault(x => x.Id == id);
        
        if (reservation is null)
        {
            return NotFound($"Rezerwacja o id: {id} nie istnieje");
        }
        reservation.RoomId = e.RoomId;
        reservation.OrganizerName = e.OrganizerName;
        reservation.Topic = e.Topic;
        reservation.Date = e.Date;
        reservation.StartTime = e.StartTime;
        reservation.EndTime = e.EndTime;
        reservation.Status = e.Status;
        
        return NoContent();
    }
    // DELETE /api/reservations/1
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var reservation = reservations.FirstOrDefault(x => x.Id == id);
        
        if (reservation is null)
        {
            return NotFound($"Rezerwacja o id: {id} nie istnieje");
        }
        
        reservations.Remove(reservation);
        
        return NoContent();
    }

}