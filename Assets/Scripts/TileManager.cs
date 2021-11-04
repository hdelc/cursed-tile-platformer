using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
  public List<TileTransformer> availableTransformers;
  private int availableTransformersIterator = 0;
  public LevelRendering level;
  // Start is called before the first frame update
  void Start()
  {
    
  }

  private void Awake()
  {
    availableTransformers = new List<TileTransformer>();
    availableTransformers.Add(new DeadlyTileTransformer());
    availableTransformers.Add(new LowJumpTileTransformer());
    availableTransformers.Add(new SlipperyTileTransformer());
    availableTransformers.Add(new SlowTileTransformer());
    availableTransformers.Add(new FreezeDashTileTransformer());
    availableTransformers.Add(new CleanseTileTransformer());

    level = GameObject.Find("Level").GetComponent<LevelRendering>();

    Debug.Log("TileManager loaded");
  }

  // Update is called once per frame
  void Update()
  {
    
  }

  public void TransformTiles()
  {
    float pos_x = Random.Range(0, level.width);
    float pos_y = Random.Range(-level.height, 0);
    float size_x = Random.Range(2, 6);
    float size_y = Random.Range(2, 6);

    TileTransformExplosion.MakeExplosion(new Vector2(pos_x, pos_y), new Vector2(size_x, size_y), availableTransformers[availableTransformersIterator]);

    availableTransformersIterator++;
    availableTransformersIterator %= availableTransformers.Count;
  }
}
