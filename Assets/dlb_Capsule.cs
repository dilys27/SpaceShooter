using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dlb_Capsule : MonoBehaviour
{
    public enum Bonus
	{
		tir, invincibilite, vitesse
	}
	public static Bonus dlb_bonus;

    public static Color color1 = Color.green; // couleur du halo quand c'est un bonus de tir
    public static Color color2 = Color.yellow; // couleur du halo quand c'est un bonus d'invincibilité
    public static Color color3 = Color.blue; // couleur du halo quand c'est un bonus de vitesse

    public float dlb_delai = 40f;

    Camera dlb_cam;
	float dlb_height;
	float dlb_width;

    void Start()
    {
        dlb_cam = Camera.main;
		dlb_height = dlb_cam.orthographicSize;
		dlb_width = dlb_height * dlb_cam.aspect;

        // déterminer aléatoirement le type de la capsule à sa création
        int nb = Random.Range(0, 3);
        if (nb == 0)
        {
            dlb_bonus = Bonus.tir;
            print("Bonus de tir");
        } else if (nb == 1)
        {
            dlb_bonus = Bonus.invincibilite;
            print("Bonus d'invincibilité");
        } else if (nb == 2)
        {
            dlb_bonus = Bonus.vitesse;
            print("Bonus de vitesse");
        }
    }

    void Update()
    {
        // détruire la capsule lorsqu'elle sort de l'écran
        if (transform.position.y > dlb_height)
        {
            Destroy(gameObject);
        }
    }

}
