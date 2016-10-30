using UnityEngine;
using System.Collections;

public class DrawingAnimation : MonoBehaviour {
    public Sprite without_euter;
    public Sprite with_euter;

    void SetEuter() {
        gameObject.GetComponent<SpriteRenderer>().sprite = with_euter;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
