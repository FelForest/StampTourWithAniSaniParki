using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace iLLi.Game.UI {
    /// <summary>
    /// TODO: Not only work for row constraint, Working with any constraint
    /// </summary>
    [RequireComponent(typeof(GridLayoutGroup))]
    public class GridLayoutResolutionSize : UIBehaviour 
    {
        public enum ResoultionType
        {
            Square,
            Rectangle,
        }

        [SerializeField]
        ResoultionType _resolutionType = ResoultionType.Square;

        [SerializeField]
        Vector2 _defaultTargetSize = new Vector2(100, 100);
        [SerializeField]
        Vector2 _defaultCellSize = new Vector2(100, 100);
        [SerializeField]
        Vector2 _defaultSpacingSize = new Vector2(100, 100);
    #if UNITY_EDITOR
        [SerializeField]
        Vector2 _defaultRectSize = new Vector2(100, 100);
    #endif

        [SerializeField]
        RectTransform _targetRect;
        [SerializeField]
        RectTransform _myRect;
        [SerializeField]
        GridLayoutGroup _gridLayoutGroup;
        

        protected override void Start()
        {
            base.Start();
            
            ChangeResolutionGrid();
        }

    #if UNITY_EDITOR
        /// <summary>
        /// Only Use EditMode!
        /// </summary>
        [ContextMenu("GetStandardSize")]
        private void GetStandardSize()
        {
            if (_myRect == null)
            {
                _myRect = GetComponent<RectTransform>();
            }
            _defaultRectSize = new Vector2(_myRect.rect.width, _myRect.rect.height);

            if (_gridLayoutGroup == null)
            {
                _gridLayoutGroup = GetComponent<GridLayoutGroup>();
            }
            if (_targetRect == null)
            {
                _targetRect = transform.parent.GetComponent<RectTransform>();
            }
            _defaultTargetSize = _targetRect.rect.size;
            _defaultCellSize = _gridLayoutGroup.cellSize;
            _defaultSpacingSize = _gridLayoutGroup.spacing;
        }

        [ContextMenu("ResetResolution")]
        private void ResetResolution()
        {
            if (_myRect == null)
            {
                _myRect = GetComponent<RectTransform>();
            }

            if (_gridLayoutGroup == null)
            {
                _gridLayoutGroup = GetComponent<GridLayoutGroup>();
            }
            if (_targetRect == null)
            {
                _targetRect = transform.parent.GetComponent<RectTransform>();
            }

            _gridLayoutGroup.cellSize = _defaultCellSize;
            _gridLayoutGroup.spacing = _defaultSpacingSize;
            _myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _defaultRectSize.x);
            _myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _defaultRectSize.y);

        }
    #endif

        [ContextMenu("ChangeResolutionGrid")]
        public void ChangeResolutionGrid()
        {
            switch (_resolutionType)
            {
                case ResoultionType.Square:
                    ChangeResolutionSquareGrid();
                    break;
                case ResoultionType.Rectangle:
                    ChangeResolutionRectangleGrid();
                    break;
            }
        }

        protected Vector2 GetGridInterpolation()
        {
            if (_gridLayoutGroup == null)
            {
                _gridLayoutGroup = GetComponent<GridLayoutGroup>();
            }
            if (_targetRect == null)
            {
                _targetRect = transform.parent.GetComponent<RectTransform>();
            }
            // Exception Handle : _standardSize 0 divine infinity error;
            _defaultTargetSize.x = Mathf.Max(_defaultTargetSize.x, 1.0f);
            _defaultTargetSize.y = Mathf.Max(_defaultTargetSize.y, 1.0f);

            float gridInterpolationX = _targetRect.rect.width/_defaultTargetSize.x;
            float gridInterpolationY = _targetRect.rect.height/_defaultTargetSize.y;

            return new Vector2(gridInterpolationX, gridInterpolationY);
        }

        protected virtual void ChangeResolutionRectangleGrid()
        {
            Vector2 gridInterpolation = GetGridInterpolation();

            _gridLayoutGroup.cellSize = new Vector2
                (
                    _defaultCellSize.x * gridInterpolation.x,
                    _defaultCellSize.y * gridInterpolation.y
                );

            
            _gridLayoutGroup.spacing = new Vector2
                (
                    _defaultSpacingSize.x * gridInterpolation.x,
                    _defaultSpacingSize.y * gridInterpolation.y
                );
        }

        protected virtual void ChangeResolutionSquareGrid()
        {
            if (_myRect == null)
            {
                _myRect = GetComponent<RectTransform>();
            }

            float minRectValue = Mathf.Min(_myRect.rect.width, _myRect.rect.height);
            _myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minRectValue);
            _myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minRectValue);

            Vector2 gridInterpolation = GetGridInterpolation();

            // Get Min Cell
            float cellMin = Mathf.Min(_defaultCellSize.x * gridInterpolation.x,
                                        _defaultCellSize.y * gridInterpolation.y);
            float spacingMin = Mathf.Min(_defaultSpacingSize.x * gridInterpolation.x,
                                        _defaultSpacingSize.y * gridInterpolation.y);
            _gridLayoutGroup.cellSize = new Vector2
                (
                    cellMin,
                    cellMin
                );

            
            _gridLayoutGroup.spacing = new Vector2
                (
                    spacingMin,
                    spacingMin
                );
        }
    }
}