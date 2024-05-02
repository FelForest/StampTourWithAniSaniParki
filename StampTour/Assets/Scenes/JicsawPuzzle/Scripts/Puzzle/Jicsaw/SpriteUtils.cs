using UnityEngine;

public class SpriteUtils
{
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

  public static Texture2D LoadTexture(string resourcePath)
  {
    Texture2D tex = Resources.Load<Texture2D>(resourcePath);
    return tex;
  }

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

  public static Texture2D ResizeTexture(Texture2D source, int width, int height)
  {
    RenderTexture renderTexture = new RenderTexture(width, height, 0);
    Graphics.Blit(source, renderTexture);
    Texture2D newTexture = new Texture2D(width, height);
    newTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
    newTexture.Apply();
    RenderTexture.active = null;
    renderTexture.Release();
    return newTexture;
  }
}
