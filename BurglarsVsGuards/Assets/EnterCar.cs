using UnityEngine;
using System.Collections;

public class EnterCar : MonoBehaviour {

	public HideNSteal thiefScript;
	public movement Movement;
	//public guardScript guardScript;
	GameObject ActionButton;
	float ridingCounterTime = 0;
	bool myRBButtonIsReady = true;

	private CarsToRideIn CurrentCarToRideIn = null;

	static int finalThiefScore;
	bool approachedCar = false;

	bool readyToRide = false;

	// Use this for initialization
	void Start ()
	{
		ActionButton = thiefScript.GetActionButton();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (readyToRide && thiefScript.GetCurrentPropType() == propTypes.car)
    		{
    		        //ActionButton.transform.renderer.enabled = true;
    		        // start hiding
    		        if (OuyaInput.GetButtonDown(OuyaButton.RB, Movement.observedPlayer))
    		        {
    		            Vector3 pos = thiefScript.GetCurrentProp().transform.position + new Vector3(0, 0, -3f);
    		            ActionButton.transform.renderer.enabled = false;
    		            thiefScript.ProgressBar(true, pos);
    		            //progress = (GameObject)Instantiate(progressBar, pos, Quaternion.identity);
    		            Movement.SetHiding(true);
    		        }
		
    		        if (OuyaInput.GetButtonUp(OuyaButton.RB, Movement.observedPlayer))
    		        {
    		            if (thiefScript.GetProgress() != null)
    		            {
    		                ridingCounterTime = 0;
    		                ActionButton.transform.renderer.enabled = true;
    		                thiefScript.ProgressBar(false);
    		                //Destroy(progress);
    		                Movement.SetHiding(false);
    		            }
    		        }
		
    		        if (OuyaInput.GetButton(OuyaButton.RB, Movement.observedPlayer) && myRBButtonIsReady)
    		        {
    		            ridingCounterTime += Time.deltaTime;
    		            
    		            if (thiefScript.GetProgress() != null)
    		            {
    		        	    thiefScript.SetProgressTime(ridingCounterTime / CurrentCarToRideIn.RidingTime);
    		            }
		
		
    		            // hiding time reached
    		            if (ridingCounterTime >= CurrentCarToRideIn.RidingTime)
    		            {
		
    		                // has hidden in prop - her sker magien
		
							thiefScript.Hide(thiefScript.GetCurrentProp());
							thiefScript.SetCar(CurrentCarToRideIn);               
    		            }
    		        }
    		    } else
		    {
		        ridingCounterTime = 0;
		        ActionButton.transform.renderer.enabled = false;
		    }
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.gameObject.tag == "Car" && !approachedCar)
		{
			finalThiefScore += thiefScript.DepositCash();
			//Fjern ting i inventory...
			approachedCar = true;
			readyToRide = true;
			thiefScript.SetCurrentPropAndType(coll.gameObject, propTypes.car);
			CurrentCarToRideIn = thiefScript.GetCurrentProp().GetComponent<CarsToRideIn>();
		}
		
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if(coll.gameObject.tag == "Car")
		{
			readyToRide = false;
			approachedCar = false;
		}
	}
}
