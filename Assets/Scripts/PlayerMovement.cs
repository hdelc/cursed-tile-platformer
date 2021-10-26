using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float RunSpeed { get => runSpeed; set => runSpeed = value; }
    private float runSpeed = 5f;

    public float HorizontalAcceleration { get => horizontalAcceleration; set => horizontalAcceleration = value; }
    private float horizontalAcceleration = 20f;

    public float Gravity { get => gravity; set => gravity = value; }
    private float gravity = -6f;
    public float TerminalVelocity { get; set; }
    private float terminalVelocity = -6f;

    public int JumpDuration { get; set; }
    private int jumpDuration = 7;

    public float JumpSpeed { get; set; }
    private float jumpSpeed = 7f;

    public int DashDuration { get; set; }
    private int dashDuration = 5;

    public float DashSpeed { get; set; }
    private float dashSpeed = 15f;

    private bool onGround = false;

    public PlayerMovementState MovementState { get; private set; }

    // Can only be 1/-1 for right/left respectively
    public int FacingDirection 
    { 
        get => facingDirection;
        set
        {
            if (value > 0) facingDirection = 1;
            if (value < 0) facingDirection = -1;
        }
    }
    private int facingDirection = 1;

    private Rigidbody2D rb2d;
    private BoxCollider2D hitbox;

    #region tempInputBindSystem
    private InputManager inputManager;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        hitbox = this.GetComponent<BoxCollider2D>();
        inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Debug.Log(inputManager.HorizontalAxis);



        // Input acceleration
        float horizontalDeltaV = inputManager.HorizontalAxis * HorizontalAcceleration * Time.fixedDeltaTime;

        // Apply drag force
        if (inputManager.HorizontalAxis * rb2d.velocity.x <= 0)
            horizontalDeltaV += -Mathf.Sign(rb2d.velocity.x) * Mathf.Min(HorizontalAcceleration * Time.fixedDeltaTime, Mathf.Abs(rb2d.velocity.x));

        float verticalDeltaV = Gravity * Time.fixedDeltaTime;

        Vector2 newVelocity = rb2d.velocity + new Vector2(horizontalDeltaV, verticalDeltaV);

        // Cap speed
        newVelocity.x = Mathf.Clamp(newVelocity.x, -RunSpeed, RunSpeed);
        newVelocity.y = Mathf.Max(terminalVelocity, newVelocity.y);

        switch (MovementState)
        {
            case PlayerMovementState.JUMP:

                break;

            default:
                break;
        }

        
        Debug.Log(newVelocity);
        rb2d.velocity = newVelocity;
    }

    private void UpdateOnGround()
    {
        RaycastHit2D boxCast = Physics2D.BoxCast(hitbox.transform.position, hitbox.transform.lossyScale, 0, Vector2.down, 0.1f, 1);
        onGround = (boxCast.collider != null);
    }

    private void UpdatePlayerState()
    {

    }
}

public enum PlayerMovementState
{
    NONE,
    JUMP,
    DASH,
    WALLSLIDE
}

public class BufferedAction
{
    public InputBind ActionBind { get; private set; }
    public int FrameBuffer { get => buffer; private set => buffer = value; }
    private int buffer;
    public int FramesSinceActuation;
    private int actuationFrame = 0;

    public BufferedAction(InputBind binding, int frameBuffer = 4)
    {
        ActionBind = binding;
        FrameBuffer = frameBuffer;
    }

    public 
}