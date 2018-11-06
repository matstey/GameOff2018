using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerExtensions
{    public static bool Contains(this LayerMask layerMask, int layer)
    {
        return layerMask == (layerMask | (1 << layer));
    }
}
