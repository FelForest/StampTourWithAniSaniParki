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
  private Image boardImage;
  [SerializeField]
  private PieceContainer[] pieceContainers;

  Sprite mBaseSpriteOpaque;
  Sprite mBaseSpriteTransparent;

  GameObject mGameObjectOpaque;

  [Space(10), Header("Setting")]
  [SerializeField]
  private string imageFilePath;
  [SerializeField]
  private float ghostTransparency = 0.7f;
  [Min(1)]
  public int NumTileX = 1;
  [Min(1)]
  public int NumTileY = 1;
  public int TotalTileCount = 0;

  protected int _fitTileCount = 0;

  Tile[,] mTiles = null;
  GameObject[,] mTileGameObjects= null;

  public Transform parentForTiles = null;
  
  private List<Coroutine> activeCoroutines = new List<Coroutine>();

  private Vector2 tileSize;

  [Space(10), Header("TileAudio")]
  public AudioClip TilePointerDownSFX;
  public AudioClip TilePointerUpSFX;


  Texture2D ResizeTexture(Texture2D source, int width, int height)
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

  Sprite LoadBaseTexture(string filePath, float colorAlpha)
  {
    Texture2D tex = SpriteUtils.LoadTexture(filePath);
    if (!tex.isReadable)
    {
      Debug.Log("Error: Texture is not readable");
      return null;
    }

    tex = ResizeTexture(tex, 100 * NumTileX, 100 * NumTileY);
    // Debug.Log($"[JicsawPuzzle] {filePath} Texture Size: ({tex.width}, {tex.height})");
    
    // Add padding to the image.
    Texture2D newTex = new Texture2D(
        tex.width + Tile.padding * 2,
        tex.height + Tile.padding * 2,
        TextureFormat.ARGB32,
        false);

    // Copy the colours.
    for (int x = 0; x < tex.width; ++x)
    {
      for (int y = 0; y < tex.height; ++y)
      {
        Color color = tex.GetPixel(x, y);
        color.a = colorAlpha;
        newTex.SetPixel(x + Tile.padding , y + Tile.padding, color);
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

    boardImage.sprite = mBaseSpriteTransparent;
    boardImage.rectTransform.anchorMin = new Vector2(0, 0);
    boardImage.rectTransform.anchorMax = new Vector2(1, 1);
    boardImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);

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
    image.rectTransform.localPosition = new Vector3(tile.xIndex * imageSize.x + imageSize.x / 2 - boardImage.rectTransform.rect.width/2,
                                                     tile.yIndex * imageSize.y + imageSize.y / 2 - boardImage.rectTransform.rect.height/2, 0.0f);

    BoxCollider box = obj.AddComponent<BoxCollider>();

    TileMovement tileMovement = obj.AddComponent<TileMovement>();
    tileMovement.tile = tile;
    tileMovement.canvas = locateCanvas.GetComponent<Canvas>();
    tileMovement.PointerUpSFX = TilePointerUpSFX;
    tileMovement.PointerDownSFX = TilePointerDownSFX;
    tileMovement.TileFitParent = parentForTiles;

    return obj;
  }

  IEnumerator Coroutine_CreateJigsawTiles()
  {
    Texture2D baseTexture = mBaseSpriteOpaque.texture;
    float aspect = Tile.padding / (float)Tile.tileSize;
    float imageTileSizeX =  (boardImage.rectTransform.rect.width / NumTileX);
    float imageTilePaddingX = (imageTileSizeX * aspect);
    float imageTileSizeY =  (boardImage.rectTransform.rect.height / NumTileY);
    float imageTilePaddingY = (imageTileSizeY * aspect);
    Vector2 imageTileSize = new Vector2(imageTileSizeX, imageTileSizeY);
    Vector2 imageTilePadding = new Vector2(imageTilePaddingY, imageTilePaddingY);
    tileSize = new Vector2(imageTileSizeX + 2*imageTilePaddingX, imageTileSizeY + 2*imageTilePaddingY);
    Vector2Int textureTileSize = new Vector2Int((int)(baseTexture.width / NumTileX), (int)(baseTexture.height / NumTileY));

    mTiles = new Tile[NumTileX, NumTileY];
    mTileGameObjects = new GameObject[NumTileX, NumTileY];

    TotalTileCount = NumTileX * NumTileY;

    for (int i = 0; i < NumTileX; i++)
    {
      for (int j = 0; j < NumTileY; j++)
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
    if (i == NumTileX - 1)
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
    if(j == NumTileY - 1)
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

  private IEnumerator Coroutine_MoveOverSeconds(PieceContainer pieceContainers, RectTransform objectToMove, Vector3 end, float seconds)
  {
      float elaspedTime = 0.0f;
      Vector3 startingPosition = objectToMove.position;
      while(elaspedTime < seconds)
      {
      objectToMove.position = Vector3.Lerp(
          startingPosition, end, (elaspedTime / seconds));
      elaspedTime += Time.deltaTime;

      yield return new WaitForEndOfFrame();
      }
      // objectToMove.localPosition = end;

      pieceContainers.Push(objectToMove);
  }

  void Shuffle(GameObject obj)
  {
    if(pieceContainers.Length == 0)
    {
      Debug.LogError($"{name} : pieceContainers Null");
    }

    obj.TryGetComponent(out RectTransform objRectTransform);
    obj.TryGetComponent(out TileMovement objTileMovement);
    
    // Select pieceContainers
    int regionIndex = UnityEngine.Random.Range(0, pieceContainers.Length);

    if (!pieceContainers[regionIndex].CanPush())
    {
      for(int i = 0; i < pieceContainers.Length; i++)
      {
        if (pieceContainers[i].CanPush())
        {
          regionIndex = i;
          break;
        }
      }
    }

    // Occupies a spot in the list.
    pieceContainers[regionIndex].PushReserve();

    // Get Random Move End Pos
    float regionWidth = pieceContainers[regionIndex].Size.x;
    float regionHeight = pieceContainers[regionIndex].Size.y;
    Vector2 regionCenter = new Vector2(regionWidth / 2, regionHeight / 2);
    Vector2 regionLocalPosition = transform.localPosition;

    float aspectWidth = pieceContainers[regionIndex].CellSize.x / tileSize.x;
    float aspectHeight = pieceContainers[regionIndex].CellSize.y / tileSize.y;
    float aspect = MathF.Min(aspectWidth, aspectHeight);
    objTileMovement.SmalScale = aspect;
    
    Vector3 pos = new Vector3(pieceContainers[regionIndex].rectTransform.position.x, pieceContainers[regionIndex].rectTransform.position.y, pieceContainers[regionIndex].rectTransform.position.z);
    
    // Push Tile to Container
    Coroutine moveCoroutine = StartCoroutine(Coroutine_MoveOverSeconds(pieceContainers[regionIndex], objRectTransform, pos, 1.0f));
    activeCoroutines.Add(moveCoroutine);
  }

  IEnumerator Coroutine_Shuffle()
  {
    // Set Piece Container
    int totalTileNum = NumTileX * NumTileY;
      // Prevent odd divide.
    int containerMaxCount = Mathf.CeilToInt((float)totalTileNum / pieceContainers.Length );
    foreach (var container in pieceContainers)
    {
      container.MaxCount = containerMaxCount;
    }
    // Suffle Tile
    for(int i = 0; i < NumTileX; ++i)
    {
      for(int j = 0; j < NumTileY; ++j)
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

    for(int i = 0; i < NumTileX; ++i)
    {
      for(int j = 0; j < NumTileY; ++j)
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
