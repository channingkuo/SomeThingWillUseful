using System;
using BaiduMapSDK;
using RekTec.Corelib.Utils;
using MapKit;

namespace RekTec.Corelib
{
	//public class GeoCodeSearchDelegate : BMKGeoCodeSearchDelegate
	//{
	//	public Action<string> GetGeoCodeSearchCallBack;

	//	public override void OnGetGeoCodeResult (BMKGeoCodeSearch searcher, BMKGeoCodeResult result, BMKSearchErrorCode error)
	//	{
	//		//正向地理编码不处理
	//		if (error == BMKSearchErrorCode.NoError) {
	//			if (GetGeoCodeSearchCallBack != null) {
	//				GetGeoCodeSearchCallBack ("sss");
	//			}
	//		} else {
	//			AlertUtil.Error (error.ToString ());
	//		}
	//	}

	//	public override void OnGetReverseGeoCodeResult (BMKGeoCodeSearch searcher, BMKReverseGeoCodeResult result, BMKSearchErrorCode error)
	//	{
	//		if (error == BMKSearchErrorCode.NoError) {
	//			if (GetGeoCodeSearchCallBack != null) {
	//				GetGeoCodeSearchCallBack ("sss");
	//			}
	//		} else {
	//			AlertUtil.Error (error.ToString ());
	//		}
	//	}
	//}
	public class RTMKMapViewDelegate : MKMapViewDelegate
	{
		public Action<MKUserLocation> GetGeoCodeSearchCallBack;

		public override void DidUpdateUserLocation (MKMapView mapView, MKUserLocation userLocation)
		{
			MKCoordinateRegion region;
			region.Center.Latitude = userLocation.Location.Coordinate.Latitude;
			region.Center.Longitude = userLocation.Location.Coordinate.Longitude;
			region.Span.LatitudeDelta = 0.2;
			region.Span.LongitudeDelta = 0.2;
			if (mapView != null) {
				mapView.Region = region;
			}
			//if (GetGeoCodeSearchCallBack != null) {
			//	GetGeoCodeSearchCallBack (userLocation);
			//}
		}
	}
}
