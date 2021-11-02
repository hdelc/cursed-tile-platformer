﻿using UnityEngine;

public class NeutralTileTransformer : TileTransformer
{
  public override Sprite Sprite { get => sprite; }
  private readonly Sprite sprite = Resources.Load<Sprite>(@"Sprites\default tile");

  protected override void Revert_Extension(TileBehavior tile, TileState ot) { }
  protected override void Transform_Extension(TileBehavior tile) { }
}
