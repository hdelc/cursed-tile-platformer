using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public float RunSpeed { get => runSpeed; set => runSpeed = value; }
  [SerializeField] private float runSpeed = 5f;

  public float HorizontalAcceleration { get => horizontalAcceleration; set => horizontalAcceleration = value; }
  [SerializeField] private float horizontalAcceleration = 20f;

  public float Gravity { get => gravity; set => gravity = value; }
  [SerializeField] float gravity = -6f;
  public float TerminalVelocity { get; set; }
  [SerializeField] float terminalVelocity = -6f;

  [SerializeField] float jumpEnergy = 18f;

  [SerializeField] float jumpDecrease = 0.8f;

  [SerializeField] int dashDuration = 5;

  [SerializeField] float dashSpeed = 15f;

  // [SerializeField] Vector2 dashColliderSize = new Vector2(0.8f, 0.8f);
  [SerializeField] float dashColliderScale = 0.9f;

  [SerializeField] bool onGround = false;

  public PlayerMovementState MovementState { get; private set; }

  public bool isFacingRight = true;

  [SerializeField] Rigidbody2D rb2d;
  [SerializeField] BoxCollider2D hitbox;
  [SerializeField] GameObject collisionObj;
  // [SerializeField] EdgeCollider2D bottomEdgeCollider;

#region tempInputBindSystem
  [SerializeField] InputManager inputManager;
#endregion

  private int jumpState = 0; // 0 - can jump, 1 - jump in progress, 2 - can't jump
  private float tempJumpEnergy;

  private int dashState = 0;
  private int tempDashLength;
  // private Vector2 defaultColliderSize;
  private Vector3 defaultColliderSize;

  // Start is called before the first frame update
  void Start()
  {
    rb2d = this.GetComponent<Rigidbody2D>();
    hitbox = this.GetComponent<BoxCollider2D>();
    // inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
    tempJumpEnergy = jumpEnergy;
    tempDashLength = dashDuration;
    defaultColliderSize = collisionObj.transform.localScale;
  }

  // Update is called once per frame
  void Update()
  {
    if (dashState != 1) {
      if (inputManager.HorizontalAxis > 0)
      {
        isFacingRight = true;
      } else if (inputManager.HorizontalAxis < 0)
      {
        isFacingRight = false;
      }
    }
  }

  private void FixedUpdate()
  {
    Vector2 newVelocity = new Vector2();
    if (dashState == 1)
    {
      // hitbox.size = dashColliderSize;
      collisionObj.transform.localScale = dashColliderScale * defaultColliderSize;
      newVelocity.y = 0;
      newVelocity.x = isFacingRight ? dashSpeed : -dashSpeed;
      if (--tempDashLength <= 0)
      {
        dashState = 2;
        // hitbox.size = defaultColliderSize;
        collisionObj.transform.localScale = defaultColliderSize;
      }
    } else
    {
      // Input acceleration
      float horizontalDeltaV = inputManager.HorizontalAxis * HorizontalAcceleration * Time.fixedDeltaTime;

      // Apply drag force
      if (inputManager.HorizontalAxis * rb2d.velocity.x <= 0)
        horizontalDeltaV += -Mathf.Sign(rb2d.velocity.x) * Mathf.Min(HorizontalAcceleration * Time.fixedDeltaTime, Mathf.Abs(rb2d.velocity.x));

      float verticalDeltaV = Gravity * Time.fixedDeltaTime;

      newVelocity = rb2d.velocity + new Vector2(horizontalDeltaV, verticalDeltaV);

      // Cap speed
      newVelocity.x = Mathf.Clamp(newVelocity.x, -RunSpeed, RunSpeed);
      newVelocity.y = Mathf.Max(terminalVelocity, newVelocity.y);

      // Debug.Log("jump");
      // Debug.Log(inputManager.Jump);

      if (inputManager.Jump)
      {
        if (jumpState == 0) 
        {
          jumpState = 1;
        }

        if (jumpState == 1)
        {
          Vector2 newPosition = new Vector2(rb2d.position.x, rb2d.position.y + tempJumpEnergy);
          
          LayerMask mask = LayerMask.GetMask("Walls");
          RaycastHit2D raycast = Physics2D.Linecast(rb2d.position, newPosition, mask);
          if (raycast.transform)
          {
            Debug.Log(newPosition.x);
            newPosition = raycast.point;
            float overlap = raycast.distance - (newPosition.y - rb2d.position.y) + 0.5f;
            Debug.Log(overlap);
            newPosition.y -= overlap;
            tempJumpEnergy = 0;
          }

          tempJumpEnergy = Mathf.Max(tempJumpEnergy * jumpDecrease, 0);
          if (tempJumpEnergy < 0.05)
          {
            tempJumpEnergy = 0;
          }
          if (tempJumpEnergy > 0) {
            newVelocity.y = 0f;
          }
          rb2d.position = newPosition;
        }
      } else {
        if (jumpState == 1)
        {
          if (tempJumpEnergy > 0) {
            newVelocity.y = 2f;
          }
          jumpState = 2;
        }
      }

      if (inputManager.Dash)
      {
        if (dashState == 0)
        {
          dashState = 1;
        }
      }
    }

    // Debug.Log(newVelocity);
    rb2d.velocity = newVelocity;
  }

  private void RefreshJump()
  {
    if (!inputManager.Jump) {
      tempJumpEnergy = jumpEnergy;
      jumpState = 0;
    }
    if (dashState != 1 && !inputManager.Dash)
    {
      dashState = 0;
      tempDashLength = dashDuration;
    }
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
}
