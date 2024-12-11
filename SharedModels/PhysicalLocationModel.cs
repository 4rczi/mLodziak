using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class PhysicalLocationModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public int LocationId { get; set; }
        public int CategoryId { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int AlertStartEventMinutes { get; set; }
        public int AlertEndEventMinutes { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public float Radius { get; set; }
        public bool IsVisited { get; set; }
        public bool IsOmmitted { get; set; }
    }
}
