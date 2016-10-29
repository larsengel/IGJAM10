using UnityEngine;
using System.Collections;
using Spine.Unity;

public class ToasterAnimations : MonoBehaviour {

    void SetBurning() {
        gameObject.GetComponent<SkeletonAnimation>().AnimationName = "toaster_burning";
        gameObject.GetComponent<SkeletonAnimation>().loop = true;

    }

    void SetBurnt() {
        gameObject.GetComponent<SkeletonAnimation>().AnimationName = "toaster_burnt";
        gameObject.GetComponent<SkeletonAnimation>().loop = true;

    }

    void SetSwitch() {
        gameObject.GetComponent<SkeletonAnimation>().loop = false;
        gameObject.GetComponent<SkeletonAnimation>().AnimationName = "toaster_burnt_switch";
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
