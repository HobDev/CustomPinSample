using CustomizedMap.CustomControl;
using CustomizedMap.Models;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
namespace CustomizedMap.Views;

public class MapPage : ContentPage
{
	Map map;
	MapViewModel viewModel;
	public MapPage(MapViewModel viewModel)
	{
		try
		{
            this.viewModel = viewModel;

            map = new Map()
            {

                IsShowingUser = true,
                IsTrafficEnabled = false,
                IsZoomEnabled = true,
                MapType = MapType.Street,
                ItemsSource = viewModel.PinData,
                ItemTemplate = new DataTemplate(() =>
                {
                    CustomPin pin = new CustomPin();
                    pin.Bind(Pin.AddressProperty, nameof(MapData.Address));
                    pin.Bind(Pin.LabelProperty, nameof(MapData.Label));
                    pin.Bind(Pin.LocationProperty,nameof(MapData.PinLocation));
                    pin.Type = PinType.Place;
                    pin.ImageSource =ImageSource.FromResource("CustomizedMap.Resources.icon.jpeg");
                    pin.Map = map;
                    return pin;
                })
            };

            Content = map;
            InitializeMap();

            BindingContext = viewModel;
        }
		catch (Exception ex)
		{

			
		}
		
	}

	async void InitializeMap()
	{
		try
		{
            MapSpan mapSpan = await viewModel.GetLocation();
            map.MoveToRegion(mapSpan);

            await viewModel.GetPins();
        }
		catch (Exception ex)
		{

			await Shell.Current.DisplayAlert("Error", ex.Message,"OK");
		}
       
    }
}