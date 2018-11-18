using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthHeart : MonoBehaviour {

    [SerializeField]
    Sprite m_full;

    [SerializeField]
    Sprite m_empty;

    Image m_image;

    public bool Active {
        set
        {
            m_image.sprite = value ? m_full : m_empty;
        }
    }

	// Use this for initialization
	void Awake () {
        m_image = GetComponent<Image>();
	}
        
}
