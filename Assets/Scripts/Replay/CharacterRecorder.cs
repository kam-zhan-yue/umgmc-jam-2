using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;

public class CharacterRecorder : MonoBehaviour
{
    private Character _character;
    private CharacterHorizontalMovement _characterHorizontalMovement;
    private CharacterJump _characterJump;
    private CharacterRun _characterRun;
    private CharacterWallClinging _characterWallClinging;
    private CharacterWalljump _characterWallJump;
    private Recorder _recorder;

    private void Awake()
    {
        _character = GetComponent<Character>();
        _characterHorizontalMovement = GetComponent<CharacterHorizontalMovement>();
        _characterJump = GetComponent<CharacterJump>();
        _characterRun = GetComponent<CharacterRun>();
        _characterWallClinging = GetComponent<CharacterWallClinging>();
        _characterWallJump = GetComponent<CharacterWalljump>();
        _recorder = GetComponent<Recorder>();
    }
    
    private void LateUpdate()
    {
        bool alive = _character.ConditionState.CurrentState != CharacterStates.CharacterConditions.Dead;
        bool idle = _character.MovementState.CurrentState == CharacterStates.MovementStates.Idle;
        ReplayData data = new PlayerReplayData(transform.position, alive, idle, _character.Grounded, _character.IsFacingRight,
            _characterHorizontalMovement.HorizontalSpeed, _characterHorizontalMovement.Walking,
            _characterJump.Jumping, _characterJump.DoubleJumping, _characterJump.HitTheGround, _characterJump.NumberOfJumpsLeft,
            _characterRun.Running, _characterWallClinging.IsWallClinging, _characterWallJump.WallJumping);
        _recorder.RecordFrame(data);
    }
}
