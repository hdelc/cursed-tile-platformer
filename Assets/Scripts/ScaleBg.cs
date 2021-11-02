using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBg : MonoBehaviour
{
  public LevelRendering lvl;
  public GameObject player;
  public GameObject camera;

  // Start is called before the first frame update
  void Start()
  {
    // this.transform.localScale.x = lvl.width / 6f;
    // this.transform.localScale.y = lvl.height / 6f;
    float newScale = lvl.width / 10f;
    this.transform.localScale = new Vector3(newScale, newScale, 1);
  }

  // Update is called once per frame
  void Update()
  {
    Vector3 newPos = new Vector3();
    newPos.x = (camera.transform.position.x + lvl.width / 2) / 2;
    newPos.y = (camera.transform.position.y - lvl.height / 2) / 2;
    // newPos.x = player.transform.position.x;
    // newPos.y = player.transform.position.y;
    newPos.z = 10;
    this.transform.position = newPos;
  }
}
