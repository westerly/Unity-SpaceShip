using UnityEngine;
using System.Collections;


public class EnnemyMovement : MonoBehaviour {

	private GameObject spaceShip;

	// Le script qui gère les mouvements du space ship
	[HideInInspector]
	public SpaceShipMovement spaceShipMovement;
	
	// max speed sur le plan XY (déplacement haut bas gauche droite)
	public float maxSpeedXY;
	
	public float speedPhaseApproach = 100;
	public float deacceleration;
	[HideInInspector]
	public float deaccelerationVolx;
	[HideInInspector]
	public float deaccelerationVoly;
	
	// La rotation actuelle du SpaceShip autour de l'axe Z
	[HideInInspector]
	public float targetZRotation;
	[HideInInspector]
	public float targetZRotationV;
	
	// La rotation actuelle du SpaceShip autour de l'axe X
	[HideInInspector]
	public float targetXRotation;
	[HideInInspector]
	public float targetXRotationV;
	
	// L'angle de rotation maximum sur l'axe des Z
	public float angleRotationMaxZ = 45;
	// L'angle de rotation maximum sur l'axe des X
	public float angleRotationMaxX = 25;
	public float rotateSpeed = 0.06f;
	
	[HideInInspector]
	// Vecteur mouvement sur le plan XY
	public Vector2 movementXY;
	
	// Permet desavoir si l'ennemie est en phase d'approche vers le joueur
	public bool phaseApproach = true;
	
	// La distance minimal par rapport a la camera sur l'axe des Z ou l'ennemie peut se trouver (à la fin de la phase d'approche)
	public float minDistZFromCamera = 10;
	
	// La distance maximal par rapport a la camera sur l'axe des Z ou l'ennemie peut se trouver
	public float maxDistZFromCamera = 20;
	
	// La distance d'écart entre l'ennemie et la camera sur l'axe des Z à la fin de la phase d'approche
	[HideInInspector]
	public float distZFromCamera;
	// Cette variable représente la distance depuis la position de départ de la camera (ou du space ship c'est equivalent car 
	// la camera est situé à la meme position xy que le space ship) sur l'axe des x
	// ou si l'ennemie attérit entre pos_départ_space_ship_X - cette distance et pos_départ_space_shiX + cette distance
	// alors il sera visible par la caméra 
	[HideInInspector]
	public float distXFromSpaceShip;
	[HideInInspector]
	public float distYFromSpaceShip;
	// La position 
	[HideInInspector]
	public float targetPositionXAtterissage;
	[HideInInspector]
	public float targetPositionYAtterissage;

	// Use this for initialization
	void Start () {

		spaceShip = GameObject.FindWithTag("SpaceShip");
		spaceShipMovement = (SpaceShipMovement)spaceShip.GetComponent<SpaceShipMovement>();
		
		distZFromCamera = Random.Range(minDistZFromCamera, maxDistZFromCamera);
		distXFromSpaceShip =  Mathf.Tan(Camera.main.fieldOfView* Mathf.Deg2Rad) * distZFromCamera;
		distYFromSpaceShip =  Mathf.Tan((Camera.main.fieldOfView/2)* Mathf.Deg2Rad) * distZFromCamera;
		
		targetPositionXAtterissage = Random.Range(spaceShipMovement.posXStartLevel - distXFromSpaceShip, spaceShipMovement.posXStartLevel + distXFromSpaceShip);
		targetPositionYAtterissage = Random.Range(spaceShipMovement.posYStartLevel - distYFromSpaceShip, spaceShipMovement.posYStartLevel + distYFromSpaceShip);
	}
	
	// Update is called once per frame
	void Update () {
		// On récupère le vecteur mouvement actuel de l'ennemie sur le plan horizontal xy
		movementXY = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y);
		
		// magnitude est la longueur du vecteur mouvement sur le plan xy, de 0 au point xy
		// Si ce vecteur est plus grand que maxSpeedXY c'est que l'ennemie va trop vite
		if(movementXY.magnitude > maxSpeedXY)
		{
			// Permet de normaliser le vecteur mouvement c'est à dire de lui donner une longueur de 1
			// mais celui ci reste toujours dans la meme direction
			movementXY = movementXY.normalized;
			// On mutliplie le vecteur mouvement par maxSpeedXY pour donner au personnage la vitesse max paramétré
			movementXY *= maxSpeedXY;
		}
		
		// On affecte à l'ennemie les composantes du vecteur mouvement calculées
		rigidbody.velocity = new Vector3 (movementXY.x, movementXY.y, rigidbody.velocity.z);
		
		// SmoothDamp permet de réduire la vitesse de mouvement en fonction du paramètre walkDeacceleration qui est en seconde
		// current 	The current position.
		// target 	The position we are trying to reach.
		// currentVelocity 	The current velocity, this value is modified by the function every time you call it.
		// smoothTime 	Approximately the time it will take to reach the target. A smaller value will reach the target faster.
		
		// Le mouvement du perso lorsque les touches mouvement sont relaché est donc atténué jusqu'a arriver à 0 (le perso ne ralentit pas brusquement)
		// smoothDamp permet d'avoir le meme comportement quelque soit le framerate du jeu puisque il utilise un temps en seconde pour atténuer la valeur velocity du mouvement
		rigidbody.velocity = new Vector3(Mathf.SmoothDamp (rigidbody.velocity.x, 0, ref deaccelerationVolx, deacceleration), 
		                                 Mathf.SmoothDamp (rigidbody.velocity.y, 0, ref deaccelerationVoly, deacceleration),
		                                 rigidbody.velocity.z);            
		
		if(phaseApproach){
			
			Vector3 targetPosition = new Vector3(targetPositionXAtterissage, targetPositionYAtterissage, Camera.main.transform.position.z + distZFromCamera); 
			
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speedPhaseApproach);
			
			if(transform.position.x == targetPosition.x){
				phaseApproach = false;
			}
			
		}
	}
}
