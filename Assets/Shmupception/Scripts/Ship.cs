using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour 
{	
	// Move indicies
	static readonly int left = 0;
	static readonly int up = 1;
	static readonly int right = 2;
	static readonly int down = 3;
	
	public float speed;
	public float roll;
	public Transform rollT;
	public Gun maingun;
	
	Transform thisTransform;
	Vector4 input;
	Vector3 dir;
	Vector3 velocity;
	Vector3? bounder;
	
	InputFilter f;
	
	void Awake()
	{
		thisTransform = transform;
	}
	void Start()
	{
		f = InputFilter.Instance;
		
		f.OnUp += MoveUp;
		f.OnDown += MoveDown;
		f.OnRight += MoveRight;
		f.OnLeft += MoveLeft;
		//f.OnFire += Fire;
	}
	
	void Update()
	{
		velocity = dir * speed * Time.smoothDeltaTime;
		thisTransform.Translate(velocity);
	}
	void LateUpdate()
	{
		bounder = CameraBounds.MaintainBounds(CameraBounds.BoundsMode.Clamp, 
												CameraBounds.BoundsMode.Wrap, 
												thisTransform.position);
		if (bounder != null)
			thisTransform.position = (Vector3)bounder;
	}
	
	void MoveUp(ButtonState state)
	{
		if (state == ButtonState.DownNow)
		{
			input[up] = 1f;
			dir[2] = 1f;
		}
		else if (state == ButtonState.UpNow)
		{
			input[up] = 0f;
			if (input[down] == 1f)
				dir[2] = -1f;
			else
				dir[2] = 0f;
					
		}
	}
	void MoveDown(ButtonState state)
	{
		if (state == ButtonState.DownNow)
		{
			input[down] = 1f;
			dir[2] = -1f;
		}
		else if (state == ButtonState.UpNow)
		{
			input[down] = 0f;
			if (input[up] == 1f)
				dir[2] = 1f;
			else
				dir[2] = 0f;
					
		}
	}
	void MoveRight(ButtonState state)
	{
		if (state == ButtonState.DownNow)
		{
			input[right] = 1f;
			SetMovingRight();
		}
		else if (state == ButtonState.UpNow)
		{
			input[right] = 0f;
			if (input[left] == 1f)
				SetMovingLeft();
			else
				HorzMoveNeutral();
					
		}
	}
	void MoveLeft(ButtonState state)
	{
		if (state == ButtonState.DownNow)
		{
			input[left] = 1f;
			SetMovingLeft();
		}
		else if (state == ButtonState.UpNow)
		{
			input[left] = 0f;
			if (input[right] == 1f)
				SetMovingRight();
			else
				HorzMoveNeutral();
					
		}
	}
	void SetMovingLeft()
	{
		dir[0] = -1f;
		rollT.eulerAngles = new Vector3(0f,0f,roll);//todo cache
	}
	void SetMovingRight()
	{
		dir[0] = 1f;
		rollT.eulerAngles = new Vector3(0f,0f,-roll);//todo cache
	}
	void HorzMoveNeutral()
	{
		dir[0] = 0f;
		rollT.eulerAngles = Vector3.zero;
	}
	
	void Fire(ButtonState state)
	{
		if (state == ButtonState.DownNow)
			maingun.firing = !maingun.firing;
	}
}
