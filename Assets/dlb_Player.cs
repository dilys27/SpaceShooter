using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dlb_Player : MonoBehaviour
{
	readonly float dlb_speed = 10f;
	float dlb_moveHorizontal;

	public GameObject dlb_projectile;
	readonly float dlb_projectileSpeed = 4f;
	readonly float dlb_fireRate = .25f;
	float dlb_nextFire;

	Rigidbody2D dlb_rb;

	dlb_GameManager dlb_gameManager;

	void Start()
	{
		dlb_gameManager = GameObject.Find("GameManager").GetComponent<dlb_GameManager>();

		dlb_rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if (dlb_GameManager.dlb_state == dlb_GameManager.States.play)
		{
			dlb_Move();
			dlb_Fire();
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
			dlb_gameManager.dlb_KillPlayer();
		}
	}

}
