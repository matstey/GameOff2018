using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour {

    public bool Attacking { get; set; } = false;

    [SerializeField]
    Animator m_headAnimController;

    [SerializeField]
    Animator m_bodyAnimController;

    [SerializeField]
    Animator m_legsAnimController;

    [SerializeField]
    float m_maxWalkAnimSpeed = 0.5f;

    [SerializeField]
    float m_idleAnimSpeed = 0.2f;

    [SerializeField]
    Color m_hitColor = Color.red;

    SpriteRenderer[] m_renderers;

    public void AttackMelee(PlayerModifier.BodyPart attackPart)
    {
        switch(attackPart)
        {
            case PlayerModifier.BodyPart.Body:
                m_bodyAnimController.SetTrigger("MeleeAttack");
                break;
            case PlayerModifier.BodyPart.Head:
                m_headAnimController.SetTrigger("MeleeAttack");
                break;
        }

        m_legsAnimController.SetTrigger("MeleeAttack");
    }

    public void AttackRanged(PlayerModifier.BodyPart attackPart)
    {
        switch (attackPart)
        {
            case PlayerModifier.BodyPart.Body:
                m_bodyAnimController.SetTrigger("RangedAttack");
                break;
            case PlayerModifier.BodyPart.Head:
                m_headAnimController.SetTrigger("RangedAttack");
                break;
        }

        m_legsAnimController.SetTrigger("RangedAttack");
    }

    public void Move(Vector2 move)
    {
        float animSpeed = move.magnitude * m_maxWalkAnimSpeed;
        bool running = animSpeed > 0.01;

        m_bodyAnimController.SetBool("Running", running);
        m_headAnimController.SetBool("Running", running);
        m_legsAnimController.SetBool("Running", running);

        if (!running)
        {
            animSpeed = m_idleAnimSpeed;
        }

        m_legsAnimController.speed = animSpeed;
        m_headAnimController.speed = animSpeed;
        m_bodyAnimController.speed = Attacking ? 1.0f : animSpeed;



        if (move.x != 0)
        {
            SetDirection(move.x < 0);
        }
    }

    public void Hit(float invincibilityTime)
    {
        StartCoroutine(Flash(invincibilityTime, m_hitColor));
    }

    public void ReplaceController(RuntimeAnimatorController controller, PlayerModifier.BodyPart type)
    {
        Animator anim = null;
        switch (type)
        {
            case PlayerModifier.BodyPart.Head:
                anim = m_headAnimController;
                break;
            case PlayerModifier.BodyPart.Body:
                anim = m_bodyAnimController;
                break;
            case PlayerModifier.BodyPart.Legs:
                anim = m_legsAnimController;
                break;
        }

        if (anim != null)
        {
            anim.runtimeAnimatorController = controller;
        }
    }

    void SetSpriteColor(Color col)
    {
        foreach (SpriteRenderer r in m_renderers)
        {
            r.color = col;
        }
    }

    public void SetChargeAmount(float amount)
    {
        SetSpriteColor(Color.Lerp(Color.white, Color.red, amount));
    }


    IEnumerator Flash(float duration, Color color)
    {
        float m_startTime = Time.time;

        while (Time.time - m_startTime < duration)
        {
            SetSpriteColor(color);
            yield return new WaitForSeconds(0.1f);
            SetSpriteColor(Color.white);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void SetDirection(bool left)
    {
        foreach (SpriteRenderer r in m_renderers)
        {
            r.flipX = left;
        }
    }

    private void Awake()
    {
        m_renderers = GetComponentsInChildren<SpriteRenderer>();
    }
}
