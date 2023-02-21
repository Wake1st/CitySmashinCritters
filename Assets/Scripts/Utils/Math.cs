using UnityEngine;

namespace Utils
{
  public class Math
  {
    //  converts a Vector3 to Vector2 [x=x, y=z]
    public static Vector2 Vector3To2(Vector3 v3)
    {
      return new Vector2(v3.x, v3.z);
    }

    // To find orientation of ordered triplet (p, q, r).
    // The function returns following values
    // 0 --> p, q and r are collinear
    // 1 --> Clockwise
    // 2 --> Counterclockwise
    //  source: https://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/
    public static int Orientation(Vector2 p, Vector2 q, Vector2 r)
    {
      // See https://www.geeksforgeeks.org/orientation-3-ordered-points/
      // for details of below formula.
      float val = (q.y - p.y) * (r.x - q.x) -
              (q.x - p.x) * (r.y - q.y);

      if (val == 0) return 0; // collinear

      return (val > 0) ? 1 : 2; // clock or counterclock wise
    }

    // Given three collinear points p, q, r, the function checks if
    // point q lies on line segment 'pr'
    //  source: https://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/
    static bool onSegment(Vector2 p, Vector2 q, Vector2 r)
    {
      if (q.x <= Mathf.Max(p.x, r.x)
        && q.x >= Mathf.Min(p.x, r.x)
        && q.y <= Mathf.Max(p.y, r.y)
        && q.y >= Mathf.Min(p.y, r.y)
      )
        return true;

      return false;
    }

    // The main function that returns true if line segment 'p1q1'
    // and 'p2q2' intersect.
    //  source: https://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/
    public static bool DoIntersect(
      Vector2 p1,
      Vector2 q1,
      Vector2 p2,
      Vector2 q2
    )
    {
      // Find the four orientations needed for general and
      // special cases
      int o1 = Orientation(p1, q1, p2);
      int o2 = Orientation(p1, q1, q2);
      int o3 = Orientation(p2, q2, p1);
      int o4 = Orientation(p2, q2, q1);

      // General case
      if (o1 != o2 && o3 != o4)
        return true;

      // Special Cases
      // p1, q1 and p2 are collinear and p2 lies on segment p1q1
      if (o1 == 0 && onSegment(p1, p2, q1)) return true;

      // p1, q1 and q2 are collinear and q2 lies on segment p1q1
      if (o2 == 0 && onSegment(p1, q2, q1)) return true;

      // p2, q2 and p1 are collinear and p1 lies on segment p2q2
      if (o3 == 0 && onSegment(p2, p1, q2)) return true;

      // p2, q2 and q1 are collinear and q1 lies on segment p2q2
      if (o4 == 0 && onSegment(p2, q1, q2)) return true;

      return false; // Doesn't fall in any of the above cases
    }

    private static float CrossVec2(Vector2 v1, Vector2 v2)
    {
      return v1.x * v2.y - v1.y * v2.x;
    }

    public static Vector2 FindIntersect(
      Vector2 p1,
      Vector2 p2,
      Vector2 q1,
      Vector2 q2
    )
    {
      Vector2 r = p2 - p1;
      Vector2 s = q2 - q1;

      float rXs = CrossVec2(r, s);
      float t = CrossVec2(q1 - p1, s) / rXs;

      return p1 + t * r;
    }
  }
}