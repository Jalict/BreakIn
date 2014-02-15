using UnityEngine;
using System.Collections;

public class Run : MonoBehaviour {

	void Start () {
        Debug.Log("Starting Game...");
        if (!BreakNetwork.ConnectRandom()) {
            BreakNetwork.server = true;
            Debug.Log("Starting Server...");
        } else {
            Debug.Log("Connecting...");
        }
	}
}
