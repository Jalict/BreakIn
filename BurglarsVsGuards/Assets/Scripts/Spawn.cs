using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

    public bool isStatic;
    public string name;
    public string prefab;
    public string id;

	void Start () {
        if (Network.connections.Length > 0) {
            Connector.AddEntity(name, transform.position, transform.rotation, prefab, id, isStatic);
        } else {
            GameObject o = (GameObject)Instantiate(Resources.Load(prefab), transform.position, transform.rotation);
            o.name = name;
        }
        Destroy(gameObject);
	}
}
