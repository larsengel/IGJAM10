using UnityEngine;
using System.Collections;
using Spine.Unity;

public class PortalAnimations : MonoBehaviour {

    void StartPortal() {
        gameObject.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "created", false);

    }

    void SetPast() {
        gameObject.GetComponent<SkeletonAnimation>().AnimationName = "past";

    }

    void SetFuture() {
        gameObject.GetComponent<SkeletonAnimation>().AnimationName = "fututre";

    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
