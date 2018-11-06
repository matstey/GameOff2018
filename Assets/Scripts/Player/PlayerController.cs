using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventAggregator;

public class PlayerController : MonoBehaviour, IHasAttack, IAttackable
{
    [SerializeField]
    float m_maxSpeed = 10.0f;

    [SerializeField]
    float m_maxWalkAnimSpeed = 0.5f;

    [SerializeField]
    Animator m_bodyAnimController;

    [SerializeField]
    Animator m_legsAnimController;

    [SerializeField]
    LayerMask m_attackMask;

    [SerializeField]
    float m_attackRange = 1;

    public bool Attacking { get; set; } = false;
    
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

        m_legsAnimController.speed = animSpeed;
        m_bodyAnimController.speed = Attacking ? 1.0f : animSpeed;
        
        if (animSpeed <= 0.01)
        {
            m_legsAnimController.Play("LegsRun", -1, 0.0f);
        }

        SetDirection(moveX < 0);

        if (attack && !Attacking)
        {
            Attack(moveX < 0);
        }
    }

    void Attack(bool left)
    {
        m_bodyAnimController.SetTrigger("Attack");

        Collider2D hit = Physics2D.OverlapCircle(transform.position, m_attackRange, m_attackMask);
        if (hit != null)
        {
            hit.transform.SendMessage("OnHit", (hit.transform.position - transform.position).magnitude);
        }
    }

    void SetDirection(bool left)
    {
        foreach (SpriteRenderer r in m_renderers)
        {
            r.flipX = left;
        }
    }

    public void OnHit(float damage)
    {
        //Debug.Log($"{name} hit with {damage} damage");
        EventAggregator.SendMessage(new PlayerDamagedEvent() { Damage = damage });
    }
}
