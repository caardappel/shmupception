using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	public CMRSpline 	introSpline;
	public MovePhase[] 	moves;
	public Gun			mainGun;
	public int			health;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}


public class MovePhase
{
	public Vector3 direction;
	public float duration;
	public float speedMod;
}