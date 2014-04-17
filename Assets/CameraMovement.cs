using UnityEngine;
using System.Collections;


public class CameraMovement : MonoBehaviour {

	// le spaceShip suivi par la camera
	private GameObject spaceShip; 
	
	// La hauteur de la camera par rapport au spaceShip
	//var hauteurCamera : float = 2;
	
	// La distance entre la position de la camera et la position du spaceShip sur l'axe des Z
	public float distanceEcartZ = 4;
	
	// Angle du horizontal du champ de vu de la camera dans lequel le spaceShip doit rester
	public float horizontalAngleSpaceShipStay = 100;
	
	// Angle du vertical du champ de vu de la camera dans lequel le spaceShip doit rester
	public float verticallAngleSpaceShipStay = 50;
	
	// Distance que le spaceShip peut parcourir sur l'axe des X depuis la position de la camera 
	// avant que la position de la camera sur l'axe des X doive etre changée pour pouvoir suivre le space ship
	[HideInInspector]
	private float distanceXBeforeMove;
	
	// Distance que le spaceShip peut parcourir sur l'axe des Y depuis la position de la camera 
	// avant que la position de la camera sur l'axe des Y doive etre changée pour pouvoir suivre le space ship
	[HideInInspector]
	private float distanceYBeforeMove;

	// Use this for initialization
	void Start () {
		spaceShip = GameObject.FindWithTag("SpaceShip");
		transform.position = spaceShip.transform.position + new Vector3(0, 0, -distanceEcartZ);
	}
	
	// Update is called once per frame
	void Update () {
		// Le code commenté permet à la camera de suivre le space ship (idée abandonnée)
		
		//distanceXBeforeMove = Mathf.Tan((horizontalAngleSpaceShipStay/2)* Mathf.Deg2Rad) * distanceEcartZ;
		//distanceYBeforeMove = Mathf.Tan((verticallAngleSpaceShipStay/2)* Mathf.Deg2Rad) * distanceEcartZ;
		
		//if(spaceShip.transform.position.x - transform.position.x >= distanceXBeforeMove)
		//transform.position.x = spaceShip.transform.position.x - distanceXBeforeMove;
		
		//if((spaceShip.transform.position.y) - transform.position.y >= distanceYBeforeMove)
		//transform.position.y = spaceShip.transform.position.y - distanceYBeforeMove;
		
		//if(transform.position.x  - spaceShip.transform.position.x >= distanceXBeforeMove)
		//transform.position.x = spaceShip.transform.position.x + distanceXBeforeMove;
		
		//if(transform.position.y - (spaceShip.transform.position.y) >= distanceYBeforeMove)
		//transform.position.y = spaceShip.transform.position.y + distanceYBeforeMove;
		
		// Suivre le space ship sur le plan z
		transform.position =  new Vector3(transform.position.x, transform.position.y, spaceShip.transform.position.z - distanceEcartZ);
	}
}
