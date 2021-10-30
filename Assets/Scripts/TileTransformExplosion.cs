using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTransformExplosion : MonoBehaviour
{
  static GameObject explosionPrefab;
  private PolygonCollider2D trigger;
  private TileTransformer transformer;

  private bool exploded = false;
  private int lifespan = 1;


  // Start is called before the first frame update
  void Start()
  {
    explosionPrefab = GameObject.Find("Explosion");
    
  }

  private void Awake()
  {
    gameObject.TryGetComponent<PolygonCollider2D>(out trigger);
    if(trigger == null)
    {
      Debug.LogError($"Explosion \"{gameObject.name}\" does not have a PolygonCollider2D");
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if(exploded)
    {
      Debug.Log("Trigger enter: " + other.gameObject.name);
      TileBehavior tile = other.GetComponentInParent<TileBehavior>();
      if (tile != null)
      {
        Debug.Log(tile.gameObject.name);
        transformer.Transform(tile);
      }
    }
  }

  private void FixedUpdate()
  {
    if (exploded)
    {
      if (lifespan > 0)
        lifespan--;
      else
        Destroy(gameObject);
    }
  }

  public static void MakeExplosion(Vector2 position, Vector2 size, TileTransformer transformer)
  {
    if (explosionPrefab == null)
      Debug.LogError("TileTransformExplosion could not find the Explosion prefab");
    GameObject explosion = Instantiate(explosionPrefab);
    TileTransformExplosion explosionScript = explosion.GetComponent<TileTransformExplosion>();
    explosionScript.Explode(position, size, transformer);
  }

  private void Explode(Vector2 position, Vector2 size, TileTransformer transformer)
  {
    gameObject.transform.position = position;
    this.transformer = transformer;
    trigger.SetPath(0, PathGenerator.GenerateEllipticalPath(size));
    exploded = true;
  }
}
