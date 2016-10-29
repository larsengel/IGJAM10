using UnityEngine;
using System.Collections;

public interface SpineData
{
	void DirectionSetup_Down (Spine.Skeleton skeleton);
	void DirectionSetup_Left (Spine.Skeleton skeleton);
	void DirectionSetup_Right (Spine.Skeleton skeleton);
	void DirectionSetup_Up (Spine.Skeleton skeleton);
	void DirectionSetup_DownLeft (Spine.Skeleton skeleton);
	void DirectionSetup_DownRight (Spine.Skeleton skeleton);
	void DirectionSetup_UpLeft (Spine.Skeleton skeleton);
	void DirectionSetup_UpRight (Spine.Skeleton skeleton);
}
