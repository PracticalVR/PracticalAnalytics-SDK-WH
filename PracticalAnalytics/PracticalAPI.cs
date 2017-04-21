using System;
using System.Diagnostics;
using Assets;
using Assets.PracticalAssets;
using Assets.PracticalAssets.Practical_Analytics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Runtime.InteropServices;
using HoloToolkit.Unity.InputModule;


public class PracticalAPI : PracticalSingleton<PracticalAPI>
{
	/**
	* @api {C# - Class} PracticalAPI Overview
	* @apiGroup Basics
	* @apiDescription PracticalAPI contains methods that enable you to track/record events in your application. A helper script is available for gaze input.
	* @apiParam (Methods) {Gaze} RecordGazeOn Tracks what objects are being gazed on.
	* @apiParam (Methods) {Gaze} RecordGazeOff Ends object tracking and reports seconds gazed.
	* @apiParam (Methods) {Gesture} RecordGesture Records when gestures are being used.
	* @apiParam (Methods) {Gesture} RecordHoldStarted Records when hold gesture starts.
	* @apiParam (Methods) {Gesture} RecordHoldCompleted Records when hold gesture completes and reports seconds held.
	* @apiParam (Methods) {Mapping} IncludeMapping Adds gaze tracking to mapped objects.
	* @apiParam (Methods) {Keyword} RecordKeyword Records what keywords are being said.
	* @apiParam (Methods) {Gain} RecordGain Records when user experiences a gain in application.
	* @apiParam (Methods) {Loss} RecordLoss Records when user experiences a loss in application.
	* @apiParam (Methods) {CustomStat} RecordCustomStat Records a customized metric/stat.
	* @apiParam (Methods) {GetErrorMessage} GetErrorMessage Returns specific error message.
	* @apiParam (Methods) {GetLastErrorMessage} GetLastErrorMessage Returns last error message.
	* 
	* 
	* @apiParam (Enums) {Formula} Formula Identifies if user is requesting count or average data analytics.
	* @apiParam (Enums) {Measurment} Measurment Identifies how measurment will be taken.
	* @apiParam (Enums) {GestureType} GestureType Identifies what type of gesture is being used.
	* 
	* @apiParam (Helpers) {Script} PracticalGazeTracker Provides a custom identifier that group GameObject stats together.
	* 
	* 
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/

	public bool logSummary;
	private GazeManager gazeManager;
	private Stopwatch gazeStopWatch;
	private Stopwatch gestureStopWatch;
	private string viewedObject;

	/**
	* @api {C# - Enum} Measurement Measurement
	* @apiName Measurement
	* @apiDescription Measurement identifies how measurment will be taken.
	* 
	* @apiParam (DataField) {int} Count Measures by adding 1 (default).
	* @apiParam (DataField) {int} Second Measures in seconds.
	* @apiParam (DataField) {int} Feet Measures in feet.
	* @apiParam (DataField) {int} Meter Measures in meters.
	* 
	* @apiGroup Enums
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/

	public enum Measurement
	{
		Count,
		Second,
		Feet,
		Meter
	};

	/**
	* @api {C# - Enum} Formula Formula
	* @apiName Formula
	* @apiDescription Formula identifies if user is requesting total or average data analytics.
	* 
	* @apiParam (DataField) {int} Total Adds 1 (default).
	* @apiParam (DataField) {int} Average Finds Average.
	* 
	* @apiGroup Enums
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/
	public enum Formula
	{
		Total,
		Average
	};

	/**
	* @api {C# - Enum} GestureType GestureType
	* @apiName GestureType
	* @apiDescription GestureType identifies if user is requesting total or average data analytics.
	* 
	* @apiParam (DataField) {int} Tap Identifies as a tap gesture.
	* @apiParam (DataField) {int} Hold Identifies as a hold gesture.
	* 
	* @apiGroup Enums
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/

	public enum GestureType
	{
		Tap,
		DoubleTap,
		Hold
	};

	
	void Start()
	{
		gazeStopWatch = new Stopwatch();
		gestureStopWatch = new Stopwatch();
		gazeManager = GazeManager.Instance;
	}

	/**
	* @api {C# - Method} GetErrorMessage() GetErrorMessage()
	* @apiName GetErrorMessage()
	* @apiDescription GetErrorMessage() returns specific error message.
	* 
	* 
	* @apiParam (Parameters) {int} errorCode References specific error code.
	* @apiParamExample {json} Structure:
	* GetErrorMessage(int errorCode);
	* @apiParamExample {json} Example:
	* GetErrorMessage(1);
	* @apiGroup PracticalAPI
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/

#if WINDOWS_UWP
	public string GetErrorMessage(int errorCode)
	{
		return Marshal.PtrToStringAnsi(PracticalDLL.GetErrorMessage(errorCode));
	}
#endif

	/**
	* @api {C# - Method} GetLastErrorMessage() GetLastErrorMessage()
	* @apiName GetLastErrorMessage()
	* @apiDescription GetLastErrorMessage() returns last error message.
	* 
	* @apiParamExample {json} Example:
	* GetLastErrorMessage();
	* @apiGroup PracticalAPI
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/

#if WINDOWS_UWP
	public string GetLastErrorMessage()
	{
		return Marshal.PtrToStringAnsi(PracticalDLL.GetLastErrorMessage());
	}
#endif


	/**
	* @api {C# - Method} RecordCustomStat() RecordCustomStat()
	* @apiName RecordCustomStat()
	* @apiDescription RecordCustomStat() records a custom metric/stat. Custom stats can be used in variety of ways.
	* 
	* 
	* @apiParam (Parameters) {string} uniqueIdentifier References identification of customized metric/stat.
	* @apiParam (Parameters) {float} value References value of customized metric/stat.
	* @apiParam (Parameters) {Measurment} measurement References measurement of customized metric/stat. Count, Second, Feet, or Meter. (optional)
	* @apiParam (Parameters) {Formula} formula References formula of customized metric/stat. Total or Average. (optional)
	* @apiParam (Parameters) {string} gazedObject References the GameObject being gazed while stat is being recorded. (AutoGenerated)
	* @apiParamExample {json} Structure:
	* PracticalAPI.Instance.RecordCustomStat(string uniqueIdentifier, float value);
	* PracticalAPI.Instance.RecordCustomStat(string uniqueIdentifier, float value, Measurement measurement, Formula formula);
	* @apiParamExample {json} Examples:
	* PracticalAPI.Instance.RecordCustomStat("WavesCompleted", 1);
	* PracticalAPI.Instance.RecordCustomStat("Combo", 1, Measurement.Count, Formula.Total);
	* PracticalAPI.Instance.RecordCustomStat("SecondsLeft", 32, Measurement.Count, Formula.Average);
	* @apiGroup PracticalAPI
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/

	public void RecordCustomStat(string uniqueIdentifier, float value, Measurement measurement = Measurement.Count, Formula formula = Formula.Total, string gazedObject = "")
	{
		if (logSummary)
		{
			Debug.Log("Custom stat recorded: \n");
			Debug.Log("Unique Identifier " + uniqueIdentifier + "\n");
			Debug.Log("Value: " + value + "\n");

			if (gazeManager.HitObject != null)
			{
				var objName = gazeManager.HitObject.gameObject.name;
				Debug.Log("Object name: " + objName + "\n");
			}

			Debug.Log("Measurement:" + (int)measurement);
			Debug.Log("Formula: " + (int)formula);
		}

#if WINDOWS_UWP
		if (gazeManager.HitObject != null)
		{
			var objName = gazeManager.HitObject.gameObject.name;

			int ret = PracticalDLL.RecordCustomStat(uniqueIdentifier, value.ToString(), ((int)measurement).ToString(), ((int)formula).ToString(), objName);

			if (ret > 0)
			{
				Debug.Log(GetErrorMessage(ret));
			}
		}
		else
		{
			int ret = PracticalDLL.RecordCustomStat(uniqueIdentifier, value.ToString(), ((int)measurement).ToString(), ((int)formula).ToString(), gazedObject);

			if (ret > 0)
			{
				Debug.Log(GetErrorMessage(ret));
			}
		}
#endif
	}


	/**
	* @api {C# - Method} RecordGain() RecordGain()
	* @apiName RecordGain()
	* @apiDescription RecordGain() records when user gains resources.
	* 
	* @apiParam (Parameters) {string} uniqueIdentifier Reference identification of stat.
	* @apiParam (Parameters) {float} value References the stat value.
	* @apiParamExample {json} Structure:
	* PracticalAPI.Instance.RecordGain(string uniqueIdentifier, float value);
	* @apiParamExample {json} Example:
	* PracticalAPI.Instance.RecordGain("HealthGained", 25);
	* @apiGroup PracticalAPI
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/

	public void RecordGain(string uniqueIdentifier, float value)
	{
		if (logSummary)
		{
			Debug.Log("Resource gained recorded: \n");
			Debug.Log("Unique Identifier: " + uniqueIdentifier + "\n");
			Debug.Log("Value: " + value + "\n");
		}
#if WINDOWS_UWP
		int ret = PracticalDLL.RecordGain(uniqueIdentifier, value.ToString());
		
		if(ret > 0) {
			Debug.Log(GetErrorMessage(ret));
		}
#endif
	}

	/**
	* @api {C# - Method} RecordLoss() RecordLoss()
	* @apiName RecordLoss()
	* @apiDescription RecordLoss() records when user looses resources.
	* 
	* @apiParam (Parameters) {string} uniqueIdentifier Reference identification of stat.
	* @apiParam (Parameters) {float} value Referencese the stat value.
	* 
	* @apiParamExample {json} Structure:
	* PracticalAPI.Instance.RecordLoss(string uniqueIdentifier, float value)
	* @apiParamExample {json} Example:
	* PracticalAPI.Instance.RecordLoss("StageFailed", 1);
	* @apiGroup PracticalAPI
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/

	public void RecordLoss(string uniqueIdentifier, float value)
	{
		if (logSummary)
		{
			Debug.Log("Resource loss recorded: \n");
			Debug.Log("Unique Identifier: " + uniqueIdentifier + "\n");
			Debug.Log("Value: " + value + "\n");
		}
#if WINDOWS_UWP
		int ret = PracticalDLL.RecordLoss(uniqueIdentifier, value.ToString());
		
		if(ret > 0) {
			Debug.Log(GetErrorMessage(ret));
		}
#endif
	}

	/**
	* @api {C# - Method} RecordKeyword() RecordKeyword()
	* @apiName RecordKeyword()
	* @apiDescription RecordKeyword() records what keywords are being used.
	* 
	* @apiParam (Parameters) {string} keyword Records keyword use in application.
	* @apiParam (Parameters) {string} gazedObject References the GameObject being gazed while stat is being recorded. (AutoGenerated)
	* @apiParamExample {json} Structure:
	* PracticalAPI.Instance.RecordKeyword(string keyword);
	* @apiParamExample {json} Example:
	* PracticalAPI.Instance.RecordKeyword(“Combo”);
	* @apiGroup PracticalAPI
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/

	public void RecordKeyword(string keyword, string interactObject = "")
	{
		if (logSummary)
		{
			Debug.Log("Keyword recorded: " + keyword + "\n");

			if (gazeManager.HitObject != null)
			{
				var objName = gazeManager.HitObject.gameObject.name;
				Debug.Log("Target: " + objName + "\n");
			}
		}

		if (gazeManager.HitObject != null)
		{
			var objName = gazeManager.HitObject.gameObject.name;
#if WINDOWS_UWP
			int ret = PracticalDLL.RecordKeyword(keyword, "1.0", objName);
			if (ret > 0)
			{
				Debug.Log(GetErrorMessage(ret));
			}

		}
		else
		{

			int ret = PracticalDLL.RecordKeyword(keyword, "1.0", interactObject);

			if (ret > 0)
			{
				Debug.Log(GetErrorMessage(ret));
			}
#endif
		}
	}

	/**
	* @api {C# - Method} RecordGesture() RecordGesture()
	* @apiName RecordGesture()
	* @apiDescription RecordGesture() records what gestures are being used. GestureType defaults to Tap.
	* 
	* @apiParam (Parameters) {string} uniqueIdentifier Records name of object being gesture to. (Example: HitObject.gameObject.name)
	* @apiParam (Parameters) {GestureType} gestureType Identifies which type of gesture is being used.
	* @apiParam (Parameters) {string} gazedObject Reference the GameObject being gazed while stat is being recorded. (AutoGenerated)
	* @apiParamExample {json} Structure:
	* PracticalAPI.Instance.RecordGesture(string uniqueIdentifier);
	* @apiParamExample {json} Example:
	* protected void OnTappedEvent(InteractionSourceKing source, int tapCount, Ray headRay)
	*{
	*   "Get the current Gaze's hit object"
	*   var hitObj = GazeManager.Instance.HitObject;
	*		
	*   "Send an OnSelect message to the focused object and it's ancestors."
	*   if (hitObj != null)
	*   {
	*      if(hitObj.layer == 11)
	*      {
	*         PracticalAPI.Instance.RecordGesture("Mapping");
	*      }
	*      else
	*      {
	*         PracticalAPI.Instance.RecordGesture(hitObj.gameObject.name);
	*      }
	*   }
	*   else
	*   {
    *      PracticalAPI.Instance.RecordGesture("No Hit");
    *   }
	*}
	* @apiGroup PracticalAPI
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/

	public void RecordGesture(string uniqueIdentifier, float HoldLength = 0.0f, GestureType gestureType = GestureType.Tap, string gazedObject = "")
	{
		if (logSummary)
		{
			Debug.Log("Tap event recorded: " + uniqueIdentifier + "\n");
			Debug.Log("GestureType: " + gestureType + "\n");
			if (gazeManager.HitObject != null)
			{
				var objName = gazeManager.HitObject.gameObject.name;
				Debug.Log("Object name: " + objName + "\n");
			}
			if (HoldLength > 0)
			{
				Debug.Log("Tap Hold Length : " + HoldLength + " seconds\n");
			}
		}

#if WINDOWS_UWP
		if (gazeManager.HitObject != null)
		{
			// Will pass objName from HitObject & HoldLength default values if not provided. 
			var objName = gazeManager.HitObject.gameObject.name;
			int ret = PracticalDLL.RecordGesture(uniqueIdentifier, "1.0", ((int)gestureType).ToString(), objName, HoldLength.ToString());

			if (ret > 0)
			{
				Debug.Log(GetErrorMessage(ret));
			}
		}
		else
		{
			// Will pass just the HoldLength value, and interacted object with default values if not provided. 
			int ret = PracticalDLL.RecordGesture(uniqueIdentifier, "1.0", ((int)gestureType).ToString(), gazedObject, HoldLength.ToString());

			if (ret > 0)
			{
				Debug.Log(GetErrorMessage(ret));
			}

		}
#endif
	}

	/**
	* @api {C# - Method} RecordHoldStarted() RecordHoldStarted()
	* @apiName RecordHoldStarted()
	* @apiDescription RecordHoldStarted() starts on hold tracking.
	* 
	* @apiParamExample {json} Structure:
	* PracticalAPI.Instance.RecordHoldStarted();
	* @apiParamExample {json} Example:
	* PracticalAPI.Instance.RecordHoldStarted();
	* @apiGroup PracticalAPI
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/

	public void RecordHoldStarted()
	{
		gestureStopWatch.Start();
	}

	/**
	* @api {C# - Method} RecordHoldComplete() RecordHoldComplete()
	* @apiName RecordHoldComplete()
	* @apiDescription RecordHoldComplete() reports seconds spent holding.
	* @apiParam (Parameters) {string} uniqueIdentifier Records gesture event name.
	* 
	* @apiParamExample {json} Structure:
	* PracticalAPI.Instance.RecordHoldComplete(string uniqueIdentifier);
	* @apiParamExample {json} Example:
	* PracticalAPI.Instance.RecordHoldComplete("Charged laser");
	* @apiGroup PracticalAPI
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/

	public void RecordHoldComplete(string uniqueIdentifier)
	{
		gestureStopWatch.Stop();

		double milliseconds = gazeStopWatch.Elapsed.TotalSeconds;

		RecordGesture(uniqueIdentifier, (float)milliseconds, GestureType.Hold);

		gazeStopWatch.Reset();
	}

	/**
	* @api {C# - Method} RecordGazeOn() RecordGazeOn()
	* @apiName RecordGazeOn()
	* @apiDescription RecordGazeOn() tracks what objects are being gazed on.
	* 
	* @apiParam (Parameters) {string} gazedObject Records the object name that is being gazed.
	* @apiParamExample {json} Structure:
	* PracticalAPI.Instance.RecordGazeOn(string gazedObject);
	* 
	* See "PracticalGazeTracker.cs" for additional gaze tracking implementations.
	* @apiParamExample {json} Example:
	* PracticalAPI.Instance.RecordGazeOn(Target.name);
	* @apiGroup PracticalAPI
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/

	public void RecordGazeOn(string gazedObject)
	{
		viewedObject = gazedObject;
		gazeStopWatch.Start();
	}


	/**
	* @api {C# - Method} RecordGazeOff() RecordGazeOff()
	* @apiName RecordGazeOff()
	* @apiDescription RecordGazeOff() ends object tracking and reports seconds gazed.
	* 
	* @apiParamExample {json} Structure:
	* PracticalAPI.Instance.RecordGazeOff();
	* 
	* See "PracticalGazeTracker.cs" for additional gaze tracking implementations.
	* @apiParamExample {json} Example:
	* PracticalAPI.Instance.RecordGazeOff();
	* @apiGroup PracticalAPI
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/

	public void RecordGazeOff()
	{
		gazeStopWatch.Stop();
		
		double milliseconds = gazeStopWatch.Elapsed.TotalSeconds;

		RecordGaze((float)milliseconds);

		gazeStopWatch.Reset();
	}

	private void RecordGaze(float gazeLength)
	{
		if (logSummary)
		{
			Debug.Log("Object Viewed: " + viewedObject + " for " + gazeLength + " seconds. \n");
		}
#if WINDOWS_UWP
		int ret = PracticalDLL.RecordGaze(viewedObject, gazeLength.ToString());

		if (ret > 0)
		{
			Debug.Log(GetErrorMessage(ret));
		}
#endif
	}

	/**
	 * @api {C# - Method} IncludeMapping() IncludeMapping()
	 * @apiName IncludeMapping()
	 * @apiDescription IncludeMapping() will add gaze tracking to mapped objects.
	 * 
	 * @apiParamExample {json} Structure:
	 * PracticalAPI.Instance.IncludeMapping();
	 * 
	 * @apiGroup PracticalAPI
	 * @apiPermission Beta
	 * @apiVersion 0.1.0
	 */

