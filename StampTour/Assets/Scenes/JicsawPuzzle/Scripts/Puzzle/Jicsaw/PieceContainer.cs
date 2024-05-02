using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PieceContainer : MonoBehaviour
{
    [Header("Reference")]
    [HideInInspector]
    public RectTransform rectTransform;
    [SerializeField]
    protected RectTransform contentsRectTransfrom;
    [SerializeField]
    protected GridLayoutGroup gridLayoutGroup;
    [Header("Settings")]
    [SerializeField]
    protected int _constraintCount = 2;
    public int ConstraintCount
    {
        get { return _constraintCount; }
        set { _constraintCount = value; MaxCount = _maxCount;}
    }
    protected Vector2 _cellSize;
    public Vector2 CellSize
    {
        get { return _cellSize; }
        protected set { _cellSize = value; }
    }
    protected int _maxCount;
    public int MaxCount
    {
        get
        {
            return _maxCount;
        }
        set
        {
            _maxCount = value;
            
            int colCount = Mathf.CeilToInt((float)_maxCount / ConstraintCount);
            int rowCount = ConstraintCount;
            SetCellSizeWithCount(colCount, rowCount);
        }
    }

    public Vector2 Size => new Vector2(rectTransform.rect.width, rectTransform.rect.height);
    public int Count => Pieces.Count;
    protected int reserveCount = 0;
    [HideInInspector]
    public List<RectTransform> Pieces = new List<RectTransform>();

#if UNITY_EDITOR
    private void OnValidate() {
        gridLayoutGroup.constraintCount = ConstraintCount;
    }
#endif

    private void Start() {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        if (contentsRectTransfrom == null)
        {
            contentsRectTransfrom = GetComponentsInChildren<RectTransform>().Where(t => t.gameObject != gameObject).First();
        }

        if (gridLayoutGroup == null)
        {
            gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
        }
    }

    public void Reset()
    {
        Pieces.Clear();
    }

    public bool PushReserve()
    {
        if (!CanPush())
        {
            return false;
        }

        reserveCount++;
        return true;
    }

    public bool Push(RectTransform targetRectTransfrom)
    {
        if (reserveCount > 0)
        {
            reserveCount--;
        }

        if (!CanPush())
        {
            return false;
        }

        GameObject pieceBox = new GameObject("PieceBox " + Count);
        pieceBox.transform.SetParent(contentsRectTransfrom);
        pieceBox.transform.localScale = Vector3.one;
        pieceBox.AddComponent<RectTransform>().localPosition = Vector3.zero;
        pieceBox.AddComponent<CanvasRenderer>();
        targetRectTransfrom.SetParent(pieceBox.transform);
        targetRectTransfrom.anchorMin = new Vector2(0.5f, 0.5f);
        targetRectTransfrom.anchorMax = new Vector2(0.5f, 0.5f);
        targetRectTransfrom.anchoredPosition = Vector3.zero;
        Pieces.Add(targetRectTransfrom);

        return true;
    }

    public bool CanPush()
    {
        return Count + reserveCount < MaxCount;
    }

    public void SetCellSizeWithCount(int column, int row)
    {
        CellSize = new Vector2(Size.x / column, Size.y / row);
        gridLayoutGroup.cellSize = CellSize;
    }
}