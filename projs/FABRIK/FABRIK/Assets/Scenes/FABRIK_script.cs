using System;
using UnityEngine;

public class FABRIK_script : MonoBehaviour {
	const int maxIterations = 100;
	public float minAcceptableDst = 0.01f;
	public LineRenderer line;
	Vector3 Target;
	Vector3[] Positions;
	private SpriteRenderer spriteR;

	public void Solve(Vector3[] points, Vector3 target){
		Vector3 origin = points[0];
		float[] segmentLengths = new float[points.Length - 1];
		
		for(int i=0; i < points.Length - 1; i++){
			segmentLengths[i] = (points[i + 1] - points[i]).magnitude;
		}
		
		for (int iteration = 0; iteration < maxIterations; iteration++){
			bool startingFromTarget = iteration % 2 == 0;
			// Reverse arrays to alterante between forward and backward passes
			System.Array.Reverse(points);
			System.Array.Reverse(segmentLengths);
			points[0] = (startingFromTarget) ? target : origin;

			// Constrain lengths
			for (int i = 1; i < points.Length; i++){
				Vector3 dir = (points[i] - points[i-1]).normalized;
				points[i] = points[i-1] + dir * segmentLengths[i-1];
			}

			// Terminate if close enough to target
			float dstToTarget = (points[points.Length - 1] - target).magnitude;
			if (!startingFromTarget && dstToTarget <= minAcceptableDst){
				return;
			}
		}
	}

	void Start (){
		// The sprite is a child to Master
		spriteR = gameObject.GetComponentInChildren<SpriteRenderer>();
		Positions = new Vector3[line.positionCount];
		for (int i=0; i < line.positionCount; i++){
			Positions[i] = line.GetPosition(i);
		}
	}

	void Update(){
		// Input.mousePosition reflects the position of the mouse in relation
		// to the screen (i.e. camera), starting from the lower left corner. 
		Target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		line.SetPosition(line.positionCount-1, Target);
		Solve(Positions, Target);
		for (int i=0; i < line.positionCount-1; i++){
			line.SetPosition(i, Positions[i]);
			spriteR.transform.position = line.GetPosition(i);
		}
	}

}
