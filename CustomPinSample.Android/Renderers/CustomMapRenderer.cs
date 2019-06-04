using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using CustomPinSample.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(Map), typeof(CustomMapRenderer))]
namespace CustomPinSample.Droid.Renderers
{
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {

        public CustomMapRenderer(Context context) : base(context)
        {
        }


        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.SetInfoWindowAdapter(this);
        }


        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);
            marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
            return marker;
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;


                view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);


                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);

                if (infoTitle != null)
                {
                    infoTitle.Text = marker.Title;
                }
                if (infoSubtitle != null)
                {
                    infoSubtitle.Text = marker.Snippet;
                }

                return view;
            }
            return null;
        }



        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }
    }
}
