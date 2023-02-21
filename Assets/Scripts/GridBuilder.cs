using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GridBuilderProps
{
  //  how many lines are there to be drawn
  [Min(0)]
  public float lineCount;

  //  points must be a min distance from other points, else they may intersect
  [SerializeField]
  [Min(0f)]
  public float minimumIntersectionDistance;

  //  weighted value to determine if a line should end or cross an existing line
  [SerializeField]
  [Range(0f, 1f)]
  public float crossoverPossibility;

  //  weighted value to decide if two close points should intersect or not
  [SerializeField]
  [Range(0f, 1f)]
  public float intersectionPossibility;

  //  weighted value to determin if the line moves "outward"(+) or "inward"(-)
  [SerializeField]
  [Range(-1f, 1f)]
  public float entropy;

  //  the "origin" of the grid
  [SerializeField]
  public Vector3 origin;

  //  the minimum distance from the origin which lines should not cross
  [SerializeField]
  [Min(0f)]
  public float minimumGridBoundary = 0;

  //  the maximum distance from the origin which lines should not cross
  [SerializeField]
  public float maximumGridBoundary;

  //  amount the grid is offset from a projected surface
  [SerializeField]
  [Min(0)]
  public float surfaceOffset;

  //  median, or median amount of line distance
  [SerializeField]
  [Min(0)]
  public float medianLineDist;

  //  percentage of median line distance boundary
  [SerializeField]
  [Range(0, 1)]
  public float lineDistancePrecisionBoundary;
};

public class Line
{
  public Line(Vector3 start, Vector3 end)
  {
    Start = start;
    End = end;
    Points = new List<Vector3>() {
      start,
      end
    };
  }

  public Vector3 Start { get; set; }
  public Vector3 End { get; set; }
  public List<Vector3> Points { get; set; }
}

public class GridBuilder
{
  GridBuilderProps props;
  public List<Line> lines;

  public GridBuilder(GridBuilderProps props)
  {
    this.props = props;
    lines = new List<Line>();
  }

  public void BuildGrid()
  {
    //  draw line, storing result in array
    //  pick a parent line from existing lines, off of which a new line will be drawn
    //  repeat

    FirstLine();

    for (int i = 1; i < props.lineCount; i++)
    {
      AddLine();
    }
  }

  private void FirstLine()
  {
    Vector3 startPoint = GenerateFreeStartPoint();
    Vector3 endPoint = GenerateFreeEndPoint(startPoint);

    //  add new line to the list
    lines.Add(new Line(startPoint, endPoint));
  }

  private void AddLine()
  {
    //  keep track of where new line point will be inserted
    int insertIndex = 1;

    //  find parent
    Line parent;

    //  get start point
    Vector3 startPoint = Vector3.zero;
    List<Line> possibleParents = new List<Line>(lines);
    bool hasStart = false;
    do
    {
      parent = possibleParents[
        Mathf.FloorToInt(Random.Range(0, possibleParents.Count))
      ];

      for (int i = 0; i < parent.Points.Count - 1; i++)
      {
        float dist = Vector3.Distance(
          parent.Points[i],
          parent.Points[i + 1]
        );

        if (dist > props.minimumIntersectionDistance)
        {
          float minIntersectRatio = props.minimumIntersectionDistance / dist;
          float lerpFactor = Random.Range(
            minIntersectRatio,
            1 - minIntersectRatio
          );

          startPoint = Vector3.Lerp(
            parent.Start,
            parent.End,
            lerpFactor
          );

          insertIndex = i + 1;
        }
      }

      if (startPoint != Vector3.zero) hasStart = true;

      if (!hasStart)
      {
        possibleParents.Remove(parent);
        startPoint = Vector3.zero;
      }
    } while (!hasStart);

    //  now, get the end point
    Vector3 endpoint;
    bool outOfBounds = true;
    do
    {
      //  TODO: this endpoint must be 
      //  - checking crossovers
      //  - checking intersections

      endpoint = GenerateBranchingEndPoint(
        startPoint,
        parent
      );

      //  now we must check for crossover/intersection
      endpoint = IntersectCheck(startPoint, endpoint, parent);

      //  check if the endpoint is valid
      float endX = Mathf.Abs(endpoint.x);
      float endZ = Mathf.Abs(endpoint.z);
      float minBound = props.minimumGridBoundary;
      float maxBound = props.maximumGridBoundary;

      outOfBounds = (endX > maxBound || endX < minBound)
        && (endZ > maxBound || endZ < minBound);

    } while (outOfBounds);

    //  add it to the list
    lines.Add(new Line(startPoint, endpoint));

    //  add start point to it's parent
    parent.Points.Insert(insertIndex, startPoint);
  }

  private Vector3 GenerateFreeStartPoint()
  {
    float startPointX = Random.Range(
      props.minimumGridBoundary,
      props.maximumGridBoundary
    );
    float startPointZ = Random.Range(
      props.minimumGridBoundary,
      props.maximumGridBoundary
    );

    //  will set the value as pos or neg
    float flipX = (Random.value - 0.5f) >= 0 ? 1 : -1;
    float flipZ = (Random.value - 0.5f) >= 0 ? 1 : -1;

    return new Vector3(
      flipX * startPointX,
      props.surfaceOffset,
      flipZ * startPointZ
    );
  }

