using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestLocationService : MonoBehaviour
{

	public string gps_info = "";
	public int flash_num = 1;
	TextMeshProUGUI log;
	public Button AButton;
	public Button BButton;

	public TextMeshProUGUI PointALongitude;
	public TextMeshProUGUI PointALatitude;
	public TextMeshProUGUI PointAAltitude;

	public TextMeshProUGUI PointBLongitude;
	public TextMeshProUGUI PointBLatitude;
	public TextMeshProUGUI PointBAltitude;


	private float LongitudeA;
	private float LatitudeA;
	private float AltitudeA;

	private float LongitudeB;
	private float LatitudeB;
	private float AltitudeB;

	private float roundFloat(float val)
	{
		return (float)System.Math.Round(val, 5);
	}


	void updatePoint(Point point)
	{
		if (point == Point.newPoint)
		{
			PointBLongitude.text = $"Longitude: { LongitudeB}";
			PointBLatitude.text = $"Latitude: { LatitudeB}";
			PointBAltitude.text = $"Altitude: { AltitudeB}";
		}
		if (point == Point.oldPoint)
		{
			PointALongitude.text = $"Longitude: { LongitudeA}";
			PointALatitude.text = $"Latitude: { LatitudeA}";
			PointAAltitude.text = $"Altitude: { AltitudeA}";
		}
	}

	public enum Point
	{
		oldPoint, newPoint
	}


	// Use this for initialization  
	void Start()
	{
		StartCoroutine(StartGPS(Point.oldPoint));
	}



	// Input.location = LocationService  
	// LocationService.lastData = LocationInfo   

	void StopGPS()
	{
		Input.location.Stop();
	}

	public void B_pointGPS(int pt)
	{
		StartCoroutine(StartGPS((Point)pt));
	}
	public void A_pointGPS(int pt)
	{
		StartCoroutine(StartGPS((Point)pt));
	}
	public IEnumerator StartGPS(Point pt)
	{
		//Input.location for accessing device location attributes (handheld devices), static Location Service locations  
		//Is Location Service.isEnabledByUser enabled in user settings  

		//yield return new WaitForSeconds(1);

		Button selectedButton = AButton;
		if (pt == Point.oldPoint)
		{
			AltitudeA = LatitudeA = AltitudeA = 0;
			selectedButton = AButton;
		}

		if (pt == Point.newPoint)
		{
			AltitudeB = LatitudeB = AltitudeB = 0;
			selectedButton = BButton;
		}


		if (!Input.location.isEnabledByUser)
		{
			//log.text = "isEnabledByUser value is:" + Input.location.isEnabledByUser.ToString() + " Please turn on the GPS";
			yield break;
		}

		//LocationService.Start() Starts the update of location service, and the last location coordinate is used  
		Input.location.Start(10.0f, 10.0f);

		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			//Suspend the execution of the collaborative program (1 second)  
			yield return new WaitForSeconds(1);
			maxWait--;
		}


		if (maxWait < 1)
		{
			log.text = "Init GPS service time out";
			StopGPS();
			yield break;

		}

		if (Input.location.status == LocationServiceStatus.Failed)
		{
			log.text = "Unable to determine device location";
			StopGPS();
			yield break;

		}
		else
		{



			Debug.Log($"longitude:  { Input.location.lastData.longitude}");
			Debug.Log($"latitude:  { Input.location.lastData.latitude}");
			Debug.Log($"altitude:  { Input.location.lastData.altitude}");
			if (pt == Point.oldPoint)
			{
				LongitudeA = Input.location.lastData.longitude;
				LatitudeA = Input.location.lastData.latitude;
				AltitudeA = Input.location.lastData.altitude;
				updatePoint(pt);
			}

			if (pt == Point.newPoint)
			{

				LongitudeB = Input.location.lastData.longitude;
				LatitudeB = Input.location.lastData.latitude;
				AltitudeB = Input.location.lastData.altitude;
				updatePoint(pt);

			}
			yield return new WaitForSeconds(100);
		}
		StopGPS();

	}
}