using UnityEngine;
using System.Collections;

public class PropsToPickUp : MonoBehaviour
{

    public bool CanBePickedUp = true;

    public bool HasBeenPickedUp = false;
    public int Money = 100;

    public float PickupTime = 3f;
    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (HasBeenPickedUp)
        {
            transform.renderer.enabled = false;
            collider2D.enabled = false;
        }

    }
}
