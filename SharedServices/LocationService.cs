using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using SharedModels;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace SharedServices
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IUserHistoryRepository _userHistoryRepository;
        private readonly IPhysicalLocationRepository _physicalLocationRepository;

        public LocationService(ILocationRepository locationRepository,
                               IUserHistoryRepository userLocationHistoryRepository,
                               IPhysicalLocationRepository physicalLocationRepository)
        {
            _locationRepository = locationRepository;
            _userHistoryRepository = userLocationHistoryRepository;
            _physicalLocationRepository = physicalLocationRepository;
        }
        public async Task<List<LocationModel>> GetLocationModelsAsync(string userId, int categoryId)
        {
            var userHistory = await _userHistoryRepository.GetUserHistoryAsync(userId);
            var physicalLocation = await _physicalLocationRepository.GetPhysicalLocationsAsync();
            var locations = await _locationRepository.GetLocationsAsync();


            var visitedLocations = from pl in physicalLocation
                                   join uh in userHistory.DefaultIfEmpty()
                                   on pl.Id equals uh?.PhysicalLocationId into joinResult
                                   where pl.CategoryId == categoryId
                                   group joinResult by new { pl.LocationId, pl.CategoryId } into aggregated_query
                                   select new
                                   {
                                       aggregated_query.Key.LocationId,
                                       aggregated_query.Key.CategoryId,
                                       visitedLocationsCount = aggregated_query.Count(uh => uh != null && uh.Any(u => u.UserId == userId)),
                                       physLocationsCount = aggregated_query.Count()
                                   };

            var locationModelList = from vl in visitedLocations
                                    join l in locations
                                    on new { LocationId = vl.LocationId, CategoryId = vl.CategoryId } equals new { LocationId = l.Id, CategoryId = l.CategoryId } into joined
                                    from l in joined.DefaultIfEmpty()
                                    orderby vl.LocationId ascending
                                    select new LocationModel
                                    {
                                        Id = l.Id,
                                        CategoryId = l.CategoryId,
                                        Name = l.Name,
                                        Description = l.Description,
                                        ImagePath = l.ImagePath,
                                        Latitude = l.Latitude,
                                        Longitude = l.Longitude,
                                        ZoomLevel = l.ZoomLevel,
                                        VisitedPhysicalLocationsCount = vl.visitedLocationsCount,
                                        PhysicalLocationsCount = vl.physLocationsCount
                                    };

            return locationModelList.ToList();

        }

        public async Task<Dictionary<int, List<LocationModel>>> GetAllLocationModelsAsync(string userId)
        {
            var userHistory = await _userHistoryRepository.GetUserHistoryAsync(userId);
            var physicalLocation = await _physicalLocationRepository.GetPhysicalLocationsAsync();
            var locations = await _locationRepository.GetLocationsAsync();


            var visitedLocations = from pl in physicalLocation
                                   join uh in userHistory.DefaultIfEmpty()
                                   on pl.Id equals uh?.PhysicalLocationId into joinResult /// Uh?
                                   group joinResult by new { pl.LocationId, pl.CategoryId } into aggregated_query
                                   select new
                                   {
                                       aggregated_query.Key.LocationId,
                                       aggregated_query.Key.CategoryId,
                                       visitedLocationsCount = aggregated_query.Count(uh => uh != null && uh.Any(u => u.UserId == userId)),
                                       physLocationsCount = aggregated_query.Count()
                                   };

            var allLocationModelList = from vl in visitedLocations
                                       join l in locations
                                       on new { LocationId = vl.LocationId, CategoryId = vl.CategoryId } equals new { LocationId = l.Id, CategoryId = l.CategoryId } into joined
                                       from l in joined.DefaultIfEmpty()
                                       orderby vl.CategoryId ascending, vl.LocationId ascending
                                       select new LocationModel
                                       {
                                           Id = l.Id,
                                           CategoryId = l.CategoryId,
                                           Name = l.Name,
                                           Description = l.Description,
                                           ImagePath = l.ImagePath,
                                           Latitude = l.Latitude,
                                           Longitude = l.Longitude,
                                           ZoomLevel = l.ZoomLevel,
                                           VisitedPhysicalLocationsCount = vl.visitedLocationsCount,
                                           PhysicalLocationsCount = vl.physLocationsCount
                                       };

            var locationDictionary = allLocationModelList.GroupBy(lm => lm.CategoryId)
                                                         .ToDictionary(entry => entry.Key, entry => entry.ToList());

            return locationDictionary;
        }

        public async Task<LocationModel> GetLocationModelAsync(int physicalLocationId, string userId)
        {
            var physicalLocation = await _physicalLocationRepository.GetPhysicalLocationAsync(physicalLocationId);
            var location = physicalLocation.Location;
            var userHistory = await _userHistoryRepository.GetUserHistoryAsync(userId);

            var visitedLocations = (from pl in new[] { physicalLocation } // Casting single value to array to be able to perform join operation
                                    join uh in userHistory.DefaultIfEmpty()
                                    on pl.Id equals uh?.PhysicalLocationId into joinResult
                                    where pl.Id == physicalLocationId
                                    group joinResult by new { pl.LocationId, pl.CategoryId } into aggregated_query
                                    select new
                                    {
                                        aggregated_query.Key.LocationId,
                                        aggregated_query.Key.CategoryId,
                                        visitedLocationsCount = aggregated_query.Count(uh => uh != null && uh.Any(u => u.UserId == userId)),
                                        physLocationsCount = aggregated_query.Count()
                                    })
                                    .SingleOrDefault();

            var locationModel = new LocationModel()
            {
                Id = location.Id,
                CategoryId = location.CategoryId,
                Name = location.Name,
                Description = location.Description,
                ImagePath = location.ImagePath,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                ZoomLevel = location.ZoomLevel,
                VisitedPhysicalLocationsCount = visitedLocations.visitedLocationsCount,
                PhysicalLocationsCount = visitedLocations.physLocationsCount
            };


            return locationModel;
        }
    }
}
