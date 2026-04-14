using System.ComponentModel.DataAnnotations;

namespace PJATK_APBD_Cw5_s33847.Models;

public class Room
{
    public int ID { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string BuildingCode { get; set; }
    public int Floor { get; set; }
    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }
    public  bool HasProjector  { get; set; }
    public bool IsActive { get; set; }
}