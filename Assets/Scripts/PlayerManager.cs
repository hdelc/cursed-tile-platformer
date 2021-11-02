using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  static GameObject player_instance;
  Cinemachine.CinemachineVirtualCamera camera;
  
  private Collider2D[] contacts;
  private List<TileBehavior> uniqueTileContacts;
  [SerializeField] BoxCollider2D hitbox;

  // Start is called before the first frame update
  void Start()
  {

  }

  private void Awake()
  {
    if (player_instance == null)
    {
      player_instance = gameObject;
      gameObject.name = "Player";
    }
    else if (player_instance != gameObject)
      Destroy(gameObject);

    if (player_instance == gameObject)
    {
      camera = GameObject.Find("CameraSystem/Virtual Follow Camera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
      camera.Follow = transform;
      contacts = new Collider2D[40];
      uniqueTileContacts = new List<TileBehavior>();
    }
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void FixedUpdate()
  {
    int numContacts = hitbox.GetContacts(contacts);
    InvokeUniqueTileCollisions(numContacts);
  }

  private void InvokeUniqueTileCollisions(int numContacts)
  {
    uniqueTileContacts.Clear();
    for(int i=0; i<numContacts; i++)
    {
      TileBehavior tile = contacts[i].GetComponentInParent<TileBehavior>();
      if (tile != null && !uniqueTileContacts.Contains(tile))
      {
        uniqueTileContacts.Add(tile);
      }
    }
    foreach (TileBehavior tile in uniqueTileContacts)
      tile.NotifyContact(gameObject);
  }
}
