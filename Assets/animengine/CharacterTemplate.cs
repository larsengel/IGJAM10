using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AC;
using Spine.Unity;

public class Henry : MonoBehaviour, SpineData
{
	// upon a direction change, you can perform some actions here. for example, setting an attachment.
	// skeleton.SetAttachment("hair", "hair flipped");

	public void DirectionSetup_Down(Spine.Skeleton skeleton){}
	public void DirectionSetup_Left(Spine.Skeleton skeleton){}
	public void DirectionSetup_Right(Spine.Skeleton skeleton){}
	public void DirectionSetup_Up(Spine.Skeleton skeleton){}
	public void DirectionSetup_DownLeft(Spine.Skeleton skeleton){}
	public void DirectionSetup_DownRight(Spine.Skeleton skeleton){}
	public void DirectionSetup_UpLeft(Spine.Skeleton skeleton){}
	public void DirectionSetup_UpRight(Spine.Skeleton skeleton){}
}
