using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using CustomPinSample.Models;
using Newtonsoft.Json;
using PropertyChanged;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CustomPinSample.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MapViewModel
    {
        public ObservableCollection<MapData> Data { get; set; }
        readonly string search = "";
        public Position Location { get; set; }
        RootObject rootObject;

        public MapViewModel()
        {

            search = Shell.Current.CurrentItem.CurrentItem.Title.ToLower();
            Data = new ObservableCollection<MapData>();

        }

        public async Task<Xamarin.Forms.Maps.Position> GetLocation()
        {

            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);

                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {

                    Location = new Position(location.Latitude, location.Longitude);
                    return new Position(location.Latitude, location.Longitude);

                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return new Position(0, 0);
        }



        async public Task GetPins()
        {
            rootObject = null;
            HttpClient client = new HttpClient();
            CultureInfo In = new CultureInfo("en-IN");
            string latitude = Location.Latitude.ToString(In);
            string longitude = Location.Longitude.ToString(In);
            string restUrl = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + latitude + "," + longitude + "&radius=10000&type=" + search + "&key=" + Constants.GOOGLE_PLACES_API_KEY;
            var uri = new Uri(restUrl);

            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                rootObject = JsonConvert.DeserializeObject<RootObject>(content);
                var result = rootObject.results;
                if (result != null)
                {
                    Data.Clear();
                    foreach (Place place in result)
                    {
                        
                        Data.Add(new MapData()
                        {
                            Position = new Position(place.geometry.location.lat, place.geometry.location.lng),
                            Label = place.name,
                            Address = place.vicinity,

                        });
                    }

                }
            }

        }


    }

    
}
