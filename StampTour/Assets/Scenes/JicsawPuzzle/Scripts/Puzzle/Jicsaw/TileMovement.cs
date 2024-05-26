using UnityEngine;
using UnityEngine.EventSystems;

namespace JicsawPuzzle
{
  public class TileMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
  {
    public Canvas canvas;
    public Tile tile { get; set; }
    public Vector3 OriginPosition;
    protected float _smalScale = 0.0f;
    public float SmalScale
    {
      get { return _smalScale; }
      set { _smalScale = value; SetScale(_smalScale); }
    }
    public Transform TileFitParent;

    public delegate void DelegateOnTileInPlace(TileMovement tm);
    public DelegateOnTileInPlace onTileInPlace;
    private RectTransform rectTransform;
    public float MaxDist = 40.0f;

    [SerializeField]
    protected float dist;

    [SerializeField]
    private Vector2 currentPos;
    [SerializeField]
    private Vector2 fitPos;

    private Vector3 startPos;
    private Transform startParent;

    [Header("Audio")]
    public AudioClip PointerDownSFX;
    public AudioClip PointerUpSFX;

    private void Awake() {
      TryGetComponent(out rectTransform);
    }

    private void Start()
    {
      OriginPosition = rectTransform.localPosition;
    }

    public void SetScale(float scale)
    {
      transform.localScale = Vector3.one * scale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      SetScale(1.0f);
      startPos = transform.localPosition;
      startParent = transform.parent;

      RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out Vector2 pos);
      transform.position = canvas.transform.TransformPoint(pos);
      transform.SetParent(TileFitParent);
      rectTransform.SetAsLastSibling();

      AudioController.Instance.PlaySFXOneShot(PointerDownSFX);

      // Debug.Log($"[JicsawPuzzl] {gameObject.name} Tile Click");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      fitPos =  OriginPosition;
      currentPos = rectTransform.localPosition;
      dist = (currentPos - fitPos).magnitude;
      if(dist < MaxDist)
      {
        rectTransform.localPosition = OriginPosition;
        onTileInPlace?.Invoke(this);
        // Debug.Log($"[JicsawPuzzl] {gameObject.name} Tile Fit");
        AudioController.Instance.PlaySFXOneShot(PointerUpSFX);
      }
      else
      {
        transform.SetParent(startParent);
        SetScale(SmalScale);
        transform.localPosition = startPos;

      }
    }

    public void OnDrag(PointerEventData eventData)
    {
      // Debug.Log($"[JicsawPuzzl] {gameObject.name} Tile Draging");

      RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out Vector2 pos);
      transform.position = canvas.transform.TransformPoint(pos);
    }

  }
}