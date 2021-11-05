using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
  private InputManager inputManager;
  private bool hasBeenPressed = false;

  [SerializeField] AudioClip sfx;
  [SerializeField] float vol = 0.5f;
  private AudioSource audio;

  void Start()
  {
    inputManager = GameObject.FindObjectOfType<InputManager>();
    audio = GetComponent<AudioSource>();
  }

  void Update()
  {
    if (!hasBeenPressed)
    {
      if (inputManager.Jump)
      {
        hasBeenPressed = true;
        Play();
      }
    }
  }

  public void Play()
  {
    StartCoroutine("PlayCo");
  }

  IEnumerator PlayCo()
  {
    audio.PlayOneShot(sfx, vol);
    yield return new WaitForSeconds(0.7f);
    SceneManager.LoadScene("TestScene");
  }
}
