using UnityEditor;
using UnityEngine;

public class SpriteUtils
{
  /// <summary>
  /// Create Sprite Using input Texture
  /// </summary>
  /// <param name="spriteTexture">Target Texture</param>
  /// <param name="x">start x</param>
  /// <param name="y">start y</param>
  /// <param name="w">sprite width</param>
  /// <param name="h">sprite height</param>
  /// <param name="pixelsPerUnit">sprite pixelsPerUnit</param>
  /// <param name="spriteType">sprite mesh type</param>
  /// <returns>new sprite</returns>
  public static Sprite CreateSpriteFromTexture2D(
    Texture2D spriteTexture,
    int x,
    int y,
    int w,
    int h,
    float pixelsPerUnit = 1.0f,
    SpriteMeshType spriteType = SpriteMeshType.Tight)
  {
    Sprite newSprite = Sprite.Create(
      spriteTexture,
      new Rect(x, y, w, h),
      new Vector2(0, 0),
      pixelsPerUnit,
      0,
      spriteType);
    return newSprite;
  }

  /// <summary>
  /// Load Texture type in "Resources" Folder Path.
  /// </summary>
  /// <param name="resourcePath">Below Target File ResourcePath</param>
  /// <returns>new Texture2D </returns>
  public static Texture2D LoadTexture(string resourcePath)
  {
    Texture2D tex = Resources.Load<Texture2D>(resourcePath);
    return tex;
  }

  /// <summary>
  /// First Use LoadTexture, next resize texture and create sprite.
  /// </summary>
  /// <param name="filePath">target file Paht in below "Resources" Folder.</param>
  /// <param name="width">texture resize width</param>
  /// <param name="height">texture resize hegiht</param>
  /// <returns>new sprite</returns>
  public static Sprite LoadSprite(string filePath, int width = 0, int height = 0)
  {
    Texture2D tex = LoadTexture(filePath);
    if (!tex.isReadable)
    {
      Debug.Log("Error: Texture is not readable");
      return null;
    }

    if (width != 0 && height != 0)
    {
      tex = ResizeTexture(tex, width, height);
    }

    Sprite sprite = CreateSpriteFromTexture2D(
        tex,
        0,
        0,
        tex.width,
        tex.height);
    return sprite;
  }

  /// <summary>
  /// Texture Size Resize.
  /// </summary>
  /// <param name="source">Target Texture</param>
  /// <param name="width">texture resize width</param>
  /// <param name="height">texture resize height</param>
  /// <returns></returns>
  public static Texture2D ResizeTexture(Texture2D source, int width, int height)
  {
    source.filterMode = FilterMode.Bilinear;
    RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
    renderTexture.filterMode = FilterMode.Bilinear;
    RenderTexture.active = renderTexture;
    Graphics.Blit(source, renderTexture);
    Texture2D newTexture = new Texture2D(width, height);
    newTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
    newTexture.Apply();
    RenderTexture.active = null;
    RenderTexture.ReleaseTemporary(renderTexture);
    return newTexture;
  }

}
