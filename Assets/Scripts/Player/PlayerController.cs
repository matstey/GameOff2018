using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEventAggregator;

[RequireComponent(typeof(PlayerAnimationManager))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IHasAttack, IAttackable
{
    [SerializeField]
    PlayerStats m_playerStats = new PlayerStats();

    [SerializeField]
    LayerMask m_attackMask;

    [SerializeField]
    Projectile m_projectilePrefab;

    bool m_attacking = false;
    public bool Attacking
    {
        get
        {
            return m_attacking;
        }
        set
        {
            m_attacking = value;

            if(m_animationManager)
            {
                m_animationManager.Attacking = value;
            }
        }
    }

    Rigidbody2D m_rigidbody;
    PlayerAnimationManager m_animationManager;

    int m_health = 0;
    int m_maxHealth = 0;

    float m_lastFireTime = 0;
    Vector2 m_knockback = new Vector2(0, 0);
    Vector2 m_knockbackVelocity = new Vector2();

    float m_invincibilityTime = 0.5f;
    float m_hitTime = 0;

    public bool IsAlive { get { return m_health > 0.0f; } }

    // Use this for initialization
    private void Awake () {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animationManager = GetComponent<PlayerAnimationManager>();
    }
	
	// Update is called once per frame
	private void Update ()
    {
        float rangedAttackX = Input.GetAxis("FireHorizontal");
        float rangedAttackY = Input.GetAxis("FireVertical");

        bool ranged = rangedAttackX != 0 || rangedAttackY != 0;
        bool melee = Mathf.Abs(Input.GetAxis("Melee")) > 0.1f;

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

        m_animationManager.Move(move);

        Vector2 newVelocity = move * m_playerStats.MaxSpeed;
        newVelocity += m_knockback;

        m_rigidbody.velocity = newVelocity;

        m_knockback = Vector2.SmoothDamp(m_knockback, Vector2.zero, ref m_knockbackVelocity, 0.2f);
    }

    void Attack()
    {
        if (m_playerStats.HasMelee)
        {
            m_animationManager.AttackMelee();

            Collider2D hit = Physics2D.OverlapCircle(transform.position, m_playerStats.MeleeRange, m_attackMask);
            if (hit != null)
            {
                IAttackable t = hit.GetComponent<IAttackable>();
                if (t != null)
                {
                    Vector2 dir = hit.transform.position - transform.position;
                    t.OnHit(m_playerStats.MeleeDamage, dir);
                }
            }
        }
    }

    void AttackRanged(Vector2 direction)
    {
        if(m_playerStats.HasRanged)
        {
            m_animationManager.AttackRanged();

            Projectile proj = Instantiate(m_projectilePrefab, transform.position, Quaternion.identity) as Projectile;
            if (proj != null)
            {
                proj.Range = m_playerStats.RangedRange;
                proj.Speed = m_playerStats.RangedSpeed;
                proj.Damage = m_playerStats.RangedDamage;

                proj.Fire(direction);
            }
        }
    }

    public void OnHit(int damage, Vector2 direction)
    {
        if(m_hitTime == 0 || Time.time - m_hitTime > m_invincibilityTime)
        {
            m_hitTime = Time.time;

            //Do we have varying damage on the player? I like the idea of 1 "hit" being one health but this may need to be revisited
            UpdateHealth(0, -damage);
            m_knockback = direction * 5;
            m_animationManager.Hit(m_invincibilityTime);
        }
    }

    public void AddModifier(PlayerModifier modifier)
    {
        if (modifier != null)
        {
            if (modifier.StatModifiers != null)
            {
                PlayerStats statModifier = modifier.StatModifiers;

                //Health is treat differently, do we need a more generic way of doing this? Events for each?
                UpdateHealth(statModifier.MaxHealth, statModifier.MaxHealth);

                m_playerStats.MaxSpeed += statModifier.MaxSpeed;
                m_playerStats.RangedDelay += statModifier.RangedDelay;
                m_playerStats.RangedDamage += statModifier.RangedDamage;
                m_playerStats.RangedRange += statModifier.RangedRange;
                m_playerStats.RangedSpeed += statModifier.RangedSpeed;
                m_playerStats.MeleeRange += statModifier.MeleeRange;
                m_playerStats.MeleeDamage += statModifier.MeleeDamage;
                m_playerStats.HasMelee &= statModifier.HasMelee;
                m_playerStats.HasRanged &= statModifier.HasRanged;
            }

            if (modifier.ReplacementAnimator != null)
            {
                Attacking = false;
                m_animationManager.ReplaceController(modifier.ReplacementAnimator, modifier.Type);
            }
        }
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
