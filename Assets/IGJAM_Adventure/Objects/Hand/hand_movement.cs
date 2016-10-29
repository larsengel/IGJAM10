using UnityEngine;
using System.Collections;

public class hand_movement : MonoBehaviour {
    public GameObject _start;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
        transform.position = new Vector3(pos_move.x, pos_move.y, pos_move.z);

        LineRenderer lr = GetComponent<LineRenderer>();
        Vector3[] pos = new Vector3[2];
        pos[0] = transform.position;
        pos[1] = _start.transform.position;
        lr.SetPositions(pos);
    }
}
