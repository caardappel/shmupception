using UnityEngine;
using System.Collections;

public delegate void existenceEvent(bool created);

public class Shot : MonoBehaviour 
{
	public existenceEvent OnExistence;
	
	public GameObject effectOverride;
	public float delay;
	public Vector3 velocity;
	
	Transform thisTransform;
	ParticleSystem particle;
	bool isActive;
	
	void Awake()
	{
		thisTransform = transform;
		particle = GetComponent<ParticleSystem>();
		isActive = false;
	}
	
	public IEnumerator Fire(Transform origin)
	{
		yield return null;
		yield return new WaitForSeconds(delay);
		
		if (particle != null)
		{
			particle.Play();
			isActive = true;
			thisTransform.position = origin.position;
		}
//		OnExistence(true);
	}
	
	void Update()
	{
		if (isActive)
			thisTransform.Translate(velocity*Time.smoothDeltaTime);
	}
	
	void LateUpdate()
	{
		if (!CameraBounds.InBounds(thisTransform.position))
		{
			gameObject.SetActive(false);
			if (particle != null)
				particle.Stop();
			isActive = false;
//			OnExistence(false);
		}
	}
	
	public void Clone(Shot source)
	{
		delay = source.delay;
		velocity = source.velocity;
	}
	
	static public Shot Create(Shot source, Gun gun)
	{
		GameObject go = null;
		
		GameObject effect = gun.effect;
		if (effect == null)
			effect = source.effectOverride;
		
		if (effect != null)
			go = (GameObject)Instantiate(effect);
		else
			go = new GameObject("Shot Delayer");
		
		Shot shot = go.AddComponent<Shot>();
		shot.Clone(source);
		
		return shot;
	}
}
