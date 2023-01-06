
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using CustomizedMap.CustomControl;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Maps.Handlers;
using Microsoft.Maui.Platform;
using Application = Android.App.Application;
using IMap = Microsoft.Maui.Maps.IMap;

namespace CustomizedMap.Platforms.Android
{
    public class CustomMapHandler: MapHandler
    {
        public static readonly IPropertyMapper<IMap, IMapHandler> CustomMapper =
            new PropertyMapper<IMap, IMapHandler> (Mapper)
            {
                [nameof(IMap.Pins)]=MapPins,
            };

        public CustomMapHandler(): base (CustomMapper, CommandMapper)
        {

        }

        public CustomMapHandler(IPropertyMapper? mapper = null, CommandMapper? commandMapper= null): base(
            mapper ?? CustomMapper, commandMapper ?? CommandMapper)
        {

        }

        public List<Marker>? Markers { get; private set; }

        protected override async void ConnectHandler(MapView platformView)
        {
            try
            {
                base.ConnectHandler(platformView);
                var mapReady = new MapCallbackHandler(this);
                PlatformView.GetMapAsync(mapReady);
            }
            catch (Exception ex)
            {

                await Shell.Current.DisplayAlert("Error", ex.Message,"OK");
            }
            
        }

        private static new async void MapPins(IMapHandler handler, IMap map)
        {
            try
            {
                if (handler is CustomMapHandler mapHandler)
                {
                    if (mapHandler.Markers != null)
                    {
                        foreach (var marker in mapHandler.Markers)
                        {
                            marker.Remove();
                        }

                        mapHandler.Markers = null;
                    }
                    await mapHandler.AddPins(map.Pins);
                }
            }
            catch (Exception ex)
            {

                await Shell.Current.DisplayAlert("Error", ex.Message,"OK");
            }
          
        }

        private async Task AddPins(IEnumerable<IMapPin> mapPins)
        {
            try
            {
                if (Map is null || MauiContext is null)
                {
                    return;
                }

                Markers ??= new List<Marker>();
                foreach (var pin in mapPins)
                {
                    var pinHandler = pin.ToHandler(MauiContext);
                    if (pinHandler is IMapPinHandler mapPinHandler)
                    {
                        var markerOption = mapPinHandler.PlatformView;
                        if (pin is CustomPin cp)
                        {
                            var imageSourceHandler = new ImageLoaderSourceHandler();
                            var bitmap = await imageSourceHandler.LoadImageAsync(cp.ImageSource, Application.Context);
                            markerOption?.SetIcon(bitmap is null
                                ? BitmapDescriptorFactory.DefaultMarker()
                                : BitmapDescriptorFactory.FromBitmap(bitmap));
                        }

                        var marker = Map.AddMarker(markerOption);
                        pin.MarkerId = marker.Id;
                        Markers.Add(marker);
                    }
                }
            }
            catch (Exception ex)
            {

               await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
           
        }
    }
}
