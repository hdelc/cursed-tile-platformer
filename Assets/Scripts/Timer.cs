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
  public int collectCount = 0;

  [SerializeField] Text totalTimeFront;
  [SerializeField] Text totalTimeBack;
  [SerializeField] Text timeRemainingText;

  [SerializeField] Text finalTimeText;
  [SerializeField] Text finalCountText;
  [SerializeField] Text finalCharText;
  [SerializeField] GameObject overlay1;
  [SerializeField] GameObject overlay2;

  [SerializeField] AudioClip death;
  [SerializeField] AudioClip warning;
  [SerializeField] float volume;
  AudioSource audio;

  [SerializeField] RuntimeAnimatorController deathAnim;
  private PlayerMovement player;

  private bool warning1 = false;
  private bool warning2 = false;
  private bool warning3 = false;

  private TileManager tileManager;

  void Start()
  {
    overlay1.SetActive(false);
    overlay2.SetActive(false);

    audio = GetComponent<AudioSource>();
    player = GameObject.FindObjectOfType<PlayerMovement>();
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
    } else if (timeRemaining < 3 && !warning3)
    {
      audio.PlayOneShot(warning, volume);
      warning3 = true;
    } else if (timeRemaining < 2 && !warning2)
    {
      audio.PlayOneShot(warning, volume);
      warning2 = true;
    } else if (timeRemaining < 1 && !warning1)
    {
      audio.PlayOneShot(warning, volume);
      warning1 = true;
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
    
    finalCountText.text = "" + collectCount;
    finalTimeText.text = timestamp;
    finalCharText.text = GlobalData.characterName;

    timeRemainingText.text = ("" + Math.Max(0, Math.Ceiling(timeRemaining))).PadLeft(2, '0');
  }

  public void resetCountdown()
  {
    lastCollectTime = totalTime;

    warning1 = false;
    warning2 = false;
    warning3 = false;

    collectCount++;
  }

  public void lostGame()
  {
    if (hasLost)
    {
      return;
    }
    hasLost = true;
    finalTime = totalTime;

    audio.PlayOneShot(death, volume);

    player.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
    Animator anim = player.gameObject.AddComponent<Animator>();
    anim.runtimeAnimatorController = deathAnim;

    StartCoroutine("delayedOverlay");
  }

  private IEnumerator delayedOverlay()
  {
    yield return new WaitForSeconds(0.5f);
    overlay1.SetActive(true);
    yield return new WaitForSeconds(0.5f);
    overlay2.SetActive(true);
  }
}
