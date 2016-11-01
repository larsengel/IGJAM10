using UnityEngine;
using System.Collections;

public class FridgeAnimations : MonoBehaviour {

    public Sprite closed;
    public Sprite open;

	// Use this for initialization
	void Start () {
	
	}

    void SetOpen() {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = open;
    }

    void SetClosed() {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = closed;

    }

    // Update is called once per frame
    void Update () {
	
	}
}
