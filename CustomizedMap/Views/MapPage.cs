using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
namespace CustomizedMap.Views;

public class MapPage : ContentPage
{
	public MapPage(MapViewModel viewModel)
	{
		Content = new Map
		{

            IsShowingUser = true,
            MapType = MapType.Street,
        };

		BindingContext= viewModel;
	}
}