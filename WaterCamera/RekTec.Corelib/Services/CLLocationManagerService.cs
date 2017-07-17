using System;
using CoreLocation;
using UIKit;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Foundation;

namespace RekTec.Corelib
{
	public class CLLocationManagerService
	{
		protected CLLocationManager _locMgr;
		public Action<CLLocationModel> GetLocationCallBack;

		public CLLocationManagerService ()
		{
			_locMgr = new CLLocationManager ();
			_locMgr.PausesLocationUpdatesAutomatically = false;

			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
				_locMgr.RequestAlwaysAuthorization ();
			}

			StartLocationUpdates ();
		}

		public CLLocationManager LocationManager {
			get { return _locMgr; }
		}

		private void StartLocationUpdates ()
		{
			if (!CLLocationManager.LocationServicesEnabled) {
				return;
			}
			_locMgr.DesiredAccuracy = CLLocation.AccuracyBest;
			_locMgr.LocationsUpdated += (sender, e) => {
				Debug.WriteLine ("Altitude: " + e.Locations [e.Locations.Length - 1].Altitude);
				Debug.WriteLine ("Longitude: " + e.Locations [e.Locations.Length - 1].Coordinate.Longitude);
				Debug.WriteLine ("Latitude: " + e.Locations [e.Locations.Length - 1].Coordinate.Latitude);
				var mm = new CLLocationModel {
					Longitude = e.Locations [e.Locations.Length - 1].Coordinate.Longitude,
					Latitude = e.Locations [e.Locations.Length - 1].Coordinate.Latitude
				};

				// 反编码
				var cLLocation = new CLLocation (mm.Latitude, mm.Longitude);
				var geoCoder = new CLGeocoder ();
				geoCoder.ReverseGeocodeLocation (cLLocation, (placemarks, error) => {
					var address = "";
					var country = "";
					var state = "";
					var city = "";
					var sublocality = "";
					var throughtfare = "";
					if (error == null && placemarks != null) {
						foreach (var placemark in placemarks) {
							address = placemark.AddressDictionary ["FormattedAddressLines"].Description;
							country = placemark.AddressDictionary ["Country"].Description;
							state = placemark.AddressDictionary ["State"].Description;
							city = placemark.AddressDictionary ["City"].Description;
							sublocality = placemark.AddressDictionary ["SubLocality"].Description;
							throughtfare = placemark.AddressDictionary ["Thoroughfare"].Description;
						}
					}
					address = address.Replace ("\n", "").Replace ("\"", "").Replace ("(", "").Replace (")", "").Replace (" ", "");
					Regex reg = new Regex (@"(?i)\\[uU]([0-9a-f]{4})");
					address = reg.Replace (address, delegate (Match m) {
						return ((char)Convert.ToInt32 (m.Groups [1].Value, 16)).ToString ();
					});
					country = reg.Replace (country, delegate (Match m) {
						return ((char)Convert.ToInt32 (m.Groups [1].Value, 16)).ToString ();
					});
					state = reg.Replace (state, delegate (Match m) {
						return ((char)Convert.ToInt32 (m.Groups [1].Value, 16)).ToString ();
					});
					city = reg.Replace (city, delegate (Match m) {
						return ((char)Convert.ToInt32 (m.Groups [1].Value, 16)).ToString ();
					});
					sublocality = reg.Replace (sublocality, delegate (Match m) {
						return ((char)Convert.ToInt32 (m.Groups [1].Value, 16)).ToString ();
					});
					throughtfare = reg.Replace (throughtfare, delegate (Match m) {
						return ((char)Convert.ToInt32 (m.Groups [1].Value, 16)).ToString ();
					});
					mm.Address = country + state + city + sublocality + throughtfare;
					mm.FullAddress = address;
					mm.CameraAddress = city + sublocality + throughtfare;

					// 坐标系转换
					var gcj = LocationUtils.gps84_To_Gcj02 (mm.Latitude, mm.Longitude);
					var bd09 = LocationUtils.gcj02_To_Bd09 (gcj.lat, gcj.lon);
					var bdLocationAddress = new CLLocationModel {
						Longitude = bd09.lon,
						Latitude = bd09.lat,
						Address = mm.Address,
						CameraAddress = mm.CameraAddress,
						FullAddress = mm.FullAddress
					};

					if (GetLocationCallBack != null) {
						_locMgr.StopUpdatingLocation ();
						GetLocationCallBack (bdLocationAddress);
					}
				});
			};
			_locMgr.StartUpdatingLocation ();
		}
	}

	public class CLLocationModel
	{
		public double Latitude;
		public double Longitude;
		public string Address;
		public string FullAddress;
		public string CameraAddress;
	}
}
