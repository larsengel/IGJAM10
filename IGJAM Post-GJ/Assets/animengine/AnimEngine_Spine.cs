using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	public class AnimEngine_Spine : AnimEngine
	{

		private SpineData SpineData;
		private CharSpine CharSpine;

		public override void Declare (AC.Char _character)
		{
			character = _character;
			turningStyle = TurningStyle.Linear;
			isSpriteBased = true;
			character.doDirections = true;
			character.doDiagonals = true;
			SpineData = character.GetComponent<SpineData> ();
			CharSpine = character.GetComponent<CharSpine> ();
			CharSpine.skeletonAnimation = character.spriteChild.GetComponent<SkeletonAnimation> ();
		}


		public override void CharSettingsGUI ()
		{
			#if UNITY_EDITOR

			EditorGUILayout.BeginVertical ("Button");
			EditorGUILayout.LabelField ("Mecanim parameters:", EditorStyles.boldLabel);

			character.spriteChild = (Transform) EditorGUILayout.ObjectField ("Sprite child:", character.spriteChild, typeof (Transform), true);

			character.moveSpeedParameter = EditorGUILayout.TextField ("Move speed float:", character.moveSpeedParameter);
			character.turnParameter = EditorGUILayout.TextField ("Turn float:", character.turnParameter);
			character.talkParameter = EditorGUILayout.TextField ("Talk bool:", character.talkParameter);

			character.verticalMovementParameter = EditorGUILayout.TextField ("Vertical movement float:", character.verticalMovementParameter);
			character.talkingAnimation = TalkingAnimation.Standard;

			if (character.useExpressions)
			{
				character.expressionParameter = EditorGUILayout.TextField ("Expression ID integer:", character.expressionParameter);
			}

			EditorGUILayout.EndVertical ();

			EditorGUILayout.BeginVertical ("Button");
			EditorGUILayout.LabelField ("Other settings:", EditorStyles.boldLabel);

			character.doWallReduction = EditorGUILayout.BeginToggleGroup ("Slow movement near wall colliders?", character.doWallReduction);
			character.wallLayer = EditorGUILayout.TextField ("Wall collider layer:", character.wallLayer);
			character.wallDistance = EditorGUILayout.Slider ("Collider distance:", character.wallDistance, 0f, 2f);
			EditorGUILayout.EndToggleGroup ();

			EditorGUILayout.EndVertical ();

			if (GUI.changed)
			{
				EditorUtility.SetDirty (character);
			}

			#endif
		}


		public override void ActionSpeechGUI (ActionSpeech action, Char speaker)
		{
			#if UNITY_EDITOR

			action.headClip2D = EditorGUILayout.TextField ("Head animation:", action.headClip2D);
			action.mouthClip2D = EditorGUILayout.TextField ("Mouth animation:", action.mouthClip2D);

			if (GUI.changed)
			{
				try
				{
					EditorUtility.SetDirty (action);
				} catch {}
			}

			#endif
		}


		public override void ActionSpeechRun (ActionSpeech action)
		{
			if (action.headClip2D != "" || action.mouthClip2D != "")
			{
				if (character.GetAnimator () == null)
				{
					return;
				}

				if (action.headClip2D != "")
				{
					character.GetAnimator ().CrossFade (action.headClip2D, 0.1f, character.headLayer);
				}
				if (action.mouthClip2D != "")
				{
					character.GetAnimator ().CrossFade (action.mouthClip2D, 0.1f, character.mouthLayer);
				}
			}
		}


		public override void ActionSpeechSkip (ActionSpeech action)
		{}

		public override bool ActionCharHoldPossible ()
		{
			return true;
		}


		public void ActionAnimSpineGUI (ActionAnimSpine action, List<ActionParameter> parameters)
		{
			#if UNITY_EDITOR

			action.methodMecanim = (AnimMethodMecanim) EditorGUILayout.EnumPopup ("Method:", action.methodMecanim);

			if (action.methodMecanim == AnimMethodMecanim.PlayCustom)
			{
				action.parameterID = AC.Action.ChooseParameterGUI ("Skeleton Animation:", parameters, action.parameterID, ParameterType.GameObject);
				if (action.parameterID >= 0)
				{
					action.constantID = 0;
					action.skeletonAnimation = null;
				}
				else
				{
					action.skeletonAnimation = (SkeletonAnimation) EditorGUILayout.ObjectField ("Skeleton Animation:", action.skeletonAnimation, typeof (SkeletonAnimation), true);

					action.constantID = action.FieldToID <SkeletonAnimation> (action.skeletonAnimation, action.constantID);
					action.skeletonAnimation = action.IDToField <SkeletonAnimation> (action.skeletonAnimation, action.constantID, false);
				}
			}

			if (action.methodMecanim == AnimMethodMecanim.PlayCustom)
			{
				action.clip2D = EditorGUILayout.TextField ("Clip:", action.clip2D);

				action.loop = EditorGUILayout.Toggle ("Loop?", action.loop);
				if (action.loop)
				{
					action.parameterID2 = AC.Action.ChooseParameterGUI ("Looping times:", parameters, action.parameterID2, ParameterType.Integer);
					if (action.parameterID2 >= 0)
					{
						action.loopTimes = 0;
					}
					else
					{
						action.loopTimes = EditorGUILayout.IntField ("Looping times:", action.loopTimes);
					}
				}

				action.willWait = EditorGUILayout.Toggle ("Wait until finish?", action.willWait);
			}

			if (GUI.changed)
			{
				EditorUtility.SetDirty (action);
			}

			#endif
		}


		public string ActionAnimSpineLabel (ActionAnimSpine action)
		{
			string label = "";

			if (action.skeletonAnimation)
			{
				label = action.skeletonAnimation.name;

				if (action.methodMecanim == AnimMethodMecanim.ChangeParameterValue && action.parameterName != "")
				{
					label += " - " + action.parameterName;
				}
			}

			return label;
		}


		public float ActionAnimSpineRun (ActionAnimSpine action)
		{
			if (!action.isRunning)
			{
				action.isRunning = true;

				if (action.methodMecanim == AnimMethodMecanim.PlayCustom && action.skeletonAnimation && action.clip2D != "")
				{
					action.animationComplete = false;

					action.skeletonAnimation.state.Complete += action.HandleAnimComplete;

					SetAnimation(action.clip2D, action.loop, action.skeletonAnimation);

					if (action.willWait)
					{
						return (action.defaultPauseTime);
					}
				}
			}
			else
			{
				if (action.methodMecanim == AnimMethodMecanim.PlayCustom)
				{
					if (action.skeletonAnimation && action.clip2D != "")
					{
						if (action.animationComplete)
						{
							action.skeletonAnimation.state.Complete -= action.HandleAnimComplete;

							action.isRunning = false;
							return 0f;
						}
						return (action.defaultPauseTime / 6f);
					}
				}
			}

			return 0f;
		}


		public void ActionAnimSpineSkip (ActionAnimSpine action)
		{
			ActionAnimSpineRun (action);
		}


		public override void ActionCharRenderGUI (ActionCharRender action)
		{
			#if UNITY_EDITOR

			EditorGUILayout.Space ();
			action.renderLock_scale = (RenderLock) EditorGUILayout.EnumPopup ("Character scale:", action.renderLock_scale);
			if (action.renderLock_scale == RenderLock.Set)
			{
				action.scale = EditorGUILayout.IntField ("New scale (%):", action.scale);
			}

			EditorGUILayout.Space ();
			action.renderLock_direction = (RenderLock) EditorGUILayout.EnumPopup ("Sprite direction:", action.renderLock_direction);
			if (action.renderLock_direction == RenderLock.Set)
			{
				action.direction = (CharDirection) EditorGUILayout.EnumPopup ("New direction:", action.direction);
			}

			if (GUI.changed)
			{
				EditorUtility.SetDirty (action);
			}

			#endif
		}


		public override float ActionCharRenderRun (ActionCharRender action)
		{
			if (action.renderLock_scale == RenderLock.Set)
			{
				action._char.lockScale = true;
				float _scale = (float) action.scale / 100f;

				if (action._char.spriteChild != null)
				{
					action._char.spriteScale = _scale;
				}
				else
				{
					action._char.transform.localScale = new Vector3 (_scale, _scale, _scale);
				}
			}
			else if (action.renderLock_scale == RenderLock.Release)
			{
				action._char.lockScale = false;
			}

			if (action.renderLock_direction == RenderLock.Set)
			{
				action._char.SetSpriteDirection (action.direction);
			}
			else if (action.renderLock_direction == RenderLock.Release)
			{
				action._char.lockDirection = false;
			}

			return 0f;
		}

        public float ActionCharAnimSpineRun(ActionCharAnimSpine action)
        {
            if (!action.isRunning)
            {
                action.isRunning = true;
                
				if (action.method == ActionCharAnimSpine.AnimMethodChar.PlayCustom && action.clip2D != "")
				{
                    action.animationComplete = false;

                    if (action.forceDirection)
                    {
                        character.SetLookDirection(action.GetLookVector(), true);
						SetDirection(character.GetSpriteDirectionInt ());
                    }

					character.charState = CharState.Custom;

					CharSpine.skeletonAnimation.timeScale = action.timeScale;
						
					SetAnimation(action.clip2D, action.loop, CharSpine.skeletonAnimation, action.layerInt);

					CharSpine.skeletonAnimation.state.End += action.HandleAnimComplete;

                    if (action.atTime > 0)
                    {
						CharSpine.skeletonAnimation.state.GetCurrent(0).Time = action.atTime;
                    }

                    if (action.willWait)
                    {
                        return (action.defaultPauseTime);
                    }
                }
				else if (action.method == ActionCharAnimSpine.AnimMethodChar.ResetToIdle)
				{
					if (action.idleAfterCustom)
					{
						action.layerInt = 0;
						return (action.defaultPauseTime);
					}
					else
					{
						action.animChar.ResetBaseClips ();
						action.animChar.charState = CharState.Idle;
					}
				}
				else if (action.method == ActionCharAnimSpine.AnimMethodChar.SetStandard)
				{
					if (action.clip2D != "")
					{
						if (action.standard == AnimStandard.Idle)
						{
							action.animChar.idleAnimSprite = action.clip2D;
						}
						else if (action.standard == AnimStandard.Walk)
						{
							action.animChar.walkAnimSprite = action.clip2D;
						}
						else if (action.standard == AnimStandard.Talk)
						{
							action.animChar.talkAnimSprite = action.clip2D;
						}
						else if (action.standard == AnimStandard.Run)
						{
							action.animChar.runAnimSprite = action.clip2D;
						}
					}

					if (action.changeSpeed)
					{
						if (action.standard == AnimStandard.Walk)
						{
							action.animChar.walkSpeedScale = action.newSpeed;
						}
						else if (action.standard == AnimStandard.Run)
						{
							action.animChar.runSpeedScale = action.newSpeed;
						}
					}

					if (action.changeSound)
					{
						if (action.standard == AnimStandard.Walk)
						{
							if (action.newSound != null)
							{
								action.animChar.walkSound = action.newSound;
							}
							else
							{
								action.animChar.walkSound = null;
							}
						}
						else if (action.standard == AnimStandard.Run)
						{
							if (action.newSound != null)
							{
								action.animChar.runSound = action.newSound;
							}
							else
							{
								action.animChar.runSound = null;
							}
						}
					}
				}
            }
            else
            {
                if (action.clip2D != "")
                {
                    if (action.animationComplete)
                    {
						CharSpine.skeletonAnimation.state.End -= action.HandleAnimComplete;
                        action.isRunning = false;
                        return 0f;
                    }
                    return (action.defaultPauseTime / 6f);
                }
            }

            return 0f;
        }

        public void ActionCharAnimSpineSkip(ActionCharAnimSpine action)
        {
            ActionCharAnimSpineRun(action);
        }

		public int DirectionToInt(CharDirection charDirection)
		{
			if (charDirection == CharDirection.Down)
			{
				return 0;
			}
			if (charDirection == CharDirection.Left)
			{
				return 1;
			}
			if (charDirection == CharDirection.Right)
			{
				return 2;
			}
			if (charDirection == CharDirection.Up)
			{
				return 3;
			}
			if (charDirection == CharDirection.DownLeft)
			{
				return 4;
			}
			if (charDirection == CharDirection.DownRight)
			{
				return 5;
			}
			if (charDirection == CharDirection.UpLeft)
			{
				return 6;
			}
			if (charDirection == CharDirection.UpRight)
			{
				return 7;
			}

			return 0;
		}


        public override void PlayIdle ()
		{
			MoveCharacter ();
		}


		public override void PlayWalk ()
		{
			MoveCharacter ();
		}


		private void MoveCharacter ()
		{
			SetDirection (character.GetSpriteDirectionInt());

			if (GetDirection() != null && GetDirection().animation != null)
			{
				if (character.GetMoveSpeed() < 0.01f)
				{
					CharSpine.skeletonAnimation.timeScale = 0.8f;
					SetAnimation(GetDirection().idleAnim);
				}

				if (character.GetMoveSpeed() >= 0.01f)
				{
					CharSpine.skeletonAnimation.timeScale = 1.3f;
					SetAnimation(GetDirection().walkAnim);
				}
			}
		}

		public override void PlayRun ()
		{
			MoveCharacter ();
		}


		public override void PlayTalk ()
		{
			MoveCharacter ();
		}


		public override void PlayVertical ()
		{
//			if (character.verticalMovementParameter != "")
//			{
//				character.GetAnimator ().SetFloat (character.verticalMovementParameter, character.GetHeightChange ());
//			}
		}

		public Direction GetDirection()
		{
			return CharSpine.currentDirection;
		}

		public void SetDirection(int i)
		{
			if (GetDirection().charDirection == CharSpine.directions[i].charDirection)
				return;

			CharSpine.currentDirection = CharSpine.directions[i];
			CharSpine.skeletonAnimation.skeletonDataAsset = CharSpine.currentDirection.animation;
			CharSpine.skeletonAnimation.AnimationName = null;
			CharSpine.skeletonAnimation.Initialize (true);

			Spine.Skeleton skeleton = CharSpine.skeletonAnimation.skeleton;
			skeleton.FlipX = GetDirection().flipX;

			if (SpineData != null)
			{
				switch (GetDirection().charDirection)
				{
				case CharDirection.Left:
					SpineData.DirectionSetup_Left(skeleton);
					break;
				case CharDirection.Right:
					SpineData.DirectionSetup_Right(skeleton);
					break;
				case CharDirection.Down:
					SpineData.DirectionSetup_Down(skeleton);
					break;
				case CharDirection.Up:
					SpineData.DirectionSetup_Up(skeleton);
					break;
				case CharDirection.DownLeft:
					SpineData.DirectionSetup_DownLeft(skeleton);
					break;
				case CharDirection.DownRight:
					SpineData.DirectionSetup_DownRight(skeleton);
					break;
				case CharDirection.UpLeft:
					SpineData.DirectionSetup_UpLeft(skeleton);
					break;
				case CharDirection.UpRight:
					SpineData.DirectionSetup_UpRight(skeleton);
					break;
				}
			}
		}

		public void SetAnimation(string anim, bool loop = true, SkeletonAnimation skelAnim = null, int layer = 0)
		{
			if (skelAnim == null)
				skelAnim = CharSpine.skeletonAnimation;
			
			var track = skelAnim.state.GetCurrent(0);

			if (track != null)
			{
				if (track.Animation.Name == anim)
					return;
			}

			skelAnim.state.SetAnimation(layer, anim, loop);
		}

	}

}
