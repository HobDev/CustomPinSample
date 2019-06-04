using CustomPinSample.Controls;
using CustomPinSample.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CustomPinSample.Views
{
    public partial class MapPage : ContentPage
    {
        MapViewModel viewModel;
        CustomMap customMap;

        public MapPage()
        {

            InitializeComponent();
            BindingContext = viewModel = new MapViewModel();

            customMap = new CustomMap
            {

                IsShowingUser = true,
                ItemsSource = viewModel.Data,
                MapType = MapType.Street,

                ItemTemplate = new DataTemplate(() =>
               {
                   var pin = new Pin();

                   pin.SetBinding(Pin.AddressProperty, "Address");
                   pin.SetBinding(Pin.LabelProperty, "Label");
                   pin.SetBinding(Pin.PositionProperty, "Position");

                   return pin;
               })

            };
            Content = customMap;

            MapInitialization();

        }

        private async void MapInitialization()
        {
            Xamarin.Forms.Maps.Position position = await viewModel.GetLocation();

            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(5)));

            await viewModel.GetPins();

        }


    }
}
