using System;
using CoreLocation;
using UIKit;
using Foundation;
using MapKit;

namespace RekTec.Corelib
{
	public class MapLocationManagerService
	{
		protected CLLocationManager _locMgr;
		//protected MKMapView _map;
		public Action<CLLocationModel> GetLocationCallBack;

		public MapLocationManagerService ()
		{
			//_map = new MKMapView ();
			//_map.UserTrackingMode = MKUserTrackingMode.None;
			//_map.ShowsUserLocation = true;
			_locMgr = new CLLocationManager ();
			_locMgr.PausesLocationUpdatesAutomatically = false;

			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
				_locMgr.RequestAlwaysAuthorization ();
			}

			StartLocationUpdates ();
		}

		private void StartLocationUpdates ()
		{
			if (!CLLocationManager.LocationServicesEnabled) {
				return;
			}
			//_map.DidUpdateUserLocation += (sender, e) => {
			//	_map.ShowsUserLocation = false;

			//	var mm = new CLLocationModel {
			//		Longitude = e.UserLocation.Coordinate.Longitude,
			//		Latitude = e.UserLocation.Coordinate.Latitude
			//	};

			//	var geoCoder = new MKReverseGeocoder (e.UserLocation.Coordinate);
			//	var geocoderDelete = new MapReverseGeocoderDelete ();
			//	geocoderDelete.geocoderCallBack += (obj) => {
			//		mm.Address = obj;

			//		//	// 坐标系转换
			//		var gcj = LocationUtils.gps84_To_Gcj02 (mm.Latitude, mm.Longitude);
			//		var bd09 = LocationUtils.gcj02_To_Bd09 (gcj.lat, gcj.lon);
			//		var bdLocationAddress = new CLLocationModel {
			//			Longitude = bd09.lon,
			//			Latitude = bd09.lat,
			//			Address = mm.Address,
			//			FullAddress = mm.FullAddress
			//		};

			//		if (GetLocationCallBack != null) {
			//			GetLocationCallBack (bdLocationAddress);
			//		}
			//	};
			//	geoCoder.Delegate = geocoderDelete;
			//	geoCoder.Start ();
			//};
			_locMgr.DesiredAccuracy = CLLocation.AccuracyBest;
			_locMgr.DistanceFilter = 0F;
			_locMgr.LocationsUpdated += (sender, e) => {
				var mm = new CLLocationModel {
					Longitude = e.Locations [e.Locations.Length - 1].Coordinate.Longitude,
					Latitude = e.Locations [e.Locations.Length - 1].Coordinate.Latitude
				};

				var geoCoder = new MKReverseGeocoder (e.Locations [e.Locations.Length - 1].Coordinate);
				var geocoderDelete = new MapReverseGeocoderDelete ();
				geocoderDelete.geocoderCallBack += (obj) => {
					mm.Address = obj;

					//	// 坐标系转换
					var gcj = LocationUtils.gps84_To_Gcj02 (mm.Latitude, mm.Longitude);
					var bd09 = LocationUtils.gcj02_To_Bd09 (gcj.lat, gcj.lon);
					var bdLocationAddress = new CLLocationModel {
						Longitude = bd09.lon,
						Latitude = bd09.lat,
						Address = mm.Address,
						FullAddress = mm.FullAddress
					};

					if (GetLocationCallBack != null) {
						_locMgr.StopUpdatingLocation ();
						GetLocationCallBack (bdLocationAddress);
					}
				};
				geoCoder.Delegate = geocoderDelete;
				geoCoder.Start ();
			};
			_locMgr.StartUpdatingLocation ();
		}

		private class MapReverseGeocoderDelete : MKReverseGeocoderDelegate
		{
			public Action<string> geocoderCallBack;

			public override void FoundWithPlacemark (MKReverseGeocoder geocoder, MKPlacemark placemark)
			{
				if (geocoderCallBack != null) {
					geocoderCallBack (placemark.AdministrativeArea
									  + placemark.Locality
									  + placemark.Thoroughfare
									  + placemark.SubThoroughfare);
				}
			}

			public override void FailedWithError (MKReverseGeocoder geocoder, NSError error)
			{
				geocoder.Cancel ();
				if (geocoderCallBack != null) {
					geocoderCallBack ("无法获取位置信息");
				}
			}
		}
	}
}
