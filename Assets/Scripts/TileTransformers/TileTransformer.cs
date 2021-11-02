using System.Collections.Generic;
using UnityEngine;

public abstract class TileTransformer
{
  private Dictionary<TileBehavior, TileState> originalTiles;
  public abstract Sprite Sprite { get; }

  public TileTransformer()
  {
	  originalTiles = new Dictionary<TileBehavior, TileState>();
  }

  public void Transform(TileBehavior tile)
  {
	  if (!originalTiles.ContainsKey(tile))
	  {
	    tile.transformer.Revert(tile);
	    originalTiles.Add(tile, ProduceTileState(tile));
      tile.SpriteRenderer.sprite = Sprite;
	    tile.transformer = this;
	    Transform_Extension(tile);
	  }
  }
  protected abstract void Transform_Extension(TileBehavior tile);

  public void Revert(TileBehavior tile)
  {
	  if (originalTiles.ContainsKey(tile))
	  {
	    Revert_Extension(tile, originalTiles[tile]);
	    originalTiles.Remove(tile);
	  }
  }
  protected abstract void Revert_Extension(TileBehavior tile, TileState ot);

  protected abstract TileState ProduceTileState(TileBehavior tile);

  protected abstract class TileState { }
}
