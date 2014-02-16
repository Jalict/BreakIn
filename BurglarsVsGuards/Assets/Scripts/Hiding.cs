using UnityEngine;
using System.Collections;

public class Hiding : MonoBehaviour
{

    public movement Movement;

    public GameObject[] Inventory = new GameObject[3];
    public GameObject currentProp = null;
    private PropsToHideIn CurrentPropToHideIn = null;
    

    private float hidingCounterTime = 0;
    bool hidden = false;

    public bool readyToHide = false;

    public GameObject progressBar;
	public GameObject RBButton;
    
    private GameObject progress;
    private GameObject ActionButton;

    public Vector3[] Offset = new Vector3[3];

    public int ThiefScore = 0;

    bool myRBButtonIsReady = true;


    // Use this for initialization
    private void Start()
    {

        progress = null;


        Movement = gameObject.GetComponent<movement>();

        ActionButton = (GameObject)Instantiate(RBButton, transform.position, Quaternion.identity);
        ActionButton.transform.renderer.enabled = false;

        if (Movement == null)
            Debug.Log("ERROR - need to be assigned movement for player");

        for (int i = 0; i < 3; i++)
            Inventory[i] = null;
    }

    // Update is called once per frame
    private void Update()
    {
        if (readyToHide)
            StartHideInProp();

        /*for (int i = 0; i < 3; i++)
        {
            if (Inventory[i] != null)
                print("2");
        }*/

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
                }
            }
        }
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
                }
            }
        } else
        {
            print("HVORFOR!?");
            hidingCounterTime = 0;
            ActionButton.transform.renderer.enabled = false;
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
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2, Screen.height - 130, 500, 60), gameObject.name + " score:" + ThiefScore);
        GUI.Label(new Rect(Screen.width / 2, Screen.height - 100, 500, 60), "Slot1: " + Inventory[0]);
    }
}
