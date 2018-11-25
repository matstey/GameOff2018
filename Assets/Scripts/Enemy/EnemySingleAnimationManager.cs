using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemySingleAnimationManager : MonoBehaviour, IEnemyAnimationManager {

    public bool Attacking { get; set; } = false;

    [SerializeField]
    float m_maxWalkAnimSpeed = 0.5f;

    [SerializeField]
    Animator m_animController;

    SpriteRenderer[] m_renderers;

    // Use this for initialization
    void Awake () {
        m_renderers = GetComponentsInChildren<SpriteRenderer>();
        m_animController.speed = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void Attack()
    {
        m_animController.SetTrigger("Attack");
    }

    public void Move(Vector2 move)
    {
        float animSpeed = move.magnitude * m_maxWalkAnimSpeed;

        m_animController.speed = Attacking ? 1.0f : animSpeed;

        if (animSpeed <= 0.01)
        {
            m_animController.Play("Run", -1, 0.0f);
        }

        if (move.x != 0)
        {
            SetDirection(move.x < 0);
        }
    }

    public void Hit()
    {
        StartCoroutine(Flash());
    }

    public void Dead()
    {
        m_animController.speed = 1.0f;
        m_animController.SetTrigger("Dead");
    }

    void SetSpriteColor(Color col)
    {
        foreach (SpriteRenderer r in m_renderers)
        {
            r.color = col;
        }
    }

    IEnumerator Flash()
    {
        SetSpriteColor(Color.red);
        yield return new WaitForSeconds(0.15f);
        SetSpriteColor(Color.white);
    }

    void SetDirection(bool left)
    {
        foreach (SpriteRenderer r in m_renderers)
        {
            r.flipX = left;
        }
    }
}
