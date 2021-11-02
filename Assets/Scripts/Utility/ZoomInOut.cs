using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInOut : MonoBehaviour
{
  private Transform t;
  [SerializeField] float min = 0.8f;
  [SerializeField] float max = 1.2f;
  [SerializeField] float speed = 1.0f;

  void Start()
  {
    t = GetComponent<Transform>();
  }

  void Update()
  {
    t.localScale = new Vector3(1,1,1) *
      ((max - min) * ((1 + Mathf.Sin(Time.time * speed)) / 2) + min);
  }
}
