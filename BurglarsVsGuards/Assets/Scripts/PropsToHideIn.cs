using UnityEngine;
using System.Collections;

public class PropsToHideIn : MonoBehaviour
{

    public bool CanBeHiddenIn = true;
    public bool isCar = false;
    public bool SomeoneIsHidingInHere = false;
    public int Money = 100;
    GameObject ThiefHidingHere;

    public float HidingTime = 3f;
    // Use this for initialization
    
    public GameObject GetThiefHidingHere()
    {
        return ThiefHidingHere;
    }

    public void SetThiefHidingHere(GameObject newThief)
    {
        ThiefHidingHere = newThief;
    }

    public void KickOutThief()
    {
        if(ThiefHidingHere != null)
        {
           ThiefHidingHere.GetComponent<HideNSteal>().Unhide(); 
        } else
        {
            Debug.Log("NO! NONONONO!");
        }
        
    }

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
