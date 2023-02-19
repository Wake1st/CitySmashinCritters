using System.Collections.Generic;
using UnityEngine;

public class GridBuilderProps
{
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
  public float minimumGridRadial = 0;

  //  the maximum distance from the origin which lines should not cross
  [SerializeField]
  public float maximumGridRadial;

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

    //  create first line
    Vector3 startPoint = GeneratePoint();

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
    Vector3 endPoint = unitFlip * lineDist * outwardUnit
      + new Vector3(
        outwardUnit.x == 0 ? startPoint.x : 0,
        props.surfaceOffset,
        outwardUnit.z == 0 ? startPoint.z : 0
      );

    //  add new line to the list
    lines.Add(new Line(startPoint, endPoint));
  }

  private Vector3 GeneratePoint()
  {
    float startPointX = Random.Range(
      props.minimumGridRadial,
      props.maximumGridRadial
    );
    float startPointY = Random.Range(
      props.minimumGridRadial,
      props.maximumGridRadial
    );

    //  will set the value as pos or neg
    float flipX = (Random.value - 0.5f) >= 0 ? 1 : -1;
    float flipY = (Random.value - 0.5f) >= 0 ? 1 : -1;

    return new Vector3(
      flipX * startPointX,
      props.surfaceOffset,
      flipY * startPointY
    );
  }

  public void DrawLines()
  {
    Color lineColor = new Color(1f, 0f, 0f);
    foreach (Line line in lines)
    {
      Debug.DrawLine(line.Start, line.End, lineColor);
    }
  }
}
