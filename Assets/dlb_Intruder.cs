using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dlb_Intruder : MonoBehaviour
{
	public int points = 10;
	int life = 3;

    float maxPosX, maxPosY;
	float minPosX, minPosY;
	readonly float initialSpeed = 3f;
	Vector2 speed;
	//readonly float initialRotation = 100f;
	//float rotation;

	Rigidbody2D rb;

	dlb_GameManager gameManager;

	Camera cam;
	float height, width;

	void Start()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<dlb_GameManager>();

		cam = Camera.main;
		height = cam.orthographicSize;
		width = height * cam.aspect;

		// déterminer la vitesse x / y
		float x = Random.Range(0, initialSpeed);
		float y = Random.Range(0, initialSpeed);
		speed = new Vector2(x, y);

		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		StartCoroutine(dlb_Move());
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			gameManager.KillPlayer();
		}
		else if (collision.tag == "Bullet")
		{
			// détruire la bullet
			Destroy(collision.gameObject);
			// destruction
			if(life>0)
			{
				life -= 1;
			}else{
				Destroy(gameObject);
			}
			// score
			gameManager.AddScore(points);
		}
	}

	IEnumerator dlb_Move()
	{
		if(rb.position.y >= -height){
			GetComponent<dlb_BlockScreen>().enabled = true;
		} else {
			yield return new WaitForSecondsRealtime(1f);
			rb.velocity = speed;
			//rb.MovePosition(rb.position + speed * Time.fixedDeltaTime);
			Vector3 force = transform.TransformDirection(0, 0.5f* speed.y * Time.deltaTime, 0);
			rb.AddForce(force);
		}
		//transform.Translate(Vector2.up * speed.y * Time.deltaTime);
		//rb.velocity = speed;
		//Vector3 force = transform.TransformDirection(0, initialSpeed * Time.deltaTime, 0);
		//rb.AddForce(force);
		//rb.MovePosition(rb.position + speed * Time.fixedDeltaTime);
	}


}
