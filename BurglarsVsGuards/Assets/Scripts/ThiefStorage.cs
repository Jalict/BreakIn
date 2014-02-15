using UnityEngine;
using System.Collections;

public class ThiefStorage : MonoBehaviour
{

    public movement Movement;

    public GameObject[] Inventory = new GameObject[3];
    public GameObject currentProp = null;
    private float pickupCounterTime = 0;

    // Use this for initialization
    private void Start()
    {

        Movement = gameObject.GetComponent<movement>();

        if (Movement == null)
            Debug.Log("ERROR - need to be assigned movement for player");

        for (int i = 0; i < 3; i++)
            Inventory[i] = null;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    void StartPickupProp()
    {
        // has available slots?
        bool availableSlots = false;
        for (int i = 0; i < 3; i++)
        {
            if (Inventory[i] == null) // pickup if empty slot
                availableSlots = true;
        }
        if (availableSlots == false)
            return;


        // close enough?
        if (Vector2.Distance(transform.position, currentProp.transform.position) < 5)
        {
            // start picking up
            if (OuyaInput.GetButton(OuyaButton.RB, Movement.observedPlayer))
            {
                pickupCounterTime += Time.deltaTime;
                print(pickupCounterTime);

                // pickup time reached
                if (pickupCounterTime >= currentProp.GetComponent<PropsToPickUp>().PickupTime)
                {
                    int availableSlot = -1;

                    // look through bag for available slots
                    for (int i = 3 - 1; i >= 0; i--)
                    {
                        if (Inventory[i] == null) // pickup if empty slot
                            availableSlot = i;
                    }

                    // has picked up prop
                    if (availableSlot != -1)
                    {
                        Inventory[availableSlot] = currentProp;
                        currentProp.GetComponent<PropsToPickUp>().CanBePickedUp = false;
                    }
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {

        // check to see if it's a pickupable prop
        if ((coll.gameObject.GetComponent("PropsToPickUp") as PropsToPickUp) != null)
        {
            if (coll.gameObject.GetComponent<PropsToPickUp>().CanBePickedUp)
                currentProp = coll.gameObject;
        }
        else
        {
            currentProp = null;
            return;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2, Screen.height - 130, 500, 60), gameObject.name);
        GUI.Label(new Rect(Screen.width / 2, Screen.height - 100, 500, 60), "Slot1: " + Inventory[0]);
        GUI.Label(new Rect(Screen.width / 2, Screen.height - 70, 500, 60), "Slot2: " + Inventory[1]);
        GUI.Label(new Rect(Screen.width / 2, Screen.height - 40, 500, 60), "Slot3: " + Inventory[2]);
    }
}
