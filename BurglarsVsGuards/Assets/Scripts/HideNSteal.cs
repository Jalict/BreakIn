using UnityEngine;
using System.Collections;

enum propTypes
{
	hiding,
	pickup
}

public class HideNSteal : MonoBehaviour
{

    public movement Movement;

    public GameObject[] Inventory = new GameObject[3];
    public GameObject currentProp = null;
    propTypes currentPropType;

    private PropsToHideIn CurrentPropToHideIn = null;
    private PropsToPickUp CurrentPropToPickUp = null;

    private float hidingCounterTime = 0;
    private float pickupCounterTime = 0;
    bool hidden = false;

    public bool readyToHide = false;
    private bool readyToPickup = false;

    public GameObject progressBar;
	public GameObject RBButton;
    
    private GameObject progress;
    private GameObject ActionButton;

    public Transform[] PickupDisplayIcons = new Transform[3];
    public Vector3[] Offset = new Vector3[3];

    public int ThiefScore = 0;

    bool myRBButtonIsReady = true;


    // Use this for initialization
    private void Start()
    {

        progress = null;

        Offset[0] = new Vector3(-0.4f, -0.81f, 0);
        Offset[1] = new Vector3(-0f, -0.81f, 0);
        Offset[2] = new Vector3(0.4f, -0.81f, 0);

        Movement = gameObject.GetComponent<movement>();

        ActionButton = (GameObject)Instantiate(RBButton, transform.position, Quaternion.identity);
        ActionButton.transform.renderer.enabled = false;

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
        if (readyToHide && currentPropType == propTypes.hiding)
            StartHideInProp();

        if (readyToPickup && currentPropType == propTypes.pickup)
        StartPickupProp();

        for (int i = 0; i < 3; i++)
        {
            PickupDisplayIcons[i].transform.position = transform.position + Offset[i];
        }

        if (OuyaInput.GetButtonUp(OuyaButton.RB, Movement.observedPlayer))
        {
            myRBButtonIsReady = true;
        }

        if(hidden)
        {
            if (OuyaInput.GetButtonDown(OuyaButton.RB, Movement.observedPlayer))
            {
                Vector3 pos = currentProp.transform.position + new Vector3(0, 0, -3f);
                ActionButton.transform.renderer.enabled = false;
                progress = (GameObject)Instantiate(progressBar, pos, Quaternion.identity);
            }

            if (OuyaInput.GetButtonUp(OuyaButton.RB, Movement.observedPlayer))
            {
                if (progress != null)
                {
                    hidingCounterTime = 0;
                    ActionButton.transform.renderer.enabled = true;
                    Destroy(progress);
                }
            }

            if (OuyaInput.GetButton(OuyaButton.RB, Movement.observedPlayer) && myRBButtonIsReady)
            {
                hidingCounterTime += Time.deltaTime;
                
                if (progress != null)
                {
                    progress.GetComponent<ProgressBar>().SetProgressTime(hidingCounterTime / CurrentPropToHideIn.HidingTime);
                }


                // hiding time reached
                if (hidingCounterTime >= CurrentPropToHideIn.HidingTime)
                {

                    // has hidden in prop - her sker magien

                    hidden = false;
                    Movement.SetHiding(false);
                    transform.renderer.enabled = true;
                    collider2D.enabled = true;
                    CurrentPropToHideIn.SomeoneIsHidingInHere = false;
                    ActionButton.transform.renderer.enabled = false;
                    hidingCounterTime = 0;
                    myRBButtonIsReady = false;
                    SetPickupDisplay(true);
                }
            }
        }
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


        if (!currentProp.GetComponent<PropsToPickUp>() == null)
        {
        	if (currentProp.GetComponent<PropsToPickUp>().CanBePickedUp == false
        	|| (availableSlots == false))
        	    return;
        }
        

        
        // close enough?
        if (Vector2.Distance(transform.position, currentProp.transform.position) < 2)
        {

            // start picking up
            if (OuyaInput.GetButtonDown(OuyaButton.RB, Movement.observedPlayer))
            {
                Vector3 pos = currentProp.transform.position + new Vector3(0, 0, -3f);
                ActionButton.transform.renderer.enabled = false;
                progress = (GameObject)Instantiate(progressBar, pos, Quaternion.identity);
            }

            if (OuyaInput.GetButtonUp(OuyaButton.RB, Movement.observedPlayer))
            {
                if (progress != null)
                {
                    ActionButton.transform.renderer.enabled = true;
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

    void StartHideInProp()
    {
        if (currentProp.GetComponent<PropsToHideIn>().SomeoneIsHidingInHere == true)
            return;
        
        // close enough?
        if (Vector2.Distance(transform.position, currentProp.transform.position) < 2)
        {
            //ActionButton.transform.renderer.enabled = true;
            // start hiding
            if (OuyaInput.GetButtonDown(OuyaButton.RB, Movement.observedPlayer))
            {
                Vector3 pos = currentProp.transform.position + new Vector3(0, 0, -3f);
                ActionButton.transform.renderer.enabled = false;
                progress = (GameObject)Instantiate(progressBar, pos, Quaternion.identity);
                Movement.SetHiding(true);
            }

            if (OuyaInput.GetButtonUp(OuyaButton.RB, Movement.observedPlayer))
            {
                if (progress != null)
                {
                    hidingCounterTime = 0;
                    ActionButton.transform.renderer.enabled = true;
                    Destroy(progress);
                    Movement.SetHiding(false);
                }
            }

            if (OuyaInput.GetButton(OuyaButton.RB, Movement.observedPlayer) && myRBButtonIsReady)
            {
                hidingCounterTime += Time.deltaTime;
                
                if (progress != null)
                {
            	    progress.GetComponent<ProgressBar>().SetProgressTime(hidingCounterTime / CurrentPropToHideIn.HidingTime);
                }


                // hiding time reached
                if (hidingCounterTime >= CurrentPropToHideIn.HidingTime)
                {

                    // has hidden in prop - her sker magien

					hidden = true;
                    Movement.SetHiding(true);
                    transform.renderer.enabled = false;
            		collider2D.enabled = false;
                    CurrentPropToHideIn.SomeoneIsHidingInHere = true;
                    Vector3 pos = currentProp.transform.position + new Vector3(0, 0, -3f);
                    ActionButton.transform.position = pos;
                    ActionButton.transform.renderer.enabled = true;
                    hidingCounterTime = 0;
                    myRBButtonIsReady = false;
                    SetPickupDisplay(false);                    
                }
            }
        } else
        {
            hidingCounterTime = 0;
            ActionButton.transform.renderer.enabled = false;
        }
    }

    void SetPickupDisplay(bool newState)
    {
		int availableSlot = -1;
		for (int i = 0 ; i < 3; i++)
            {
                if (Inventory[i] != null) // pickup if empty slot
                    PickupDisplayIcons[i].renderer.enabled = newState;
            }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        // check to see if it's a pickupable prop
        if (coll.gameObject.tag == "CanHideIn")
        {
            PropsToHideIn theThingYouCanHideIn = coll.gameObject.GetComponent<PropsToHideIn>() as PropsToHideIn;
            if (theThingYouCanHideIn != null)
            {
            	if (!coll.gameObject.GetComponent<PropsToHideIn>().SomeoneIsHidingInHere)
            	{
            	    currentProp = coll.gameObject;
            	    currentPropType = propTypes.hiding;
            	    CurrentPropToHideIn = currentProp.GetComponent<PropsToHideIn>();
            	    readyToHide = true;
            	    Vector3 pos = currentProp.transform.position + new Vector3(0, 0, -3f);
                    ActionButton.transform.position = pos;
                    ActionButton.transform.renderer.enabled = true;
            	}
            	else
            	{
            	    currentProp = null;
            	    CurrentPropToHideIn = null;
            	    readyToHide = false;
	
            	    return;
            	}
            }
        }
        else if (coll.gameObject.tag == "CanBePickedUp")
        {
        	if (coll.gameObject.GetComponent<PropsToPickUp>().CanBePickedUp)
            {
                currentProp = coll.gameObject;
                currentPropType = propTypes.pickup;
                CurrentPropToPickUp = currentProp.GetComponent<PropsToPickUp>();
                readyToPickup = true;
                Vector3 pos = currentProp.transform.position + new Vector3(0, 0, -3f);
                ActionButton.transform.position = pos;
                ActionButton.transform.renderer.enabled = true;
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
