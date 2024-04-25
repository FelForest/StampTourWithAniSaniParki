using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BoardGen : MonoBehaviour
{
  public PuzzleController puzzleController;

  [Header("Reference")]
  [SerializeField]
  private Canvas locateCanvas;
  [SerializeField]
  private string imageFilePath;
  [SerializeField]
  private Image BoardImage;

  Sprite mBaseSpriteOpaque;
  Sprite mBaseSpriteTransparent;

  GameObject mGameObjectOpaque;

  [Space(10), Header("Setting")]
  public float ghostTransparency = 0.7f;

  [HideInInspector]
  public int numTileX = 0;
  [HideInInspector]
  public int numTileY = 0;
  public int TotalTileCount = 0;
  [SerializeField]
  protected int _fitTileCount = 0;

  Tile[,] mTiles = null;
  GameObject[,] mTileGameObjects= null;

  public Transform parentForTiles = null;
  
  [SerializeField]
  private RectTransform[] pieceContainer;
  private List<Coroutine> activeCoroutines = new List<Coroutine>();
  [SerializeField]
  private Vector2 tileSize;

  [Space(10), Header("TileAudio")]
  public AudioClip TilePointerDownSFX;
  public AudioClip TilePointerUpSFX;


  Sprite LoadBaseTexture(string filePath, float colorAlpha)
  {
    Texture2D tex = SpriteUtils.LoadTexture(filePath);
    if (!tex.isReadable)
    {
      Debug.Log("Error: Texture is not readable");
      return null;
    }
    
    // Add padding to the image.
    Texture2D newTex = new Texture2D(
        tex.width ,
        tex.height ,
        TextureFormat.ARGB32,
        false);

    numTileX = tex.width / Tile.tileSize;
    numTileY = tex.height / Tile.tileSize;
    
    // Copy the colours.
    for (int x = 0; x < tex.width; ++x)
    {
      for (int y = 0; y < tex.height; ++y)
      {
        Color color = tex.GetPixel(x, y);
        color.a = colorAlpha;
        newTex.SetPixel(x , y , color);
      }
    }
    newTex.Apply();

    Sprite sprite = SpriteUtils.CreateSpriteFromTexture2D(
        newTex,
        0,
        0,
        newTex.width,
        newTex.height);
    return sprite;
  }

  public void Generate(PuzzleController puzzleController, string imgFilePath = "")
  {
    this.puzzleController = puzzleController;

    if (imgFilePath != string.Empty)
    {
      this.imageFilePath = imgFilePath;
    }

    mBaseSpriteOpaque = LoadBaseTexture(this.imageFilePath, ghostTransparency);
    mBaseSpriteTransparent = LoadBaseTexture(this.imageFilePath, ghostTransparency);

    BoardImage.sprite = mBaseSpriteTransparent;
    BoardImage.rectTransform.anchorMin = new Vector2(0, 0);
    BoardImage.rectTransform.anchorMax = new Vector2(1, 1);
    BoardImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);

    // Create the Jigsaw tiles.
    StartCoroutine(Coroutine_CreateJigsawTiles());
  }

  public GameObject CreateGameObjectFromTile(Tile tile, Vector2 imageSize, Vector2 imagePadding)
  {
    GameObject obj = new GameObject();
    obj.name = "TileGameObe_" + tile.xIndex.ToString() + "_" + tile.yIndex.ToString();
    obj.transform.parent = parentForTiles;
    obj.transform.localRotation = Quaternion.identity;
    obj.transform.localScale = Vector3.one;

    Image image = obj.AddComponent<Image>();
    image.sprite = SpriteUtils.CreateSpriteFromTexture2D(
      tile.finalCut,
      0,
      0,
      Tile.padding * 2 + Tile.tileSize,
      Tile.padding * 2 + Tile.tileSize);
    image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tileSize.x);
    image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tileSize.y);
    image.rectTransform.anchorMin = new Vector2(0, 0);
    image.rectTransform.anchorMax = new Vector2(0, 0);
    image.rectTransform.localPosition = new Vector3(tile.xIndex * imageSize.x + imageSize.x / 2 - BoardImage.rectTransform.rect.width/2,
                                                     tile.yIndex * imageSize.y + imageSize.y / 2 - BoardImage.rectTransform.rect.height/2, 0.0f);

    BoxCollider box = obj.AddComponent<BoxCollider>();

    TileMovement tileMovement = obj.AddComponent<TileMovement>();
    tileMovement.tile = tile;
    tileMovement.canvas = locateCanvas.GetComponent<Canvas>();
    tileMovement.PointerUpSFX = TilePointerUpSFX;
    tileMovement.PointerDownSFX = TilePointerDownSFX;

    return obj;
  }

  IEnumerator Coroutine_CreateJigsawTiles()
  {
    Texture2D baseTexture = mBaseSpriteOpaque.texture;
    float aspect = Tile.padding / (float)Tile.tileSize;
    Debug.Log(BoardImage.rectTransform.rect.width);
    float imageTileSizeX =  (BoardImage.rectTransform.rect.width / numTileX);
    float imageTilePaddingX = (imageTileSizeX * aspect);
    float imageTileSizeY =  (BoardImage.rectTransform.rect.height / numTileY);
    float imageTilePaddingY = (imageTileSizeY * aspect);
    Vector2 imageTileSize = new Vector2(imageTileSizeX, imageTileSizeY);
    Vector2 imageTilePadding = new Vector2(imageTilePaddingY, imageTilePaddingY);
    tileSize = new Vector2(imageTileSizeX + 2*imageTilePaddingX, imageTileSizeY + 2*imageTilePaddingY);
    Vector2Int textureTileSize = new Vector2Int((int)(baseTexture.width / numTileX), (int)(baseTexture.height / numTileY));

    mTiles = new Tile[numTileX, numTileY];
    mTileGameObjects = new GameObject[numTileX, numTileY];

    TotalTileCount = numTileX * numTileY;

    for (int i = 0; i < numTileX; i++)
    {
      for (int j = 0; j < numTileY; j++)
      {
        mTiles[i, j] = CreateTile(i, j, baseTexture, textureTileSize);
        mTileGameObjects[i, j] = CreateGameObjectFromTile(mTiles[i, j], imageTileSize, imageTilePadding);
        if (parentForTiles != null)
        {
          // mTileGameObjects[i, j].gameObject.SetActive(false);
        }

        yield return null;
      }
    }

    ShuffleTiles();
  }


  Tile CreateTile(int i, int j, Texture2D baseTexture, Vector2Int size)
  {
    Tile tile = new Tile(baseTexture);
    tile.xIndex = i;
    tile.yIndex = j;

    // Left side tiles.
    if (i == 0)
    {
      tile.SetCurveType(Tile.Direction.LEFT, Tile.PosNegType.NONE);
    }
    else
    {
      // We have to create a tile that has LEFT direction opposite curve type.
      Tile leftTile = mTiles[i - 1, j];
      Tile.PosNegType rightOp = leftTile.GetCurveType(Tile.Direction.RIGHT);
      tile.SetCurveType(Tile.Direction.LEFT, rightOp == Tile.PosNegType.NEG ?
        Tile.PosNegType.POS : Tile.PosNegType.NEG);
    }

    // Bottom side tiles
    if (j == 0)
    {
      tile.SetCurveType(Tile.Direction.DOWN, Tile.PosNegType.NONE);
    }
    else
    {
      Tile downTile = mTiles[i, j - 1];
      Tile.PosNegType upOp = downTile.GetCurveType(Tile.Direction.UP);
      tile.SetCurveType(Tile.Direction.DOWN, upOp == Tile.PosNegType.NEG ?
        Tile.PosNegType.POS : Tile.PosNegType.NEG);
    }

    // Right side tiles.
    if (i == numTileX - 1)
    {
      tile.SetCurveType(Tile.Direction.RIGHT, Tile.PosNegType.NONE);
    }
    else
    {
      float toss = UnityEngine.Random.Range(0f, 1f);
      if(toss < 0.5f)
      {
        tile.SetCurveType(Tile.Direction.RIGHT, Tile.PosNegType.POS);
      }
      else
      {
        tile.SetCurveType(Tile.Direction.RIGHT, Tile.PosNegType.NEG);
      }
    }

    // Up side tile.
    if(j == numTileY - 1)
    {
      tile.SetCurveType(Tile.Direction.UP, Tile.PosNegType.NONE);
    }
    else
    {
      float toss = UnityEngine.Random.Range(0f, 1f);
      if (toss < 0.5f)
      {
        tile.SetCurveType(Tile.Direction.UP, Tile.PosNegType.POS);
      }
      else
      {
        tile.SetCurveType(Tile.Direction.UP, Tile.PosNegType.NEG);
      }
    }

    tile.Apply();
    return tile;
  }

  #region Shuffling related codes

  private IEnumerator Coroutine_MoveOverSeconds(RectTransform objectToMove, Vector3 end, float seconds)
  {
    float elaspedTime = 0.0f;
    Vector3 startingPosition = objectToMove.localPosition;
    while(elaspedTime < seconds)
    {
      objectToMove.localPosition = Vector3.Lerp(
        startingPosition, end, (elaspedTime / seconds));
      elaspedTime += Time.deltaTime;

      yield return new WaitForEndOfFrame();
    }
    objectToMove.localPosition = end;
  }

  void Shuffle(GameObject obj)
  {
    if(pieceContainer.Length == 0)
    {
      Debug.LogError($"{name} : pieceContainer Null");
    }

    obj.TryGetComponent(out RectTransform objRectTransform);
    obj.TryGetComponent(out TileMovement objTileMovement);
    int regionIndex = UnityEngine.Random.Range(0, pieceContainer.Length);
    float regionWidth = pieceContainer[regionIndex].rect.width;
    float regionHeight = pieceContainer[regionIndex].rect.height;
    Vector2 regionCenter = new Vector2(regionWidth / 2, regionHeight / 2);
    Vector2 regionLocalPosition = pieceContainer[regionIndex].localPosition;

    float aspectWidth = regionWidth / BoardImage.rectTransform.rect.width;
    float aspectHeight = regionHeight / BoardImage.rectTransform.rect.height;
    float aspect = MathF.Min(aspectWidth, aspectHeight);
    objTileMovement.SmalScale = aspect;

    Vector2 objTileSize = tileSize * aspect;

    float minRangeX = regionLocalPosition.x - regionCenter.x + objTileSize.x;
    float maxRangeX = regionLocalPosition.x + regionCenter.x - objTileSize.x;
    float minRangeY = regionLocalPosition.y - regionCenter.y + objTileSize.y;
    float maxRangeY = regionLocalPosition.y + regionCenter.y - objTileSize.y;
    float x = UnityEngine.Random.Range(minRangeX, maxRangeX);
    float y = UnityEngine.Random.Range(minRangeY, maxRangeY);
    
    
    Vector3 pos = new Vector3(x, y, 0.0f);
    Coroutine moveCoroutine = StartCoroutine(Coroutine_MoveOverSeconds(objRectTransform, pos, 1.0f));
    activeCoroutines.Add(moveCoroutine);
    // objRectTransform.localPosition = pos;
  }

  IEnumerator Coroutine_Shuffle()
  {
    for(int i = 0; i < numTileX; ++i)
    {
      for(int j = 0; j < numTileY; ++j)
      {
        Shuffle(mTileGameObjects[i, j]);
        yield return null;
      }
    }

    foreach(var item in activeCoroutines)
    {
      if(item != null)
      {
        yield return null;
      }
    }

    OnFinishedShuffling();
  }

  public void ShuffleTiles()
  {
    StartCoroutine(Coroutine_Shuffle());
  }

  void OnFinishedShuffling()
  {
    activeCoroutines.Clear();

    StartTimer();

    for(int i = 0; i < numTileX; ++i)
    {
      for(int j = 0; j < numTileY; ++j)
      {
        TileMovement tm = mTileGameObjects[i, j].GetComponent<TileMovement>();
        tm.onTileInPlace += OnTileInPlace;
        SpriteRenderer spriteRenderer = tm.gameObject.GetComponent<SpriteRenderer>();
        // Tile.tilesSorting.BringToTop(spriteRenderer);

        tm.gameObject.SetActive(true);
      }
      
    }

  }

  IEnumerator Coroutine_CallAfterDelay(System.Action function, float delay)
  {
    yield return new WaitForSeconds(delay);
    function();
  }


  public void StartTimer()
  {
    StartCoroutine(Coroutine_Timer());
  }

  IEnumerator Coroutine_Timer()
  {
    while(true)
    {
      yield return new WaitForSeconds(1.0f);
    }
  }

  public void StopTimer()
  {
    StopCoroutine(Coroutine_Timer());
  }

  #endregion

  public void ShowOpaqueImage()
  {
    mGameObjectOpaque.SetActive(true);
  }

  public void HideOpaqueImage()
  {
    mGameObjectOpaque.SetActive(false);
  }

  void OnTileInPlace(TileMovement tm)
  {
    tm.enabled = false;
    Destroy(tm);

    if (++_fitTileCount >= TotalTileCount)
    {
      ClearPuzzle();
    }
  }

  public void ClearPuzzle()
  {
    Debug.Log("Clear!");
    puzzleController.Pass();
  }
}
