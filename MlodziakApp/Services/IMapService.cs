using MlodziakApp.Messages;
using MlodziakApp.Messages.MessageItems;
using SharedModels;
using System.Collections.ObjectModel;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace MlodziakApp.Services
{
    public interface IMapService
    {
        Task<Map> InitalizeMapAsync(object bindingContext, LocationInfoMessageItem locationInfoMessageItem);
        PhysicalLocationModel? HandleMapClicked(Location touchPosition);
    }
}