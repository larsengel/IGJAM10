using UnityEngine;
using System.Collections;
using Spine.Unity;

public class MammutAnimation : MonoBehaviour {

    void SetLaserFuture() {
        gameObject.GetComponent<SkeletonAnimation>().skeleton.SetSkin("laser");
    }

    void SetDefault() {
        gameObject.GetComponent<SkeletonAnimation>().skeleton.SetSkin("normal");
    }

    void SetLaserPirat() {
        gameObject.GetComponent<SkeletonAnimation>().skeleton.SetSkin("pirat");
    }
    void SetLaserPiratEuter() {
        gameObject.GetComponent<SkeletonAnimation>().skeleton.SetSkin("euter");
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
