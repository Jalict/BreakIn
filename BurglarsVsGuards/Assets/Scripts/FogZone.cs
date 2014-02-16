using UnityEngine;
using System.Collections;

public class FogZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if(!Network.isServer)
            renderer.enabled = false;
	}
	
	// Update is called once per frame
    void Update() {
        if (Network.connections.Length == 0 || Network.isClient) {
            foreach (GameObject p in GameObject.FindGameObjectsWithTag("Thief"))
                if (collider.bounds.Contains(p.transform.position))
                    renderer.enabled = true;
        } else if (Network.isServer) {
            foreach (GameObject p in GameObject.FindGameObjectsWithTag("Guard"))
                if (collider.bounds.Contains(p.transform.position))
                    renderer.enabled = true;
        }
	}
    void OnDrawGizmos() {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
    }
}
