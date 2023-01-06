
using Microsoft.Maui.Maps;
using Android.Gms.Maps;
using Microsoft.Maui.Maps.Handlers;
using IMap = Microsoft.Maui.Maps.IMap;

namespace CustomizedMap.Platforms.Android
{
    public class MapCallbackHandler : Java.Lang.Object, IOnMapReadyCallback
    {

        private readonly IMapHandler mapHandler;

        public MapCallbackHandler(IMapHandler mapHandler)
        {
            this.mapHandler = mapHandler;
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            mapHandler.UpdateValue(nameof(IMap.Pins));
        }
    }
}
