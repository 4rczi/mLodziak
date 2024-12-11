using SharedModels;

namespace MlodziakApp.Logic.Map
{
    public interface IMapHandler
    {
        PhysicalLocationModel? HandleMapClicked(List<PhysicalLocationModel> physicalLocationModels, Location touchPosition);
    }
}