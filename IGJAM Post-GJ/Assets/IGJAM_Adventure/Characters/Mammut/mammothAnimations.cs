using UnityEngine;
using System.Collections;
using Spine.Unity;


public class mammothAnimations : MonoBehaviour
{

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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
