/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionTemplate.cs"
 * 
 *	This is a blank action template.
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionMaterialColour : Action
	{
		
		public int parameterID = -1;
		public int constantID = 0;
		public bool isPlayer;
		public Char _char;

		public bool isInstant;
		public float fadeSpeed = 0.5f;

		public Color newColor;

		public ActionMaterialColour ()
		{
			this.isDisplayed = true;
			category = ActionCategory.Character;
			title = "Change Character Colour (Spine)";
			description = "Change the material colour for a character.";
		}
		
		
		override public float Run ()
		{
			if (!isRunning)
			{
				isRunning = true;

				if (_char)
				{
					CharSpine charSpine = _char.GetComponent<CharSpine> ();
					if (charSpine)
					{
						charSpine.skeletonAnimation.skeleton.SetColor (newColor);
					}
				}

				if (willWait && !isInstant)
				{
					return (fadeSpeed);
				}

				return 0f;
			}

			else
			{
				isRunning = false;
				return 0f;
			}
		}
		
		
		#if UNITY_EDITOR

		override public void ShowGUI (List<ActionParameter> parameters)
		{
			isPlayer = EditorGUILayout.Toggle ("Is Player?", isPlayer);
			if (isPlayer)
			{
				if (Application.isPlaying)
				{
					_char = KickStarter.player;
				}
				else
				{
					_char = AdvGame.GetReferences ().settingsManager.GetDefaultPlayer ();
				}
			}
			else
			{
				parameterID = Action.ChooseParameterGUI ("Character:", parameters, parameterID, ParameterType.GameObject);
				if (parameterID >= 0)
				{
					constantID = 0;
					_char = null;
				}
				else
				{
					_char = (Char) EditorGUILayout.ObjectField ("Character:", _char, typeof (Char), true);

					constantID = FieldToID <Char> (_char, constantID);
					_char = IDToField <Char> (_char, constantID, false);
				}
			}

			if (_char)
			{
				EditorGUILayout.Space ();
				newColor = (Color) EditorGUILayout.ColorField ("New Color:", newColor);
			}
			else
			{
				EditorGUILayout.HelpBox ("This Action requires a Character before more options will show.", MessageType.Info);
			}

			EditorGUILayout.Space ();

			isInstant = EditorGUILayout.Toggle ("Instant?", isInstant);
			if (!isInstant)
			{
				fadeSpeed = EditorGUILayout.Slider ("Time to fade:", fadeSpeed, 0, 5);
				willWait = EditorGUILayout.Toggle ("Wait until finish?", willWait);
			}

			AfterRunningOption ();
		}
		

		public override string SetLabel ()
		{
			string labelAdd = "";

			if (isPlayer)
			{
				labelAdd = " (Player)";
			}
			else if (_char)
			{
				labelAdd = " (" + _char.name + ")";
			}

			return labelAdd;
		}

		#endif
		
	}

}