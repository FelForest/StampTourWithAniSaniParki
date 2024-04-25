using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TileMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
  public Canvas canvas;
  public Tile tile { get; set; }
  public Vector2 OriginPosition;
  protected float _smalScale = 0.0f;
  public float SmalScale
  {
    get { return _smalScale; }
    set { _smalScale = value; SetScale(_smalScale); }
  }

  public delegate void DelegateOnTileInPlace(TileMovement tm);
  public DelegateOnTileInPlace onTileInPlace;
  private RectTransform rectTransform;
  public float MaxDist = 30.0f;
  public float dist;
  [SerializeField]
  private Vector2 currentPos;
  [SerializeField]
  private Vector2 fitPos;

  private Vector3 startPos;

  [Header("Audio")]
  public AudioClip PointerDownSFX;
  public AudioClip PointerUpSFX;

  private void Awake() {
    TryGetComponent(out rectTransform);
  }

  private void Start()
  {
    OriginPosition = rectTransform.anchoredPosition;
  }

  public void SetScale(float scale)
  {
    transform.localScale = Vector3.one * scale;
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    Debug.Log(name);
    SetScale(1.0f);
    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out Vector2 pos);
    transform.position = canvas.transform.TransformPoint(pos);
    startPos = transform.position;
    rectTransform.SetAsLastSibling();

    AudioController.Instance.PlaySFXOneShot(PointerDownSFX);
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    fitPos =  OriginPosition;
    currentPos = rectTransform.anchoredPosition;
    dist = (currentPos - fitPos).magnitude;
    if(dist < MaxDist)
    {
      rectTransform.anchoredPosition = OriginPosition;
      onTileInPlace?.Invoke(this);
      Debug.Log("FIt");
      AudioController.Instance.PlaySFXOneShot(PointerUpSFX);
    }
    else
    {
      transform.position = startPos;
      SetScale(SmalScale);
    }
  }

  public void OnDrag(PointerEventData eventData)
  {
    Debug.Log("EEE");

    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out Vector2 pos);
    transform.position = canvas.transform.TransformPoint(pos);
  }

}
