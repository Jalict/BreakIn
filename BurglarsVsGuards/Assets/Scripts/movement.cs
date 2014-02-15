using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {

	Vector3 MoveVector;
	public float Speed = 1;
    public float turnSpeed = 0.3f;

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


		MoveVector = new Vector3(OuyaInput.GetAxis(OuyaAxis.LX, OuyaPlayer.P01),
            OuyaInput.GetAxis(OuyaAxis.LY, OuyaPlayer.P01),
            0);
        transform.Translate(MoveVector * Speed * Time.deltaTime, Space.World);
        //print(MoveVector);

        Vector3 LookVector = new Vector3(OuyaInput.GetAxis(OuyaAxis.RX, OuyaPlayer.P01),
            OuyaInput.GetAxis(OuyaAxis.RY, OuyaPlayer.P01),
            0);


        //Quaternion rot = Quaternion.LookRotation(LookVector);
		//rot *= Quaternion.FromToRotation(Vector3.forward, Vector3.right);
		//transform.rotation = Quaternion.Slerp(transform.rotation, rot, (Time.deltaTime*1.5f));

        //Quaternion rotation = Quaternion.LookRotation(LookVector);
        //transform.rotation = rotation;

        LookVector.z = transform.position.z;

        if(LookVector.magnitude > 0)
            transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.eulerAngles.z,Mathf.Atan2(LookVector.y, LookVector.x) * Mathf.Rad2Deg,turnSpeed));

	}
}
