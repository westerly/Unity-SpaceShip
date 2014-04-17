using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float speed = 30;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(transform.forward * Time.deltaTime * speed);
	}
}
