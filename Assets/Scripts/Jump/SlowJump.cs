using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowJump : MonoBehaviour
{
  public float RunSpeed { get => runSpeed * playerManager.MovementSpeedScalar; set => runSpeed = value; }
  [SerializeField] private float runSpeed = 14f;

  public float HorizontalAcceleration { get => horizontalAcceleration * playerManager.MovementAccelerationScalar; set => horizontalAcceleration = value; }
  [SerializeField] private float horizontalAcceleration = 70f;

  //public float Gravity { get => gravity; set => gravity = value; }
  private float gravity;
  [SerializeField] float baseGravity = -90f;
  public float TerminalVelocity { get; set; }
  [SerializeField] float terminalVelocity = -20f;

  [SerializeField] float jumpSpeed = 20f;
  [SerializeField] float jumpSpeedCutoff = 12f;
  [SerializeField] float jumpGravityScale = 0.33f;

  [SerializeField] int dashDuration = 9;

  public float DashSpeed { get => dashSpeed * playerManager.DashSpeedScalar; set => dashSpeed = value; }
  [SerializeField] float dashSpeed = 25f;

  // [SerializeField] Vector2 dashColliderSize = new Vector2(0.8f, 0.8f);
  [SerializeField] float dashColliderScale = 0.9f;

  public bool isFacingRight = true;
  [SerializeField] Rigidbody2D rb2d;
  [SerializeField] BoxCollider2D hitbox;
  [SerializeField] GameObject collisionObj;
  // [SerializeField] EdgeCollider2D bottomEdgeCollider;

  [SerializeField] InputManager inputManager;

  private int jumpState = 0; // 0 - can jump, 1 - jump in progress, 2 - can't jump
  private float tempJumpSpeed;

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
    spriteRenderer = this.GetComponent<SpriteRenderer>();
    // inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
    tempJumpSpeed = jumpSpeed;
    tempDashLength = dashDuration;
    defaultColliderSize = collisionObj.transform.localScale;
    gravity = baseGravity;

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

      Debug.Log("Gravity: " + gravity);
      float verticalDeltaV = gravity * Time.fixedDeltaTime;

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
          gravity = jumpGravityScale * baseGravity;
          newVelocity.y = jumpSpeed * Mathf.Sqrt(playerManager.JumpHeightScalar);
          jumpState = 1;
        }

        if (jumpState == 1)
        {

          if (newVelocity.y < jumpSpeedCutoff)
          {
            jumpState = 2;
            gravity = baseGravity;
          }

          /*if (tempJumpEnergy > 0) {
            newVelocity.y = 0f;
            }
            rb2d.position = newPosition;*/
        }
      } else {
        if (jumpState == 1)
        {
          jumpState = 2;
          gravity = baseGravity;
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
      if (boxcast.collider && rb2d.velocity.y > 0)
      {
        positionCorrection.y = boxcast.distance;
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
      }
    }

    rb2d.position += positionCorrection;
  }

  private void RefreshJump()
  {
    if (!inputManager.Jump) {
      tempJumpSpeed = jumpSpeed;
      jumpState = 0;
    }
    if (dashState != 1 && !inputManager.Dash)
    {
      dashState = 0;
      tempDashLength = dashDuration;
    }
  }
}

