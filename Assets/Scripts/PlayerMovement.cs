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
  private Timer timer;
  private Component cmp = null;

  void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    timer = GameObject.FindObjectOfType<Timer>();

    int index = GlobalData.characterIndex;
    switch (index)
    {
      case 0:
        cmp = gameObject.AddComponent(Type.GetType("SlowJump"));
        spriteRenderer.sprite = slowJumpSprite;
        break;
      case 1:
        cmp = gameObject.AddComponent(Type.GetType("JankJump"));
        spriteRenderer.sprite = jankJumpSprite;
        break;
      case 2:
        cmp = gameObject.AddComponent(Type.GetType("ConsJump"));
        spriteRenderer.sprite = consJumpSprite;
        break;
      default:
        cmp = gameObject.AddComponent(Type.GetType("ConsJump"));
        spriteRenderer.sprite = consJumpSprite;
        break;
    }
  }

  void Update()
  {
    if (cmp != null)
    {
      if (timer.hasLost)
      {
        Destroy(cmp);
      }
    }
  }
}
