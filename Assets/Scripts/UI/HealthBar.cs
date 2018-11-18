using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEventAggregator;

public class HealthBar : MonoBehaviour, IListener<PlayerHealthChangedEvent>
{
    [SerializeField]
    HealthHeart m_heartPrefab;

    [SerializeField]
    int m_heartsPerRow = 6;

    List<HealthHeart> m_hearts = new List<HealthHeart>();

    void Awake()
    {
        EventAggregator.Register<PlayerHealthChangedEvent>(this);
    }

    public void Handle(PlayerHealthChangedEvent message)
    {
        if (message.NewData != null)
        {
            int newMax = message.NewData.Max;
            if (newMax != m_hearts.Count)
            {
                while (newMax > m_hearts.Count)
                {
                    HealthHeart healthHeart = Instantiate(m_heartPrefab);
                    m_hearts.Add(healthHeart);

                    int index = m_hearts.Count - 1;
                    int x = index % m_heartsPerRow;
                    int y = index / m_heartsPerRow;

                    healthHeart.transform.SetParent(transform);
                    healthHeart.transform.localPosition = new Vector3(x * 32.0f, y * -32.0f, 0);
                    healthHeart.transform.localScale = Vector3.one;
                }

                if(newMax < m_hearts.Count)
                {
                    m_hearts.RemoveRange(newMax - 1, m_hearts.Count - newMax);
                }
            }

            int newCurrent = message.NewData.Current;
            for(int i = 0; i <= m_hearts.Count - 1; i++)
            {
                m_hearts[i].Active = i < newCurrent;
            }
        }
    }
}
