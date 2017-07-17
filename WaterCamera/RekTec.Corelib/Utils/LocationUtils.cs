using System;

namespace RekTec.Corelib
{
	public class GpsLocation
	{
		public double lat;
		public double lon;
	}
	public class LocationUtils
	{
		public static string BAIDU_LBS_TYPE = "bd09ll";
		public static double pi = 3.1415926535897932384626;
		public static double a = 6378245.0;
		public static double ee = 0.00669342162296594323;

		/**
		 * 84 to 火星坐标系 (GCJ-02) World Geodetic System ==> Mars Geodetic System
		 * `
		 * @param lat
		 * @param lon
		 * @return
		 */
		public static GpsLocation gps84_To_Gcj02 (double lat, double lon)
		{
			if (outOfChina (lat, lon)) {
				return null;
			}
			double dLat = transformLat (lon - 105.0, lat - 35.0);
			double dLon = transformLon (lon - 105.0, lat - 35.0);
			double radLat = lat / 180.0 * pi;
			double magic = Math.Sin (radLat);
			magic = 1 - ee * magic * magic;
			double sqrtMagic = Math.Sqrt (magic);
			dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
			dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos (radLat) * pi);
			double mgLat = lat + dLat;
			double mgLon = lon + dLon;
			return new GpsLocation {
				lat = mgLat,
				lon = mgLon
			};
		}

		/**
		 * 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换算法 将 GCJ-02 坐标转换成 BD-09 坐标
		 * 
		 * @param gg_lat
		 * @param gg_lon
		 */
		public static GpsLocation gcj02_To_Bd09 (double gg_lat, double gg_lon)
		{
			double x = gg_lon, y = gg_lat;
			double z = Math.Sqrt (x * x + y * y) + 0.00002 * Math.Sin (y * pi);
			double theta = Math.Atan2 (y, x) + 0.000003 * Math.Cos (x * pi);
			double bd_lon = z * Math.Cos (theta) + 0.0065;
			double bd_lat = z * Math.Sin (theta) + 0.006;
			return new GpsLocation {
				lat = bd_lat,
				lon = bd_lon
			};
		}

		public static bool outOfChina (double lat, double lon)
		{
			if (lon < 72.004 || lon > 137.8347)
				return true;
			if (lat < 0.8293 || lat > 55.8271)
				return true;
			return false;
		}

		public static GpsLocation transform (double lat, double lon)
		{
			if (outOfChina (lat, lon)) {
				return new GpsLocation {
					lat = lat,
					lon = lon
				};
			}
			double dLat = transformLat (lon - 105.0, lat - 35.0);
			double dLon = transformLon (lon - 105.0, lat - 35.0);
			double radLat = lat / 180.0 * pi;
			double magic = Math.Sin (radLat);
			magic = 1 - ee * magic * magic;
			double sqrtMagic = Math.Sqrt (magic);
			dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
			dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos (radLat) * pi);
			double mgLat = lat + dLat;
			double mgLon = lon + dLon;
			return new GpsLocation {
				lat = mgLat,
				lon = mgLon
			};
		}

		public static double transformLat (double x, double y)
		{
			double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt (Math.Abs (x));
			ret += (20.0 * Math.Sin (6.0 * x * pi) + 20.0 * Math.Sin (2.0 * x * pi)) * 2.0 / 3.0;
			ret += (20.0 * Math.Sin (y * pi) + 40.0 * Math.Sin (y / 3.0 * pi)) * 2.0 / 3.0;
			ret += (160.0 * Math.Sin (y / 12.0 * pi) + 320 * Math.Sin (y * pi / 30.0)) * 2.0 / 3.0;
			return ret;
		}

		public static double transformLon (double x, double y)
		{
			double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt (Math.Abs (x));
			ret += (20.0 * Math.Sin (6.0 * x * pi) + 20.0 * Math.Sin (2.0 * x * pi)) * 2.0 / 3.0;
			ret += (20.0 * Math.Sin (x * pi) + 40.0 * Math.Sin (x / 3.0 * pi)) * 2.0 / 3.0;
			ret += (150.0 * Math.Sin (x / 12.0 * pi) + 300.0 * Math.Sin (x / 30.0 * pi)) * 2.0 / 3.0;
			return ret;
		}
	}
}
