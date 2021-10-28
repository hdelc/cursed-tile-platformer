using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomBoxCollider : MonoBehaviour
{
    private BoxCollider2D bottomBoxCollider;

    // Start is called before the first frame update
    void Start()
    {
        bottomBoxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
      if (collision.otherCollider == this.bottomBoxCollider)
      {
        SendMessageUpwards("RefreshJump");
      }
    }

}
