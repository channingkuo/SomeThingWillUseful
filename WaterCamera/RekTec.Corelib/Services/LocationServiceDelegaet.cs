using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using BaiduMapSDK;
using CoreLocation;
using Foundation;
using RekTec.Corelib.Utils;
using UIKit;

namespace RekTec.Corelib
{
	public class LocationServiceDelegate : BMKLocationServiceDelegate
	{
		public Action<Location> GetLocationCallBack;

		/// <summary>
		/// 处理定位失败
		/// </summary>
		/// <returns>The fail to locate user with error.</returns>
		/// <param name="error">Error.</param>
		public override void DidFailToLocateUserWithError (Foundation.NSError error)
		{
			AlertUtil.Error (error.LocalizedFailureReason);
		}

		/// <summary>
		/// 定位位置更新后调用
		/// </summary>
		/// <returns>The update BMKUser location.</returns>
		/// <param name="userLocation">User location.</param>
		public override void DidUpdateBMKUserLocation (BMKUserLocation userLocation)
		{
			//var location = new CLLocation (userLocation.Location.Coordinate.Latitude, userLocation.Location.Coordinate.Longitude);
			//var geoCoder = new CLGeocoder ();
			//geoCoder.ReverseGeocodeLocation (location, (placemarks, error) => {
			//	var address = "";
			//	var country = "";
			//	var state = "";
			//	var city = "";
			//	var sublocality = "";
			//	var throughtfare = "";
			//	if (error == null && placemarks != null) {
			//		foreach (var placemark in placemarks) {
			//			//Debug.WriteLine (placemark.AddressDictionary);
			//			address = placemark.AddressDictionary ["FormattedAddressLines"].Description;
			//			country = placemark.AddressDictionary ["Country"].Description;
			//			state = placemark.AddressDictionary ["State"].Description;
			//			city = placemark.AddressDictionary ["City"].Description;
			//			sublocality = placemark.AddressDictionary ["SubLocality"].Description;
			//			throughtfare = placemark.AddressDictionary ["Thoroughfare"].Description;
			//		}
			//	}
			//	address = address.Replace ("\n", "").Replace ("\"", "").Replace ("(", "").Replace (")", "").Replace (" ", "");
			//	Regex reg = new Regex (@"(?i)\\[uU]([0-9a-f]{4})");
			//	address = reg.Replace (address, delegate (Match m) {
			//		return ((char)Convert.ToInt32 (m.Groups [1].Value, 16)).ToString ();
			//	});
			//	country = reg.Replace (country, delegate (Match m) {
			//		return ((char)Convert.ToInt32 (m.Groups [1].Value, 16)).ToString ();
			//	});
			//	state = reg.Replace (state, delegate (Match m) {
			//		return ((char)Convert.ToInt32 (m.Groups [1].Value, 16)).ToString ();
			//	});
			//	city = reg.Replace (city, delegate (Match m) {
			//		return ((char)Convert.ToInt32 (m.Groups [1].Value, 16)).ToString ();
			//	});
			//	sublocality = reg.Replace (sublocality, delegate (Match m) {
			//		return ((char)Convert.ToInt32 (m.Groups [1].Value, 16)).ToString ();
			//	});
			//	throughtfare = reg.Replace (throughtfare, delegate (Match m) {
			//		return ((char)Convert.ToInt32 (m.Groups [1].Value, 16)).ToString ();
			//	});

			if (GetLocationCallBack != null) {
				GetLocationCallBack (new Location {
					latitude = userLocation.Location.Coordinate.Latitude,
					longitude = userLocation.Location.Coordinate.Longitude
					//address = country + state + city + sublocality + throughtfare,
					//fullAddress = address
				});
			}
			//});
		}

		/// <summary>
		/// 定位方向改变后调用
		/// </summary>
		/// <returns>The update user heading.</returns>
		/// <param name="userLocation">User location.</param>
		public override void DidUpdateUserHeading (BMKUserLocation userLocation)
		{

		}

		public class Location
		{
			public double latitude;
			public double longitude;
			public string address;
			public string fullAddress;
		}
	}
}

