using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CycleImageColor : MonoBehaviour
{
  private Image img;
  [SerializeField] private float speed = 1f;
  [SerializeField] private float saturation = 1f;
  [SerializeField] private float value = 1f;

  void Start()
  {
    img = GetComponent<Image>();
  }

  // Update is called once per frame
  void Update()
  {
    img.color = Color.HSVToRGB((speed * Time.time) % 1, saturation, value);
  }
}
