using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
  private InputManager inputManager;
  private bool hasBeenPressed = false;

  void Start()
  {
    inputManager = GameObject.FindObjectOfType<InputManager>();
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
    SceneManager.LoadScene("TestScene");
  }
}
