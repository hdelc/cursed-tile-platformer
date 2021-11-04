using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  static GameObject player_instance;
  Cinemachine.CinemachineVirtualCamera camera;

  private Collider2D[] contacts;
  private List<TileBehavior> uniqueTileContacts;
  public PlayerEffect playerEffect;
  [SerializeField] BoxCollider2D hitbox;

  private PlayerEffect requestedPlayerEffect;
  private bool isPlayerEffectRequested;

  // Access scalars from movement module
  public float MovementSpeedScalar
  {
    get => movementSpeedScalar;
    set => movementSpeedScalar = value;
  }
  private float movementSpeedScalar = 1f;
  public float MovementAccelerationScalar
  {
    get => movementAccelerationScalar;
    set => movementAccelerationScalar = value;
  }
  private float movementAccelerationScalar = 1f;
  public float JumpHeightScalar
  {
    get => jumpHeightScalar;
    set => jumpHeightScalar = value;
  }
  private float jumpHeightScalar = 1f;
  public float DashSpeedScalar
  {
    get => dashSpeedScalar; set => dashSpeedScalar = value;
  }
  private float dashSpeedScalar;

  // Start is called before the first frame update
  void Start()
  {

  }

  private void Awake()
  {
    if (player_instance == null)
    {
      player_instance = gameObject;
      gameObject.name = "Player";
    }
    else if (player_instance != gameObject)
      Destroy(gameObject);

    if (player_instance == gameObject)
    {
      camera = GameObject.Find("CameraSystem/Virtual Follow Camera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
      camera.Follow = transform;
      contacts = new Collider2D[40];
      uniqueTileContacts = new List<TileBehavior>();
      //requestedEffectors = new List<PlayerEffect>();

      Debug.Log("Player loaded");
    }
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void FixedUpdate()
  {
    int numContacts = hitbox.GetContacts(contacts);
    InvokeUniqueTileCollisions(numContacts);
  }

  private void InvokeUniqueTileCollisions(int numContacts)
  {
    uniqueTileContacts.Clear();
    for (int i = 0; i < numContacts; i++)
    {
      TileBehavior tile = contacts[i].GetComponentInParent<TileBehavior>();
      if (tile != null && !uniqueTileContacts.Contains(tile))
      {
        uniqueTileContacts.Add(tile);
      }
    }
    foreach (TileBehavior tile in uniqueTileContacts)
      tile.NotifyContact(gameObject);
  }

  public void Kill()
  {
    Debug.Log("oh no i died");
  }

  public void RequestEffect(PlayerEffect effect)
  {
    isPlayerEffectRequested = true;
    if (effect.CompareTo(requestedPlayerEffect) > 0)
      requestedPlayerEffect = effect;
  }

  private void ApplyEffect()
  {
    switch (playerEffect)
    {
      case PlayerEffect.NONE:
        break;
      case PlayerEffect.SLIPPERY:
        MovementAccelerationScalar = 0.3f;
        break;
      case PlayerEffect.LOW_SPEED:
        MovementSpeedScalar = 0.3f;
        break;
      case PlayerEffect.LOW_JUMP:
        JumpHeightScalar = 0.3f;
        break;
      case PlayerEffect.FREEZE_DASH:
        DashSpeedScalar = 0f;
        break;
      case PlayerEffect.DEATH:
        Kill();
        break;
      case PlayerEffect.CLEANSE:
        break;
    }
  }

  private void RemoveEffect()
  {
    switch (playerEffect)
    {
      case PlayerEffect.NONE:
        break;
      case PlayerEffect.SLIPPERY:
        MovementAccelerationScalar = 1f;
        break;
      case PlayerEffect.LOW_SPEED:
        MovementSpeedScalar = 1f;
        break;
      case PlayerEffect.FREEZE_DASH:
        DashSpeedScalar = 1f;
        break;
      case PlayerEffect.LOW_JUMP:
        JumpHeightScalar = 1f;
        break;
      case PlayerEffect.DEATH:
        break;
      case PlayerEffect.CLEANSE:
        break;
    }
  }

  private void UpdateEffect()
  {
    if (requestedPlayerEffect != PlayerEffect.NONE)
    {
      RemoveEffect();
      playerEffect = requestedPlayerEffect;
      ApplyEffect();
    }
  }

  private void LateUpdate()
  {
    if (isPlayerEffectRequested)
    {
      UpdateEffect();
    }
    requestedPlayerEffect = PlayerEffect.DEATH;
    isPlayerEffectRequested = false;
  }
}

public enum PlayerEffect
{
  DEATH,
  FREEZE_DASH,
  SLIPPERY,
  LOW_SPEED,
  LOW_JUMP,
  NONE,
  CLEANSE
}