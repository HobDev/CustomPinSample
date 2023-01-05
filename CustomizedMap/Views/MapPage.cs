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

		this.viewModel = viewModel;

		map = new Map()
		{

			IsShowingUser = true,
			IsTrafficEnabled = false,
			IsZoomEnabled = true,
			MapType = MapType.Street,
			ItemTemplate = new DataTemplate(()=>
			{
				Pin pin = new Pin();
				pin.Bind(Pin.AddressProperty, "Address");
				pin.Bind(Pin.LabelProperty, "Label");
				pin.Bind(Pin.LocationProperty,"Location");
				return pin;
			})
        };

		Content = map;
	   InitializeMap();

		BindingContext= viewModel;
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