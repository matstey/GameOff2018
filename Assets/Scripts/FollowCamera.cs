using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    [SerializeField]
    Transform m_target;

    [SerializeField]
    float m_smoothTime = 0.3f;

    Vector3 m_velocity = Vector3.zero;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (m_target != null)
        {
            Vector3 targetPosition = m_target.TransformPoint(new Vector3(0, 0, transform.position.z));
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_velocity, m_smoothTime);
        }
	}
}
