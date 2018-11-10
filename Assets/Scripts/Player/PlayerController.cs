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
    float m_rangedDelay = 0.8f;

    [SerializeField]
    float m_rangedDamage = 1.0f;

    [SerializeField]
    float m_rangedRange = 1.0f;

    [SerializeField]
    float m_rangedSpeed = 1.0f;

    [SerializeField]
    float m_meleeRange = 1;

    [SerializeField]
    float m_meleeDamage = 2.0f;

    [SerializeField]
    Animator m_bodyAnimController;

    [SerializeField]
    Animator m_legsAnimController;

    [SerializeField]
    LayerMask m_attackMask;

    [SerializeField]
    Projectile m_projectilePrefab;

    public bool Attacking { get; set; } = false;

    Rigidbody2D m_rigidbody;
    SpriteRenderer[] m_renderers;

    float m_health = 0.0f;
    float m_startHealth = 1.0f;

    float m_lastFireTime = 0;

    public bool IsAlive { get { return m_health > 0.0f; } }

    // Use this for initialization
    void Start () {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_renderers = GetComponentsInChildren<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        float rangedAttackX = Input.GetAxis("FireHorizontal");
        float rangedAttackY = Input.GetAxis("FireVertical");

        bool ranged = rangedAttackX != 0 || rangedAttackY != 0;
        bool melee = Input.GetButtonDown("Melee");

        if (!Attacking)
        {
            float currentTime = Time.time;

            if (melee)
            {
                Attack();
            }
            else if (ranged && currentTime > m_lastFireTime + m_rangedDelay)
            {
                m_lastFireTime = currentTime;

                Vector2 rangedDir;
                if (rangedAttackX != 0)
                {
                    rangedDir = new Vector2(rangedAttackX, 0).normalized;
                }
                else
                {
                    rangedDir = new Vector2(0, rangedAttackY).normalized;
                }

                rangedDir += (m_rigidbody.velocity / 15.0f);
                rangedDir.Normalize();

                AttackRanged(rangedDir);
            }
        }
    }

    private void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(moveX, moveY);
        if (move.magnitude > 1.0f)
        {
            move.Normalize();
        }

        m_rigidbody.velocity = move * m_maxSpeed;

        float animSpeed = move.magnitude * m_maxWalkAnimSpeed;

        m_legsAnimController.speed = animSpeed;
        m_bodyAnimController.speed = Attacking ? 1.0f : animSpeed;
        
        if (animSpeed <= 0.01)
        {
            m_legsAnimController.Play("LegsRun", -1, 0.0f);
        }

        SetDirection(moveX < 0);
    }

    void Attack()
    {
        m_bodyAnimController.SetTrigger("Attack");

        Collider2D hit = Physics2D.OverlapCircle(transform.position, m_meleeRange, m_attackMask);
        if (hit != null)
        {
            hit.transform.SendMessage("OnHit", (hit.transform.position - transform.position).magnitude);
        }
    }

    void AttackRanged(Vector2 direction)
    {
        Projectile proj = Instantiate(m_projectilePrefab, transform.position, Quaternion.identity) as Projectile;
        if (proj != null)
        {
            proj.Range = m_rangedRange;
            proj.Speed = m_rangedSpeed;
            proj.Damage = m_rangedDamage;

            proj.Fire(direction);
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
        m_health -= damage;

        EventAggregator.SendMessage(new PlayerHealthChangedEvent()
        {
            NewData = m_health,
            OldData = m_health + damage,
            Normalised = m_health / m_startHealth
        });
    }

    public void SetStartHealth(float health)
    {
        m_startHealth = m_health = health;

        EventAggregator.SendMessage(new PlayerHealthChangedEvent() { NewData = m_health, Normalised = 1.0f });
    }
}
