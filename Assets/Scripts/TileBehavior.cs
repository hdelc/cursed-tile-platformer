using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
  public GameObject TileCollision { get => TileCollision; private set => tileCollision = value; }
  private GameObject tileCollision;
  public SpriteRenderer SpriteRenderer;

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

public interface ITileTransformer
{
  void Transform(TileBehavior tile);
}

// For testing
public class TileColorChange : ITileTransformer
{
  private Color color;

  public TileColorChange(Color color)
  {
    this.color = color;
  }

  public void Transform(TileBehavior tile)
  {
    tile.SpriteRenderer.color = color;
  }
}