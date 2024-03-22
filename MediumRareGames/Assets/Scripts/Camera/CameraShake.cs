/*
-----------------------------------------------------------------------------
       Created By Chris Knight
-----------------------------------------------------------------------------
   CameraShake
       - Applies a light or short shake to the camera, depending on preference.
       - Its "shake range" can be changed for a more or less dramatic effect.
       - A time can be specified for the duration of a shake.

   Details:
       - Access the CameraShake class from another script, however youd like
       - Call one of the "Shake" overload functions
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShakeType { Random, UpDown, LeftRight, Diagnal, BackForth };

public class CameraShake : MonoBehaviour {

    public Vector3 range = new Vector3(5f, 5f, 5f);
    public float shakeSpeed = 1f;

    private Camera cam;
    private float passedTime;
    private Vector3 origin;

    public void SetRange(Vector3 newRange) { range = newRange; }

    void Start()
    {
        cam = Camera.main;
        origin = cam.transform.position;
    }

	public void Shake(ShakeType type)
    {
        StopCoroutine(Erupt(ShakeType.Random, 0));
        ResetShakeImmediately();
        StartCoroutine(Erupt(type, 1f));
    }

    public void Shake(ShakeType type, float time)
    {
        StopCoroutine(Erupt(ShakeType.Random ,0));
        ResetShakeImmediately();
        StartCoroutine(Erupt(type, time));
    }

    /// <summary>
    /// Transitions shake back to its default position.
    /// </summary>
    public void ResetShake()
    {
        passedTime = int.MaxValue;
        StopCoroutine(Erupt(ShakeType.Random, 0f));
        StartCoroutine(ResetPosition());
    }

    public void ResetShakeImmediately()
    {
        passedTime = int.MaxValue;
        StopCoroutine(Erupt(ShakeType.Random, 0f));
        cam.transform.position = origin;
    }

    IEnumerator Erupt(ShakeType type, float time)
    {
        passedTime = 0f;

        while (passedTime < time)
        {
            //check type and shake acordingly
            if(type == ShakeType.Random)
            {
                float x = Random.Range(-range.x, range.x);
                float y = Random.Range(-range.y, range.y);
                float z = Random.Range(-range.z, range.z);
                Vector3 pos = cam.transform.position + new Vector3(x, y, z);
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, pos, shakeSpeed * Time.deltaTime);
            }
            else if (type == ShakeType.UpDown)
            {
                float x = 0f;
                float y = Random.Range(-range.y, range.y);
                float z = 0f;
                Vector3 pos = cam.transform.position + new Vector3(x, y, z);
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, pos, shakeSpeed * Time.deltaTime);
            }
            else if (type == ShakeType.BackForth)
            {
                float x = 0f;
                float y = 0f;
                float z = Random.Range(-range.z, range.z);
                Vector3 pos = cam.transform.position + new Vector3(x, y, z);
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, pos, shakeSpeed * Time.deltaTime);
            }
            else if (type == ShakeType.LeftRight)
            {
                float x = Random.Range(-range.x, range.x);
                float y = 0f;
                float z = 0f;
                Vector3 pos = cam.transform.position + new Vector3(x, y, z);
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, pos, shakeSpeed * Time.deltaTime);
            }
            else if (type == ShakeType.Diagnal)
            {
                float x = Random.Range(-range.x, range.x);
                float y = Random.Range(-range.y / 2f, range.y / 2f);
                float z = 0f;
                Vector3 pos = cam.transform.position + new Vector3(x, y, z);
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, pos, shakeSpeed * Time.deltaTime);
            }

            passedTime += Time.deltaTime;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        yield return null;
    }

    IEnumerator ResetPosition()
    {
        while (Vector3.Distance(cam.transform.position, origin) > 0f)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, origin, shakeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
