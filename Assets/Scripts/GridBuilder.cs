using System.Collections.Generic;
using UnityEngine;

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
  }

  public Vector3 Start { get; set; }
  public Vector3 End { get; set; }
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
    //  find parent
    Line parent = lines[
      Mathf.FloorToInt(Random.Range(0, lines.Count))
    ];

    //  get start point
    float lerpFactor = Random.Range(
      props.minimumIntersectionDistance,
      1 - props.minimumIntersectionDistance
    );
    Vector3 startPoint = lerpFactor * parent.Start
      + (1 - lerpFactor) * parent.End;

    //  now, get the end point
    Vector3 endpoint;
    bool outOfBounds = true;
    do
    {
      //  TODO: this endpoint must be 
      //  - orthogonal
      //  - checking crossovers
      //  - checking intersections

      endpoint = GenerateBranchingEndPoint(
        startPoint,
        parent
      );

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

  public Vector3 ParentNormal(Line parent)
  {
    Vector3 parentVector = parent.Start - parent.End;
    return Vector3.Cross(
      parentVector,
      Vector3.up
    ).normalized;
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
