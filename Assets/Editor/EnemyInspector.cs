using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Enemy))]
public class EnemyInspector : Editor
{
	bool visible = false;
	List<MovePhase> tPhases = new List<MovePhase>();
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		EditorGUILayout.BeginVertical();
		visible = EditorGUILayout.Foldout(visible, "Move Sequence");
		if (visible)
		{
			MovePhase[] phases = ((Enemy)target).moves;
			if (phases != null)
				tPhases.AddRange(phases);
			int count = tPhases.Count;
			count = EditorGUILayout.IntField("Move count", count);
			if (count != tPhases.Count)
			{
				if (count < tPhases.Count)
					tPhases.RemoveAt(tPhases.Count-1);
				else
					tPhases.Add(new MovePhase());
			}
			
			MovePhase mp;
			for (int i = 0; i < tPhases.Count; i++)
			{
				mp = tPhases[i];
				
				mp.direction = EditorGUILayout.Vector3Field("Velocity", mp.direction).normalized;
				mp.duration = EditorGUILayout.FloatField("Duration", mp.duration);
				mp.speedMod = Mathf.Clamp01(EditorGUILayout.FloatField("Speed Modifier", mp.speedMod));
			}
			
			((Enemy)target).moves = tPhases.ToArray();
			tPhases.Clear();
			
			EditorUtility.SetDirty(target);
		}
		EditorGUILayout.EndVertical();
	}
}

