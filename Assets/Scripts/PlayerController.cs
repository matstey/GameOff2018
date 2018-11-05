using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    float m_maxSpeed = 10.0f;

    [SerializeField]
    float m_maxWalkAnimSpeed = 0.5f;

    [SerializeField]
    Animator m_bodyAnimController;

    [SerializeField]
    Animator m_legsAnimController;
    
    Rigidbody2D m_rigidbody;
    SpriteRenderer[] m_renderers;

	// Use this for initialization
	void Start () {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_renderers = GetComponentsInChildren<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        bool attack = Input.GetButtonDown("Fire1");

        Vector2 move = new Vector2(moveX, moveY);
        m_rigidbody.velocity = move * m_maxSpeed;

        float animSpeed = move.magnitude * m_maxWalkAnimSpeed;

        if (animSpeed > 0.1)
        {
            m_legsAnimController.speed = animSpeed;
            m_bodyAnimController.speed = animSpeed;

        }
        else
        {
            m_legsAnimController.Play("LegsRun", -1, 0.0f);
        }

        SetDirection(moveX < 0);

        if (attack)
        {
            m_bodyAnimController.SetTrigger("Attack");
        }
    }

    void SetDirection(bool left)
    {
        foreach (SpriteRenderer r in m_renderers)
        {
            r.flipX = left;
        }
    }
}
