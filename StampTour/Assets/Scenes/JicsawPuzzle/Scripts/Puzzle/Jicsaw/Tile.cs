using System;
using System.Collections.Generic;
using UnityEngine;

namespace JicsawPuzzle
{
  public class Tile
  {
    public enum Direction
    {
      UP, DOWN, LEFT, RIGHT,
    }
    public enum PosNegType
    {
      POS,
      NEG,
      NONE,
    }

    // The offset at which the curve will start.
    // For an image of size 140 by 140 it will start at 20, 20.
    //public Vector2Int mOffset = new Vector2Int(20, 20);
    public static int padding = 20;

    // The size of our jigsaw tile.
    public static int tileSize = 100;
    public static int tileSizeWithPadding = padding * 2 + tileSize;

    // The line renderers for all directions and types.
    private Dictionary<(Direction, PosNegType), LineRenderer> mLineRenderers
      = new Dictionary<(Direction, PosNegType), LineRenderer>();

    // Lets store the list of bezier curve points created
    // from the template bezier curve control points.
    public static List<Vector2> BezCurveWidth = BezierCurve.PointList2(TemplateBezierCurve.templateControlPoints, tileSize, 0.0001f);
    public static List<Vector2> BezCurveHeight = BezierCurve.PointList2(TemplateBezierCurve.templateControlPoints, tileSize, 0.0001f);

    // The original texture used to create the jigsaw tile.
    private Texture2D mOriginalTexture;

    public Texture2D finalCut { get; private set; }

    public static readonly Color TransparentColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

    private PosNegType[] mCurveTypes = new PosNegType[4]
    {
      PosNegType.NONE,
      PosNegType.NONE,
      PosNegType.NONE,
      PosNegType.NONE,
    };

    // A 2d boolean array that stores whether a particular
    // pixel is visited. We will need this array for the flood fill.
    private bool[,] mVisited;

    // A stack needed for the flood fill of the texture.
    private Stack<Vector2Int> mStack = new Stack<Vector2Int>();

    public int xIndex = 0;
    public int yIndex = 0;

    // For tiles sorting.
    public void SetCurveType(Direction dir, PosNegType type)
    {
      mCurveTypes[(int)dir] = type;
    }

