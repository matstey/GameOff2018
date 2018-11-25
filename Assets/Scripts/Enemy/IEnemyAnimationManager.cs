using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAnimationManager
{
    void Attack();
    void Move(Vector2 dir);
    void Hit();
    void Dead();

    bool Attacking { get; set; }
}