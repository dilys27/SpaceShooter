using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dlb_Intruder : MonoBehaviour
{
	public int dlb_points = 10;
	int dlb_life = 3;

	readonly float dlb_maxSpeed = 3f;
	readonly float dlb_minSpeed = 1f;
	float dlb_speed;
	bool dlb_arret = true; // l'ennemi est à l'arrêt au début de la vague
	Bounds dlb_boundsAttackArea; // limites de l'espace d'attaque des ennemis
	Vector3 dlb_target; // pos cible pour le déplacement auto des ennemis

	public GameObject dlb_projectile;
	readonly float dlb_projectileSpeed = 4f;
	readonly float dlb_fireRate = .50f;
	float dlb_nextFire;

	public GameObject dlb_capsule;
	readonly float dlb_capsuleSpeed = 2f;

	Rigidbody2D dlb_rb;

	dlb_GameManager dlb_gameManager;

	Camera cam;
	float height, width;

	void Start()
	{
		dlb_gameManager = GameObject.Find("GameManager").GetComponent<dlb_GameManager>();

		cam = Camera.main;
		height = cam.orthographicSize;
		width = height * cam.aspect;

		dlb_speed = Random.Range(dlb_minSpeed, dlb_maxSpeed);

		dlb_rb = GetComponent<Rigidbody2D>();
		
		dlb_boundsAttackArea = dlb_gameManager.dlb_attackArea.GetComponent<BoxCollider2D>().bounds;
	}

	void Update()
	{
		if (dlb_GameManager.dlb_state == dlb_GameManager.States.play)
		{
			StartCoroutine(dlb_Move());
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			dlb_gameManager.dlb_KillPlayer();
		}
		else if (collision.tag == "Bullet")
		{
			// détruire la bullet
			Destroy(collision.gameObject);
			// destruction
			if(dlb_life>0)
			{
				dlb_life -= 1;
			}else{
				dlb_LastEnemy(); //instancie la capsule si c'est le dernier ennemi présent de la vague
				Destroy(gameObject);
			}
			// score
			dlb_gameManager.dlb_AddScore(dlb_points);
		}
	}

	IEnumerator dlb_Move()
	{
		if(dlb_arret) {
			if (transform.position.y >= -height) // vérifie si l'ennemi est bien arrivé dans l'écran avant de commencer à tirer
			{
				dlb_Fire();
			}
			yield return new WaitForSecondsRealtime(3f);
			dlb_arret = false;
			//nouveau point aléatoire de l'aire d'attaque pour s'y diriger
			dlb_target = new Vector3(
               Random.Range(dlb_boundsAttackArea.min.x, dlb_boundsAttackArea.max.x),
               Random.Range(dlb_boundsAttackArea.min.y, dlb_boundsAttackArea.max.y),
               0
            );
			dlb_target = dlb_gameManager.dlb_attackArea.GetComponent<BoxCollider2D>().ClosestPoint(dlb_target);
	    } else {
			//se diriger vers le point donné aléatoirement dans l'aire d'attaque du jeu et s'arrêter si atteint
			float dlb_step = dlb_speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, dlb_target, dlb_step);
			if(transform.position == dlb_target){
				dlb_arret = true;
			}
		}
	}

	void dlb_Fire()
	{
		dlb_nextFire += Time.deltaTime;
		if (dlb_nextFire > dlb_fireRate)
		{
			dlb_Shoot();
			dlb_nextFire = 0;
		}
	}

	void dlb_Shoot()
	{
		GameObject bullet = Instantiate(dlb_projectile, transform.position, transform.rotation);
		bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(0, dlb_projectileSpeed, 0);
	}

	void dlb_LastEnemy()
	{
		GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
		if (enemys.Length == 1)
		{
			GameObject capsule = Instantiate(dlb_capsule, transform.position, transform.rotation);
			capsule.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(0, dlb_capsuleSpeed, 0);
		}
	}

}
