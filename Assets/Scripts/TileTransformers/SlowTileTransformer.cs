using UnityEngine;

class SlowTileTransformer : TileTransformer
{
  public override Sprite Sprite { get => sprite; }
  private readonly Sprite sprite = Resources.Load<Sprite>(@"Sprites\yellow tile");

  protected override void Revert_Extension(TileBehavior tile, TileState ot)
  {
    tile.PlayerContactEvent -= Tile_PlayerContactEvent;
  }

  protected override void Transform_Extension(TileBehavior tile)
  {
    tile.PlayerContactEvent += Tile_PlayerContactEvent;
  }

  private void Tile_PlayerContactEvent(TileBehavior tile, TileBehavior.PlayerContactEventArgs args)
  {
    args.player.GetComponent<PlayerManager>()?.RequestEffect(PlayerEffect.LOW_SPEED);
  }
}
