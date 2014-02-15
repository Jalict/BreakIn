using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	public float progressTime = 0f;


    void Start()
    {
        renderer.enabled = false;
    }

	void Update ()
	{ 
		renderer.material.SetFloat("_Cutoff", Mathf.Lerp(1, 0, progressTime));
		if(progressTime >= 1)
		{
			Destroy(gameObject);
		}
	}

	public void SetProgressTime(float newTime)
	{
	    renderer.enabled = true;

		progressTime = newTime;
        print(progressTime);
	}
}
