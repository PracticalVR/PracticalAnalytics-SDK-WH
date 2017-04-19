using System.Collections;
using System.Collections.Generic;
using Assets.PracticalAssets.Practical_Analytics;
using UnityEngine;

public class PracticalConfig : MonoBehaviour
{
	public string APIKey;

	private PracticalDLL _practicalDLL;


	// Use this for initialization
	void Start ()
	{
		_practicalDLL = GameObject.Find("PracticalManager").GetComponent<PracticalDLL>();
		_practicalDLL.SetAPIKey(APIKey);
	}
	
}
