using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
  private InputManager inputManager;
  public int menuCol = 0;
  private int vClickState = 0;
  private int hClickState = 0;

  [SerializeField] private ZoomInOut frameZoom;
  [SerializeField] private ZoomInOut playZoom;
  [SerializeField] private float playDelay = 0.5f;

  // Start is called before the first frame update
  void Start()
  {
    inputManager = GetComponent<InputManager>();
  }

  // Update is called once per frame
  void Update()
  {
    if (inputManager.VerticalAxis != 0)
    {
      if (vClickState == 0) {
        vClickState = 1;
      }
      if (vClickState == 1)
      {
        vClickState = 2;
        menuCol = (menuCol + 1) % 2;
      }
    } else {
      if (vClickState == 2)
      {
        vClickState = 0;
      }
    }

    if (menuCol == 0)
    {
      frameZoom.enabled = true;
      playZoom.enabled = false;

    if (inputManager.HorizontalAxis != 0)
    {
      if (hClickState == 0) {
        hClickState = 1;
      }
      if (hClickState == 1)
      {
        hClickState = 2;
        int temp = GlobalData.characterIndex;
        if (inputManager.HorizontalAxis > 0)
        {
          temp = (temp + 1) % 3;
        } else if (inputManager.HorizontalAxis < 0)
        {
          temp = temp - 1;
          if (temp < 0)
          {
            temp = temp + 3;
          }
          temp = temp % 3;
        }
        GlobalData.characterIndex = temp;
      }
    } else {
      if (hClickState == 2)
      {
        hClickState = 0;
      }
    }
    } else if (menuCol == 1)
    {
      frameZoom.enabled = false;
      playZoom.enabled = true;

      if (inputManager.Jump)
      {
        playZoom.gameObject.GetComponent<Image>().color = new Color(1f, 0.76f, 0.08f);
        // PlayGame();
        DelayedPlayGame();
      }
    }
  }

  public void PlayGame()
  {
    SceneManager.LoadScene("TestScene");
  }

  IEnumerator DelayedPlayGameCo()
  {
    yield return new WaitForSeconds(playDelay);
    PlayGame();
  }

  public void DelayedPlayGame()
  {
    StartCoroutine("DelayedPlayGameCo");
  }

  public void SetCharacter(int newCharacterIndex)
  {
    GlobalData.characterIndex = newCharacterIndex;
  }
}
