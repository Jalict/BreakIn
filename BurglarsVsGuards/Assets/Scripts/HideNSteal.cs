using UnityEngine;
using System.Collections;

public enum propTypes
{
	hiding,
	pickup,
	car,
	thief,
	nothing
}

public class HideNSteal : MonoBehaviour
{
	public GUIStyle style;
    public movement Movement;

    public GameObject[] Inventory = new GameObject[3];
    public GameObject currentProp = null;
    propTypes currentPropType;

    private PropsToHideIn CurrentPropToHideIn = null;
    private PropsToPickUp CurrentPropToPickUp = null;
    private CarsToRideIn CurrentCarToRideIn = null;

    private float hidingCounterTime = 0;
    private float pickupCounterTime = 0;
    private float ridingCounterTime = 0;
    bool hidden = false;

    public bool readyToHide = false;
    private bool readyToPickup = false;

    public GameObject Bloodpool;

    public GameObject progressBar;
	public GameObject RBButton;
    
    private GameObject progress;
    private GameObject ActionButton;

    public GameObject[] PickupDisplayIcons = new GameObject[3];
    public Vector3[] Offset = new Vector3[3];

    public int ThiefScore = 0;
    static int finalThiefScore = 0;
    public static int thievesInCar = 0;
    static bool thievesEscaped = false;
    static int thievesAlive = 2;
    bool hidingInCar = false;

    bool myRBButtonIsReady = true;

    bool beingTazed = false;


    // Use this for initialization
    
    void OnGUI()
	{
		if(thievesEscaped)GUI.Label(new Rect (Screen.width/2, Screen.height/2, 200,200),"The Burglars scored " + finalThiefScore + "$!",style);
	}

    private void Awake()
    {

        //progress = null;
        progress = (GameObject)Instantiate(progressBar, transform.position, Quaternion.identity);
        progress.renderer.enabled = false;

        Offset[0] = new Vector3(-0.4f, -0.81f, 0);
        Offset[1] = new Vector3(-0f, -0.81f, 0);
        Offset[2] = new Vector3(0.4f, -0.81f, 0);

    	string path = "Prefabs/" + PickupDisplayIcons[0].name;
		if (Network.connections.Length > 0)
        {
            for(int i = 0; i < 3; i++)
            {
            	PickupDisplayIcons[i] = (GameObject) Connector.AddEntityReturn(PickupDisplayIcons[i].name, transform.position, Quaternion.identity, path,
                                "Untagged",
                                false);
            	PickupDisplayIcons[i].layer = 4;
            	PickupDisplayIcons[i].renderer.enabled = false;
            }
        }

        else
        {
            for(int i = 0; i < 3;i++)
            {
            	PickupDisplayIcons[i] = (GameObject)Instantiate(Resources.Load(path), transform.position, Quaternion.identity);
            	PickupDisplayIcons[i].name = "PickupDisplayIcons";
            	PickupDisplayIcons[i].renderer.enabled = false;
            	PickupDisplayIcons[i].layer = 4;
            }
            
        }

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
        //print("Thieves alive: " + thievesAlive);
        //print("Thieves in car: " + thievesInCar);
        if(thievesInCar == thievesAlive)
        {
        	if(!thievesEscaped)
        	{
        		thievesEscaped = true;
        	}
        }

        BeingTazed(beingTazed);

        if (readyToHide && currentPropType == propTypes.hiding && !beingTazed)
            StartHideInProp();

        if (readyToPickup && currentPropType == propTypes.pickup && !beingTazed)
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
                ProgressBar(true, pos);
                //progress.transform.position = pos;
                //progress.renderer.enabled = true;
                //progress = (GameObject)Instantiate(progressBar, pos, Quaternion.identity);
            }

            if (OuyaInput.GetButtonUp(OuyaButton.RB, Movement.observedPlayer))
            {
                if (progress != null)
                {
                    hidingCounterTime = 0;
                    ActionButton.transform.renderer.enabled = true;
                    ProgressBar(false);
                    //progress.GetComponent<ProgressBar>().SetProgressTime(hidingCounterTime / CurrentPropToHideIn.HidingTime);
                    //Destroy(progress);
                }
            }

