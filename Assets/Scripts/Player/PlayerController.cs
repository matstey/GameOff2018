using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventAggregator;

public class PlayerController : MonoBehaviour, IHasAttack, IAttackable
{
    [SerializeField]
    PlayerStats m_playerStats = new PlayerStats();

    [SerializeField]
    float m_maxWalkAnimSpeed = 0.5f;

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

    int m_health = 0;
    int m_maxHealth = 0;

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
            else if (ranged && currentTime > m_lastFireTime + m_playerStats.RangedDelay)
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

        m_rigidbody.velocity = move * m_playerStats.MaxSpeed;

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

        Collider2D hit = Physics2D.OverlapCircle(transform.position, m_playerStats.MeleeRange, m_attackMask);
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
            proj.Range = m_playerStats.RangedRange;
            proj.Speed = m_playerStats.RangedSpeed;
            proj.Damage = m_playerStats.RangedDamage;

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
        //Do we have varying damage on the player? I like the idea of 1 "hit" being one health but this may need to be revisited
        UpdateHealth(0, -1);
    }

    public void SetStartHealth(int health)
    {
        UpdateHealth(health, health);
    }

    void UpdateHealth(int changeMax, int changeCurrent)
    {
        if(changeMax != 0 || changeCurrent != 0)
        {
            int oldCurrent = m_health;
            int oldMax = m_maxHealth;

            m_health += changeCurrent;
            m_maxHealth += changeMax;

            EventAggregator.SendMessage(new PlayerHealthChangedEvent()
            {
                NewData = new PlayerHealthState() { Current = m_health, Max = m_maxHealth },
                OldData = new PlayerHealthState() { Current = oldCurrent, Max = oldMax }
            });

        }
        
    }
}
