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
	* @api {C# - Class} PracticalAPI Class Description
	* @apiGroup PracticalAPI
	* @apiDescription The class "PracticalAPI" contains methods that enable you to track/record events in your application. The application must report at least 10 stats before being submitted. 
	* @apiParam (Methods) {Gaze} RecordGazeOn Tracks what objects are being gazed on.
	* @apiParam (Methods) {Gaze} RecordGazeOff Ends object tracking and reports seconds gazed.
	* @apiParam (Methods) {Gesture} RecordGesture Records when gestures are being used.
	* @apiParam (Methods) {Gesture} RecordHoldStarted Records when hold gesture starts.
	* @apiParam (Methods) {Gesture} RecordHoldCompleted Records when hold gesture completes and reports seconds held.
	* @apiParam (Methods) {Keyword} RecordKeyword Records what keywords are being said.
	* @apiParam (Methods) {Gain} RecordGain Records when user experiences a gain in application.
	* @apiParam (Methods) {Loss} RecordLoss Records when user experiences a loss in application.
	* @apiParam (Methods) {CustomStat} RecordCustomStat Records a customized metric/stat.
	* @apiParam (Methods) {GetErrorMessage} GetErrorMessage Returns specific error message.
	* @apiParam (Methods) {GetLastErrorMessage} GetLastErrorMessage Returns last error message.
	* 
	* 
	* @apiParam (Enum) {Formula} Formula Identifies if user is requesting count or average data analytics.
	* @apiParam (Enum) {Measurment} Measurment Identifies how measurment will be taken.
	* @apiParam (Enum) {GestureType} GestureType Identifies what type of gesture is being used.
	* 
	* 
	* @apiPermission admin
	* @apiVersion 0.1.0
	*/

	public bool logSummary;
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
	* @apiGroup PracticalAPI
	* @apiPermission admin
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
	* @apiGroup PracticalAPI
	* @apiPermission admin
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
	* @apiParam (DataField) {int} DoubleTap Identifies as a doubletap gesture.
	* @apiParam (DataField) {int} Hold Identifies as a hold gesture.
	* 
	* @apiGroup PracticalAPI
	* @apiPermission admin
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
	* @apiPermission admin
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
	* @apiPermission admin
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
	* RecordCustomStat(string uniqueIdentifier, float value);
	* RecordCustomStat(string uniqueIdentifier, float value, Measurement measurement, Formula formula);
	* @apiParamExample {json} Examples:
	* RecordCustomStat("WavesCompleted", 1);
	* RecordCustomStat("Combo", 1, Measurement.Count, Formula.Total);
	* RecordCustomStat("SecondsLeft", 32, Measurement.Count, Formula.Average);
	* @apiGroup PracticalAPI
	* @apiPermission admin
	* @apiVersion 0.1.0
	*/

	public void RecordCustomStat(string uniqueIdentifier, float value, Measurement measurement = Measurement.Count, Formula formula = Formula.Total, string gazedObject = "")
	{
		if (logSummary)
		{
			Debug.Log("Custom stat recorded: \n");
			Debug.Log("Unique Identifier " + uniqueIdentifier + "\n");
			Debug.Log("Value: " + value + "\n");

			if (GazeManager.Instance.HitObject != null)
			{
				var objName = GazeManager.Instance.HitObject.gameObject.name;
				Debug.Log("Object name: " + objName + "\n");
			}

			Debug.Log("Measurement:" + (int)measurement);
			Debug.Log("Formula: " + (int)formula);
		}


		if (GazeManager.Instance.HitObject != null)
		{
			var objName = GazeManager.Instance.HitObject.gameObject.name;
#if WINDOWS_UWP
			int ret = PracticalDLL.RecordCustomStat(uniqueIdentifier, value.ToString(), ((int)measurement).ToString(), ((int)formula).ToString(), objName);

			if (ret > 0)
			{
				Debug.Log(GetErrorMessage(ret));
			}
#endif
		}
		else
		{
#if WINDOWS_UWP
			int ret = PracticalDLL.RecordCustomStat(uniqueIdentifier, value.ToString(), ((int)measurement).ToString(), ((int)formula).ToString(), gazedObject);

			if (ret > 0)
			{
				Debug.Log(GetErrorMessage(ret));
			}
#endif
		}
	}


	/**
	* @api {C# - Method} RecordGain() RecordGain()
	* @apiName RecordGain()
	* @apiDescription RecordGain() records when user gains resources.
	* 
	* @apiParam (Parameters) {string} uniqueIdentifier Reference identification of stat.
	* @apiParam (Parameters) {float} value References the stat value.
	* @apiParamExample {json} Structure:
	* RecordGain(string uniqueIdentifier, float value);
	* @apiParamExample {json} Example:
	* RecordGain("HealthGained", 25);
	* @apiGroup PracticalAPI
	* @apiPermission admin
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
	* RecordLoss(string uniqueIdentifier, float value)
	* @apiParamExample {json} Example:
	* RecordLoss("StageFailed", 1);
	* @apiGroup PracticalAPI
	* @apiPermission admin
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
	* RecordKeyword(string keyword);
	* @apiParamExample {json} Example:
	* RecordKeyword(“Combo”);
	* @apiGroup PracticalAPI
	* @apiPermission admin
	* @apiVersion 0.1.0
	*/

	public void RecordKeyword(string keyword, string interactObject = "")
	{
		if (logSummary)
		{
			Debug.Log("Keyword recorded: " + keyword + "\n");

			if (GazeManager.Instance.HitObject != null)
			{
				var objName = GazeManager.Instance.HitObject.gameObject.name;
				Debug.Log("Target: " + objName + "\n");
			}
		}

		if (GazeManager.Instance.HitObject != null)
		{
			var objName = GazeManager.Instance.HitObject.gameObject.name;
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
	* @apiParam (Parameters) {string} uniqueIdentifier Records gesture event name.
	* @apiParam (Parameters) {GestureType} gestureType Identifies which type of gesture is being used.
	* @apiParam (Parameters) {string} gazedObject Reference the GameObject being gazed while stat is being recorded. (AutoGenerated)
	* @apiParamExample {json} Structure:
	* RecordGesture(string uniqueIdentifier);
	* RecordGesture(string uniqueIdentifier, GestureType.DoubleTap);
	* @apiParamExample {json} Example:
	* RecordGesture(shotFired);
	* @apiGroup PracticalAPI
	* @apiPermission admin
	* @apiVersion 0.1.0
	*/

	public void RecordGesture(string uniqueIdentifier, float HoldLength = 0.0f, GestureType gestureType = GestureType.Tap, string gazedObject = "")
	{
		if (logSummary)
		{
			Debug.Log("Tap event recorded: " + uniqueIdentifier + "\n");
			Debug.Log("GestureType: " + gestureType + "\n");
			if (GazeManager.Instance.HitObject != null)
			{
				var objName = GazeManager.Instance.HitObject.gameObject.name;
				Debug.Log("Object name: " + objName + "\n");
			}
			if (HoldLength > 0)
			{
				Debug.Log("Tap Hold Length : " + HoldLength + " seconds\n");
			}
		}


		if (GazeManager.Instance.HitObject != null)
		{
			// Will pass objName from HitObject & HoldLength default values if not provided. 
			var objName = GazeManager.Instance.HitObject.gameObject.name;
#if WINDOWS_UWP
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
#endif
		}
	}

	/**
	* @api {C# - Method} RecordHoldStarted() RecordHoldStarted()
	* @apiName RecordHoldStarted()
	* @apiDescription RecordHoldStarted() starts on hold tracking.
	* 
	* @apiParamExample {json} Structure:
	* RecordHoldStarted();
	* @apiParamExample {json} Example:
	* RecordHoldStarted();
	* @apiGroup PracticalAPI
	* @apiPermission admin
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
	* RecordHoldComplete(string uniqueIdentifier);
	* @apiParamExample {json} Example:
	* RecordHoldComplete("Charged laser");
	* @apiGroup PracticalAPI
	* @apiPermission admin
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
	* RecordGazeOn(string focusObject);
	* @apiParamExample {json} Example:
	* RecordGazeOn(Target.name);
	* @apiGroup PracticalAPI
	* @apiPermission admin
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
	* RecordGazeOff();
	* @apiParamExample {json} Example:
	* RecordGazeOff();
	* @apiGroup PracticalAPI
	* @apiPermission admin
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
	* @apiDescription IncludeMapping() will add gaze tracking to mapping objects.
	* 
	* @apiParamExample {json} Structure:
	* IncludeMapping();
	* @apiGroup PracticalAPI
	* @apiPermission admin
	* @apiVersion 0.1.0
	*/

	public void IncludeMapping()
	{
		var goArray = FindObjectsOfType<GameObject>();

		foreach (var obj in goArray)
		{
			if (obj.layer == MappingPhysicsLayer)
			{
				obj.AddComponent<PracticalGazeTracker>();
				obj.GetComponent<PracticalGazeTracker>().customIdentifier = "Mapping";
			}
		}

	}
	[Tooltip("The physics layer for spatial mapping objects to be set to.")]
	public int MappingPhysicsLayer = 31;
}

