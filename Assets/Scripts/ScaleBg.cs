using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBg : MonoBehaviour
{
  public LevelRendering lvl;
  public GameObject camera;

  [SerializeField] private float centerWeight = 1f;
  [SerializeField] private float cameraWeight = 1f;
  [SerializeField] private int zLayer = 10;

  // Start is called before the first frame update
  void Start()
  {
    // this.transform.localScale.x = lvl.width / 6f;
    // this.transform.localScale.y = lvl.height / 6f;
    float newScale = lvl.width / 6f;
    this.transform.localScale = new Vector3(newScale, newScale, 1);
  }

  // Update is called once per frame
  void Update()
  {
    Vector3 newPos = new Vector3();
    newPos.x = (cameraWeight * camera.transform.position.x + centerWeight * lvl.width / 2) / (cameraWeight + centerWeight);
    newPos.y = (cameraWeight * camera.transform.position.y - centerWeight * lvl.height / 2) / (cameraWeight + centerWeight);
    // newPos.x = player.transform.position.x;
    // newPos.y = player.transform.position.y;
    newPos.z = zLayer;
    this.transform.position = newPos;
  }
}
