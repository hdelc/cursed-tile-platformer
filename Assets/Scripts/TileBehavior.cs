using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
  public GameObject TileCollision { get => TileCollision; private set => tileCollision = value; }
  private GameObject tileCollision;
  private TransformMaskBehavior transformMask;
  public SpriteRenderer SpriteRenderer;
  public TileTransformer transformer;

  public delegate void PlayerContactEventHandler(TileBehavior tile, GameObject player);
  public event PlayerContactEventHandler PlayerContactEvent;

  // Start is called before the first frame update
  void Start()
  {
	  
  }

  private void Awake()
  {
    transformer = new NeutralTileTransformer();
	  SpriteRenderer = GetComponent<SpriteRenderer>();
    foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
    {
      if (child.name == "TileCollision")
        tileCollision = child.gameObject;
      if (child.name == "TransformMask")
        transformMask = child.gameObject.GetComponent<TransformMaskBehavior>();
    }
    if (tileCollision == null)
      Debug.LogError($"Tile \"{gameObject.name}\" could not find its TileCollision");
    if(transformMask == null)
      Debug.LogError($"Tile \"{gameObject.name}\" could not find its TransformMask");
  }

  // Update is called once per frame
  void Update()
  {
		
  }

  public bool RequestTransform(TileTransformer transformer)
  {
    return transformMask.RequestTransform(transformer);
  }

  public void NotifyContact(GameObject player)
  {
    PlayerContactEvent?.Invoke(this, player);
  }
}

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

public class NeutralTileTransformer : TileTransformer
{
  public override Sprite Sprite { get => sprite; }
  private readonly Sprite sprite = Resources.Load<Sprite>(@"Sprites\default tile");

  protected override TileState ProduceTileState(TileBehavior tile) { return new NeutralTileState(); }
  protected override void Revert_Extension(TileBehavior tile, TileState ot) { }
  protected override void Transform_Extension(TileBehavior tile) { }
  protected class NeutralTileState : TileState { }
}

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