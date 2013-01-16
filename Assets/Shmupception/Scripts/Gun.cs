using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour 
{
	public Shot[] pattern; 
	public Transform origin;
	public bool firing = false;
	
	List<Shot> magazine = new List<Shot>();
	int shot = 0;
	
	void Awake()
	{
		// Load magazine
		for (int i = 0; i < pattern.Length; i++)
		{
			magazine.Add(Shot.Create(pattern[i]));
			magazine[magazine.Count-1].gameObject.SetActive(false);
		}
	}
	void Start()
	{
		StartCoroutine(AutoFire());
	}
	
	IEnumerator AutoFire()
	{
		while (true)
		{
			yield return null;
			
			if (firing)
			{
				Shot s = magazine.Find(x => x.name == pattern[shot].name && x.gameObject.activeInHierarchy == false);
				if (s == null)
				{
					s = Shot.Create(pattern[shot]);
					magazine.Add(s);
				}
				
				s.gameObject.SetActive(true);
				yield return s.StartCoroutine(s.Fire(origin));
				
				shot++;
				if (shot >= pattern.Length)
					shot = 0;
			}
		}
	}
}
