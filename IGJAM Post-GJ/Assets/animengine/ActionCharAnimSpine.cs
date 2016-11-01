using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionCharAnimSpine : Action
	{
		public int parameterID = -1;
		public int constantID = 0;
		public AnimEngine_Spine editingAnimEngine;

		public bool isPlayer;
		public Char animChar;
		public string clip2D;

		public enum AnimMethodChar { PlayCustom, ResetToIdle, SetStandard };
		public AnimMethodChar method;

        public float atTime = 0.0f;
		
		public AnimStandard standard;

        public bool animationComplete = false;

		public bool changeSound = false;
		public AudioClip newSound;
		public int newSoundParameterID = -1;

		public int layerInt = 0;
		public bool idleAfter = true;
		public bool idleAfterCustom = false;

		public bool willTransform = false;
		public Vector3 transformOffset = Vector3.zero;

		public AnimPlayMode playMode;
		public AnimPlayModeBase playModeBase = AnimPlayModeBase.PlayOnceAndClamp;

		public bool changeSpeed = false;
		public float newSpeed = 0f;

		public bool forceDirection = false;
		public CharDirection newDirection;

        public bool loop = true;
        public int loopTimes = 0;

		public float timeScale = 1f;

        public ActionCharAnimSpine ()
		{
			this.isDisplayed = true;
			category = ActionCategory.Character;
			title = "Animate (Spine)";
			description = "Affects a Character's animation. Can play or stop a custom animation, change a standard animation (idle, walk or run), change a footstep sound, or revert the Character to idle.";
		}

		public void HandleAnimComplete(Spine.AnimationState state, int trackIndex)
        {
			if (idleAfter)
			{
				if (willTransform)
				{
					animChar.transform.position += transformOffset;
				}

				animChar.charState = CharState.Idle;
			}

			animationComplete = true;
        }

		override public void AssignValues (List<ActionParameter> parameters)
		{
			animChar = AssignFile <Char> (parameters, parameterID, constantID, animChar);

			if (isPlayer)
			{
				animChar = KickStarter.player;
			}
		}

		
		override public float Run ()
		{
			if (animChar)
			{
				if (animChar.GetAnimEngine () != null && animChar.GetAnimEngine() is AnimEngine_Spine)
				{
                    AnimEngine_Spine animEngine = animChar.GetAnimEngine() as AnimEngine_Spine;
                    return animEngine.ActionCharAnimSpineRun(this);
                }
				else
				{
					ACDebug.LogWarning ("Could not create animation engine for " + animChar.name);
				}
			}
			else
			{
				ACDebug.LogWarning ("Could not create animation engine!");
			}

			return 0f;
		}


		override public void Skip ()
		{
			if (animChar)
			{
				if (animChar.GetAnimEngine () != null)
				{
					AnimEngine_Spine animEngine = animChar.GetAnimEngine() as AnimEngine_Spine;
                    animEngine.ActionCharAnimSpineSkip (this);
				}
			}
		}

		public Vector3 GetLookVector()
		{
			Vector3 lookVector = Vector3.zero;
			Vector3 upVector = Camera.main.transform.up;
			Vector3 rightVector = Camera.main.transform.right - new Vector3(0f, 0.01f); // Angle slightly so that left->right rotations face camera

			if (newDirection == CharDirection.Down)
			{
				lookVector = -upVector;
			}
			else if (newDirection == CharDirection.Left)
			{
				lookVector = -rightVector;
			}
			else if (newDirection == CharDirection.Right)
			{
				lookVector = rightVector;
			}
			else if (newDirection == CharDirection.Up)
			{
				lookVector = upVector;
			}
			else if (newDirection == CharDirection.DownLeft)
			{
				lookVector = (-upVector - rightVector).normalized;
			}
			else if (newDirection == CharDirection.DownRight)
			{
				lookVector = (-upVector + rightVector).normalized;
			}
			else if (newDirection == CharDirection.UpLeft)
			{
				lookVector = (upVector - rightVector).normalized;
			}
			else if (newDirection == CharDirection.UpRight)
			{
				lookVector = (upVector + rightVector).normalized;
			}

			lookVector = new Vector3(lookVector.x, 0f, lookVector.y);
			return lookVector;
		}


#if UNITY_EDITOR

        override public void ShowGUI (List<ActionParameter> parameters)
		{
			isPlayer = EditorGUILayout.Toggle ("Is Player?", isPlayer);
			if (isPlayer)
			{
				if (Application.isPlaying)
				{
					animChar = KickStarter.player;
				}
				else
				{
					animChar = AdvGame.GetReferences ().settingsManager.GetDefaultPlayer ();
				}
			}
			else
			{
				parameterID = Action.ChooseParameterGUI ("Character:", parameters, parameterID, ParameterType.GameObject);
				if (parameterID >= 0)
				{
					constantID = 0;
					animChar = null;
				}
				else
				{
					animChar = (Char) EditorGUILayout.ObjectField ("Character:", animChar, typeof (Char), true);
					
					constantID = FieldToID <Char> (animChar, constantID);
					animChar = IDToField <Char> (animChar, constantID, true);
				}
			}

			if (animChar)
			{
				ResetAnimationEngine (animChar.animationEngine);
			}

			if (editingAnimEngine != null)
			{
				method = (ActionCharAnimSpine.AnimMethodChar) EditorGUILayout.EnumPopup ("Method:", method);

				if (method == ActionCharAnimSpine.AnimMethodChar.PlayCustom)
				{
					clip2D = EditorGUILayout.TextField ("Clip:", clip2D);

					layerInt = EditorGUILayout.IntField ("Layer:", layerInt);

					timeScale = EditorGUILayout.FloatField ("Timescale:", timeScale);

					atTime = EditorGUILayout.FloatField ("At time:", atTime);

					forceDirection = EditorGUILayout.Toggle ("Force direction?", forceDirection);
					if (forceDirection) {
						newDirection = (CharDirection)EditorGUILayout.EnumPopup ("Direction:", newDirection);
					}

					loop = EditorGUILayout.Toggle ("Loop?", loop);
					if (loop) {
						loopTimes = EditorGUILayout.IntField ("Looping times:", loopTimes);
					}

					willWait = EditorGUILayout.Toggle ("Wait until finish?", willWait);
					if (willWait) {
						idleAfter = EditorGUILayout.Toggle ("Return to idle after?", idleAfter);
						if (idleAfter) {
							willTransform = EditorGUILayout.Toggle ("Reposition before idle?", willTransform);
							if (willTransform) {
								transformOffset = EditorGUILayout.Vector3Field ("Point:", transformOffset);
							}
						}
					}
				}
				else if (method == ActionCharAnimSpine.AnimMethodChar.SetStandard)
				{
					clip2D = EditorGUILayout.TextField ("Clip:", clip2D);
					standard = (AnimStandard) EditorGUILayout.EnumPopup ("Change:", standard);

					if (standard == AnimStandard.Walk || standard == AnimStandard.Run)
					{
						changeSound = EditorGUILayout.Toggle ("Change sound?", changeSound);
						if (changeSound)
						{
							newSoundParameterID = Action.ChooseParameterGUI ("New sound:", parameters, newSoundParameterID, ParameterType.UnityObject);
							if (newSoundParameterID < 0)
							{
								newSound = (AudioClip) EditorGUILayout.ObjectField ("New sound:", newSound, typeof (AudioClip), false);
							}
						}
						changeSpeed = EditorGUILayout.Toggle ("Change speed?", changeSpeed);
						if (changeSpeed)
						{
							newSpeed = EditorGUILayout.FloatField ("New speed:", newSpeed);
						}
					}
				}
				else if (method == ActionCharAnimSpine.AnimMethodChar.ResetToIdle)
				{
					idleAfterCustom = EditorGUILayout.Toggle ("Wait for animation to finish?", idleAfterCustom);
				}

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(this);
                }
            }
            else
			{
				EditorGUILayout.HelpBox ("This Action requires a Character before more options will show.", MessageType.Info);
			}

			AfterRunningOption ();
		}

		
		override public string SetLabel ()
		{
			string labelAdd = "";
			
			if (isPlayer)
			{
				labelAdd = " (Player)";
			}
			else if (animChar)
			{
				labelAdd = " (" + animChar.name + ")";
			}
			
			return labelAdd;
		}


		override public void AssignConstantIDs (bool saveScriptsToo)
		{
			if (!isPlayer)
			{
				AssignConstantID <Char> (animChar, constantID, parameterID);
			}
		}


		private void ResetAnimationEngine (AnimationEngine animationEngine)
		{
			string className = "AnimEngine_Spine";
			
			if (editingAnimEngine == null || editingAnimEngine.ToString () != className)
			{
				editingAnimEngine = (AnimEngine_Spine) ScriptableObject.CreateInstance ("AnimEngine_Spine");
			}
		}

		
		#endif

	}

}