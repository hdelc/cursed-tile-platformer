using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame(int charIndex)
    { // either have it set the character here or every time smth is clicked
      SceneManager.LoadScene("TestScene");
    }
}
