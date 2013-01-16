using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CMRSpline : SplineBase
{
#if UNITY_EDITOR
	public Color	lineColor = Color.white;
#endif
	
	bool			calculatedTangents = false;

	Vector3			m0;
	Vector3			m1;
	
	public Vector3[] Points
	{
		get { return points.ToArray(); }
		set
		{
			Clear();
			points.AddRange(value);
		}
	}
	
	public CMRSpline()
	{
		index = 1;
	}
	
	public override int SegmentCount
	{
		get { return points.Count - 2; }
	}
		
	public override void Clear()
	{
		calculatedTangents = false;
		points.Clear();
		lengths = null;
		distances = null;
		distance = 0;
		index = 1;
		time = 0;
		isMoving = true;
	}
		
	public override void AddDistance(float deltaDist)
	{
		if (lengths == null)
			CalculateLengths();
		
		if (deltaDist > 0)
		{
			distance += deltaDist;
		
			while (distance > lengths[index])
			{
				distance -= lengths[index];
				if (++index > points.Count - 3)
				{
					isMoving = false;
					distance = lengths[--index];
				}
				else
					calculatedTangents = false;
			}
		}
		else
		{
			distance += deltaDist;
			
			while (distance < 0)
			{
				if (--index < 1)
				{
					isMoving = false;
					index = 1;
					distance = 0;
				}
				else
				{
					distance += lengths[index];
					calculatedTangents = false;
				}
			}
		}
		time = distance / lengths[index];
	}
	
	public override void AddDistanceWithDirection(float deltaDist, Vector3 direction)
	{
		Vector3 lineDirection = points[index+1]-points[index];
		
		float directionDot = Vector3.Dot(direction, lineDirection);
		
		if(directionDot < 0)
			deltaDist *= -1;
		
		AddDistance(deltaDist);
	}
	
	public override Vector3 GetPosition()
	{
		if (!calculatedTangents)
			CalculateTangents();
		
		return GetPoint();
	}
	
	public override Vector3 GetPosition(float atDistance)
	{
		float oldTime = time;
		int oldIndex = index;
		
		if (lengths == null)
			CalculateLengths();

		index = 1;
		while (atDistance > lengths[index])
		{
			atDistance -= lengths[index];
			index++;
		}
		
		CalculateTangents();
		time = atDistance / lengths[index];
		Vector3 point = GetPoint();
		time = oldTime;
		index = oldIndex;
		CalculateTangents();
		
		return point;
	}
	
	public override Vector3 RandomPosition()
	{
		float temp;
		return RandomPosition(out temp);
	}
	
	public override Vector3 RandomPosition(out float randDistance)
	{
		float oldTime = time;
		int oldIndex = index;
		
		if (lengths == null)
			CalculateLengths();
		
		randDistance = Random.Range(0f, totalLength);
		float dist = randDistance;
		index = 1;
		while (dist > lengths[index])
		{
			dist -= lengths[index];
			index++;
		}
		
		CalculateTangents();
		time = dist / lengths[index];
		Vector3 point = GetPoint();
		time = oldTime;
		index = oldIndex;
		CalculateTangents();
		
		return point;
	}
	
#if UNITY_EDITOR
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		
		if (points.Count == 0)
			return;
		
		float origTime = time;
		int origIndex = index;
		bool oldIsMoving = isMoving;
		
		isMoving = true;
		
		if (lengths == null)
			CalculateLengths();
		
		time = 0;
		Vector3 last = GetPoint();
		Vector3	cur;
		
		// Calc distance of segments
		for (index = 1; index < lengths.Length - 2; index++)
		{

			CalculateTangents();
			
			for (time = 0.01f; time < 1.0f; time += 0.01f)
			{
				cur = GetPoint();
				Debug.DrawLine(last, cur, lineColor);
				last = cur;
			}
			time = 0;
		}
		
		index = origIndex;
		time = origTime;
		isMoving = oldIsMoving;
		CalculateTangents();
	}
#endif
	
	void CalculateTangents()
	{
		m0 = Tangent(points[index + 1], points[index - 1]);
		m1 = Tangent(points[index + 2], points[index]);
		
		calculatedTangents = true;
	}
	
	protected override void CalculateLengths()
	{
		lengths = new float[points.Count];
		distances = new float[points.Count];
		totalLength = 0;
		
		// Calc distance of segments
		for (index = 1; index < lengths.Length - 2; index++)
		{
			time = 0;
			lengths[index] = 0;
			distances[index] = 0;

			CalculateTangents();
			
			Vector3 last = GetPoint();
			Vector3	cur;
			for (time = 0.01f; time < 1.0f; time += 0.01f)
			{
				cur = GetPoint();
				lengths[index] += (cur - last).magnitude;
				last = cur;
			}
			for (int d = 1; d < index; d++)
			{
				distances[index] += lengths[d];
			}
			totalLength += lengths[index];
			//Debug.Log("Segment Length: " + index + " = " + lengths[index]);
		}

		index = 1;
		time = 0;
		calculatedTangents = false;
	}
	
	Vector3 Tangent(Vector3 pk1, Vector3 pk_1)
	{
		return new Vector3((pk1.x - pk_1.x) * 0.5f, (pk1.y - pk_1.y) * 0.5f, (pk1.z - pk_1.z) * 0.5f);
	}

	protected override Vector3 GetPoint()
	{
		float t2 = time * time;
		float _1t2 = (1f - time) * (1f - time);
		float _2t = 2 * time;
		
		float h00 = (1f + _2t) * _1t2;
		float h10 = time * _1t2;
		float h01 = t2 * (3f - _2t);
		float h11 = t2 * (time - 1f);
		
		return new Vector3(h00 * points[index].x + h10 * m0.x + h01 * points[index + 1].x + h11 * m1.x,
		                   h00 * points[index].y + h10 * m0.y + h01 * points[index + 1].y + h11 * m1.y,
		                   h00 * points[index].z + h10 * m0.z + h01 * points[index + 1].z + h11 * m1.z);
	}
	
	public override Vector3 GetClosestPointOnSplineToPoint(Vector3 point)
	{
		Vector3 returnPoint = Vector3.zero;
		
		return returnPoint;
	}
	
	public override Vector3 GetClosestPointOnSplineToPoint(Vector3 point, int newIndex)
	{
		Vector3 returnPoint = Vector3.zero;
		
		return returnPoint;
	}
	
	public override void resetDistance(float newDistance)
	{
		index = 0;
		distance = 0;
		AddDistance(newDistance);
	}
}
