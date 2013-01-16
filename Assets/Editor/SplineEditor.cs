using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

[CustomEditor(typeof(CMRSpline))]
public class SplineEditor : Editor
{
	CMRSpline	spline;
	Vector3[]	points;
	int numPoints;
	int oldNumPoints;
	bool pointsFoldout;
	string fileDirectory = "Assets/Resources/Splines/";
	
	void OnEnable()
	{
		spline = (CMRSpline)target;
		points = spline.Points;
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		numPoints = EditorGUILayout.IntField("Point Count", numPoints, EditorStyles.numberField, null);
		
		pointsFoldout = EditorGUILayout.Foldout(pointsFoldout, "Spline positions");
		if (pointsFoldout)
		{
			EditorGUI.indentLevel++;
			float x, y, z;
			for (int i = 0; i < points.Length; i++)
			{
				x = points[i].x;
				y = points[i].y;
				z = points[i].z;
						
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Point "+i);
				GUILayout.Space(10f);
				
				GUILayout.Label("X:");
				x = EditorGUILayout.FloatField(x);
				
				GUILayout.Label("Y:");
				y = EditorGUILayout.FloatField(y);
				
				GUILayout.Label("Z:");
				z = EditorGUILayout.FloatField(z);
				EditorGUILayout.EndHorizontal();
				
				points[i] = new Vector3(x, y, z);
			}
			EditorGUI.indentLevel--;
		}
		
		
		if(GUILayout.Button("Update Spline Points") && numPoints != oldNumPoints)
		{
			Vector3[] newPoints = new Vector3[numPoints];
			
			spline.Clear();
			
			//adding points
			if (points.Length == 0)
			{
				for (int i = 0; i < numPoints; i++)
					spline.AddPoint(new Vector3(10+i*10, 10+i*10, 0));
			}
			else if(numPoints > oldNumPoints)
			{
				for(int i = 0; i < oldNumPoints; i++)
				{
					newPoints[i] = points[i];
					spline.AddPoint(newPoints[i]);
				}
				
				Vector3 resumePoint = newPoints[oldNumPoints-1];
				
				for(int i = oldNumPoints; i < numPoints; i++)
					spline.AddPoint(new Vector3(resumePoint.x+((i-oldNumPoints)*10), resumePoint.y+((i-oldNumPoints)*10), 0));
			}
			else //removing points
			{
				for(int i = 0; i < numPoints; i++)
				{
					newPoints[i] = points[i];
					spline.AddPoint(newPoints[i]);
				}
			}
			
			Vector3 pos;
			for (int i = 0; i < points.Length; i++)
			{
				pos = Handles.PositionHandle(points[i], Quaternion.identity);
				if (pos != points[i])
					points[i] = pos;
			}
			
			newPoints = spline.Points;
			points = newPoints;
			oldNumPoints = numPoints;
		}
		
		string fileName = string.Empty;
		if(GUILayout.Button("Save Points"))
		{
			//parse spline and create gameobject children to use
			GameObject go = spline.gameObject;
			Transform goTransform = go.transform;
			for (int i = 0; i < goTransform.childCount; i++)
		    {
				DestroyImmediate(goTransform.GetChild(i).gameObject);
		    }
			
			string[] path = EditorApplication.currentScene.Split(char.Parse("/"));
			
            fileName = path[path.Length-1] + go.tag;
			Debug.Log("save Filename:"+fileName);
			
			string localPath = fileDirectory + fileName + ".bytes";
            if (AssetDatabase.LoadAssetAtPath(localPath, typeof(TextAsset)))
			{
                if (EditorUtility.DisplayDialog("Are you sure?", "The file already exists. Do you want to overwrite it?", "Yes", "No"))
					Write(fileName);
            } 
			else 
			{
				Write(fileName);
            }
		}
		
		fileName = string.Empty;
		if(GUILayout.Button("Load Points"))
		{
			GameObject go = spline.gameObject;
			spline.Clear();
			
			string[] path = EditorApplication.currentScene.Split(char.Parse("/"));
            fileName = path[path.Length-1] + go.tag;
			Debug.Log("load Filename:"+fileName);
		}
		
		if (GUILayout.Button("Load Points From File"))
		{
			fileName = EditorUtility.OpenFilePanel("Choose Spline Points Source", Application.dataPath+"/Resources/Splines/", "bytes");
			if (!string.IsNullOrEmpty(fileName))
			{
				fileName = fileName.Substring(fileName.LastIndexOf('/')+1).Replace(".bytes", "");
				Debug.Log("load Filename:"+fileName);
			}
		}
		
		if (fileName != string.Empty)
		{
			Read(fileName);
			for(int i = 0; i < numPoints; i++)
			{
				Debug.Log("adding point:"+points[i].x+" "+points[i].y+" "+points[i].z);
				spline.AddPoint(points[i]);
			}
			
			Vector3 pos;
			for (int i = 0; i < points.Length; i++)
			{
				pos = Handles.PositionHandle(points[i], Quaternion.identity);
				if (pos != points[i])
					points[i] = pos;
			}
		}
	}
	
