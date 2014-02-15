using UnityEngine;
using System.Collections;

public class Synched : MonoBehaviour {

	void Start () {
        if (Network.isServer) {
            Network.Instantiate(gameObject, transform.position, transform.rotation, 0);
        }
	}
	
	void Update () {
	    
	}

    void OnNetworkConnect() {
        Debug.Log("Success!!");
        Destroy(gameObject);
    }
}