	[Tooltip("The physics layer for spatial mapping objects to be set to.")]
    public int MappingPhysicsLayer = 31;

    public void IncludeMapping()
	{
		var goArray = FindObjectsOfType<GameObject>();

		foreach (var obj in goArray)
		{
			if (obj.layer == MappingPhysicsLayer)
			{
				var tracker = obj.AddComponent<PracticalGazeTracker>();
                tracker.customName = true;
				tracker.customIdentifier = "Mapping";
			}
		}

	}
	/**
	* @api {C# - Class} PracticalGazeTracker.cs PracticalGazeTracker.cs
	* @apiName PracticalGazeTracker.cs
	* @apiDescription PracticalGazeTracker.cs provides a field for a custom identifier that groups GameObject stats together when they share the same identifier.
	* 
	* @apiExample Usage:
	* If you want to group multiple GameObject stats together, 
	* you can drag this script onto those objects and give them an identical custom name.
	* The custom name will show in the portal with the collected stats from that group. 
	* 
	* See IncludeMapping() to add mapping to gaze tracking. 
	* 
	* @apiParam (Variables) {bool} customName Triggers use of a seperate custom GameObject name.
	* @apiParam (Variables) {string} customIdentifier Assigns a custom GameObject name.
	* 
	* @apiGroup Helpers
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/

	/**
	* @api {beta release} Info Info
	* @apiName Info
	* @apiExample Getting Started:
	* Repository:
	* https://github.com/PracticalVR/PracticalAnalytics-SDK-WH
	* 
	* Import the PracticalAnayltics package and drag the PracticalManager prefab into your scene.
	* 
	* Access the API Key from the "My Apps" section of http://analytics.practicalvr.com
	* 
	* You can enable Log Summary in the inspector to see data being collected in the debug log.
	* 
	* Give feedback in the inspector when PracticalManager is selected.
	* 
	* To set up Dll, locate the plugins folder. 
	* Inside the folder will be the DLL.
	* In inspector, set only WSAPlayer.
	* Make sure CPU is set to X86.
	* Click Apply.
	* 
	* A clean build is required when implementing DLL.
	* You must delete the content in your build folder when doing the initial build with DLL.
	* 
	* You must gather a minimum of 10 stats before session data is accepted. 
	* 
	* Capabilities in Unity must include InternetClient and PicturesLibrary for platform integration.
	* 
	* @apiGroup Basics
	* @apiPermission Beta
	* @apiVersion 0.1.0
	*/
}

