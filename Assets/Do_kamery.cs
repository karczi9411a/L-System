using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Do_kamery : MonoBehaviour
{
	[SerializeField]
	private float predkosc;

	private void Start()
	{
		predkosc = 40;
	}

	void Update()
	{	
		if(Input.GetKey(KeyCode.Q))
		{
			transform.RotateAround(Vector3.zero, -Vector3.up, predkosc * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.E))
		{
			transform.RotateAround(Vector3.zero, Vector3.up, predkosc * Time.deltaTime);
		}
	}
}