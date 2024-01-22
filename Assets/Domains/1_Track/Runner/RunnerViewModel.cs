using Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class RunnerViewModel
    : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float idleSpeed = 1;
    [SerializeField] float walkSpeed = 2;
    [SerializeField] Rigidbody body ;
    [SerializeField] Collider self ;

    private GameStateData gameState;
    private Collider otherCollider;
    private bool isColliding;
    private bool endGame;
    private readonly float fallForce = 5f;

    public event Action<Collider> OnCollision;
    public Collider Collider => self;

    [Inject]
    public void Construct(GameStateData gameState)
    {
        this.gameState = gameState;
    }

    public void Walk(bool walk)
    {
        if (this == null || animator == null)
        {
            return;
        }
        animator.SetBool("Walk", walk);
        animator.speed = walk ? walkSpeed : idleSpeed;
    }

    internal void Fall()
    {
        endGame = true;
        body.useGravity = true;
        self.isTrigger = false;
    }

    void OnTriggerEnter(Collider other)
    {
        OnCollision?.Invoke(other);
        this.otherCollider = other;
        isColliding = true;
    }

    void OnTriggerExit(Collider other)
    {
        isColliding = false;
    }

    private void FixedUpdate()
    {
        if (isColliding) 
        {
            OnCollision?.Invoke(otherCollider);
        }

        if (endGame)
        {
            body.velocity = new Vector2(0, 0f); // Reset vertical velocity before applying force
            body.AddForce(Vector2.right * fallForce, ForceMode.Impulse);
        }
    }

}