using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthChangedEvent : ChangedEvent<PlayerHealthState>
{
    public PlayerHealthState HealthState { get; set; }
}
