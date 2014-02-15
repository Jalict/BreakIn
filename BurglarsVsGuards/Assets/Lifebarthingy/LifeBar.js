var progressTime = 0f;

function Update ()
{ 
	renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, Screen.width, progressTime));
	if(progressTime > 1)
	{
		Destroy(this);
	}
}