using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
  public List<TileTransformer> availableTransformers;
  // Start is called before the first frame update
  void Start()
  {
    availableTransformers = new List<TileTransformer>();
    availableTransformers.Add(new CleanseTileTransformer());
    availableTransformers.Add(new DeadlyTileTransformer());
    availableTransformers.Add(new LowJumpTileTransformer());
    availableTransformers.Add(new SlipperyTileTransformer());
    availableTransformers.Add(new SlowTileTransformer());

  }

  // Update is called once per frame
  void Update()
  {
    
  }
}
