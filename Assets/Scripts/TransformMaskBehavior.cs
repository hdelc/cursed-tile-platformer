using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMaskBehavior : MonoBehaviour
{
  public TileBehavior Tile { get; private set; }
  public bool Transforming { get; private set; }
  public SpriteRenderer SpriteRenderer { get; private set; }
  private TileTransformer queuedTransformer;

  public float TransformTime;
  private float thisTransformTime;
  private float transformTimeElapsed;
  private bool transformed;

  // Start is called before the first frame update
  void Start()
  {
    
  }

  private void Awake()
  {
    Tile = GetComponentInParent<TileBehavior>();
    SpriteRenderer = GetComponent<SpriteRenderer>();
    Transforming = false;
  }

  // Update is called once per frame
  void Update()
  {
    if (Transforming)
    {
      transformTimeElapsed += Time.deltaTime;
      float progress = transformTimeElapsed / thisTransformTime;
      if (progress >= 1f) // Ends transform
      {
        if (!transformed)
          queuedTransformer.Transform(Tile);
        transformed = false;
        SpriteRenderer.color = new Color(1, 1, 1, 0);
        Transforming = false;
      }
      else // Continue transform
      {
        float alpha = 1 - (2*Mathf.Abs(progress - 0.5f));
        Debug.Log(alpha);
        SpriteRenderer.color = new Color(1, 1, 1, alpha);
        if (progress >= 0.5f && !transformed)
        {
          queuedTransformer.Transform(Tile);
          transformed = true;
        }
      }
    }
  }

  public bool RequestTransform(TileTransformer transformer)
  {
    if (Transforming)
      return false;
    BeginTransform(transformer);
    return true;
  }

  private void BeginTransform(TileTransformer transformer)
  {
    queuedTransformer = transformer;
    thisTransformTime = TransformTime;
    transformTimeElapsed = 0;
    transformed = false;
    Transforming = true;
  }
}
