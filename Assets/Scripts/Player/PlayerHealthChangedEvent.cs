using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthChangedEvent : ChangedEvent<float>
{
    public float Normalised { get; set; }
}
