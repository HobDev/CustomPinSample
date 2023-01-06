

using CommunityToolkit.Mvvm.ComponentModel;
using CustomizedMap.Models;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace CustomizedMap.ViewModels
{
    public partial class MapViewModel: ObservableObject
    {

        [ObservableProperty]
        ObservableCollection<MapData> pinData;

        [ObservableProperty]
       string searchType=string.Empty;

        [ObservableProperty]
        Microsoft.Maui.Devices.Sensors.Location currentMapLocation;
        public MapViewModel() 
        {
            SearchType = "restaurant";
            pinData= new ObservableCollection<MapData>();
           
        }

       public async Task<MapSpan> GetLocation()
        {
            MapSpan mapSpan = null;
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium);
              currentMapLocation = await Geolocation.GetLocationAsync(request);
                // MapSpan mapSpan = new MapSpan(currentMapLocation, 0.01, 0.01);
                mapSpan = MapSpan.FromCenterAndRadius(currentMapLocation, Distance.FromKilometers(0.444));
               
            }
            catch (Exception ex)
            {

                await Shell.Current.DisplayAlert("Error", ex.Message,"OK");
            }
           return mapSpan;
        }

        public async Task GetPins()
        {
            try
            {
               
                using (HttpClient client = new HttpClient())
                {
                    //cultureinfo is set to en-US to avoid error in some regions where decimal is written with commas in place of dots
                    CultureInfo cultureInfo = new CultureInfo("en-US");
                    string latitude = currentMapLocation.Latitude.ToString(cultureInfo);
                    string longitude = currentMapLocation.Longitude.ToString(cultureInfo);
                    string restUrl = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + latitude + "," + longitude + "&radius=10000&type=" + SearchType + "&key=" + Constants.GOOGLE_PLACES_API_KEY;
                    Uri uri = new Uri(restUrl);
                    HttpResponseMessage responseMessage = await client.GetAsync(uri);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        string content = await responseMessage.Content.ReadAsStringAsync();
                       Root rootObject = JsonSerializer.Deserialize<Root>(content);
                        List<Result> results = rootObject?.Results;
                        if (results.Count > 0)
                        {
                            PinData.Clear();
                            foreach (Result result in results)
                            {
                                MapData mapData= new MapData();
                                mapData.PinLocation = new Microsoft.Maui.Devices.Sensors.Location(result.Geometry.Location.Lat, result.Geometry.Location.Lng);
                                mapData.Label = result.Name;
                                mapData.Address = result.Vicinity;
                                PinData.Add(mapData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

               await Shell.Current.DisplayAlert("Error",ex.Message,"OK");
            }

          
        }

    }
}
