using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class PhysicalLocation : BaseEntity
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int AlertStartEventMinutes { get; set; }
        public int AlertEndEventMinutes { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public float Radius { get; set; }
        public Location? Location { get; set; }
        public ICollection<UserHistory>? UserHistory { get; set; }
    }
}
