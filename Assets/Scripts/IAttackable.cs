﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    void OnHit(int damage, Vector2 dir);
}
