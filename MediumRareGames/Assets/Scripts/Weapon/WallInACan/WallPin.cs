using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;

public class WallPin : MonoBehaviour
{
    private Wall wall;
    private Health.Health unpinnable; //prevent the same player from being pinned twice in the same second

    // Use this for initialization
    void Start()
    {
        wall = transform.parent.GetComponent<Wall>();
    }

    private void OnCollisionEnter(Collision c)
    {
        //Ignore self
        if (transform.parent.gameObject == c.gameObject || gameObject == c.gameObject)
            return;

        //When hitting a player
        if (c.gameObject.tag == "Player")
        {
            //deal extra damage to player on impact
            wall.DealPushDamage(c);
        }

        //When hitting a wall
        if (c.gameObject.tag == "Untagged") //collides player with environment
        {
            //destroy wall on impact
            wall.DealAdditionalDamage();
        }

    }

    private void OnCollisionStay(Collision c)
    {
        //Ignore self
        if (transform.parent.gameObject == c.gameObject || gameObject == c.gameObject)
            return;

        //When hitting a player
        if (c.gameObject.tag == "Player")
        {
            //deal extra damage to player on impact
            wall.PushPinnedPlayer(c);
        }

    }
}
