using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerReplayObject : ReplayObject
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private static readonly int Alive = Animator.StringToHash("Alive");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int DoubleJumping = Animator.StringToHash("DoubleJumping");
    private static readonly int HitTheGround = Animator.StringToHash("HitTheGround");
    private static readonly int NumberOfJumpsLeft = Animator.StringToHash("NumberOfJumpsLeft");
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int WallJumping = Animator.StringToHash("WallJumping");
    private static readonly int WallClinging = Animator.StringToHash("WallClinging");
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public override void SetDataForFrame(ReplayData data)
    {
        PlayerReplayData playerReplayData = (PlayerReplayData)data;
        transform.position = playerReplayData.position;
        _spriteRenderer.flipX = !playerReplayData.IsFacingRight;
        _animator.SetBool(Alive, playerReplayData.Alive);
        _animator.SetBool(Idle, playerReplayData.Idle);
        _animator.SetBool(Grounded, playerReplayData.Grounded);
        _animator.SetFloat(Speed, Mathf.Abs(playerReplayData.HorizontalSpeed));
        _animator.SetBool(Walking, playerReplayData.Walking);
        _animator.SetBool(Jumping, playerReplayData.Jumping);
        _animator.SetBool(DoubleJumping, playerReplayData.DoubleJumping);
        _animator.SetBool(HitTheGround, playerReplayData.HitTheGround);
        _animator.SetInteger(NumberOfJumpsLeft, playerReplayData.NumberOfJumpsLeft);
        _animator.SetBool(Running, playerReplayData.Running);
        _animator.SetBool(WallClinging, playerReplayData.WallClinging);
        _animator.SetBool(WallJumping, playerReplayData.WallJumping);
    }
}
