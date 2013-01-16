using UnityEngine;
using System.Collections;

public class CameraBounds : MonoBehaviour 
{
	public enum BoundsMode
	{
		Clamp,
		Wrap,
		None
	}
	
	static CameraBounds _instance;
	
	Camera c;
	Rect pixelRect;
	Vector3 screen;
	bool update;
	
	void Awake()
	{
		_instance = this;
		c = GetComponent<Camera>();
		pixelRect = c.pixelRect;
	}
	
	public static Vector3? MaintainBounds(BoundsMode vertical, BoundsMode horizontal, Vector3 pos)
	{
		return _instance._MaintainBounds(vertical,horizontal,pos);
	}
	Vector3? _MaintainBounds(BoundsMode vertical, BoundsMode horizontal, Vector3 pos)
	{
		screen = c.WorldToScreenPoint(pos);
		if (InBounds(screen, false))
			return null;
	
		update = false; 
		if (horizontal == BoundsMode.Clamp)
		{
			if (screen.x <= 1f)
			{
				screen.x = 0f;
				update = true;
			}
			else if (screen.x >= pixelRect.width)
			{
				screen.x = pixelRect.width;
				update = true;
			}
		}
		else if (horizontal == BoundsMode.Wrap)
		{
			if (screen.x <= 0f)
			{
				screen.x = pixelRect.width;
				update = true;
			}
			else if (screen.x >= pixelRect.width)
			{
				screen.x = 0f;
				update = true;
			}
		}
		
		if (vertical == BoundsMode.Clamp)
		{
			if (screen.y <= 0)
			{
				screen.y = 0;
				update = true;
			}
			else if (screen.y >= pixelRect.height)
			{
				screen.y = pixelRect.height;
				update = true;
			}
		}
		else if (vertical == BoundsMode.Wrap)
		{
			if (screen.y <= 0)
			{
				screen.y = pixelRect.height;
				update = true;
			}
			else if (screen.y >= pixelRect.height)
			{
				screen.y = 0;
				update = true;
			}		
		}
		
		if (update)
		{
			pos = c.ScreenToWorldPoint(screen);
		}
		
		return pos;
	}
	
	public static bool InBounds(Vector3 pos)
	{
		return InBounds(pos, true);
	}
	public static bool InBounds(Vector3 pos, bool autoconvert)
	{
		return _instance.pixelRect.Contains(autoconvert?_instance.c.WorldToScreenPoint(pos):pos);
	}
}
