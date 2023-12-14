using Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class RaccoonViewModel : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float idleSpeed = 1;
    [SerializeField] float walkSpeed = 2;
    [SerializeField] Rigidbody body ;
    [SerializeField] Collider self ;

    private GameStateData gameState;

    public event Action<Collider> OnCollision;

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
        body.useGravity = true;
        self.isTrigger = false;
    }

    void OnTriggerEnter(Collider other)
    {
        OnCollision?.Invoke(other);
    }
}