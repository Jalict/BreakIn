using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{

    public float Speed = 1;

	
	// do we want to scan for trigger and d-pad button events ?
	public bool continuousScan = true;
	
	// the player we want to observe
	public OuyaPlayer observedPlayer = OuyaPlayer.P01;
	
	// the type of deadzone we want to use for convenience access
	public DeadzoneType deadzoneType = DeadzoneType.CircularMap;
	
	// the size of the deadzone
	public float deadzone = 0.25f;
	public float triggerThreshold = 0.1f;

	// Use this for initialization
	void Start ()
    {
        // set button state scanning to receive input state events for trigger and d-pads
        OuyaInput.SetContinuousScanning(continuousScan);

        // define the deadzone if you want to use advanced joystick and trigger access
        OuyaInput.SetDeadzone(deadzoneType, deadzone);
        OuyaInput.SetTriggerThreshold(triggerThreshold);

        // do one controller update here to get everything started as soon as possible
        OuyaInput.UpdateControllers();
	}
	
	// Update is called once per frame
	void Update ()
	{
        OuyaInput.UpdateControllers();


        Vector3 MoveVector = new Vector3(OuyaInput.GetAxis(OuyaAxis.LX, observedPlayer),
            OuyaInput.GetAxis(OuyaAxis.LY, observedPlayer),
            0);

        transform.Translate(MoveVector * Speed * Time.deltaTime);
	}
}
