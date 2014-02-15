using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{

    public float Speed = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
        Vector3 MoveVector = new Vector3(OuyaInput.GetAxis(OuyaAxis.LX, OuyaPlayer.P01),
            OuyaInput.GetAxis(OuyaAxis.LY, OuyaPlayer.P01),
            0);

        transform.Translate(MoveVector * Speed * Time.deltaTime);
	}
}