    public PosNegType GetCurveType(Direction dir)
    {
      return mCurveTypes[(int)dir];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="texture">Cut base Texture</param>
    /// <param name="alpha">Tile Color alpha value.</param>
    public Tile(Texture2D texture)
    {
      mOriginalTexture = texture;
      tileSizeWithPadding = 2 * padding + tileSize ;

      finalCut = new Texture2D(tileSizeWithPadding, tileSizeWithPadding, TextureFormat.ARGB32, false);

      // We initialise this newly created texture with transparent color.
      for (int i = 0; i < tileSizeWithPadding; ++i)
      {
        for (int j = 0; j < tileSizeWithPadding; ++j)
        {
          finalCut.SetPixel(i, j, TransparentColor);
        }
      }
    }

    public void Apply()
    {
      FloodFillInit();
      FloodFill();
      finalCut.Apply();
    }

    /// <summary>
    /// Draw&Caculate Puzzl Cut Line
    /// </summary>
    void FloodFillInit()
    {
      mVisited = new bool[tileSizeWithPadding, tileSizeWithPadding];
      for (int i = 0; i < tileSizeWithPadding; ++i)
      {
        for (int j = 0; j < tileSizeWithPadding; ++j)
        {
          mVisited[i, j] = false;
        }
      }

      List<Vector2> pts = new List<Vector2>();
      for (int i = 0; i < mCurveTypes.Length; ++i)
      {
        pts.AddRange(CreateCurve((Direction)i, mCurveTypes[i]));
      }


      // Now we should have a closed curve.
      for (int i = 0; i < pts.Count; ++i)
      {
        mVisited[(int)pts[i].x, (int)pts[i].y] = true;
      }
      // start from the center.
      Vector2Int start = new Vector2Int(tileSizeWithPadding / 2, tileSizeWithPadding / 2);

      mVisited[start.x, start.y] = true;
      mStack.Push(start);
    }

    /// <summary>
    /// Fill Target Pixel
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void Fill(int x, int y)
    {
      Color c = mOriginalTexture.GetPixel(x + xIndex * tileSize , y + yIndex * tileSize);
      c.a = 1.0f;
      finalCut.SetPixel(x, y, c);
    }

    void FloodFill()
    {
      while (mStack.Count > 0)
      {
        Vector2Int v = mStack.Pop();

        int xx = v.x;
        int yy = v.y;

        Fill(v.x, v.y);

        // Check right.
        int x = xx + 1;
        int y = yy;

        if (x < tileSizeWithPadding)
        {
          Color c = finalCut.GetPixel(x, y);
          if (!mVisited[x, y])
          {
            mVisited[x, y] = true;
            mStack.Push(new Vector2Int(x, y));
          }
        }

        // check left.
        x = xx - 1;
        y = yy;
        if (x > 0)
        {
          Color c = finalCut.GetPixel(x, y);
          if (!mVisited[x, y])
          {
            mVisited[x, y] = true;
            mStack.Push(new Vector2Int(x, y));
          }
        }

        // Check up.
        x = xx;
        y = yy + 1;

        if (y < tileSizeWithPadding)
        {
          Color c = finalCut.GetPixel(x, y);
          if (!mVisited[x, y])
          {
            mVisited[x, y] = true;
            mStack.Push(new Vector2Int(x, y));
          }
        }

        // Check down.
        x = xx;
        y = yy - 1;

        if (y >= 0)
        {
          Color c = finalCut.GetPixel(x, y);
          if (!mVisited[x, y])
          {
            mVisited[x, y] = true;
            mStack.Push(new Vector2Int(x, y));
          }
        }
      }
    }

    public static LineRenderer CreateLineRenderer(UnityEngine.Color color, float lineWidth = 1.0f)
    {
      GameObject obj = new GameObject();
      LineRenderer lr = obj.AddComponent<LineRenderer>();

      lr.startColor = color;
      lr.endColor = color;
      lr.startWidth = lineWidth;
      lr.endWidth = lineWidth;
      lr.material = new Material(Shader.Find("Sprites/Default"));
      return lr;
    }

    public static void TranslatePoints(List<Vector2> iList, Vector2 offset)
    {
      for (int i = 0; i < iList.Count; i++)
      {
        iList[i] += offset;
      }
    }

    public static void InvertY(List<Vector2> iList)
    {
      for (int i = 0; i < iList.Count; i++)
      {
        iList[i] = new Vector2(iList[i].x, -iList[i].y);
      }
    }

    public static void SwapXY(List<Vector2> iList)
    {
      for (int i = 0; i < iList.Count; ++i)
      {
        iList[i] = new Vector2(iList[i].y, iList[i].x);
      }
    }

    /// <summary>
    /// Draw Tile 1 Direction Cut Outline
    /// </summary>
    /// <param name="dir">Cut line direction</param>
    /// <param name="type">Line type</param>
    /// <returns></returns>
    public List<Vector2> CreateCurve(Direction dir, PosNegType type)
    {
      int padding_x = padding;
      int padding_y = padding;
      int sw = tileSize;
      int sh = tileSize;

      List<Vector2> pts = new();
      switch (dir)
      {
        case Direction.UP:
          pts = new List<Vector2>(BezCurveWidth);
          if (type == PosNegType.POS)
          {
            TranslatePoints(pts, new Vector2(padding_x,  sh + padding_y));
          }
          else if (type == PosNegType.NEG)
          {
            InvertY(pts);
            TranslatePoints(pts, new Vector2(padding_x,  sh + padding_y));
          }
          else
          {
            pts.Clear();
            for (int i = 0; i < sw; ++i)
            {
              pts.Add(new Vector2(i + padding_x, padding_y + sh));
            }
          }
          break;
        case Direction.RIGHT:
          pts = new List<Vector2>(BezCurveHeight);
          if (type == PosNegType.POS)
          {
            SwapXY(pts);
            TranslatePoints(pts, new Vector2( sw + padding_x, padding_y));

          }
          else if (type == PosNegType.NEG)
          {
            InvertY(pts);
            SwapXY(pts);
            TranslatePoints(pts, new Vector2( sw + padding_x, padding_y));
          }
          else
          {
            pts.Clear();
            for (int i = 0; i < sh; ++i)
            {
              pts.Add(new Vector2( sw + padding, i + padding_y));
            }
          }
          break;
        case Direction.DOWN:
          pts = new List<Vector2>(BezCurveWidth);
          if (type == PosNegType.POS)
          {
            InvertY(pts);
            TranslatePoints(pts, new Vector2(padding_x, padding_y));
          }
          else if (type == PosNegType.NEG)
          {
            TranslatePoints(pts, new Vector2(padding_x, padding_y));
          }
          else
          {
            pts.Clear();
            for (int i = 0; i < sw; ++i)
            {
              pts.Add(new Vector2(i + padding_x, padding_y));
            }
          }
          break;
        case Direction.LEFT:
          pts = new List<Vector2>(BezCurveHeight);
          if (type == PosNegType.POS)
          {
            InvertY(pts);
            SwapXY(pts);
            TranslatePoints(pts, new Vector2(padding_x, padding_y));
          }
          else if (type == PosNegType.NEG)
          {
            SwapXY(pts);
            TranslatePoints(pts, new Vector2(padding_x, padding_y));
          }
          else
          {
            pts.Clear();
            for (int i = 0; i < sh; ++i)
            {
              pts.Add(new Vector2(padding_x, i + padding_y));
            }
          }
          break;
      }
      return pts;
    }

    public void DrawCurve(Direction dir, PosNegType type, UnityEngine.Color color)
    {
      if (!mLineRenderers.ContainsKey((dir, type)))
      {
        mLineRenderers.Add((dir, type), CreateLineRenderer(color));
      }

      LineRenderer lr = mLineRenderers[(dir, type)];
      lr.gameObject.SetActive(true);
      lr.startColor = color;
      lr.endColor = color;
      lr.gameObject.name = "LineRenderer_" + dir.ToString() + "_" + type.ToString();
      List<Vector2> pts = CreateCurve(dir, type);

      lr.positionCount = pts.Count;
      for (int i = 0; i < pts.Count; ++i)
      {
        lr.SetPosition(i, pts[i]);
      }
    }

    public void HideAllCurves()
    {
      foreach (var item in mLineRenderers)
      {
        item.Value.gameObject.SetActive(false);
      }
    }

    public void DestroyAllCurves()
    {
      foreach (var item in mLineRenderers)
      {
        GameObject.Destroy(item.Value.gameObject);
      }

      mLineRenderers.Clear();
    }

  }
}