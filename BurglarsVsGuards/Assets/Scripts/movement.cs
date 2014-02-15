using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {

	//public CameraScript CamScript;

    Vector3 MoveVector;
	public float Speed = 1;
    public float turnSpeed = 0.3f;
    bool hiding = false;

		// do we want to scan for trigger and d-pad button events ?
	public bool continuousScan = true;
	
	// the player we want to observe
	public OuyaPlayer observedPlayer;
	
	// the type of deadzone we want to use for convenience access
	public DeadzoneType deadzoneType = DeadzoneType.CircularMap;
	
	// the size of the deadzone
	public float deadzone = 0.25f;
	public float triggerThreshold = 0.1f;

	// Use this for initialization
	
    public void SetHiding (bool newState)
    {
        hiding = newState;
    }

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


		MoveVector = new Vector3(OuyaInput.GetAxis(OuyaAxis.LX, observedPlayer),
            OuyaInput.GetAxis(OuyaAxis.LY, observedPlayer),
            0);
        if(!hiding) rigidbody2D.AddForce(new Vector2(OuyaInput.GetAxis(OuyaAxis.LX, observedPlayer) * Speed * Time.deltaTime,
                                       OuyaInput.GetAxis(OuyaAxis.LY, observedPlayer) * Speed * Time.deltaTime));

        //transform.Translate(MoveVector * Speed * Time.deltaTime, Space.World);
        //print(MoveVector);

        Vector3 LookVector = new Vector3(OuyaInput.GetAxis(OuyaAxis.RX, observedPlayer),
            OuyaInput.GetAxis(OuyaAxis.RY, observedPlayer),
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
