using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
  public float totalTime = 0f;
  [SerializeField] public float survivalTime = 10f;
  [SerializeField] public float tileSpawnInterval = 2f;
  private float lastCollectTime = 0f;
  private float lastTileSpawnTime = 0f;
  public float timeRemaining { get { return survivalTime - (totalTime - lastCollectTime); }}
  private float finalTime = 0f;
  public bool hasLost = false;

  [SerializeField] Text totalTimeFront;
  [SerializeField] Text totalTimeBack;
  [SerializeField] Text timeRemainingText;

  [SerializeField] Text finalTimeText;
  [SerializeField] GameObject overlay1;
  [SerializeField] GameObject overlay2;

  private TileManager tileManager;

  void Start()
  {
    overlay1.SetActive(false);
    overlay2.SetActive(false);
  }

  private void Awake()
  {
    tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
  }

  void Update()
  {
    if (!hasLost)
    {
      totalTime += Time.deltaTime;
      if (totalTime - lastTileSpawnTime > tileSpawnInterval)
      {
        lastTileSpawnTime = totalTime;
        tileManager.TransformTiles();
      }
    }
    if (timeRemaining < 0)
    {
      lostGame();
    }

    float time = totalTime;
    if (finalTime > 0f)
    {
      time = finalTime;
    }
    int m = (int)(time / 60);
    int s = (int)(time % 60);
    int ms = (int)((time - System.Math.Truncate(time)) * 1000);
    string timestamp = ("" + m)/*.PadLeft(2, '0')*/ + ":" + ("" + s).PadLeft(2, '0') + "." + ("" + ms).PadLeft(3, '0');

    totalTimeFront.text = timestamp;
    totalTimeBack.text = timestamp;
    finalTimeText.text = timestamp;

    timeRemainingText.text = ("" + Math.Max(0, Math.Ceiling(timeRemaining))).PadLeft(2, '0');
  }

  public void resetCountdown()
  {
    lastCollectTime = totalTime;
  }

  public void lostGame()
  {
    if (hasLost)
    {
      return;
    }
    hasLost = true;
    finalTime = totalTime;

    StartCoroutine("delayedOverlay");
  }

  private IEnumerator delayedOverlay()
  {
    yield return new WaitForSeconds(0.8f);
    overlay1.SetActive(true);
    yield return new WaitForSeconds(0.7f);
    overlay2.SetActive(true);
  }
}
