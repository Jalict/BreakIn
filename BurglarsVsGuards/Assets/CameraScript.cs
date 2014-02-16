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
    public GameObject[] Players = new GameObject[4];


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
        Players = GameObject.FindGameObjectsWithTag("Player");
        if (Players.Length == 0) return;

	    if (Input.GetKeyDown(KeyCode.R))
	        Application.LoadLevel(0);

		//Camera size and position depending on players
		if(Players.Length == 2)
		{
            distanceBetweenPlayers = Vector2.Distance(Players[0].transform.position, Players[1].transform.position);
            newPosition = Players[0].transform.position + (Players[1].transform.position - Players[0].transform.position) * 0.5f;
			newPosition.z = -10;
			transform.position = newPosition;
		}
		if(Players.Length == 1)
		{
			transform.position = Players[0].transform.position;
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
