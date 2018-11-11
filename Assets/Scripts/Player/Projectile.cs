using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent((typeof(Rigidbody2D)))]
public class Projectile : MonoBehaviour
{
    public float Range { get; set; }
    public float Damage { get; set; }
    public float Speed { get; set; }

    Animator m_animator;
    Rigidbody2D m_rigidbody;

    float m_fireTime;
    float m_lifetime;
    bool m_hit = false;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        AnimationEventStateBehaviour sb = m_animator.GetBehaviour<AnimationEventStateBehaviour>();
        if(sb != null)
        {
            sb.StateExit += OnExpireAnimationComplete;
        }
    }

    public void Fire(Vector2 direction)
    {
        m_rigidbody.AddForce(direction * Speed, ForceMode2D.Impulse);
        m_fireTime = Time.time;
        m_lifetime = Range;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_hit || Time.time > m_fireTime + m_lifetime)
        {
            m_rigidbody.velocity = Vector2.zero;
            m_animator.SetTrigger("Expire");
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_hit = true;

        IAttackable hitObj = collision.gameObject.GetComponent<IAttackable>();
        if (hitObj != null)
        {
            collision.gameObject.SendMessage("OnHit", Damage);
        }
    }

    public void OnExpireAnimationComplete(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }
}
