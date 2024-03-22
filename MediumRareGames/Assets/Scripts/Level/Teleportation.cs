using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Teleportation : MonoBehaviour
{

    //Exists to prevent Editor from allowing any other objects that do NOT count as a "portal" to be set as the end target of the Portal script.

    public float cooldownTime = 0.5f; //the time it takes to re-allow it to go back through what it entered (prevents portal OnEnter glitching)
    public float pushForce = 4f; //how far this object will travel after teleporting out intially

    [HideInInspector]
    public bool canUse = true;

    private void OnDrawGizmos()
    {
        if (GetComponent<Portal>()) { Debug.Log("You cant add a Portal to a Teleportation object."); DestroyImmediate(GetComponent<Portal>()); }
    }

    public void Teleport(Portal startPortal)
    {
        if(canUse && (this.tag == "Player" || this.tag == "Bullet"))
        {
            //teleport object...
            transform.position = startPortal.endPortal.transform.position;
            transform.eulerAngles = startPortal.endPortal.transform.eulerAngles;

            transform.position += startPortal.endPortal.transform.forward * pushForce;

            //cooldown before next tele is allowed
            Deactivate();
        }
    }

    public void Deactivate()
    {
        canUse = false;
        Invoke("Reactivate", cooldownTime);
    }

    void Reactivate()
    {
        canUse = true;
    }
}
