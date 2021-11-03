using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
  public List<TileTransformer> availableTransformers;
  private int availableTransformersIterator = 0;

  public Vector2 LevelBottomLeft
  {
    get => levelRegion[0];
    set => levelRegion[0] = value;
  }
  public Vector2 LevelTopRight
  {
    get => levelRegion[1];
    set => levelRegion[1] = value;
  }
  private Vector2[] levelRegion;
  // Start is called before the first frame update
  void Start()
  {
    availableTransformers = new List<TileTransformer>();
    availableTransformers.Add(new DeadlyTileTransformer());
    availableTransformers.Add(new LowJumpTileTransformer());
    availableTransformers.Add(new SlipperyTileTransformer());
    availableTransformers.Add(new SlowTileTransformer());
    availableTransformers.Add(new CleanseTileTransformer());

    levelRegion = new Vector2[2];
  }

  // Update is called once per frame
  void Update()
  {
    
  }

  public void TransformTiles()
  {
    float pos_x = Random.Range(LevelBottomLeft.x, LevelTopRight.x);
    float pos_y = Random.Range(LevelBottomLeft.y, LevelTopRight.y);
    float size_x = Random.Range(2, 6);
    float size_y = Random.Range(2, 6);

    TileTransformExplosion.MakeExplosion(new Vector2(pos_x, pos_y), new Vector2(size_x, size_y), availableTransformers[availableTransformersIterator]);

    availableTransformersIterator++;
    availableTransformersIterator %= availableTransformers.Count;
  }
}
