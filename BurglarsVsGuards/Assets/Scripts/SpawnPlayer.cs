using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour {

    public string name;
    public bool thief;
    public string thiefprefab;
    public string burglorprefab;

	void Start () {
        if (Network.connections.Length > 0) {
            if(thief && Network.isClient)
                Connector.AddEntity(name, transform.position, transform.rotation, thiefprefab, "Player", false);
            if (!thief && Network.isServer)
                Connector.AddEntity(name, transform.position, transform.rotation, burglorprefab, "Player", false);
        } else {
            GameObject o = (GameObject)Instantiate(Resources.Load(thief?thiefprefab:burglorprefab), transform.position, transform.rotation);
            o.name = name;
        }
        Destroy(gameObject);
	}
}
