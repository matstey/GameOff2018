using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHasAttack, IAttackable
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
    string m_targetTag = "Player";

    [SerializeField]
    float m_attackDistance = 2;

    [SerializeField]
    float m_stopDistance = 1.8f;

    public bool Attacking { get; set; } = false;

    public event System.Action<Enemy> Died;

    Rigidbody2D m_rigidbody;
    SpriteRenderer[] m_renderers;
    GameObject m_target;

    // Use this for initialization
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_renderers = GetComponentsInChildren<SpriteRenderer>();

        m_legsAnimController.speed = 0.0f;
    }

    void FixedUpdate()
    {
        if (m_target == null)
        {
            return;
        }

        Vector2 playerToEnemy = m_target.transform.position - transform.position;
        float distance = playerToEnemy.magnitude;
        Vector2 direction = playerToEnemy.normalized;

        bool withinAttackDistance = distance < m_attackDistance;
        bool stop = distance <= m_stopDistance;

        m_rigidbody.velocity = stop ? Vector2.zero : direction * m_maxSpeed;

        float animSpeed = m_rigidbody.velocity.magnitude * m_maxWalkAnimSpeed;

        m_legsAnimController.speed = animSpeed;
        m_bodyAnimController.speed = Attacking ? 1.0f : animSpeed;

        if (animSpeed <= 0.01)
        {
            m_legsAnimController.Play("LegsRun", -1, 0.0f);
        }

        SetDirection(direction.x < 0);

        if (withinAttackDistance && !Attacking)
        {
            Attack(m_target);
        }
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        Stop();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If we have a target just focus on that
        if (m_target != null) return;

        if (!other.gameObject.CompareTag(m_targetTag))
        {
            return;
        }

        m_target = other.gameObject;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Only clear the target if we have left our current target
        if (m_target == other.gameObject)
        {
            m_target = null;
            Stop();
        }
    }

    void Stop()
    {
        m_rigidbody.velocity = Vector2.zero;
        m_legsAnimController.Play("LegsRun", -1, 0.0f);
        m_legsAnimController.speed = 0.0f;
    }

    void Attack(GameObject target)
    {
        m_bodyAnimController.SetTrigger("Attack");

        target.SendMessage("OnHit", 10);
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
        Died(this);
    }
}
