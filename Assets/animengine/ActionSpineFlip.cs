/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionAnim.cs"
 * 
 *	This action is used for standard animation playback for GameObjects.
 *	It is fairly simplistic, and not meant for characters.
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Spine.Unity;

namespace AC
{

	[System.Serializable]
	public class ActionSpineFlip : Action
	{

		public int parameterID = -1;
		public int constantID = 0;

		public SkeletonAnimation skeletonAnimation;
		public bool flipX;
		public bool flipY;

		public AnimationEngine animationEngine = AnimationEngine.Legacy;
		public AnimEngine_Spine animEngine;
		
		public ActionSpineFlip ()
		{
			this.isDisplayed = true;
			category = ActionCategory.Object;
			title = "Spine Flip";
			description = "Causes a SkeletonAnimation to flipX or flipY.";
		}

		public override void AssignValues (List<ActionParameter> parameters)
		{
			if (animEngine == null)
			{
				ResetAnimationEngine ();
			}
			
			if (animEngine != null)
			{
				skeletonAnimation = AssignFile <SkeletonAnimation> (parameters, parameterID, constantID, skeletonAnimation);
			}
		}


		override public float Run ()
		{
			if (animEngine != null)
			{
				skeletonAnimation.skeleton.FlipX = flipX;
				skeletonAnimation.skeleton.FlipY = flipY;
			}
			else
			{
				ACDebug.LogError ("Could not create animation engine!");
			}

			return 0f;
		}
		
		
		#if UNITY_EDITOR

		override public void ShowGUI (List<ActionParameter> parameters)
		{
			ResetAnimationEngine ();

			if (animEngine)
			{
				parameterID = AC.Action.ChooseParameterGUI ("Skeleton Animation:", parameters, parameterID, ParameterType.GameObject);
				if (parameterID >= 0)
				{
					constantID = 0;
					skeletonAnimation = null;
				}
				else
				{
					skeletonAnimation = (SkeletonAnimation) EditorGUILayout.ObjectField ("Skeleton Animation:", skeletonAnimation, typeof (SkeletonAnimation), true);

					constantID = FieldToID <SkeletonAnimation> (skeletonAnimation, constantID);
					skeletonAnimation = IDToField <SkeletonAnimation> (skeletonAnimation, constantID, false);
				}

				flipX = EditorGUILayout.Toggle ("FlipX?", flipX);
				flipY = EditorGUILayout.Toggle ("FlipY?", flipY);
			}

			AfterRunningOption ();
		}
		
		
		override public string SetLabel ()
		{
			return "Spine Flip";
		}
		
		#endif


		private void ResetAnimationEngine ()
		{
			string className = "AnimEngine_Spine";

			if (animEngine == null || animEngine.ToString () != className)
			{
				animEngine = (AnimEngine_Spine) ScriptableObject.CreateInstance (className);
			}
		}

	}

}