  private Vector3 GenerateBranchingStartPoint()
  {
    float startPointX = Random.Range(
      props.minimumGridBoundary,
      props.maximumGridBoundary
    );
    float startPointZ = Random.Range(
      props.minimumGridBoundary,
      props.maximumGridBoundary
    );

    //  will set the value as pos or neg
    float flipX = (Random.value - 0.5f) >= 0 ? 1 : -1;
    float flipZ = (Random.value - 0.5f) >= 0 ? 1 : -1;

    return new Vector3(
      flipX * startPointX,
      props.surfaceOffset,
      flipZ * startPointZ
    );
  }

  private Vector3 GenerateFreeEndPoint(Vector3 startPoint)
  {
    //  we must determine the "outward"(+) direction
    Vector3 longestAxis = startPoint.x > startPoint.z
      ? new Vector3(startPoint.x, props.surfaceOffset, 0)
      : new Vector3(0, props.surfaceOffset, startPoint.z);

    Vector3 outwardUnit = Vector3.Normalize(startPoint - longestAxis);

    //  now, we will check to flip the direction of the line
    float unitFlip = (props.entropy - Random.value) > 0 ? 1 : -1;

    //  get the line distance
    float lineDist = props.medianLineDist
      + props.medianLineDist * Random.Range(
        props.lineDistancePrecisionBoundary - 1,
        1 - props.lineDistancePrecisionBoundary
      );

    //  finally, calculate the end point
    return unitFlip * lineDist * outwardUnit
      + new Vector3(
        outwardUnit.x == 0 ? startPoint.x : 0,
        props.surfaceOffset,
        outwardUnit.z == 0 ? startPoint.z : 0
      );
  }

  private Vector3 GenerateBranchingEndPoint(
    Vector3 startPoint,
    Line parent
  )
  {
    //  we must determine the "outward"(+) direction
    Vector3 outwardUnit = ParentNormal(parent);
    Vector3 toOrigin = props.origin - startPoint;
    float direction = Vector3.Dot(outwardUnit, toOrigin) >= 0 ? 1 : -1;

    //  now, we will check to flip the direction of the line
    float unitFlip = (props.entropy - Random.value) > 0 ? 1 : -1;

    //  get the line distance
    float lineDist = props.medianLineDist
      + props.medianLineDist * Random.Range(
        props.lineDistancePrecisionBoundary - 1,
        1 - props.lineDistancePrecisionBoundary
      );

    //  finally, calculate the end point
    return direction * unitFlip * lineDist * outwardUnit
      + new Vector3(
        outwardUnit.x == 0 ? startPoint.x : 0,
        props.surfaceOffset,
        outwardUnit.z == 0 ? startPoint.z : 0
      );
  }

  private Vector3 ParentNormal(Line parent)
  {
    Vector3 parentVector = parent.Start - parent.End;
    return Vector3.Cross(
      parentVector,
      Vector3.up
    ).normalized;
  }

  //  if the line premeturely ends, the new endpoint is returned
  private Vector3 IntersectCheck(
    Vector3 startPoint,
    Vector3 endPoint,
    Line parent
  )
  {
    //  Collect all intersecting lines
    //  TODO: exclude parent line!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    List<Line> allButParent = new List<Line>(lines);
    allButParent.Remove(parent);
    List<Line> intersectingLines = new List<Line>();
    foreach (Line line in allButParent)
    {
      if (Math.DoIntersect(
        Math.Vector3To2(startPoint),
        Math.Vector3To2(endPoint),
        Math.Vector3To2(line.Start),
        Math.Vector3To2(line.End)
      )) intersectingLines.Add(line);
    }

    //  First, find where the lines cross
    List<(Line, Vector3, float)> crossovers = new List<(Line, Vector3, float)>();
    foreach (Line line in intersectingLines)
    {
      Vector2 flatIntersect = Math.FindIntersect(
        Math.Vector3To2(startPoint),
        Math.Vector3To2(endPoint),
        Math.Vector3To2(line.Start),
        Math.Vector3To2(line.End)
      );
      Vector3 intersect = new Vector3(
        flatIntersect.x,
        props.surfaceOffset,
        flatIntersect.y
      );

      crossovers.Add((
        line,
        intersect,
        Vector3.Distance(startPoint, intersect)
      ));
    }

    //  Now, we must order the lines closest-furthest 
    //  from the start point
    crossovers.Sort((x, y) => x.Item3.CompareTo(y.Item3));

    //  Finally, check each intersection for crossover
    float minIntersectDist = props.minimumIntersectionDistance;
    foreach ((Line, Vector3, float) crossover in crossovers)
    {
      //  check line for points close enough for intersect
      foreach (Vector3 point in crossover.Item1.Points)
      {
        float dist = Vector3.Distance(point, crossover.Item2);
        if (dist < minIntersectDist)
        {
          return point;
        }
      }

      //  now, check for crossover
      if (Random.value > props.crossoverPossibility)
      {
        return crossover.Item2;
      }
    }

    return endPoint;
  }

  public void DrawLines()
  {
    Color lineColor = new Color(1f, 0f, 0f);
    foreach (Line line in lines)
    {
      Debug.DrawLine(line.Start, line.End, lineColor);
    }
  }

  public void DrawLine(int index)
  {
    Color lineColor = new Color(1f, 0f, 0f);
    Debug.DrawLine(
      lines[index].Start,
      lines[index].End,
      lineColor
    );
  }

  public string PrintLineMsg(Line line)
  {
    return "> start: " + line.Start.ToString() +
      " | end: " + line.End.ToString() +
      " | dist: " + Vector3.Distance(line.Start, line.End);
  }
}
