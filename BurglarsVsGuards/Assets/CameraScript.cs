using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour {

    public AudioClip music;
	public Transform targetPlayer;
	Vector3 newPosition;
	float currentCameraSize = 5;
	float extraViewDistance = 9f;
	float distanceBetweenPlayers;
	bool zoomOut = false;
	float zoomTime = 0;
	public bool zoomAble = false;
	public OuyaPlayer observedPlayer = OuyaPlayer.P01;
    public GameObject[] Players = new GameObject[4];


	// Use this for initialization
	void Start ()
	{
        if (music != null) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = music;
            source.loop = true;
            source.Play(1);
        }
	}
	
	void OnApplicationQuit()
	{
		OuyaInput.ResetInput();
	}

	// Update is called once per frame
	void Update ()
	{
        if(Network.connections.Length == 0 || Network.isClient)
            Players = GameObject.FindGameObjectsWithTag("Thief");
        else if(Network.isServer)
            Players = GameObject.FindGameObjectsWithTag("Guard");
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
			Vector3 tmp = Players[0].transform.position;
			tmp.z = -10;
			transform.position = tmp;
			distanceBetweenPlayers = 0;
			extraViewDistance = 15f;
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
