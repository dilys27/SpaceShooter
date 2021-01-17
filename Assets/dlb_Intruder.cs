﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dlb_Intruder : MonoBehaviour
{
	readonly float initialSpeed = 0f;
	Vector2 speed;

	readonly float initialRotation = 100f;
	float rotation;

	public int points = 10;
	public GameObject[] divisions;

	Rigidbody2D rb;

	dlb_GameManager gameManager;


	void Start()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<dlb_GameManager>();

		rotation = Random.Range(-initialRotation, initialRotation);
		// déterminer la vitesse x / y
		float x = Random.Range(-initialSpeed, initialSpeed);
		float y = Random.Range(-initialSpeed, initialSpeed);
		speed = new Vector2(x, y);

		// appliquer la vélocité
		rb = GetComponent<Rigidbody2D>();
		rb.velocity = speed;
	}

	void Update()
	{
		rb.velocity = speed;
		transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		// Bullet  Player  Enemy

		if (collision.tag == "Player")
		{
			gameManager.KillPlayer();
		}
		else if (collision.tag == "Bullet")
		{
			// détruire la bullet
			Destroy(collision.gameObject);
			// destruction = asteroid initial
			Destroy(gameObject);
			// division    = potentiel
			foreach (GameObject enemy in divisions)
			{
				Instantiate(enemy, transform.position, Quaternion.identity);
			}
			// score
			gameManager.AddScore(points);
		}


	}


}