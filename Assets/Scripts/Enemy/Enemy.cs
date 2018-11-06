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

    Rigidbody2D m_rigidbody;
    SpriteRenderer[] m_renderers;

    // Use this for initialization
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag(m_targetTag))
        {
            return;
        }

        Vector2 playerToEnemy = other.transform.position - transform.position;
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
            Attack(other.gameObject);
        }
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
        Debug.Log($"{name} hit with {damage}");
    }
}
