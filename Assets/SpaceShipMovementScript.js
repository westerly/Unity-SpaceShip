#pragma strict

var maxSpeed : float = 10;
var acceleration : float = 4000;
var deacceleration : float = 0.1;
@HideInInspector
var deaccelerationVolx : float;
@HideInInspector
var deaccelerationVoly : float;

// La rotation actuelle du SpaceShip autour de l'axe Z
@HideInInspector
var targetZRotation : float;
@HideInInspector
var targetZRotationV : float;

// La rotation actuelle du SpaceShip autour de l'axe X
@HideInInspector
var targetXRotation : float;
@HideInInspector
var targetXRotationV : float;

// L'angle de rotation maximum sur l'axe des Z
var angleRotationMaxZ : float = 45;
// L'angle de rotation maximum sur l'axe des X
var angleRotationMaxX : float = 25;
var rotateSpeed : float = 0.06;

@HideInInspector
// Vecteur mouvement sur le plan XY
var movementXY : Vector2;

function Update () {
	
	// On récupère le vecteur mouvement actuel du personnage sur le plan horizontal xy
	movementXY = Vector2(rigidbody.velocity.x, rigidbody.velocity.y);
	
	// magnitude est la longueur du vecteur mouvement sur le plan xy, de 0 au point xy
	// Si ce vecteur est plus grand que maxWalkSpeed c'est que le personnage va trop vite
	if(movementXY.magnitude > maxSpeed)
	{
		// Permet de normaliser le vecteur mouvement c'est à dire de lui donner une longueur de 1
		// mais celui ci reste toujours dans la meme direction
		movementXY = movementXY.normalized;
		// On mutliplie le vecteur mouvement par maxWalkSpeed pour donner au personnage la vitesse max paramétré
		movementXY *= maxSpeed;
	}
	
	// On affecte au personnage les composantes du vecteur mouvement calculées
	rigidbody.velocity.x = movementXY.x;
	rigidbody.velocity.y = movementXY.y;
	
	// SmoothDamp permet de réduire la vitesse de mouvement en fonction du paramètre walkDeacceleration qui est en seconde
	// current 	The current position.
	// target 	The position we are trying to reach.
	// currentVelocity 	The current velocity, this value is modified by the function every time you call it.
	// smoothTime 	Approximately the time it will take to reach the target. A smaller value will reach the target faster.
	
	// Le mouvement du perso lorsque les touches mouvement sont relaché est donc atténué jusqu'a arriver à 0 (le perso ne ralentit pas brusquement)
	// smoothDamp permet d'avoir le meme comportement quelque soit le framerate du jeu puisque il utilise un temps en seconde pour atténuer la valeur velocity du mouvement
	rigidbody.velocity.x = Mathf.SmoothDamp(rigidbody.velocity.x, 0, deaccelerationVolx, deacceleration);
	rigidbody.velocity.y = Mathf.SmoothDamp(rigidbody.velocity.y, 0, deaccelerationVoly, deacceleration);
		
	rigidbody.AddRelativeForce(Input.GetAxis("Horizontal") * acceleration * Time.deltaTime, Input.GetAxis("Vertical") * acceleration * Time.deltaTime, 0);
	
	
	targetZRotation = Mathf.SmoothDamp(targetZRotation, -angleRotationMaxZ * Input.GetAxis("Horizontal"), targetZRotationV, rotateSpeed);
	targetXRotation = Mathf.SmoothDamp(targetXRotation, -angleRotationMaxX * Input.GetAxis("Vertical"), targetXRotationV, rotateSpeed);
	
	for (var child : Transform in transform) {
    		child.rotation = Quaternion.Euler(targetXRotation, 0, targetZRotation);
	}
	
		
}