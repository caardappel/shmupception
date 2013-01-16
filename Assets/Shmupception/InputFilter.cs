using UnityEngine;
using System.Collections;

public enum ButtonState
{
	UpNow,
//	UpHold,
	DownNow,
	DownHold
}

public delegate void up(ButtonState state);
public delegate void down(ButtonState state);
public delegate void left(ButtonState state);
public delegate void right(ButtonState state);
public delegate void fire(ButtonState state);

public class InputFilter : MonoBehaviour 
{
	static InputFilter _instance = null;
	public static InputFilter Instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject go = (GameObject)GameObject.FindObjectOfType(typeof(InputFilter));
				if (go != null)
					_instance = go.GetComponent<InputFilter>();
				else
				{
					go = new GameObject("InputFilter");
					_instance = go.AddComponent<InputFilter>();
				}
			}
			return _instance;
		}
	}
	
	public up OnUp;
	public down OnDown;
	public left OnLeft;
	public right OnRight;
	public fire OnFire;
	
	void Update()
	{
		// TODO: Expand as needed, split by platform
		if (Input.GetKeyDown(KeyCode.UpArrow) ||
			Input.GetKeyDown(KeyCode.W))
		{
			OnUp(ButtonState.DownNow);
		}
		else if (Input.GetKeyUp(KeyCode.UpArrow) || 
				Input.GetKeyUp(KeyCode.W))
		{
			OnUp(ButtonState.UpNow);
		}
		else if (Input.GetKey(KeyCode.UpArrow) ||
			Input.GetKey(KeyCode.W))
		{
			OnUp(ButtonState.DownHold);
		}		
		/*else
		{
			OnUp(ButtonState.UpHold);
		}*/
		
		
		
		
		if (Input.GetKeyDown(KeyCode.DownArrow) ||
			Input.GetKeyDown(KeyCode.S))
		{
			OnDown(ButtonState.DownNow);
		}
		else if (Input.GetKeyUp(KeyCode.DownArrow) || 
				Input.GetKeyUp(KeyCode.S))
		{
			OnDown(ButtonState.UpNow);
		}
		else if (Input.GetKey(KeyCode.DownArrow) ||
			Input.GetKey(KeyCode.S))
		{
			OnDown(ButtonState.DownHold);
		}
		/*else
		{
			OnDown(ButtonState.UpHold);
		}*/
		
		
		
		if (Input.GetKeyDown(KeyCode.RightArrow) ||
			Input.GetKeyDown(KeyCode.D))
		{
			OnRight(ButtonState.DownNow);
		}
		else if (Input.GetKeyUp(KeyCode.RightArrow) || 
				Input.GetKeyUp(KeyCode.D))
		{
			OnRight(ButtonState.UpNow);
		}
		else if (Input.GetKey(KeyCode.RightArrow) ||
			Input.GetKey(KeyCode.D))
		{
			OnRight(ButtonState.DownHold);
		}
		/*else
		{
			OnUp(ButtonState.UpHold);
		}*/
		
		
		
		if (Input.GetKeyDown(KeyCode.LeftArrow) ||
			Input.GetKeyDown(KeyCode.A))
		{
			OnLeft(ButtonState.DownNow);
		}
		else if (Input.GetKeyUp(KeyCode.LeftArrow) || 
				Input.GetKeyUp(KeyCode.S))
		{
			OnLeft(ButtonState.UpNow);
		}
		else if (Input.GetKey(KeyCode.LeftArrow) ||
			Input.GetKey(KeyCode.A))
		{
			OnLeft(ButtonState.DownHold);
		}
		/*else
		{
			OnUp(ButtonState.UpHold);
		}*/
		
		
		
		/*if (Input.GetKeyDown(KeyCode.Space))
			OnFire(ButtonState.DownNow);
		else if (Input.GetKeyUp(KeyCode.Space))
			OnFire(ButtonState.UpNow);
		else if (Input.GetKey(KeyCode.Space))
			OnFire(ButtonState.DownHold);*/
	}
}
