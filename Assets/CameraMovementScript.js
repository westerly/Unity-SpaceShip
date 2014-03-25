#pragma strict

// le spaceShip suivi par la camera
var spaceShip : GameObject;

// La hauteur de la camera par rapport au spaceShip
var hauteurCamera : float = 2;

// La distance entre la position de la camera et la position du spaceShip sur l'axe des Z
var distanceEcartZ : float = 4;

// Angle du horizontal du champ de vu de la camera dans lequel le spaceShip doit rester
var horizontalAngleSpaceShipStay : float = 100;

// Angle du vertical du champ de vu de la camera dans lequel le spaceShip doit rester
var verticallAngleSpaceShipStay : float = 50;

// Distance que le spaceShip peut parcourir sur l'axe des X depuis la position de la camera 
// avant que la position de la camera sur l'axe des X doive etre changée pour pouvoir suivre le space ship
@HideInInspector
var distanceXBeforeMove : float;

// Distance que le spaceShip peut parcourir sur l'axe des Y depuis la position de la camera 
// avant que la position de la camera sur l'axe des Y doive etre changée pour pouvoir suivre le space ship
@HideInInspector
var distanceYBeforeMove : float;

function Start () {
	transform.position = spaceShip.transform.position + Vector3(0, hauteurCamera, -distanceEcartZ);
	
}

function Update () {

	distanceXBeforeMove = Mathf.Tan((horizontalAngleSpaceShipStay/2)* Mathf.Deg2Rad) * distanceEcartZ;
	distanceYBeforeMove = Mathf.Tan((verticallAngleSpaceShipStay/2)* Mathf.Deg2Rad) * distanceEcartZ;
	
	if(spaceShip.transform.position.x - transform.position.x >= distanceXBeforeMove)
		transform.position.x = spaceShip.transform.position.x - distanceXBeforeMove;
		
	if((spaceShip.transform.position.y) - transform.position.y >= distanceYBeforeMove)
		transform.position.y = spaceShip.transform.position.y - distanceYBeforeMove;

	if(transform.position.x  - spaceShip.transform.position.x >= distanceXBeforeMove)
		transform.position.x = spaceShip.transform.position.x + distanceXBeforeMove;
		
	if(transform.position.y - (spaceShip.transform.position.y) >= distanceYBeforeMove)
		transform.position.y = spaceShip.transform.position.y + distanceYBeforeMove;
}