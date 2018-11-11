using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour {

    [System.Serializable]
    public class MenuSelection
    {
        public RectTransform Transform;
        public UnityEvent Event;
    }

    [SerializeField]
    List<MenuSelection> m_menuSelections;

    [SerializeField]
    RectTransform m_indicator;

    [SerializeField]
    float m_menuMoveTimeout = 0.4f;

    int m_index = 0;
    float m_lastMoveTime;
    bool canMove = true;

	// Use this for initialization
	void Start (){
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float currTime = Time.time;

        float move = Input.GetAxis("Vertical");
        if (move == 0 || currTime >= m_lastMoveTime + m_menuMoveTimeout)
        {
            canMove = true;
        }

        if (canMove && move != 0)
        {
            if (move < 0 && m_index < m_menuSelections.Count-1)
            {
                m_index++;
                Moved(currTime);
            }

            else if(move > 0 && m_index > 0)
            {
                m_index--;
                Moved(currTime);
            }
        }

        Vector3 newIndicatorPos = m_indicator.localPosition;
        newIndicatorPos.y = m_menuSelections[m_index].Transform.localPosition.y;
        m_indicator.localPosition = newIndicatorPos;

        if (Input.GetButtonDown("Submit"))
        {
            m_menuSelections[m_index].Event.Invoke();
        }
	}

    private void Moved(float time)
    {
        canMove = false;
        m_lastMoveTime = time;
    }
}
