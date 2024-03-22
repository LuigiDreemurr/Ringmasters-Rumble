using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject character;
    private float heightOffset;

    private void Awake()
    {
        heightOffset = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(character.transform.position.x, character.transform.position.y + heightOffset, character.transform.position.z);
    }
}
