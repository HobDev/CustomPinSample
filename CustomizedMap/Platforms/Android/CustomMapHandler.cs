
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using CustomizedMap.CustomControl;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Maps.Handlers;
using Microsoft.Maui.Platform;
using Application = Android.App.Application;
using IMap = Microsoft.Maui.Maps.IMap;
using Android.Graphics.Drawables;
using Microsoft.Maui.Controls.Maps;

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

        protected override void ConnectHandler(MapView platformView)
        {
            try
            {
                base.ConnectHandler(platformView);
                var mapReady = new MapCallbackHandler(this);
                PlatformView.GetMapAsync(mapReady);
            }
            catch (Exception ex)
            {

                Shell.Current.DisplayAlert("Error", ex.Message,"OK");
            }
            
        }

        private static new void MapPins(IMapHandler handler, IMap map)
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
                    mapHandler.AddPins(map.Pins);
                }
            }
            catch (Exception ex)
            {

                Shell.Current.DisplayAlert("Error", ex.Message,"OK");
            }
          
        }

        private void AddPins(IEnumerable<IMapPin> mapPins)
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
                            cp.ImageSource.LoadImage(MauiContext, result =>
                            {
                                if(result?.Value is BitmapDrawable bitMapDrawable)
                                {
                                    markerOption.SetIcon(BitmapDescriptorFactory.FromBitmap(bitMapDrawable.Bitmap));
                                }
                                AddMarker(Map, pin, Markers, markerOption);
                            });
                        }

                        else
                        {
                            AddMarker(Map, pin, Markers, markerOption);
                        }
                    }
                }
            }
            catch (Exception ex)            
            {

               Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }          
        }

        private static void AddMarker(GoogleMap map, IMapPin pin, List<Marker> markers, MarkerOptions markerOption)
        {
            var marker = map.AddMarker(markerOption);
            pin.MarkerId = marker.Id;
            markers.Add(marker);
        }
    }
}
