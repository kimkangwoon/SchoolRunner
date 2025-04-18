﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float forwardSpeed = 15.0f;
    private Rigidbody rig;
    private Vector3 targetPosition;
    private float[] lanes = { -3.5f, 0, 3.5f };
    private int currentLane = 1;

    private Vector2 touchStartPos;
    private bool isMove = false;
    private bool isDrag = false;
    private bool isJump = false;
    private bool isMoveOnce = false;

    private int currentHP;
    private int maxHP = 3;
    private bool isInit = false;
    private bool isGameStart = false;

    public delegate void ChangeHP(int hp);
    public event ChangeHP OnChangeHP;
    public int CurrentHP
    {
        get => currentHP;
        set
        {
            currentHP = value;
            OnChangeHP?.Invoke(currentHP);
        }
    }
    private void Awake()
    {
        TryGetComponent<Rigidbody>(out rig);
        CurrentHP = maxHP;
        moveSpeed = 12.0f;
        jumpForce = 25.0f;
    }
    private void Update()
    {
        if (isGameStart)
        {
            transform.position += Vector3.forward * (forwardSpeed * Time.deltaTime);
        }
        if (isInit)
        {
            MoveSide();
            Jump();
        } 
    }
    public void InitPlayer()
    {
        CurrentHP = maxHP;
        Debug.Log("intialize");
        GameStart();
    }
    private void MoveSide()
    {
        if (Input.GetMouseButtonDown(0) && !isMove)
        {
            touchStartPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && isDrag)
        {
            Vector3 touchEndPos = Input.mousePosition;

            float deltaX = touchEndPos.x - touchStartPos.x;

            if (Mathf.Abs(deltaX) > Screen.width * 0.1f && !isMove)
            {
                if (deltaX > 0 && currentLane < 2)
                {
                    // if player is jumping, can move once
                    if (!isJump || (isJump && !isMoveOnce))
                    { 
                        currentLane++;
                        if (isJump)
                            isMoveOnce = true;
                    }
                }
                else if (deltaX < 0 && currentLane > 0)
                {
                    if(!isJump || (isJump && !isMoveOnce))
                    {
                        currentLane--;
                        if (isJump)
                            isMoveOnce = true;
                    }
                }

                isDrag = false;
                isMove = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
        }


        targetPosition = new Vector3(lanes[currentLane], transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, (moveSpeed * Time.deltaTime));

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMove = false;
        }
    }
    public void Jump()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            isDrag = true;
        }

        if (Input.GetMouseButton(0) && isDrag)
        {
            Vector3 touchEndPos = Input.mousePosition;

            float deltaY = touchEndPos.y - touchStartPos.y;

            if (Mathf.Abs(deltaY) > Screen.height * 0.1f)
            {
                if (!isJump)
                {
                    rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    isJump = true;
                }

                isDrag = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
        }
    }
    private void GameStart()
    {
        isGameStart = true;
        StartCoroutine(SetInit());
    }
    private IEnumerator SetInit()
    {
        yield return new WaitForSeconds(2.0f);
        isGameStart = false;
        isInit = true;
        transform.position = new Vector3(0.0f, 2.0f, 0.0f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
            isMoveOnce = false;
        }
    }
    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            // game over
            Debug.Log("게임 종료");
        }
    }
}
