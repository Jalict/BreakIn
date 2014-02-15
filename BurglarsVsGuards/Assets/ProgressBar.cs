using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	public float progressTime = 0f;
	public GameObject destroyMe;

	void Update ()
	{ 
		renderer.material.SetFloat("_Cutoff", Mathf.Lerp(1, 0, progressTime));
		if(progressTime > 1)
		{
			Destroy(destroyMe);
		}
	}

	public void SetProgressTime(float newTime)
	{
		progressTime = newTime;
	}
}
