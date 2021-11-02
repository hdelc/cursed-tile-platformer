using UnityEngine;
// For testing
public class SpriteChangeTileTransformer : TileTransformer
{
  public override Sprite Sprite { get => sprite; }
  private static readonly Sprite sprite = Resources.Load<Sprite>(@"Sprites\blue tile");

  public SpriteChangeTileTransformer() { }

  protected override void Transform_Extension(TileBehavior tile) 
  {
    Debug.Log("SpriteChangeTileTransformer::Transform_Extension");
  }

  protected override void Revert_Extension(TileBehavior tile, TileState originalTileState)
  {
	  Debug.Log("SpriteChangeTileTransformer::Revert_Extension");
  }

  protected override TileState ProduceTileState(TileBehavior tile)
  {
	  return new SpriteChangeTileState();
  }

  protected class SpriteChangeTileState : TileState { }
}