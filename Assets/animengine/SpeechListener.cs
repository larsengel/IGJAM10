using UnityEngine;
using System.Collections;
using AC;
using Spine.Unity;


public class SpeechListener : MonoBehaviour {

    void GetSpeech(AC.Char speaking_char, string txt, int lineID) {
        Debug.Log(speaking_char.gameObject.GetComponentInChildren<SkeletonAnimation>().AnimationName);
        speaking_char.gameObject.GetComponentInChildren<SkeletonAnimation>().AnimationName = "talk";
    } 

    void EndSpeech(AC.Char speaking_char, string speechText, int lineID) {
        speaking_char.gameObject.GetComponentInChildren<SkeletonAnimation>().AnimationName = "idle";
    }
    private void onDisable() {
        EventManager.OnStartSpeech -= GetSpeech;
        EventManager.OnEndSpeechScroll -= EndSpeech;

    }

    // Use this for initialization
    void Start () {
        EventManager.OnStartSpeech += GetSpeech;
        EventManager.OnEndSpeechScroll += EndSpeech;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
