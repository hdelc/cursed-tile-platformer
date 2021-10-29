using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class PathGenerator
{
  public static List<Vector2> GenerateEllipticalPath(Vector2 size, int smoothness = 20)
  {
    List<Vector2> points = new List<Vector2>();
    float angleDelta = 2 * Mathf.PI / smoothness;
    for (int i=0; i<smoothness; i++)
    {
      points.Add(new Vector2(Mathf.Cos(i * angleDelta) * size.x / 2, Mathf.Sin(i * angleDelta) * size.y / 2));
    }
    return points;
  }
}
