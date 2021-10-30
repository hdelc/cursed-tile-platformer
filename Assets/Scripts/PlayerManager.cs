using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  static GameObject player_instance;
  Cinemachine.CinemachineVirtualCamera camera;

  // Start is called before the first frame update
  void Start()
  {

  }

  private void Awake()
  {
    if (player_instance == null)
    {
      player_instance = gameObject;
      this.gameObject.name = "Player";
    }
    else if (player_instance != gameObject)
      Destroy(gameObject);

    if (player_instance == gameObject)
    {
      camera = GameObject.Find("CameraSystem/Virtual Follow Camera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
      camera.Follow = transform;
    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}
