using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
  [SerializeField] private AudioClip intro;
  [SerializeField] private AudioClip loop;
  [SerializeField] private float volume = 0.7f;
  private AudioSource src;
  private AudioSource looper;
  private bool scheduled;

  private static MusicManager instance;

  void OnEnable()
  {
    src = GetComponent<AudioSource>();
    src.clip = intro;
  }

  void Awake()
  {
    DontDestroyOnLoad(this);
    if (instance == null)
    {
      instance = this;
    } else
    {
      Destroy(gameObject);
    }
  }

  void Start()
  {
    src.Play();

    GameObject child = new GameObject("Looper");
    child.transform.parent = gameObject.transform;
    looper = child.AddComponent<AudioSource>();
    looper.playOnAwake = false;
    looper.clip = loop;

    src.volume = volume;
    looper.volume = volume;

    looper.loop = true;
    src.loop = false;
  }

  void Update()
  {
    if (src.loop || scheduled)
    {
      return;
    }

    if (src.timeSamples + (src.clip.frequency * Time.deltaTime) >= src.clip.samples - src.clip.frequency)
    {
      looper.PlayScheduled(AudioSettings.dspTime + 1.0f);
      scheduled = true;
    }
  }
}
