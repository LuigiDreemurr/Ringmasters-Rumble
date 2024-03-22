using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Portal : MonoBehaviour
{

    public Portal endPortal;

    private void Start()
    {
        if (endPortal != null) { endPortal.endPortal = this; } //this portals entry is its portals exit, and vice versa
    }

    private void OnDrawGizmos()
    {
        if (GetComponent<Teleportation>()) { Debug.Log("You cant add a Teleportation to a Portal object."); DestroyImmediate(GetComponent<Teleportation>()); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("porting player");
            other.GetComponent<Teleportation>().Teleport(this);
        }
        else if (other.tag == "Bullet")
        {
            print("porting bullet");
            other.GetComponent<Teleportation>().Teleport(this);
        }
    }
}
