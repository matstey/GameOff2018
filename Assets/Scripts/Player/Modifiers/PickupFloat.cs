using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFloat : MonoBehaviour {

    [SerializeField]
    Transform m_pickupSprite;

    [SerializeField]
    Transform m_shadowSprite;

    Vector3 m_startPos;
    Vector3 m_startScale;

	// Use this for initialization
	void Start () {
        m_startPos = m_pickupSprite.localPosition;
        m_startScale = m_shadowSprite.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        float amount = ((Mathf.Sin(Time.time * 3.0f) + 0.5f) / 2.0f) * 0.2f;

        m_pickupSprite.localPosition = new Vector3(m_startPos.x, m_startPos.y + amount, m_startPos.y);
        m_shadowSprite.localScale = m_startScale * (1.0f - amount);
	}
}
