﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class LocationModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public float ZoomLevel { get; set; }
        public int VisitedPhysicalLocationsCount { get; set; }
        public int PhysicalLocationsCount { get; set; }
    }
}
