using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dlb_BlockScreen : MonoBehaviour
{
	Camera dlb_cam;
	float dlb_height;
	float dlb_width;

	void Start()
	{
		dlb_cam = Camera.main;
		dlb_height = dlb_cam.orthographicSize;
		dlb_width = dlb_height * dlb_cam.aspect;
	}


	void Update()
	{
		if (transform.position.x >= dlb_width)
		{
			transform.position = new Vector3(dlb_width, transform.position.y, 0);
		}
		else if (transform.position.x <= -dlb_width)
		{
			transform.position = new Vector3(-dlb_width, transform.position.y, 0);
		}

		if (transform.position.y >= dlb_height)
		{
			transform.position = new Vector3(transform.position.x, dlb_height, 0);
		}
		else if (transform.position.y <= -dlb_height)
		{
			transform.position = new Vector3(transform.position.x, -dlb_height, 0);
		}
	}
}
