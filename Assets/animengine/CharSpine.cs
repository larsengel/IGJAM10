using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;

public class CharSpine : MonoBehaviour
{

	// Spine data
	public List<Direction> directions = new List<Direction> ();
	public Direction currentDirection = null;
	public SkeletonAnimation skeletonAnimation;
}

[System.Serializable]
public class Direction
{
	public AC.CharDirection charDirection;
	public SkeletonDataAsset animation;
	public bool flipX;

	public string idleAnim;
	public string walkAnim;

	public Direction ()
	{
		charDirection = AC.CharDirection.Down;
		animation = null;
		flipX = false;
		idleAnim = "Idle";
		walkAnim = "Walk";
	}
}
