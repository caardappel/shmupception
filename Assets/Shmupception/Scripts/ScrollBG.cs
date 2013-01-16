using UnityEngine;
using System.Collections;

public class ScrollBG : MonoBehaviour 
{
	public float size;
	public float speed = 1f;
	
	float x, y, delta;
	
	Transform thisTransform;

	void Awake()
	{
		thisTransform = transform;
		x = thisTransform.position.x;
		y = thisTransform.position.y;
	}
	
	void LateUpdate() 
	{
		thisTransform.Translate(0f, 0f, -speed*Time.smoothDeltaTime);
		if (thisTransform.position.z <= -size)
			thisTransform.position = new Vector3(x,y,size);
	}
}