using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
  public static int characterIndex = 2; // new default: constance because please i dont wanna play as bitboy anymore please
  public static string characterName {get {
    if (characterIndex == 0)
    {
      return "Bitboy";
    } else if (characterIndex == 1)
    {
      return "Jankman";
    } else if (characterIndex == 2)
    {
      return "Constance";
    } else {
      return "????????";
    }
  }}
}
