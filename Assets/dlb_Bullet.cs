using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dlb_Bullet : MonoBehaviour
{
	float dlb_life = 5f;

	void Update()
	{
		dlb_life -= Time.deltaTime;
		if (dlb_life <= 0)
		{
			Destroy(gameObject);
		}
	}
}
