using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEventAggregator;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour, IListener<PlayerHealthChangedEvent>
{
    Image m_image;

    void Awake()
    {
        m_image = GetComponent<Image>();
        EventAggregator.Register<PlayerHealthChangedEvent>(this);
    }

    public void Handle(PlayerHealthChangedEvent message)
    {
        m_image.fillAmount = message.Normalised;
    }
}