	public void Write(string fileName)
	{
		System.IO.BinaryWriter bw = new System.IO.BinaryWriter(new System.IO.FileStream(fileDirectory+fileName+".bytes", System.IO.FileMode.Create));
		
		int bytesize = numPoints*24;
		Debug.Log("WRITE bytesize:"+bytesize);
		
		bw.Write(bytesize);
		
		for(int i = 0; i < numPoints; i++)
		{
			Vector3 v = points[i];
			//Debug.Log("x:"+v.x+" y:"+v.y+" z:"+v.z);
			
			double vx = (double)v.x;
			double vy = (double)v.y;
			double vz = (double)v.z;
			//Debug.Log("vx:"+vx+" vy:"+vy+" vz:"+vz);
			
			bw.Write(vx);
			bw.Write(vy);
			bw.Write(vz);
		}
		
		Debug.Log("WRITE DONE");
		bw.Close();
		
		AssetDatabase.Refresh();
	}

	public void Read(string fileName)
	{  
		TextAsset ta = ((TextAsset)Resources.Load("Splines/"+fileName));
		//Debug.Log("READ ta:"+ta);
		
		MemoryStream ms = new MemoryStream(ta.bytes);
		BinaryReader br = new BinaryReader(ms);
		int bytesize = br.ReadInt32();
		//Debug.Log("bytesize:"+bytesize);
		numPoints = oldNumPoints = bytesize/24;
		Vector3[] newPoints = new Vector3[numPoints];
		
		for(int i = 0; i < numPoints; i++)
		{
			Vector3 v = Vector3.zero;
			double vx = br.ReadDouble();
			double vy = br.ReadDouble();
			double vz = br.ReadDouble();
			//Debug.Log("vx:"+vx+" vy:"+vy+" vz:"+vz);
			v.x = (float)vx;
			v.y = (float)vy;
			v.z = (float)vz;
			//Debug.Log("x:"+v.x+" y:"+v.y+" z:"+v.z);
			newPoints[i] = v;
		}
		
		br.Close();
		
		points = newPoints;
	} 
	
	void OnSceneGUI()
	{
		if (points.Length == 0)
		{
			for (int i = 0; i < numPoints; i++)
				spline.AddPoint(new Vector3(i*10, 0, 0));
			
			points = spline.Points;
		}
		
		Vector3 pos;
		bool splineChanged = false;
		for (int i = 0; i < points.Length; i++)
		{
			pos = Handles.PositionHandle(points[i], Quaternion.identity);
			if (pos != points[i])
			{
				points[i] = pos;
				splineChanged = true;
			}
		}
		
		if (splineChanged)
			spline.Points = points;
		
		numPoints = oldNumPoints = spline.Points.Length;
	}
}