using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Type = System.Type;

public class BasicState
{
	Type 	enumType;
	string 	id;
	
	//public delegate OnEnter;
	//public delegate CanEnter;
}

public class CoroutineMachine<T> : MonoBehaviour where T : struct 
{
	
	
	void Start()
	{
		StartCoroutine(FSM ());
	}
	
	protected IEnumerator FSM()
	{
		while (true)
		{
			/*if (previousState != state)
			{
				StopCoroutine(previousState.ToString());
				previousState = state;
				//Debug.Log("Changing"+powerID.ToString()+"state to "+previousState.ToString());
				StartCoroutine(previousState.ToString());
			}*/
			yield return null;
		}
	} 
}
