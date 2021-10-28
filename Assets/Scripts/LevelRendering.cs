using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LevelRendering : MonoBehaviour
{
  [SerializeField] public string levelFileName = "testlevel";
  private string levelStr;
  private int[][] levelArr;

  [SerializeField] public GameObject playerPrefab;
  [SerializeField] public GameObject tempBlockPrefab;

  void Start()
  {
    levelStr = File.ReadAllText("./Assets/Scripts/LevelJson/" + levelFileName + ".txt");
    string[] temp1 = levelStr.Trim().Split('\n');
    string[][] temp2 = new string[temp1.Length][];
    for (int i = 0; i < temp1.Length; i++)
    {
      temp2[i] = temp1[i].Split(' ');
    }
    levelArr = new int[temp2.Length][];
    for (int i = 0; i < temp2.Length; i++)
    {
      levelArr[i] = new int[temp2[0].Length];
      for (int j = 0; j < temp2[0].Length; j++)
      {
        if (temp2[i][j].Trim().Length > 0) {
          levelArr[i][j] = Int32.Parse(temp2[i][j]);
        }

        if (levelArr[i][j] == 1)
        {
          Instantiate(tempBlockPrefab, new Vector3(j, -i), Quaternion.Euler(new Vector3(0,0,0)), this.transform);
        } else if (levelArr[i][j] == 2)
        {
          playerPrefab.transform.position = new Vector2(j, -i);
        }
      }
    }

  }

  // Update is called once per frame
  void Update()
  {

  }
}
