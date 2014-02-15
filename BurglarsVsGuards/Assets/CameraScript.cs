using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour {

	public Transform targetPlayer;
	Vector3 newPosition;
	float currentCameraSize = 5;
	public float extraViewDistance = 9f;
	float distanceBetweenPlayers;
	bool zoomOut = false;
	float zoomTime = 0;
	public bool zoomAble = false;
	public OuyaPlayer observedPlayer = OuyaPlayer.P01;
	public List <Transform> Players = new List <Transform>();


	// Use this for initialization
	void Start ()
	{
		
	}
	
	void OnApplicationQuit()
	{
		OuyaInput.ResetInput();
	}

	// Update is called once per frame
	void Update ()
	{
		
		//Camera size and position depending on players
		if(Players.Count == 2)
		{
			distanceBetweenPlayers = Vector2.Distance(Players[0].position,Players[1].position);
			newPosition = Players[0].position+(Players[1].position-Players[0].position)*0.5f;
			newPosition.z = -10;
			transform.position = newPosition;
		}

		//newPosition = targetPlayer.position;
		//newPosition.z = -10;
		//transform.position = newPosition;

		if(OuyaInput.GetButtonDown(OuyaButton.LB, observedPlayer))
		{
			zoomOut = true;
		}

		if(OuyaInput.GetButtonUp(OuyaButton.LB, observedPlayer))
		{
			zoomOut = false;
		}

		if(zoomOut && zoomTime < 1 && zoomAble)
		{
			zoomTime += Time.deltaTime*2;
		}
		if(!zoomOut && zoomTime > 0 && zoomAble)
		{
			zoomTime -= Time.deltaTime*2;
		}
		camera.orthographicSize = Mathf.Lerp(distanceBetweenPlayers*0.5f+extraViewDistance,25,zoomTime);
		//print(distanceBetweenPlayers);
	}
}
