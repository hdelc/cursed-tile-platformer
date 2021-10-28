using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomEdgeCollider : MonoBehaviour
{
    private EdgeCollider2D bottomEdgeCollider;

    // Start is called before the first frame update
    void Start()
    {
        bottomEdgeCollider = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
      if (collision.otherCollider == this.bottomEdgeCollider)
      {
        SendMessageUpwards("RefreshJump");
      }
    }

}
