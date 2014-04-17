using UnityEngine;
using System.Collections;

public class Gameplay : MonoBehaviour {

	// La distance maximal d'apparition des ennemies sur l'axe des Z
	public float distanceApparitionEnnemiesMaxZ  = 500;
	
	// La distance sur l'axe des Z par rapport à la position de la camera ou l'ennemie apparait (random)
	[HideInInspector] 
	public float distanceZApparition; 
	
	// La distance par rapport à la camera sur l'axe des X ou l'ennemie n'est pas encore dans le field of view de la camera
	// par exemple si posX de l'ennemie > pos Camera X + distanceAwayFromCamFOVX alors on considere que l'ennemie
	// n'est pas visible par la camera (sur l'axe des x il faut ensuite tester pour l'axe y)
	[HideInInspector] 
	public float distanceAwayFromCamFOVX;
	// La distance par rapport à la camera sur l'axe des Y ou l'ennemie n'est pas encore dans le field of view de la camera
	[HideInInspector] 
	public float distanceAwayFromCamFOVY;
	
	[HideInInspector]
	public float cptSeconde; 
	
	// Variable representant un ennemie
	public GameObject ennemy;
	
	// Objet camera
	[HideInInspector] 
	public GameObject cam;

	public int maxEnnemies;

	// Use this for initialization
	void Start () {
		cam = GameObject.FindWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {

		cptSeconde += Time.deltaTime;
		
		// Fréquence d'apparition de 5 secondes
		if(cptSeconde >= 5 && maxEnnemies<15){
			
			float posApparitionX; 
			float posApparitionY;
			
			// Choix random de la posittion d'apparition de l'ennemie sur l'axe des Z
			distanceZApparition = Random.Range(distanceApparitionEnnemiesMaxZ - (distanceApparitionEnnemiesMaxZ/5),distanceApparitionEnnemiesMaxZ);
			
			distanceAwayFromCamFOVX =  Mathf.Tan(Camera.main.fieldOfView* Mathf.Deg2Rad) * distanceZApparition;
			distanceAwayFromCamFOVY =  Mathf.Tan((Camera.main.fieldOfView/2)* Mathf.Deg2Rad) * distanceZApparition;
			
			int decisionBinaireRandom = Random.Range(0,2); 
			// Ici on décide si on fait un random sur la position sur l'axe des X ou sur l'axe des Y
			// Si decisionBinaireRandom == 0 on fait un random sur l'axe des X et on prend une position sur l'axe des Y en dehors du champ de vision de la camera
			// Sinon l'inverse
			if(decisionBinaireRandom == 0){
				posApparitionX = Random.Range(Camera.main.transform.position.x - distanceAwayFromCamFOVX, Camera.main.transform.position.x + distanceAwayFromCamFOVX);
				decisionBinaireRandom = Random.Range(0,2); 
				// Ici on décide si la position sur l'axe des Y sera au dessus du champ de vu de la camera ou au dessous
				// Si decisionBinaireRandom == 0 au dessus sinon au dessous
				if(decisionBinaireRandom == 0){
					posApparitionY = Random.Range(Camera.main.transform.position.y + distanceAwayFromCamFOVY, 
					                              Camera.main.transform.position.y + distanceAwayFromCamFOVY + (distanceAwayFromCamFOVY/5));
				}else{
					posApparitionY = Random.Range(Camera.main.transform.position.y - distanceAwayFromCamFOVY, 
					                              Camera.main.transform.position.y - distanceAwayFromCamFOVY - (distanceAwayFromCamFOVY/5));
				}
			}else{
				posApparitionY = Random.Range(Camera.main.transform.position.y - distanceAwayFromCamFOVY, Camera.main.transform.position.y + distanceAwayFromCamFOVY);
				decisionBinaireRandom = Random.Range(0,2); 
				// Ici on décide si la position sur l'axe des X sera à droite en dehors du champ de vu de la camera ou à gauche
				// Si decisionBinaireRandom == 0 au à droite sinon à gauche
				if(decisionBinaireRandom == 0){
					posApparitionX = Random.Range(Camera.main.transform.position.x + distanceAwayFromCamFOVX, 
					                              Camera.main.transform.position.x + distanceAwayFromCamFOVX + (distanceAwayFromCamFOVX/5));
				}else{
					posApparitionX = Random.Range(Camera.main.transform.position.x - distanceAwayFromCamFOVX,  
					                              Camera.main.transform.position.x - distanceAwayFromCamFOVX - (distanceAwayFromCamFOVX/5));
				}
			}
			
			if(ennemy){
				Instantiate(ennemy, new Vector3(posApparitionX, posApparitionY, distanceZApparition), Camera.main.transform.rotation);
			}
			
			cptSeconde = 0;
			maxEnnemies++;
		}
	
	}
}
