using DataAccess.Entities;
using DataAccess.Repositories;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedServices
{
    public class PhysicalLocationService : IPhysicalLocationService
    {
        private readonly IPhysicalLocationRepository _physicalLocationRepository;
        private readonly IUserHistoryRepository _userHistoryRepository;

        public PhysicalLocationService(IPhysicalLocationRepository physicalLocationRepository, IUserHistoryRepository userHistoryRepository)
        {
            _physicalLocationRepository = physicalLocationRepository;
            _userHistoryRepository = userHistoryRepository;
        }

        public async Task<List<PhysicalLocationModel>> GetPhysicalLocationsAsync(string userId, int categoryId, int locationId)
        {
            var userHistory = await _userHistoryRepository.GetUserHistoryAsync(userId);
            var physicalLocations = await _physicalLocationRepository.GetPhysicalLocationsAsync();

            var visitedLocations = (from physLoc in userHistory
                                    where physLoc.UserId == userId
                                    select physLoc.PhysicalLocationId);

            var physicalLocationModel = from physLoc in physicalLocations
                                        join uh in userHistory.DefaultIfEmpty()
                                        on physLoc.Id equals uh?.PhysicalLocationId into joinResult
                                        where physLoc.CategoryId == categoryId && physLoc.LocationId == locationId
                                        select new PhysicalLocationModel
                                        {
                                            Id = physLoc.Id,
                                            Name = physLoc.Name,
                                            Description = physLoc.Description,
                                            ImagePath = physLoc.ImagePath,
                                            CategoryId = physLoc.CategoryId,
                                            LocationId = physLoc.LocationId,
                                            DateStart = physLoc.StartDate,
                                            DateEnd = physLoc.EndDate,
                                            AlertStartEventMinutes = physLoc.AlertStartEventMinutes,
                                            AlertEndEventMinutes = physLoc.AlertEndEventMinutes,
                                            Latitude = physLoc.Latitude,
                                            Longitude = physLoc.Longitude,
                                            Radius = physLoc.Radius,
                                            IsVisited = visitedLocations.Contains(physLoc.Id),
                                            IsOmmitted = CalculateOmittness(physLoc.StartDate, physLoc.EndDate)
                                        };

            return physicalLocationModel.ToList();
        }

        public async Task<List<PhysicalLocationModel>> GetAllPhysicalLocationsAsync(string userId)
        {
            var userHistory = await _userHistoryRepository.GetUserHistoryAsync(userId);
            var physicalLocations = await _physicalLocationRepository.GetPhysicalLocationsAsync();

            var visitedPhysicalLocations = (from physLoc in userHistory
                                    where physLoc.UserId == userId
                                    select physLoc.PhysicalLocationId);

            var physicalLocationModel = from physLoc in physicalLocations
                                        join uh in userHistory.DefaultIfEmpty()
                                        on physLoc.Id equals uh?.PhysicalLocationId into joinResult
                                        select new PhysicalLocationModel
                                        {
                                            Id = physLoc.Id,
                                            Name = physLoc.Name,
                                            Description = physLoc.Description,
                                            ImagePath = physLoc.ImagePath,
                                            CategoryId = physLoc.CategoryId,
                                            LocationId = physLoc.LocationId,
                                            DateStart = physLoc.StartDate,
                                            DateEnd = physLoc.EndDate,
                                            AlertStartEventMinutes = physLoc.AlertStartEventMinutes,
                                            AlertEndEventMinutes = physLoc.AlertEndEventMinutes,
                                            Latitude = physLoc.Latitude,
                                            Longitude = physLoc.Longitude,
                                            Radius = physLoc.Radius,
                                            IsVisited = visitedPhysicalLocations.Contains(physLoc.Id),
                                            IsOmmitted = CalculateOmittness(physLoc.StartDate, physLoc.EndDate)
                                        };

            return physicalLocationModel.ToList();
        }

        public bool CalculateOmittness(DateTime? start, DateTime? end)
        {
            var nowUTC = DateTime.UtcNow;

            var centralEuropeTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var centralEuropeTimeNow = TimeZoneInfo.ConvertTimeFromUtc(nowUTC, centralEuropeTimeZone);

            return centralEuropeTimeNow > end;
        }

        public async Task<List<PhysicalLocationModel>> GetVisitablePhysicalLocationsAsync(string userId)
        {
            var allPhysicalLocations = await GetAllPhysicalLocationsAsync(userId);
            var visitablePhysicalLocations = allPhysicalLocations.Where(physLoc => 
                                             physLoc.IsVisited == false &&
                                             physLoc.IsOmmitted == false &&
                                             (DateTime.UtcNow > physLoc.DateStart && DateTime.UtcNow < physLoc.DateEnd || 
                                             physLoc.DateStart is null && physLoc.DateEnd is null))
                                             .ToList();
            return visitablePhysicalLocations;
        }
    }
}
