using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dlb_Player : MonoBehaviour
{
	readonly float speed = 10f;
	float moveHorizontal;
	//readonly float drag = 1; // résistance

	// pouvoir tirer
	public GameObject projectile;
	readonly float projectileSpeed = 4f;

	// controler la fréquence de tir
	readonly float fireRate = .25f;
	float nextFire;

	Rigidbody2D rb;

	dlb_GameManager gameManager;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		//rb.drag = drag;
	}

	void Update()
	{
		if (dlb_GameManager.state == dlb_GameManager.States.play)
		{
			Move();
			Fire();
		}
	}

	void Fire()
	{
		nextFire += Time.deltaTime;
		if (Input.GetButton("Fire1") && nextFire > fireRate)
		{
			Shoot();
			nextFire = 0;
		}
	}

	void Shoot()
	{
		GameObject bullet = Instantiate(projectile, transform.position, transform.rotation);
		bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(0, projectileSpeed, 0);
	}

	/*void Turn()
	{
		rotation = Input.GetAxisRaw("Horizontal");
		transform.Rotate(0, 0, rotation * Time.deltaTime * rotationSpeed * -1);
	}*/

	void Move()
	{
		moveHorizontal = Input.GetAxisRaw("Horizontal");

		/*thrust = Input.GetAxisRaw("Vertical");
		if (thrust < 0)
		{
			thrust = 0; // rb.drag += Mathf.Abs(thrust);
		}*/
	}

	private void FixedUpdate()
	{
		Vector3 force = transform.TransformDirection(-moveHorizontal * speed, 0, 0);
		rb.AddForce(force);
	}

    void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.tag == "BulletEnemy")
		{
			// détruire la bullet
			Destroy(collision.gameObject);
			gameManager.KillPlayer();
		}
	}

}
