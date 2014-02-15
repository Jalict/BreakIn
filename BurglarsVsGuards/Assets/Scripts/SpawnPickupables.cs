using UnityEngine;
using System.Collections;

public class SpawnPickupables : MonoBehaviour
{

    public Transform[] SpawnPoints;
    public GameObject[] Pickupables;

    // Use this for initialization
    private void Start()
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            int random = Random.Range(0, Pickupables.Length);

            string path = "Prefabs/" + Pickupables[random].name;

            if (Network.connections.Length > 0)
            {
                Connector.AddEntity(Pickupables[random].name, SpawnPoints[i].position, Quaternion.identity, path,
                                    "Untagged",
                                    true);
            }

            else
            {
                GameObject o = (GameObject)Instantiate(Resources.Load(path), SpawnPoints[i].position, Quaternion.identity);
                o.name = "bla";

            }
        }
    }
}
