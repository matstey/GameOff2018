using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierPickup : MonoBehaviour {

    [SerializeField]
    PlayerModifier m_modifier;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision != null)
        {
            Destroy(gameObject);
            collision.transform.SendMessage("AddModifier", m_modifier);
        }
    }
}
