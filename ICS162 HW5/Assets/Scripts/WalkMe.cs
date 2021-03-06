﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMe : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 3f;
    public float rotMultiplier = 250f;
    public GameObject campFire;
    public Collider attackCol;
    public UnityEngine.Events.UnityEvent madeFireEvent;


    private Animator m_Animator;
    private CharacterController m_CharacterController;
    private bool canPickUp = false;
    private bool canChop = true;
    private bool canMove = true;
    private GameObject pickUpTarget = null;
    private List<GameObject> collectibles = new List<GameObject>();



    private void Awake()
    {
        m_Animator = this.GetComponentInChildren<Animator>();
        m_CharacterController = this.GetComponent<CharacterController>();

        collectibles = new List<GameObject>(GameObject.FindGameObjectsWithTag("pickup"));
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        if (Input.GetKeyDown(KeyCode.E) && canPickUp && pickUpTarget != null)
        {
            canPickUp = false;
            //pickUpTarget.GetComponent<PickUp>();
            collectibles = new List<GameObject>(GameObject.FindGameObjectsWithTag("pickup"));

            if(pickUpTarget.gameObject.name.Contains("Fire") && collectibles.Count == 1)
            {
                m_Animator.SetTrigger("pickup");
                campFire.SetActive(true);
                madeFireEvent.Invoke();
            }
            else if(!pickUpTarget.name.Contains("Fire"))
            {
                m_Animator.SetTrigger("pickup");
            }
        }

        if(Input.GetKey(KeyCode.Q) && canChop)
        {
            m_Animator.SetTrigger("chop");
            canChop = false;
            canPickUp = false;
            canMove = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("pickup"))
        {
            canPickUp = true;
            pickUpTarget = collision.gameObject;
        }
    }

    private void OnTriggerLeave(Collider collision)
    {
        if (collision.CompareTag("pickup"))
        {
            canPickUp = false;
            pickUpTarget = null;
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDir = Vector3.zero;

        if (canMove)
        {
            moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
        }

        m_CharacterController.Move(moveDir * Time.deltaTime);
        m_Animator.SetInteger("moving", (int)moveDir.magnitude);
        HandleRotation();
    }

    private void HandleRotation()
    {
        // Rotation
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0f, 45f, 0f), 1f);
                //this.transform.eulerAngles = new Vector3(0f, 45f, 0f);
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(new Vector3(0f, 45f, 0f)), rotMultiplier * Time.deltaTime);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0f, -45f, 0f), 1f);
                //this.transform.eulerAngles = new Vector3(0f, -45f, 0f);
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(new Vector3(0f, -45f, 0f)), rotMultiplier * Time.deltaTime);
            }
            else
            {
                //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0f, 0f, 0f), 1f);
                //this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, 0f)), rotMultiplier * Time.deltaTime);
            }
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0f, 135f, 0f), 1f);
                //this.transform.eulerAngles = new Vector3(0f, 135f, 0f);
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(new Vector3(0f, 135f, 0f)), rotMultiplier * Time.deltaTime);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0f, 225f, 0f), 1f);
                //this.transform.eulerAngles = new Vector3(0f, 225f, 0f);
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(new Vector3(0f, 225f, 0f)), rotMultiplier * Time.deltaTime);
            }
            else
            {
                //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0f, 180f, 0f), 1f);
                //this.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(new Vector3(0f, 180f, 0f)), rotMultiplier * Time.deltaTime);
            }
        }
        else if (Input.GetAxisRaw("Horizontal") != 0)
        {
            //this.transform.eulerAngles = new Vector3(0f, 90f * Input.GetAxis("Horizontal"), 0f);
            //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0f, 90f * Input.GetAxis("Horizontal"), 0f), 1f);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(new Vector3(0f, 90f * Input.GetAxis("Horizontal"), 0f)), rotMultiplier * Time.deltaTime);
        }
    }


    #region AnimationEvents

    public void CanMove()
    {
        canMove = true;
    }

    public void CantMove()
    {
        canMove = false;
    }

    public void DeletePickup()
    {
        Destroy(pickUpTarget);
    }

    public void Chop()
    {
        Collider[] targets = Physics.OverlapBox(this.transform.position, attackCol.bounds.extents * 2f, attackCol.transform.rotation);
        
        foreach(Collider c in targets)
        {
            if(c.CompareTag("enemy"))
            {
                c.gameObject.GetComponent<IDamageable>().Damage();
            }
        }
    }

    public void FinishChop()
    {
        canMove = true;
        canChop = true;
    }

    #endregion
}
