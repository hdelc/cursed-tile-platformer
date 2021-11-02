using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
  public GameObject TileCollision { get => TileCollision; private set => tileCollision = value; }
  private GameObject tileCollision;
  public SpriteRenderer SpriteRenderer;
  public TileTransformer transformer = new NeutralTileTransformer();

  // Start is called before the first frame update
  void Start()
  {
    foreach(Transform child in gameObject.GetComponentsInChildren<Transform>())
    {
      if(child.name == "TileCollision")
      {
        tileCollision = child.gameObject;
      }
    }
    if(tileCollision == null)
    {
      Debug.LogError($"Tile \"{gameObject.name}\" could not find its TileCollision");
    }
  }

  private void Awake()
  {
    SpriteRenderer = GetComponent<SpriteRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}

public abstract class TileTransformer
{
  private Dictionary<TileBehavior, TileState> originalTiles;

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

public class NeutralTileTransformer : TileTransformer
{
  protected override TileState ProduceTileState(TileBehavior tile) { return new EmptyTileState(); }
  protected override void Revert_Extension(TileBehavior tile, TileState ot) { }
  protected override void Transform_Extension(TileBehavior tile) { }
  protected class EmptyTileState : TileState { }
}

// For testing
public class ColorChangeTileTransformer : TileTransformer
{

  private Color color;

  public ColorChangeTileTransformer(Color color)
  {
    this.color = color;
  }

  protected override void Transform_Extension(TileBehavior tile)
  {
    tile.SpriteRenderer.color = color;
  }

  protected override void Revert_Extension(TileBehavior tile, TileState originalTileState)
  {
    Debug.Log("Revert Color");
    ColorChangeTileState originalState = (ColorChangeTileState)originalTileState;
    tile.SpriteRenderer.color = originalState.color;
  }

  protected override TileState ProduceTileState(TileBehavior tile)
  {
    return new ColorChangeTileState(tile);
  }

  protected class ColorChangeTileState : TileState
  {
    public Color color;

    public ColorChangeTileState(TileBehavior tile)
    {
      color = tile.SpriteRenderer.color;
    }
  }
}