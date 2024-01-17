using UnityEngine;

public class PlayerReplayData : ReplayData
{
    public bool Alive { get; }
    public bool Idle { get; }
    public bool Grounded { get; }
    public bool IsFacingRight { get; }
    public float HorizontalSpeed { get; }
    public bool Walking { get; }
    public bool Jumping { get; }
    public bool DoubleJumping { get; }
    public bool HitTheGround { get; }
    public int NumberOfJumpsLeft { get; }
    public bool Running { get; }
    public bool WallClinging { get; }
    public bool WallJumping { get; }

    public PlayerReplayData(Vector3 pos, bool alive, bool idle, bool grounded, bool isFacingRight, float horizontalSpeed, bool walking,
        bool jumping, bool doubleJumping, bool hitTheGround, int numberOfJumpsLeft, bool running, bool wallClinging, bool wallJumping)
    {
        position = pos;
        Alive = alive;
        Idle = idle;
        Grounded = grounded;
        IsFacingRight = isFacingRight;
        HorizontalSpeed = horizontalSpeed;
        Walking = walking;
        Jumping = jumping;
        DoubleJumping = doubleJumping;
        HitTheGround = hitTheGround;
        NumberOfJumpsLeft = numberOfJumpsLeft;
        Running = running;
        WallClinging = wallClinging;
        WallJumping = wallJumping;
    }
}
