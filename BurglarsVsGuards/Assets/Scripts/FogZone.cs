using UnityEngine;
using System.Collections;

public class FogZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
        renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
            if (collider.bounds.Contains(p.transform.position))
                renderer.enabled = true;
	}
}
