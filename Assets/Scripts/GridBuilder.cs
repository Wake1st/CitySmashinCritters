using UnityEngine;

public class GridBuilderProps
{
  //  points must be a min distance from other points, else they may intersect
  [Min(0f)]
  float minimumIntersectionDistance;

  //  weighted value to determine if a line should end or cross an existing line
  [Range(0f, 1f)]
  float crossoverPossibility;

  //  weighted value to decide if two close points should intersect or not
  [Range(0f, 1f)]
  float intersectionPossibility;

  //  weighted value to determin if the line moves "inward"(+) or "outward"(-)
  [Range(-1f, 1f)]
  float entropy;

  //  the "origin" of the grid
  Vector3 origin;

  //  the minimum distance from the origin which lines should not cross
  [Min(0f)]
  float minimumGridRadial = 0;

  //  the maximum distance from the origin which lines should not cross
  float maximumGridRadial;
};

public class GridBuilder
{
  GridBuilderProps props;

  public GridBuilder(GridBuilderProps props)
  {
    this.props = props;
  }

  public void BuildGrid()
  {
    //  draw line, storing result in array
    //  pick a parent line from existing lines, off of which a new line will be drawn
    //  repeat

    Debug.DrawLine(new Vector3(2, 0.1f, 4), new Vector3(2, 0.1f, 3));
  }
}
