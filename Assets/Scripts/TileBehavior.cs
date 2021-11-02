using System.Collections;
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