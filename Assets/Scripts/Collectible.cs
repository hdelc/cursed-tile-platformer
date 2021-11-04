using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Collectible : MonoBehaviour
{
  [SerializeField] GameObject parent;
  [SerializeField] float timeBeforeDestroy = 1f;
  [SerializeField] Timer timer;
  private bool grabbed = false;
  private Animator anim;

  // Start is called before the first frame update
  void Start()
  {
    anim = GetComponent<Animator>();
    int index = (int)Math.Round(UnityEngine.Random.Range(0f, parent.transform.childCount - 1));
    Debug.Log(index);
    transform.position = parent.transform.GetChild(index).position;
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    Debug.Log("trigger");
    if (!grabbed)
    {
      grabbed = true;

      DoAThing();

      GameObject newObj = Instantiate(gameObject);
      newObj.name = "Collectible";
      anim.Play("collected");

      // Collectible c = gameObject.GetComponent<Collectible>();
      // c.Start();
    }
  }

  void Update()
  {
    if (grabbed)
    {
      StartCoroutine("DelayedDestroy");
    }
  }
  
  IEnumerator DelayedDestroy()
  {
    yield return new WaitForSeconds(timeBeforeDestroy);
    Destroy(gameObject);
  }

  void DoAThing()
  {
    timer.resetCountdown();
  }
}
