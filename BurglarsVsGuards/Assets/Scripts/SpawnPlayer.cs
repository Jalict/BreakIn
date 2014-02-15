using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour {

	void Start () {
        Connector.AddEntity("Player", transform.position, transform.rotation, "Prefabs/Cube", "0");
	}
}
