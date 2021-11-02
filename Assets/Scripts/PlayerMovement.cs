using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [SerializeField] private Sprite slowJumpSprite;
  [SerializeField] private Sprite jankJumpSprite;
  [SerializeField] private Sprite consJumpSprite;

  private SpriteRenderer spriteRenderer;

  void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();

    int index = GlobalData.characterIndex;
    switch (index)
    {
      case 0:
        gameObject.AddComponent(Type.GetType("SlowJump"));
        spriteRenderer.sprite = slowJumpSprite;
        break;
      case 1:
        gameObject.AddComponent(Type.GetType("JankJump"));
        spriteRenderer.sprite = jankJumpSprite;
        break;
      case 2:
        gameObject.AddComponent(Type.GetType("ConsJump"));
        spriteRenderer.sprite = consJumpSprite;
        break;
      default:
        gameObject.AddComponent(Type.GetType("ConsJump"));
        spriteRenderer.sprite = consJumpSprite;
        break;
    }
  }
}
