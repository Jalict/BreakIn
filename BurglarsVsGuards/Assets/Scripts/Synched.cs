using UnityEngine;
using System.Collections;

public class Synched : MonoBehaviour {

    public Transform prefab;

    void OnLevelWasLoaded() {
        if(Network.isServer && prefab != null)
            Network.Instantiate(prefab, transform.position, transform.rotation, 0);
        Destroy(gameObject);
    }
}
