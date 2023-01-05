

using CommunityToolkit.Mvvm.ComponentModel;
using CustomizedMap.Models;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace CustomizedMap.ViewModels
{
    public partial class MapViewModel: ObservableObject
    {

        [ObservableProperty]
        ObservableCollection<MapData> data;

        readonly string search=string.Empty;

        [ObservableProperty]
        Location location;

        Root rootObject;



        public MapViewModel() 
        {
            search = "restaurant";
            Data= new ObservableCollection<MapData>();
           
        }

       public async Task<MapSpan> GetLocation()
        {
            MapSpan mapSpan = null;
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium);
                location = await Geolocation.GetLocationAsync(request);
                // MapSpan mapSpan = new MapSpan(location, 0.01, 0.01);
                mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(0.444));
               
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
                rootObject = null;
                using (HttpClient client = new HttpClient())
                {
                    CultureInfo cultureInfo = new CultureInfo("en-US");
                    string latitude = Location.Latitude.ToString(cultureInfo);
                    string longitude = Location.Longitude.ToString(cultureInfo);
                    string restUrl = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + latitude + "," + longitude + "&radius=10000&type=" + search + "&key=" + Constants.GOOGLE_PLACES_API_KEY;
                    Uri uri = new Uri(restUrl);
                    HttpResponseMessage responseMessage = await client.GetAsync(uri);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        string content = await responseMessage.Content.ReadAsStringAsync();
                        rootObject = JsonSerializer.Deserialize<Root>(content);
                        List<Result> results = rootObject.Results;
                        if (results.Count > 0)
                        {
                            Data.Clear();
                            foreach (Result result in results)
                            {
                                Data.Add(new MapData
                                {
                                    Location = result.Geometry.Location,
                                    Label = result.Name,
                                    Address = result.Vicinity,
                                });
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
