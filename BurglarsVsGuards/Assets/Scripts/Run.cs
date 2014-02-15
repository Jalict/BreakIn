using UnityEngine;
using System.Collections;

public class Run : MonoBehaviour {

	void Start () {
        if (!BreakNetwork.ConnectRandom())
            BreakNetwork.server = true;
	}
}
