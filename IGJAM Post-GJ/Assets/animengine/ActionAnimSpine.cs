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
	public class ActionAnimSpine : Action
	{

		public int parameterID = -1;
		public int constantID = 0;

		public int parameterID2 = -1;

		// 3D variables

		public float fadeTime = 0f;
		
		// 2D variables
		
		public Transform _anim2D;
		public SkeletonAnimation skeletonAnimation;
		public string clip2D;

		public enum WrapMode2D { Once, Loop, PingPong };
		public WrapMode2D wrapMode2D;
		public int layerInt;

		public bool isPlayer = false;
		public bool animationComplete = false;

		// Mecanim variables

		public AnimMethodMecanim methodMecanim;
		public MecanimParameterType mecanimParameterType;
		public string parameterName;
		public float parameterValue;

		// Regular variables
		
		public AnimMethod method;
		
		public AnimationBlendMode blendMode = AnimationBlendMode.Blend;
		public AnimPlayMode playMode;
		
		public AnimationEngine animationEngine = AnimationEngine.Legacy;
		public AnimEngine_Spine animEngine;

		public bool loop = false;
		public int loopTimes = 1;

		
		public ActionAnimSpine ()
		{
			this.isDisplayed = true;
			category = ActionCategory.Object;
			title = "Animate (Spine)";
			description = "Causes a GameObject to play or stop a Spine animation.";
		}

		public void HandleAnimComplete(Spine.AnimationState state, int trackIndex, int loopCount)
		{
			if (!loop || (loop && loopCount == loopTimes))
				animationComplete = true;
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
				loopTimes = AssignInteger (parameters, parameterID2, loopTimes);
			}
		}


		override public float Run ()
		{
			if (animEngine != null)
			{
				return animEngine.ActionAnimSpineRun (this);
			}
			else
			{
				ACDebug.LogError ("Could not create animation engine!");
				return 0f;
			}
		}


		override public void Skip ()
		{
			if (animEngine != null)
			{
				animEngine.ActionAnimSpineSkip (this);
			}
		}
		
		
		#if UNITY_EDITOR

		override public void ShowGUI (List<ActionParameter> parameters)
		{
			ResetAnimationEngine ();

			if (animEngine)
			{
				animEngine.ActionAnimSpineGUI (this, parameters);
			}

			AfterRunningOption ();
		}
		
		
		override public string SetLabel ()
		{
			string labelAdd = "";

			if (animEngine)
			{
				labelAdd = " (" + animEngine.ActionAnimSpineLabel (this) + ")";
			}

			return labelAdd;
		}


		override public void AssignConstantIDs (bool saveScriptsToo)
		{
			if (!isPlayer && saveScriptsToo)
			{
				ResetAnimationEngine ();

				if (method == AnimMethod.PlayCustom)
				{
					if (animEngine != null && skeletonAnimation != null)
					{
						animEngine.AddSaveScript (this, skeletonAnimation.gameObject);
					}
				}
			}
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