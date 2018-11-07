using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChangedEvent : ChangedEvent<LevelData>
{
    public LevelMap Map { get; set; }
}
