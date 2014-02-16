using UnityEngine;
using System.Collections;

public class PropsToHideIn : MonoBehaviour
{

    public bool CanBeHiddenIn = true;
    public bool isCar = false;
    public bool SomeoneIsHidingInHere = false;
    public int Money = 100;

    public float HidingTime = 3f;
    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (SomeoneIsHidingInHere)
        {
            //transform.renderer.enabled = false;
            //collider2D.enabled = false;
        }

    }
}
