using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour {

    [SerializeField]
    Transform m_target;

    [SerializeField]
    float m_smoothTime = 0.3f;

    [SerializeField]
    RectTransform m_margin;

    Vector3 m_velocity = Vector3.zero;
    Camera m_camera;
    float m_lastAspect = 0;
    Rect m_bounds = new Rect();

    // Use this for initialization
    void Start () {
        m_camera = GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update () {

        if (m_camera.aspect != m_lastAspect)
        {
            Vector2 s = new Vector2(m_camera.orthographicSize * m_camera.aspect, m_camera.orthographicSize);
            s *= 2.0f;

            Vector2 newSize = new Vector2(m_margin.rect.width - s.x, m_margin.rect.height - s.y);

            m_bounds.size = newSize;
            m_bounds.center = new Vector2(m_margin.position.x, m_margin.position.y);
            
            m_lastAspect = m_camera.aspect;
        }

        if (m_target != null)
        {
            Vector3 targetPosition = m_target.TransformPoint(new Vector3(0, 0, transform.position.z));
            targetPosition.x = Mathf.Clamp(targetPosition.x, m_bounds.xMin, m_bounds.xMax);
            targetPosition.y = Mathf.Clamp(targetPosition.y, m_bounds.yMin, m_bounds.yMax);

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_velocity, m_smoothTime);
        }
	}
}
