using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class DeadlyTileTransformer : TileTransformer
{
  public override Sprite Sprite { get => sprite; }
  private readonly Sprite sprite = Resources.Load<Sprite>(@"Sprites\pink tile");

  protected override TileState ProduceTileState(TileBehavior tile) { return new TileState(); }

  protected override void Revert_Extension(TileBehavior tile, TileState ot)
  {
    tile.PlayerContactEvent -= Tile_PlayerContactEvent;
  }

  protected override void Transform_Extension(TileBehavior tile)
  {
    tile.PlayerContactEvent += Tile_PlayerContactEvent;
  }

  private void Tile_PlayerContactEvent(TileBehavior tile, GameObject player)
  {
    player.GetComponent<PlayerManager>()?.Kill();
  }
}

