using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SplineBase : MonoBehaviour
{
	protected List<Vector3>	points = new List<Vector3>();

	protected float[]		lengths;		// Length of segments
	protected float[]		distances;		// Distance to segment from start
	protected float			totalLength;
	
	// Segment variables
	protected float			distance;
	protected int			index;
	protected float			time;
	protected bool			isMoving = true;

	public bool IsMoving
	{
		get { return isMoving; }
	}
	
	public float Distance
	{
		get
		{
			if (lengths == null)
				CalculateLengths();
			return distances[index] + distance;
		}
		set
		{
			distance = 0;
			AddDistance(value);
		}
	}
	
	public float TotalDistance
	{
		get { return totalLength * time; }
	}
	
	public float Length
	{
		get { return totalLength; }
	}
	
	public float SegmentDistance
	{
		get { return distance; }
	}
	
	public float SegmentLength
	{
		get { return lengths[index]; }
	}
	
	public virtual int SegmentIndex
	{
		get { return index; }
	}
	
	public virtual int SegmentCount
	{
		get { return points.Count - 1; }
	}
	
	public virtual float Time
	{
		get { return time; }
	}
	
	public void AddPoint(Vector3 point)
	{
		points.Add(point);
	}
	
	public abstract void Clear();
	
	public abstract void AddDistance(float deltaDist);
	
	public abstract void AddDistanceWithDirection(float deltaDist, Vector3 direction);
	
	public abstract Vector3 GetPosition();
	
	public abstract Vector3 GetPosition(float atDistance);
	
	public abstract Vector3 RandomPosition();
	
	public abstract Vector3 RandomPosition(out float randDistance);
	
	//public abstract void DebugDrawSpline();
	
	public virtual void OnDrawGizmosSelected()
	{
		foreach (Vector3 point in points)
		{
			Gizmos.DrawSphere(point, 1f);
		}
	}
	
	protected abstract void CalculateLengths();
	
	protected abstract Vector3 GetPoint();
	
	public abstract Vector3 GetClosestPointOnSplineToPoint(Vector3 point);
	public abstract Vector3 GetClosestPointOnSplineToPoint(Vector3 point, int newIndex);
	
	public abstract void resetDistance(float newDistance);
}