using UnityEngine;
using System.Collections;
using Spine.Unity;

public class DinoAnimations : MonoBehaviour {

    void SetRoar() {
        gameObject.GetComponent<SkeletonAnimation>().AnimationName = "roar";

    }

    void SetIdle() {
        gameObject.GetComponent<SkeletonAnimation>().AnimationName = "idle1";

    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
