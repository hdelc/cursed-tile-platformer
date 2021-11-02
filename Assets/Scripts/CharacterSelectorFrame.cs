using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectorFrame : MonoBehaviour
{
  [SerializeField] private GameObject bitboy;
  [SerializeField] private GameObject jankman;
  [SerializeField] private GameObject constance;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    Vector3 newPos = new Vector3(0, transform.position.y, transform.position.z);
    switch (GlobalData.characterIndex)
    {
      case 0:
        newPos.x = bitboy.transform.position.x;
        break;
      case 1:
        newPos.x = jankman.transform.position.x;
        break;
      case 2:
        newPos.x = constance.transform.position.x;
        break;
    }
    transform.position = newPos;
  }
}
