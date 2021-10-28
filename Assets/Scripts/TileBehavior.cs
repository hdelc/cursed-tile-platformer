using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
  public GameObject TileCollision { get => TileCollision; private set => tileCollision = value; }
  private GameObject tileCollision;

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

  // Update is called once per frame
  void Update()
  {
        
  }
}

public interface ITileTransformer
{
  void Transform(TileBehavior tile);
}
