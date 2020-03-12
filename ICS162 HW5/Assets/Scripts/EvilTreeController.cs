using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilTreeController : MonoBehaviour, IDamageable
{
    public int health = 3;

    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = this.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Damage()
    {
        if(health == 0)
        {
            return;
        }

        health -= 1;

        if(health > 0)
        {
            m_Animator.SetTrigger("hit");
        }
        else
        {
            m_Animator.SetTrigger("die");
        }
    }

    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
