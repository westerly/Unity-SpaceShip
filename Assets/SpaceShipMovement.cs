using UnityEngine;
using System.Collections;



public class SpaceShipMovement : MonoBehaviour {

	// max speed sur le plan XY (déplacement haut bas gauche droite)
	public float maxSpeedXY = 10;
	// max speed sur le plan Z (avant arrière)
	public float maxSpeedZ = 3;
	public float acceleration = 4000;
	public float deacceleration = 0.1f;
	[HideInInspector]
	public float deaccelerationVolx;
	[HideInInspector]
	public float deaccelerationVoly;
	
	// Position Y du Space Ship au début de niveau
	[HideInInspector]
	public float posXStartLevel;
	// Position Y du Space Ship au début de niveau
	[HideInInspector]
	public float posYStartLevel;
	
	// Les distances maximales que le space ship peut parcourir sur l'axe des x et des Y
	public float maxDistMovementX = 8;
	public float maxDistMovementY = 4;
	
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
	
	// Le nombre de bullet que l'on peut tirer par seconde 
	public float fireSpeed = 15;
	
	// Cette variable est en fait un timer, si celle ci vaut 0 ou moins on peut tirer sinon on doit attendre
	[HideInInspector]
	public float waitTilNextFire = 0;
	
	public GameObject bulletSpawn;
	public GameObject bullet;


	// Use this for initialization
	void Start () {
		posXStartLevel = transform.position.x;
		posYStartLevel = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {

		// On récupère le vecteur mouvement actuel du personnage sur le plan horizontal xy
		movementXY = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y);
		
		//movementZ = rigidbody.velocity.z;
		
		// magnitude est la longueur du vecteur mouvement sur le plan xy, de 0 au point xy
		// Si ce vecteur est plus grand que maxWalkSpeed c'est que le personnage va trop vite
		if(movementXY.magnitude > maxSpeedXY)
		{
			// Permet de normaliser le vecteur mouvement c'est à dire de lui donner une longueur de 1
			// mais celui ci reste toujours dans la meme direction
			movementXY = movementXY.normalized;
			// On mutliplie le vecteur mouvement par maxWalkSpeed pour donner au personnage la vitesse max paramétré
			movementXY *= maxSpeedXY;
		}
		
		// Permet d'empecher le space ship de dépasser sa vitesse max sur l'axe z
		//if(movementZ > maxSpeedZ){
		//	movementZ = maxSpeedZ;
		//}
		
		// On affecte au personnage les composantes du vecteur mouvement calculées
		rigidbody.velocity = new Vector3 (movementXY.x, movementXY.y, rigidbody.velocity.z);
		

		// SmoothDamp permet de réduire la vitesse de mouvement en fonction du paramètre walkDeacceleration qui est en seconde
		// current 	The current position.
		// target 	The position we are trying to reach.
		// currentVelocity 	The current velocity, this value is modified by the function every time you call it.
		// smoothTime 	Approximately the time it will take to reach the target. A smaller value will reach the target faster.
		
		// Le mouvement du perso lorsque les touches mouvement sont relaché est donc atténué jusqu'a arriver à 0 (le perso ne ralentit pas brusquement)
		// smoothDamp permet d'avoir le meme comportement quelque soit le framerate du jeu puisque il utilise un temps en seconde pour atténuer la valeur velocity du mouvement
		rigidbody.velocity = new Vector3(Mathf.SmoothDamp(rigidbody.velocity.x, 0,ref deaccelerationVolx, deacceleration),
		                                 Mathf.SmoothDamp(rigidbody.velocity.y, 0,ref deaccelerationVoly, deacceleration),
		                                 rigidbody.velocity.z);
		
		rigidbody.AddRelativeForce(Input.GetAxis("Horizontal") * acceleration * Time.deltaTime, Input.GetAxis("Vertical") * acceleration * Time.deltaTime, 0);
		
		// Permet de garder le space ship dans la zone de l'écran
		if(transform.position.x > posXStartLevel + maxDistMovementX/2)
			transform.position = new Vector3(posXStartLevel + maxDistMovementX/2, transform.position.y, transform.position.z);
		
		if(transform.position.x < posXStartLevel - maxDistMovementX/2)
			transform.position = new Vector3(posXStartLevel - maxDistMovementX/2, transform.position.y, transform.position.z);
		
		if(transform.position.y > posYStartLevel + maxDistMovementY/2)
			transform.position = new Vector3(transform.position.x, posYStartLevel + maxDistMovementY/2, transform.position.z);
		
		if(transform.position.y < posYStartLevel - maxDistMovementY/2)
			transform.position = new Vector3(transform.position.x, posYStartLevel - maxDistMovementY/2, transform.position.z);
		
		targetZRotation = Mathf.SmoothDamp(targetZRotation, -angleRotationMaxZ * Input.GetAxis("Horizontal"), ref targetZRotationV, rotateSpeed);
		targetXRotation = Mathf.SmoothDamp(targetXRotation, -angleRotationMaxX * Input.GetAxis("Vertical"), ref targetXRotationV, rotateSpeed);
		
		foreach (Transform child in transform) {
			child.rotation = Quaternion.Euler(targetXRotation, 0, targetZRotation);
		}
		
		// Si on peut tirer sur cette frame
		if(waitTilNextFire <=0){ 	
			if(bullet)
				// On instancie une bullet à la position et à l'angle de rotation de bulletSpawn (situé juste devant le gun)
				Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
			
			// Lorsque on a tiré une balle on remet le timer à 1
			waitTilNextFire = 1;
		}
		// Ici on décrémente waitTilNextFire par Time.deltaTime qui est le nombre de secondes jusqu'à la prochaine frame
		// multiplié par fireSpeed. 
		// Par exemple si fireSpeed vaut 1 (on tir une balle par seconde) Time.deltaTime * 1 est décrémenté à chaque frame
		// donc waitTillNextFire arrive à 0 (ou un peu moins de 0) en une seconde
		waitTilNextFire -= Time.deltaTime * fireSpeed;
	
	}
}
