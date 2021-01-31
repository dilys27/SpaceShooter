using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class dlb_Player : MonoBehaviour
{
	float dlb_speed = 5f; // 10f si bonus vitesse
	float dlb_moveHorizontal;

	public GameObject dlb_projectile;
	readonly float dlb_projectileSpeed = 4f;
	float dlb_fireRate = .50f; // .25f si bonus tir
	float dlb_nextFire;

	bool dlb_invincible = false; // true si bonus invincibilité
	float dlb_delaiBonus;
	bool dlb_bonus = false; 

	Rigidbody2D dlb_rb;

	dlb_GameManager dlb_gameManager;

	dlb_Capsule dlb_capsule;
	
	// permet d'accéder aux propriétés de couleur du halo 
	// autre : Behaviour dlb_halo;
	UnityEditor.SerializedObject dlb_halo; // effet d'une aura autour du joueur pour indiquer si il est sous l'effet d'un bonus
	
	void Start()
	{
		dlb_gameManager = GameObject.Find("GameManager").GetComponent<dlb_GameManager>();

		dlb_rb = GetComponent<Rigidbody2D>();

		dlb_halo = new SerializedObject(gameObject.GetComponent("Halo")); // dlb_halo = (Behaviour)GetComponent("Halo");
	}

	void Update()
	{
		if (dlb_GameManager.dlb_state == dlb_GameManager.States.play)
		{
			dlb_Move();
			dlb_Fire();
		} else if (dlb_GameManager.dlb_state == dlb_GameManager.States.wait || dlb_GameManager.dlb_state == dlb_GameManager.States.nextWave) 
		{
			dlb_Move(); //peut bouger pour attraper la capsule dans l'attente d'une nouvelle vague
		}
		
		// vérifie si un bonus est appliqué
        if (dlb_bonus) 
		{
			dlb_delaiBonus += Time.deltaTime;
			// vérifie si le délai du bonus obtenu est dépassé
			if (dlb_delaiBonus > dlb_capsule.dlb_delai)
			{
				// remise des valeurs par défaut du joueur
            	dlb_speed = 5f;
				dlb_fireRate = .50f;
				dlb_invincible = false;
				dlb_bonus = false;
				dlb_delaiBonus = 0;
				dlb_halo.FindProperty("m_Enabled").boolValue = dlb_bonus; // dlb_halo.enabled = dlb_bonus;
		        dlb_halo.ApplyModifiedProperties();
				print("Fin du bonus");
			}
		}
		
	}

	void dlb_Fire()
	{
		dlb_nextFire += Time.deltaTime;
		if (Input.GetButton("Fire1") && dlb_nextFire > dlb_fireRate)
		{
			dlb_Shoot();
			dlb_nextFire = 0;
		}
	}

	void dlb_Shoot()
	{
		GameObject dlb_bullet = Instantiate(dlb_projectile, transform.position, transform.rotation);
		dlb_bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(0, dlb_projectileSpeed, 0);
	}

	void dlb_Move()
	{
		dlb_moveHorizontal = Input.GetAxisRaw("Horizontal");
	}

	private void FixedUpdate()
	{
		Vector3 dlb_force = transform.TransformDirection(-dlb_moveHorizontal * dlb_speed, 0, 0);
		dlb_rb.AddForce(dlb_force);
	}

    void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.tag == "BulletEnemy")
		{
			// détruire la bullet
			Destroy(collision.gameObject);
			if(!dlb_invincible){
				dlb_gameManager.dlb_KillPlayer();
			}
		} else if (collision.tag == "Bonus")
		{
			// appliquer le bonus de la capsule obtenu
			dlb_capsule = collision.gameObject.GetComponent<dlb_Capsule>();
			dlb_ApplyBonus();
			// détruire la capsule
			Destroy(collision.gameObject);
		}
	}

	void dlb_ApplyBonus()
	{
	    if (dlb_Capsule.dlb_bonus == dlb_Capsule.Bonus.tir)
		{
     		dlb_fireRate = .25f;
			dlb_halo.FindProperty("m_Color").colorValue = dlb_Capsule.color1; // change la couleur du halo
		} else if (dlb_Capsule.dlb_bonus == dlb_Capsule.Bonus.invincibilite)
		{
            dlb_invincible = true;
			dlb_halo.FindProperty("m_Color").colorValue = dlb_Capsule.color2;
		} else if (dlb_Capsule.dlb_bonus == dlb_Capsule.Bonus.vitesse)
		{
            dlb_speed = 10f;
			dlb_halo.FindProperty("m_Color").colorValue = dlb_Capsule.color3;
		}
		dlb_bonus = true;
		dlb_halo.FindProperty("m_Enabled").boolValue = dlb_bonus; // autre : dlb_halo.enabled = dlb_bonus;
		dlb_halo.ApplyModifiedProperties();
	}

}
