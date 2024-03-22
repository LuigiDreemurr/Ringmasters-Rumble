using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    [SerializeField] private Transform m_target;
    [SerializeField] private Vector3 m_offset = new Vector3(0, 11, 0);

    private void LateUpdate()
    {

        transform.position = m_target.position + m_offset;
    }
}
