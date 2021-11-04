using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsJump : MonoBehaviour
{
  public float RunSpeed { get => runSpeed; set => runSpeed = value; }
  [SerializeField] private float runSpeed = 14f;

  public float HorizontalAcceleration { get => horizontalAcceleration * playerManager.MovementAccelerationScalar; set => horizontalAcceleration = value; }
  [SerializeField] private float horizontalAcceleration = 70f;

  public float Gravity { get => gravity; set => gravity = value; }
  [SerializeField] float gravity = -36f;
  public float TerminalVelocity { get; set; }
  [SerializeField] float terminalVelocity = -45f;

  [SerializeField] float jumpVelocity = 24f;
  [SerializeField] int jumpLength = 11;

  [SerializeField] int dashDuration = 9;

  public float DashSpeed { get => dashSpeed * playerManager.DashSpeedScalar; set => dashSpeed = value; }
  [SerializeField] float dashSpeed = 25f;

  [SerializeField] float dashColliderScale = 0.9f;

  public bool isFacingRight = true;

  [SerializeField] Rigidbody2D rb2d;
  [SerializeField] BoxCollider2D hitbox;
  [SerializeField] GameObject collisionObj;
  // [SerializeField] EdgeCollider2D bottomEdgeCollider;

  [SerializeField] InputManager inputManager;

  private int jumpState = 0; // 0 - can jump, 1 - jump in progress, 2 - can't jump
  private int tempJumpLength;

  private int dashState = 0;
  private int tempDashLength;
  // private Vector2 defaultColliderSize;
  private Vector3 defaultColliderSize;

  private PlayerManager playerManager;
  void Awake()
  {
    if(inputManager == null)
    {
      inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
    }
    if (collisionObj == null)
    {
      collisionObj = transform.Find("PlayerCollision").gameObject;
    }
    rb2d = this.GetComponent<Rigidbody2D>();
    hitbox = this.GetComponent<BoxCollider2D>();
    // inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
    tempJumpLength = jumpLength;
    tempDashLength = dashDuration;
    defaultColliderSize = collisionObj.transform.localScale;

    playerManager = GetComponent<PlayerManager>();
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
      newVelocity.x = isFacingRight ? DashSpeed : -DashSpeed;
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
          //Vector2 newPosition = new Vector2(rb2d.position.x, rb2d.position.y + tempJumpEnergy);

          newVelocity.y = jumpVelocity * playerManager.JumpHeightScalar;
          if (--tempJumpLength <= 0)
          {
            //newVelocity.y = jumpVelocity / 2f;
            // newVelocity.y = jumpVelocity / 3f;
            newVelocity.y = rb2d.velocity.y / 4f;
            jumpState = 2;
          }
          // newVelocity.y = tempJumpEnergy / Time.fixedDeltaTime;

          // tempJumpEnergy = Mathf.Max(tempJumpEnergy * jumpDecreaseCoefficient, 0);
          // if (tempJumpEnergy < 0.05)
          // {
          //   tempJumpEnergy = 0;
          //   jumpState = 2;
          // }
          /*if (tempJumpEnergy > 0) {
            newVelocity.y = 0f;
          }
          rb2d.position = newPosition;*/
        }
      } else {
        if (jumpState == 1)
        {
          // if (tempJumpEnergy > 0) {
          //   newVelocity.y = 2f;
          // }
          if (tempJumpLength > 0)
          {
            //newVelocity.y = 2f;
            newVelocity.y = rb2d.velocity.y / 6f;
          }
          jumpState = 2;
        } else if (jumpState == 2)
        {
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
    CustomCollision();
  }

  private void CustomCollision()
  {
    LayerMask mask = LayerMask.GetMask("Walls");
    Vector2 deltaPosition = rb2d.velocity * Time.fixedDeltaTime;
    Vector2 positionCorrection = Vector2.zero;

    //Ceiling collision
    {
      Vector2 boxcast_position = new Vector2(hitbox.transform.position.x, hitbox.transform.position.y + (hitbox.transform.lossyScale.y / 4));
      Vector2 boxcast_size = new Vector2(hitbox.transform.lossyScale.x * 0.5f, hitbox.transform.lossyScale.y / 2);
      RaycastHit2D boxcast = Physics2D.BoxCast(boxcast_position, boxcast_size, 0, Vector2.up, deltaPosition.y, mask);
      if (boxcast.collider && rb2d.velocity.y > 0)
      {
        positionCorrection.y = boxcast.distance;
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        jumpState = 2;
      }
    }

    rb2d.position += positionCorrection;
  }

  private void RefreshJump()
  {
    if (!inputManager.Jump) {
      tempJumpLength = jumpLength;
      jumpState = 0;
    }
    if (dashState != 1 && !inputManager.Dash)
    {
      dashState = 0;
      tempDashLength = dashDuration;
    }
  }
}
