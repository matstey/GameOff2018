using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangedEvent<T>
{
    public T NewData { get; set; }
    public T OldData { get; set; }
}
