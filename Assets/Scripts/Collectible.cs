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

  [SerializeField] AudioClip sfx;
  [SerializeField] float vol = 0.5f;
  private AudioSource audio;

  // Start is called before the first frame update
  void Start()
  {
    audio = GetComponent<AudioSource>();

    anim = GetComponent<Animator>();
    int index = -1;
    while (index == -1 || (parent.transform.GetChild(index).position.x - GameObject.FindObjectOfType<PlayerMovement>().transform.position.x <= 1 && parent.transform.GetChild(index).position.y - GameObject.FindObjectOfType<PlayerMovement>().transform.position.y <= 1))
    index = (int)Math.Round(UnityEngine.Random.Range(0f, parent.transform.childCount - 1));
    transform.position = parent.transform.GetChild(index).position;
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    Debug.Log("trigger");
    if (!grabbed)
    {
      grabbed = true;

      audio.PlayOneShot(sfx, vol);
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
