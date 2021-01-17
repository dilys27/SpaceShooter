using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dlb_Player : MonoBehaviour
{
	// accélération / décélération
	readonly float speed = 10f;
	//readonly float drag = 1; // résistance
	float moveHorizontal; // mouvement horizontal

	// pouvoir tirer
	public GameObject projectile;
	readonly float projectileSpeed = 4f;

	// controler la fréquence de tir
	readonly float fireRate = .25f;
	float nextFire;

	Rigidbody2D rb;

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
		moveHorizontal = Input.GetAxis("Horizontal");

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


}
