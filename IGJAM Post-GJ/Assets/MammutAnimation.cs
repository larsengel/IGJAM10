using UnityEngine;
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
    void SetIdle()
    {
        gameObject.GetComponent<SkeletonAnimation>().AnimationName = "idle";
        gameObject.GetComponent<SkeletonAnimation>().loop = true;

    }

    void SetTalk()
    {
        gameObject.GetComponent<SkeletonAnimation>().AnimationName = "talk";
        gameObject.GetComponent<SkeletonAnimation>().loop = true;

    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
