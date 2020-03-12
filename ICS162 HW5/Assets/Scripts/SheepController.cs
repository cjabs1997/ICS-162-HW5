using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SheepController : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float aggroRange = 5f;
    public float moveSpeed = 3f;
    public float rotMultiplier = 250f;

    private Animator m_Animator;
    private CharacterController m_CharacterController;
    private Transform playerTrans;
    private bool canMove = true;
    private Coroutine delay;


    private void Awake()
    {
        m_Animator = this.GetComponentInChildren<Animator>();
        m_CharacterController = this.GetComponent<CharacterController>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        HandleMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            delay = StartCoroutine(DelayRoutine());
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDir = Vector3.zero;

        if(Vector3.Distance(this.transform.position, playerTrans.position) <= aggroRange && canMove)
        {
            moveDir = (playerTrans.position - this.transform.position).normalized * moveSpeed;
            m_CharacterController.Move(moveDir * Time.deltaTime);
            m_Animator.SetInteger("moving", (int)moveDir.magnitude);
            HandleRotation();
        }

        //m_CharacterController.Move(moveDir * Time.deltaTime);
        //m_Animator.SetInteger("moving", (int)moveDir.magnitude);
    }

    private void HandleRotation()
    {
        this.transform.LookAt(playerTrans, Vector3.up);
    }


    private IEnumerator DelayRoutine()
    {
        yield return new WaitForSeconds(0.80f);

        if(canMove)
            SceneManager.LoadScene("MrScene");
    }

    public void Damage()
    {
        if (!canMove)
            return;

        canMove = false;

        if (delay != null)
            StopCoroutine(delay);

        this.GetComponent<Collider>().enabled = false;
        m_CharacterController.enabled = false;
        Rigidbody r = this.GetComponent<Rigidbody>();
        r.Sleep();
        r.velocity = Vector3.zero;
        this.GetComponent<Collider>().enabled = false;

        m_Animator.SetTrigger("die");
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
