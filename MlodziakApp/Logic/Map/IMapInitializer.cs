using MlodziakApp.Messages.MessageItems;
using MauiMap = Microsoft.Maui.Controls.Maps.Map;

namespace MlodziakApp.Logic.Map
{
    public interface IMapInitializer
    {
        Task<MauiMap> InitializeMapAsync(object bindingContext, LocationInfoMessageItem locationInfoMessageItem);
    }
}