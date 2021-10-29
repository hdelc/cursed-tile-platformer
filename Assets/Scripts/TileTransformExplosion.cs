using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTransformExplosion : MonoBehaviour
{
  static GameObject explosionPrefab;
  private PolygonCollider2D trigger;
  private ITileTransformer transformer;

  private bool exploded = false;
  private int lifespan = 1;


  // Start is called before the first frame update
  void Start()
  {
    explosionPrefab = (GameObject)Resources.Load("Prefabs/Explosion", typeof(GameObject));
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
      if(other.gameObject.name == "TileCollision")
      {
        transformer.Transform(other.gameObject.GetComponentInParent<TileBehavior>());
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

  public static void MakeExplosion(Vector2 position, Vector2 size, ITileTransformer transformer)
  {
    GameObject explosion = Instantiate(explosionPrefab);
    TileTransformExplosion explosionScript = explosion.GetComponent<TileTransformExplosion>();
    explosionScript.Explode(position, size, transformer);
  }

  private void Explode(Vector2 position, Vector2 size, ITileTransformer transformer)
  {
    gameObject.transform.position = position;
    this.transformer = transformer;
    trigger.points = PathGenerator.GenerateEllipticalPath(size).ToArray();
    exploded = true;
  }
}
