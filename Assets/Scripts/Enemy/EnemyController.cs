using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IHasAttack, IAttackable
{
    enum AIState
    {
        Roaming,
        Seeking,
        Attacking,
    }

    class AIResult
    {
        public Vector2 Direction { get; set; }
        public Vector2 Velocity { get; set; }
    }

    [SerializeField]
    float m_maxSpeed = 10.0f;

    [SerializeField]
    float m_roamingSpeed = 2.0f;

    [SerializeField]
    string m_targetTag = "Player";

    [SerializeField]
    float m_attackDistance = 2;

    [SerializeField]
    float m_stopDistance = 1.8f;


    public bool Attacking { get; set; } = false;

    public event System.Action<EnemyController> Died;

    IEnemyAnimationManager m_animationManager;
    Rigidbody2D m_rigidbody;
    GameObject m_target;
    AIState m_aiState = AIState.Roaming;
    Vector2 m_currentWaypoint;
    bool m_wayPointValid = false;

    // Use this for initialization
    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animationManager = GetComponent<IEnemyAnimationManager>();
    }

    void FixedUpdate()
    {
        AIResult result = null;
        switch(m_aiState)
        {
            case AIState.Roaming:
                result = UpdateRoaming();
                break;
            case AIState.Seeking:
                result = UpdateSeeking();
                break;
            case AIState.Attacking:
                result = UpdateAttacking();
                break;
        }

        UpdateMovement(result.Direction, result.Velocity);        
    }

    Vector2 GenerateWaypoint()
    {
        float radians = Random.Range(-Mathf.PI, Mathf.PI);

        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
        float distance = Random.Range(02.0f, 10.0f);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, 1 << LayerMask.NameToLayer("Map"));

        if(hit.collider == null)
        {
            return (Vector2)transform.position + (direction * distance);
        }
        else
        {
            return hit.point + (direction * -1.0f);
        }
    }

    bool IsAtPosition(Vector2 position)
    {
        return Vector2.Distance(transform.position, position) < 1.0f;
    }

    AIResult UpdateRoaming()
    {
        if(IsAtPosition(m_currentWaypoint) || !m_wayPointValid)
        {
            m_currentWaypoint = GenerateWaypoint();
            m_wayPointValid = true;
        }

        Vector2 wayPointToEnemy = m_currentWaypoint - (Vector2)transform.position;
        float distance = wayPointToEnemy.magnitude;
        Vector2 direction = wayPointToEnemy.normalized;

        Vector2 velocity = direction * m_roamingSpeed;

        if (m_target != null)
        {
            m_aiState = AIState.Seeking;
            m_wayPointValid = false;
        }

        return new AIResult()
        {
            Velocity = velocity,
            Direction = direction
        };
    }

    AIResult UpdateSeeking()
    {
        if (m_target == null)
        {
            m_aiState = AIState.Roaming;
            return new AIResult()
            {
                Velocity = Vector2.zero,
                Direction = Vector2.right
            };
        }

        Vector2 playerToEnemy = m_target.transform.position - transform.position;
        float distance = playerToEnemy.magnitude;
        Vector2 direction = playerToEnemy.normalized;

        Vector2 velocity = direction * m_maxSpeed;

        bool withinAttackDistance = distance < m_attackDistance;

        if (withinAttackDistance)
        {
            m_aiState = AIState.Attacking;
        }

        return new AIResult()
        {
            Velocity = velocity,
            Direction = playerToEnemy.normalized
        };
    }

    AIResult UpdateAttacking()
    {
        if (m_target == null)
        {
            m_aiState = AIState.Roaming;
            return new AIResult()
            {
                Velocity = Vector2.zero,
                Direction = Vector2.right
            };
        }

        Vector2 playerToEnemy = m_target.transform.position - transform.position;
        float distance = playerToEnemy.magnitude;
        Vector2 direction = playerToEnemy.normalized;

        bool stop = distance <= m_stopDistance;
        Vector2 velocity = stop ? Vector2.zero : direction * m_maxSpeed;

        bool withinAttackDistance = distance < m_attackDistance;

        if (withinAttackDistance && !Attacking)
        {
            Attack(m_target);
        }
        else if(!withinAttackDistance)
        {
            m_aiState = AIState.Seeking;
        }

        return new AIResult()
        {
            Velocity = velocity,
            Direction = direction
        };
    }

    void UpdateMovement(Vector2 direction, Vector2 velocity)
    {
        m_rigidbody.velocity = velocity;
        m_animationManager.Move(velocity);
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        Stop();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        m_wayPointValid = false;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        m_wayPointValid = false;
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

    void Attack(GameObject target)
    {
        m_animationManager.Attack();
        target.SendMessage("OnHit", 10);
    }


    void Stop()
    {
        m_rigidbody.velocity = Vector2.zero;
        m_animationManager.Move(Vector2.zero);
    }

    public void OnHit(float damage)
    {
        m_animationManager.Hit();

        // TODO: Add health
        Died?.Invoke(this);
    }

    void OnDrawGizmos()
    {
        if (m_wayPointValid)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(m_currentWaypoint, 0.1f);
        }
    }
}
