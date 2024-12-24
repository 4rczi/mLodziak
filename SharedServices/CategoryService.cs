using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserHistoryRepository _userHistoryRepository;
        private readonly IPhysicalLocationRepository _physicalLocationRepository;

        public CategoryService(ICategoryRepository categoryRepository,
                               IUserHistoryRepository userLocationHistoryRepository,
                               IPhysicalLocationRepository physicalLocationRepository)
        {
            _categoryRepository = categoryRepository;
            _userHistoryRepository = userLocationHistoryRepository;
            _physicalLocationRepository = physicalLocationRepository;
        }

        public async Task<List<CategoryModel>> GetCategoryModelsAsync(string userId)
        {
            var userHistory = await _userHistoryRepository.GetUserHistoryAsync(userId);
            var physicalLocation = await _physicalLocationRepository.GetPhysicalLocationsAsync();
            var categories = await _categoryRepository.GetCategoriesAsync();

            var visitedPhysicalLocations = from pl in physicalLocation
                                           join uh in userHistory.DefaultIfEmpty()
                                           on pl.Id equals uh?.PhysicalLocationId into joinResult
                                           group joinResult by new { pl.LocationId, pl.CategoryId } into aggregated_query
                                           select new
                                           {
                                               aggregated_query.Key.LocationId,
                                               aggregated_query.Key.CategoryId,
                                               visitedPhysicalLocationsCount = aggregated_query.Count(uh => uh != null && uh.Any(u => u.UserId == userId)),
                                               physicalLocationsCount = aggregated_query.Count()
                                           };

            var visitedLocations = from c in categories
                                   join vl in visitedPhysicalLocations
                                   on c.Id equals vl.CategoryId into joinResult
                                   from sub in joinResult.DefaultIfEmpty()
                                   group sub by new { c.Id } into aggregated_query
                                   select new
                                   {

                                       Id = aggregated_query.Key.Id,
                                       VisitedLocationsCount = aggregated_query.Sum(loc => (loc.visitedPhysicalLocationsCount == loc.physicalLocationsCount) ? 1 : 0),
                                       LocationsCount = aggregated_query.Count(),
                                   };

            var locationModelsList = from c in categories
                                     join vl in visitedLocations
                                     on c.Id equals vl.Id into joinResult
                                     from data in joinResult
                                     select new CategoryModel
                                     {
                                         Id = c.Id,
                                         Name = c.Name,
                                         Description = c.Description,
                                         ImagePath = c.ImagePath,
                                         VisitedLocationsCount = data.VisitedLocationsCount,
                                         LocationsCount = data.LocationsCount,
                                     };

            return locationModelsList.ToList();
        }
    }
}
