using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PJATK_APBD_Cw5_s33847.Models;

namespace PJATK_APBD_Cw5_s33847.Controllers;


[ApiController ]
[Route("api/[controller]")]
public class RoomsController :ControllerBase
{
    public static List<Room> rooms =
    [
        new Room()
        {
            ID = 1,
            Name = "Sala A101",
            BuildingCode = "A",
            Floor = 1,
            Capacity = 20,
            HasProjector = true,
            IsActive = true
        },
        new Room()
        {
            ID = 2,
            Name = "Sala A202",
            BuildingCode = "A",
            Floor = 2,
            Capacity = 30,
            HasProjector = false,
            IsActive = true
        },
        new Room()
        {
            ID = 3,
            Name = "Sala B204",
            BuildingCode = "B",
            Floor = 2,
            Capacity = 24,
            HasProjector = true,
            IsActive = true
        },
        new Room()
        {
            ID = 4,
            Name = "Sala C010",
            BuildingCode = "C",
            Floor = 0,
            Capacity = 15,
            HasProjector = false,
            IsActive = false
        },
        new Room()
        {
            ID = 5,
            Name = "Aula B300",
            BuildingCode = "B",
            Floor = 3,
            Capacity = 80,
            HasProjector = true,
            IsActive = true
        }

    ];
    
    // GET /api/rooms
    /*[HttpGet]
    public IActionResult GetAll()
    {
        return Ok(rooms.Select(e => new Room()
        {
            ID = e.ID,
            Name = e.Name,
            BuildingCode = e.BuildingCode,
            Floor = e.Floor,
            Capacity = e.Capacity,
            HasProjector = e.HasProjector,
            IsActive = e.IsActive
        }));
    }*/
    
    // GET /api/rooms/1
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var e = rooms.FirstOrDefault(x => x.ID == id);

        if (e is null)
        {
            return NotFound($"Pokoj o id: {id} nie istnieje");
        }
        
        return Ok(new Room
        {
            ID = e.ID,
            Name = e.Name,
            BuildingCode = e.BuildingCode,
            Floor = e.Floor,
            Capacity = e.Capacity,
            HasProjector = e.HasProjector,
            IsActive = e.IsActive
            
        });
    }
    // GET /api/rooms/building/A
    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuildingCode(string buildingCode)
    {
        var e = rooms.Where((r => r.BuildingCode == buildingCode));
        return Ok(e);
    }
    //GET /api/rooms?minCapacity=20&hasProjector=true&activeOnly=true
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        var result = rooms.AsEnumerable();

        if (minCapacity.HasValue)
            result = result.Where(r => r.Capacity >= minCapacity.Value);

        if (hasProjector.HasValue)
            result = result.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly == true)
            result = result.Where(r => r.IsActive);

        return Ok(result);
    }
    
    //POST api/rooms
    [HttpPost]
    public IActionResult Post([FromBody] Room e)
    {
        e.ID = rooms.Max(r=>r.ID)+1;
        rooms.Add(e);
        return CreatedAtAction(nameof(GetById), new { id = e.ID }, e);
    }
    // PUT /api/rooms/1
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Room e)
    {
        var room = rooms.FirstOrDefault(x => x.ID == id);
        
        if (room is null)
        {
            return NotFound($"Pokoj o id: {id} nie istnieje");
        }
        room.Name = e.Name;
        room.BuildingCode = e.BuildingCode;
        room.Floor = e.Floor;
        room.Capacity = e.Capacity;
        room.HasProjector = e.HasProjector;
        room.IsActive = e.IsActive;
        
        return NoContent();
    }
    // DELETE /api/rooms/1
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var room = rooms.FirstOrDefault(x => x.ID == id);
        
        if (room is null)
        {
            return NotFound($"Pokoj o id: {id} nie istnieje");
        }
        
        rooms.Remove(room);
        
        return NoContent();
    }
}