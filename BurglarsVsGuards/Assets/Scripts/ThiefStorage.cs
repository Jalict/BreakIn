using UnityEngine;
using System.Collections;

public class ThiefStorage : MonoBehaviour
{

    public movement Movement;

    public GameObject[] Inventory = new GameObject[3];
    public GameObject currentProp = null;
    private PropsToPickUp CurrentPropToPickUp = null;

    private float pickupCounterTime = 0;

    private bool readyToPickup = false;

    public GameObject progressBar;

    private GameObject progress;

    public Transform[] PickupDisplayIcons = new Transform[3];
    public Vector3[] Offset = new Vector3[3];

    public int ThiefScore = 0;

    // Use this for initialization
    private void Start()
    {

        progress = null;

        Offset[0] = new Vector3(-0.4f, -0.81f, 0);
        Offset[1] = new Vector3(-0f, -0.81f, 0);
        Offset[2] = new Vector3(0.4f, -0.81f, 0);

        Movement = gameObject.GetComponent<movement>();

        if (PickupDisplayIcons[2] == null)
            Debug.Log("ERROR - needs PickupDisplayIcons");


        if (Movement == null)
            Debug.Log("ERROR - need to be assigned movement for player");

        for (int i = 0; i < 3; i++)
            Inventory[i] = null;
    }

    // Update is called once per frame
    private void Update()
    {
        if (readyToPickup)
            StartPickupProp();

        for (int i = 0; i < 3; i++)
        {
            PickupDisplayIcons[i].transform.position = transform.position + Offset[i];
        }
        /*for (int i = 0; i < 3; i++)
        {
            if (Inventory[i] != null)
                print("2");
        }*/
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


        if (currentProp.GetComponent<PropsToPickUp>().CanBePickedUp == false
        || (availableSlots == false))
            return;

        
        // close enough?
        if (Vector2.Distance(transform.position, currentProp.transform.position) < 2)
        {

            // start picking up
            if (OuyaInput.GetButtonDown(OuyaButton.RB, Movement.observedPlayer))
            {
                Vector3 pos = currentProp.transform.position + new Vector3(0, 0, -3f);

                progress = (GameObject)Instantiate(progressBar, pos, Quaternion.identity);
            }

            if (OuyaInput.GetButtonUp(OuyaButton.RB, Movement.observedPlayer))
            {
                if (progress != null)
                {
                    pickupCounterTime = 0;
                    Destroy(progress);
                }
            }

            if (OuyaInput.GetButton(OuyaButton.RB, Movement.observedPlayer))
            {
                pickupCounterTime += Time.deltaTime;
                
                if (progress != null)
                    progress.GetComponent<ProgressBar>().SetProgressTime(pickupCounterTime / CurrentPropToPickUp.PickupTime);
                //print(pickupCounterTime);

                // pickup time reached
                if (pickupCounterTime >= CurrentPropToPickUp.PickupTime)
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

                        ThiefScore += CurrentPropToPickUp.Money;

                        if (CurrentPropToPickUp.Money < 200)
                        {
                            PickupDisplayIcons[availableSlot].renderer.material.color = Color.gray;
                        }
                        else if (CurrentPropToPickUp.Money < 500)
                        {
                            PickupDisplayIcons[availableSlot].renderer.material.color = Color.green;
                            
                        }
                        else
                        {
                            PickupDisplayIcons[availableSlot].renderer.material.color = Color.yellow;                            
                        }

                        PickupDisplayIcons[availableSlot].renderer.enabled = true;


                        CurrentPropToPickUp.CanBePickedUp = false;
                        CurrentPropToPickUp.HasBeenPickedUp = true;

                        pickupCounterTime = 0;
                    }
                }
            }
        }
        else
            pickupCounterTime = 0;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        // check to see if it's a pickupable prop
        if ((coll.gameObject.GetComponent("PropsToPickUp") as PropsToPickUp) != null)
        {
            if (coll.gameObject.GetComponent<PropsToPickUp>().CanBePickedUp)
            {
                currentProp = coll.gameObject;
                CurrentPropToPickUp = currentProp.GetComponent<PropsToPickUp>();

                readyToPickup = true;
            }
            else
            {
                currentProp = null;
                CurrentPropToPickUp = null;
                readyToPickup = false;

                return;
            }
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2, Screen.height - 130, 500, 60), gameObject.name + " score:" + ThiefScore);
        GUI.Label(new Rect(Screen.width / 2, Screen.height - 100, 500, 60), "Slot1: " + Inventory[0]);
    }
}
