# CustomPinSample
For details see the dotnet Maui [docs](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/map?view=net-maui-7.0). The code to put custom pins is taken from this [Article](https://vladislavantonyuk.github.io/articles/Customize-map-pins-in-.NET-MAUI). 
The code in this sample is written to show custom pins with Microsoft.Maui.Controls.Maps.Map  on iOS and Android. The app first gets the user location and than calls Google Places Api to search for restaurants within 10 kilometers of the current location. Than put pin on each restaurant's location.
The requirements to build this sample:<br />
 Create a strings.xml file in the following folder: Platforms-> Android -> Resources -> values. 

```
<?xml version="1.0" encoding="utf-8" ?>
<resources>
	<string name="GooglePlacesApiKey">GOOGLE PLACES API KEY HERE</string>	
</resources>
```
Create class Constants in Models folder:
```
public static class Constants
    {
        public static string GOOGLE_PLACES_API_KEY { get; } = "GOOGLE PLACES API KEY HERE";
    }
```

Add strings.xml and Constants.cs to .gitignore as the Google places api key is a secret and should not to pushed to the remote repo.<br />

Replace CustomPin with Pin in MapPage to get default platform pins on the map. 