            if (OuyaInput.GetButton(OuyaButton.RB, Movement.observedPlayer) && myRBButtonIsReady)
            {
                hidingCounterTime += Time.deltaTime;
                bool hideNotRide = false;
                if (progress != null)
                {
                    if(currentPropType == propTypes.hiding)
                    {
                    	SetProgressTime(hidingCounterTime / CurrentPropToHideIn.HidingTime);
                    	hideNotRide = true;
                    }
                    else if(currentPropType == propTypes.car)
                    {
                    	SetProgressTime(ridingCounterTime / CurrentCarToRideIn.RidingTime);
                    	hideNotRide = false;
                    }
                    
                    //progress.GetComponent<ProgressBar>().SetProgressTime(hidingCounterTime / CurrentPropToHideIn.HidingTime);
                }


                // hiding time reached
                if(hideNotRide)
                {
                	if (hidingCounterTime >= CurrentPropToHideIn.HidingTime)
                	{
	
                	    Unhide();
                	}
                }else
                {
                	if (ridingCounterTime >= CurrentCarToRideIn.RidingTime)
                	{
	
                	    // has hidden in prop - her sker magien
	
                	    hidden = false;
                	    Movement.SetHiding(false);
                	    transform.renderer.enabled = true;
                	    collider2D.enabled = true;
                	    ActionButton.transform.renderer.enabled = false;
                	    ridingCounterTime = 0;
                	    myRBButtonIsReady = false;
                	    SetPickupDisplay(true);
                	    ProgressBar(false);
                	}
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
                ProgressBar(true, pos);
                //progress = (GameObject)Instantiate(progressBar, pos, Quaternion.identity);
            }

            if (OuyaInput.GetButtonUp(OuyaButton.RB, Movement.observedPlayer))
            {
                if (progress != null)
                {
                    ActionButton.transform.renderer.enabled = true;
                    pickupCounterTime = 0;
                    ProgressBar(false);
                    //Destroy(progress);
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
                            PickupDisplayIcons[availableSlot].renderer.material.color = Color.blue;
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
                        ProgressBar(false);
                        //currentProp = null;
                        readyToPickup = false;
                    }
                }
            }
        }
        else
            pickupCounterTime = 0;
    }

    public void SetCar(CarsToRideIn newScript)
    {
    	CurrentCarToRideIn = newScript;
    }

    public void GettingTazed(bool newState)
    {
    	beingTazed = true;
    	StartCoroutine(BeingTased());
    }

    void BeingTazed(bool state)
    {
    	if(!beingTazed) return;
    	Movement.SetHiding (true);
    }

    IEnumerator BeingTased()
    {
        renderer.enabled = !renderer.enabled;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(BeingTased());
    }

    public void KillFromTaze(GuardScript GS)
    {
    	if(!beingTazed) return;
    	string path = "Prefabs/" + Bloodpool.name;
    	if (Network.connections.Length > 0)
    	{
    		Connector.AddEntityReturn(Bloodpool.name, transform.position - new Vector3(0,0,3), Quaternion.identity, path,
                                "Untagged",
                                true);
    	}else
    	{
    		Instantiate(Resources.Load(path), transform.position - new Vector3(0,0,3), Quaternion.identity);
    	}
    	
    	//this.renderer.enabled = false;
    	//this.collider2D.isTrigger = true;
    	//print("Han er døøøøø'");
    	GS.DeathCount();
    	thievesAlive--;
    	Destroy(progress);
    	Destroy(ActionButton);
    	Destroy(gameObject);
    }
    public bool GetBeingTazed ()
    {
    	return beingTazed;
    }

    void StartHideInProp()
    {
        if (currentProp.GetComponent<PropsToHideIn>().SomeoneIsHidingInHere == true)
            return;
        
        // close enough?
        if (Vector2.Distance(transform.position, currentProp.transform.position) < (currentProp.transform.localScale.x+currentProp.transform.localScale.y)/2)
        {
            //ActionButton.transform.renderer.enabled = true;
            // start hiding
            if (OuyaInput.GetButtonDown(OuyaButton.RB, Movement.observedPlayer))
            {
                Vector3 pos = currentProp.transform.position + new Vector3(0, 0, -3f);
                ActionButton.transform.renderer.enabled = false;
                ProgressBar(true, pos);
                //progress = (GameObject)Instantiate(progressBar, pos, Quaternion.identity);
                Movement.SetHiding(true);
            }

            if (OuyaInput.GetButtonUp(OuyaButton.RB, Movement.observedPlayer))
            {
                if (progress != null)
                {
                    hidingCounterTime = 0;
                    ActionButton.transform.renderer.enabled = true;
                    ProgressBar(false);
                    //Destroy(progress);
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

					//hidingCounterTime = 0;
					if(!hidden)Hide(currentProp);
					ProgressBar(false);
                }
            }
        } else
        {
            hidingCounterTime = 0;
            ActionButton.transform.renderer.enabled = false;
        }
    }

    public void SetProgressTime (float newTime)
    {
    	progress.GetComponent<ProgressBar>().SetProgressTime(newTime);
    }

    public void ProgressBar(bool newState, Vector3 newPosition)
    {
    	progress.renderer.enabled = newState;
    	if(!newState) progress.GetComponent<ProgressBar>().SetProgressTime(0f);
    	else progress.transform.position = newPosition;
    }

    public void ProgressBar(bool newState)
    {
    	progress.renderer.enabled = newState;
    	if(!newState) progress.GetComponent<ProgressBar>().SetProgressTime(0f);
    	else Debug.Log("Did you forget a new position for the progress bar?");
    }

    public void Hide(GameObject prop)
    {
    	hidden = true;
        Movement.SetHiding(true);
        transform.renderer.enabled = false;
		collider2D.enabled = false;
        if(!CurrentPropToHideIn.isCar)
        	CurrentPropToHideIn.SomeoneIsHidingInHere = true;
        else
        {
        	thievesInCar++;
        	hidingInCar = true;
        } 
        Vector3 pos = prop.transform.position + new Vector3(0, 0, -3f);
        ActionButton.transform.position = pos;
        ActionButton.transform.renderer.enabled = true;
        hidingCounterTime = 0;
        myRBButtonIsReady = false;
        SetPickupDisplay(false);
    }

    public void Unhide()
    {
    	// has hidden in prop - her sker magien
	    if(hidingInCar)
	    {
	    	hidingInCar = false;
	    	thievesInCar--;
	    }
	    hidden = false;
	    Movement.SetHiding(false);
	    transform.renderer.enabled = true;
	    collider2D.enabled = true;
	    CurrentPropToHideIn.SomeoneIsHidingInHere = false;
	    CurrentPropToHideIn.SetThiefHidingHere(gameObject);
	    ActionButton.transform.renderer.enabled = false;
	    hidingCounterTime = 0;
	    myRBButtonIsReady = false;
	    SetPickupDisplay(true);
	    ProgressBar(false);
    }

    public void SetCurrentPropAndType(GameObject newProp, propTypes newType)
    {
    	currentProp = newProp;
    	currentPropType = newType;
    }

    public GameObject GetActionButton()
    {
    	return ActionButton;
    }

    public propTypes GetCurrentPropType()
    {
    	return currentPropType;
    }

    public GameObject GetCurrentProp()
    {
    	return currentProp;
    }

    public GameObject GetProgress()
    {
    	return progress;
    }

    public void SetPickupDisplay(bool newState)
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

            if(theThingYouCanHideIn.isCar)
            {
            	finalThiefScore += DepositCash();
            }
            
            if (theThingYouCanHideIn != null)
            {
            	if (!coll.gameObject.GetComponent<PropsToHideIn>().SomeoneIsHidingInHere)
            	{
            	    SetCurrentPropAndType(coll.gameObject, propTypes.hiding);
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

    public int DepositCash()
    {
    	int tmp = ThiefScore;
    	ThiefScore = 0;
    	for (int i = 0; i < 3; i++)
            Inventory[i] = null;
    	return tmp;
    }

    //void OnGUI()
    //{
    //    GUI.Label(new Rect(Screen.width / 2, Screen.height - 130, 500, 60), gameObject.name + " score:" + ThiefScore);
    //    GUI.Label(new Rect(Screen.width / 2, Screen.height - 100, 500, 60), "Slot1: " + Inventory[0]);
    //}
}
