using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiveringGuyControl : MonoBehaviour
{
    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = this.GetComponent<Animator>();
    }

    public void Warm()
    {
        m_Animator.SetTrigger("warm");
    }
}
