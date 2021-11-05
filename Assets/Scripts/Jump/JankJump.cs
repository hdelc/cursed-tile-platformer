using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JankJump : MonoBehaviour
{
  public float RunSpeed { get => runSpeed * playerManager.MovementSpeedScalar; set => runSpeed = value; }
  [SerializeField] private float runSpeed = 14f;

  public float HorizontalAcceleration { get => horizontalAcceleration * playerManager.MovementAccelerationScalar; set => horizontalAcceleration = value; }
  [SerializeField] private float horizontalAcceleration = 70f;

  public float Gravity { get => gravity; set => gravity = value; }
  [SerializeField] float gravity = -36f;
  public float TerminalVelocity { get; set; }
  [SerializeField] float terminalVelocity = -45f;

  [SerializeField] float jumpEnergy = 1.1f;
  [SerializeField] float jumpDecreaseCoefficient = 0.8f;

  [SerializeField] int dashDuration = 9;

  public float DashSpeed { get => dashSpeed * playerManager.DashSpeedScalar; set => dashSpeed = value; }
  [SerializeField] float dashSpeed = 25f;

  [SerializeField] float dashColliderScale = 0.9f;

  public bool isFacingRight = true;

  [SerializeField] Rigidbody2D rb2d;
  [SerializeField] BoxCollider2D hitbox;
  [SerializeField] GameObject collisionObj;

  [SerializeField] InputManager inputManager;

  private int jumpState = 0; // 0 - can jump, 1 - jump in progress, 2 - can't jump
  private float tempJumpEnergy;

  private int dashState = 0;
  private int tempDashLength;
  // private Vector2 defaultColliderSize;
  private Vector3 defaultColliderSize;

  private SpriteRenderer spriteRenderer;
  private PlayerManager playerManager;

  private PlayerMovement playerMovement;
  private AudioSource audio;

  // Start is called before the first frame update
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
    tempJumpEnergy = jumpEnergy;
    tempDashLength = dashDuration;
    defaultColliderSize = collisionObj.transform.localScale;

    spriteRenderer = GetComponent<SpriteRenderer>();
    playerManager = GetComponent<PlayerManager>();
    playerMovement = GetComponent<PlayerMovement>();
    audio = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update()
  {
    if (isFacingRight)
    {
      spriteRenderer.flipX = false;
    } else {
      spriteRenderer.flipX = true;
    }

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
          audio.PlayOneShot(playerMovement.jumpSound, playerMovement.jumpVol);
          jumpState = 1;
        }

        if (jumpState == 1)
        {
          //Vector2 newPosition = new Vector2(rb2d.position.x, rb2d.position.y + tempJumpEnergy);

          newVelocity.y = tempJumpEnergy * playerManager.JumpHeightScalar / Time.fixedDeltaTime;

          tempJumpEnergy = Mathf.Max(tempJumpEnergy * jumpDecreaseCoefficient, 0);
          if (tempJumpEnergy < 0.05)
          {
            tempJumpEnergy = 0;
            jumpState = 2;
          }
          /*if (tempJumpEnergy > 0) {
            newVelocity.y = 0f;
            }
            rb2d.position = newPosition;*/
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
          audio.PlayOneShot(playerMovement.dashSound, playerMovement.dashVol);
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
      if (boxcast.collider != null && deltaPosition.y > 0)
      {
        positionCorrection.y = boxcast.distance;
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        jumpState = 2;
      }

      Debug.DrawRay(boxcast_position, Vector3.up * deltaPosition.y, Color.green, Time.fixedDeltaTime);
    }

    rb2d.position += positionCorrection / 3;
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
}
