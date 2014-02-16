using UnityEngine;
using System.Collections;

public class SpawnPickupables : MonoBehaviour
{

    public GameObject[] SpawnPoints;
    public GameObject[] DifferentTypesOfPickups;
    public int[] NumberOfEach;

    private int divider;

    // Use this for initialization
    private void Start()
    {
        if (DifferentTypesOfPickups.Length != NumberOfEach.Length)
            Debug.Log("ERROR - DifferentTypesOfPickups and NumberOfEach has to be same size!");

        divider = SpawnPoints.Length/DifferentTypesOfPickups.Length;

        for (int i = 0; i < DifferentTypesOfPickups.Length; i++)
        {
            NumberOfEach[i] = divider;
        }

        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            int random = Random.Range(0, DifferentTypesOfPickups.Length);
            int counter = 0;

            while (NumberOfEach[random] <= 0 && counter < 500)
            {
                random = Random.Range(0, DifferentTypesOfPickups.Length);
                counter++;
               
            }

            string path = "Prefabs/" + DifferentTypesOfPickups[random].name;
            NumberOfEach[random]--;

            if (Network.connections.Length > 0)
            {
                Connector.AddEntity(DifferentTypesOfPickups[random].name, SpawnPoints[i].transform.position, Quaternion.identity, path,
                                    "Untagged",
                                    true);
            }

            else
            {
                GameObject o = (GameObject)Instantiate(Resources.Load(path), SpawnPoints[i].transform.position, Quaternion.identity);
                o.name = "PropToPickUp";

            }
        }

        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            Destroy(SpawnPoints[i]);
        }
    }
}
