using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

namespace GamingXRCore.Tooltip
{
    internal class TooltipController : MonoBehaviour
    {
        [SerializeField] private TooltipView view;

        private PointerEventData _pointerData;
        private readonly List<RaycastResult> _raycastResults = new();

        private IHoverable _currentHover;
        private string _currentHeader;
        private string _currentContent;

        public Action<string, string> OnHoverTextChanged;

        private void Awake()
        {
            _pointerData = new PointerEventData(EventSystem.current);

            OnHoverTextChanged += HoverTextChanged;

            view.SetHoverText(string.Empty, string.Empty);
        }

        private void HoverTextChanged(string header, string content)
        {
            view.SetHoverText(header, content);
        }

        void Update()
        {
            _pointerData.position = Input.mousePosition;
            _raycastResults.Clear();

            EventSystem.current.RaycastAll(_pointerData, _raycastResults);

            IHoverable newHover = null;
            for (int i = 0; i < _raycastResults.Count; i++)
            {
                newHover = _raycastResults[i].gameObject.GetComponentInParent<IHoverable>();
                if (newHover != null)
                    break;
            }

            if (_currentHover != newHover)
            {
                _currentHover?.OnHoverExit();
                _currentHover = newHover;
                _currentHover?.OnHoverEnter();

                _currentHeader = string.Empty;
                _currentContent = string.Empty;

                if (_currentHover != null)
                {
                    var tooltipModel = _currentHover.GetTooltipModel();
                    _currentHeader = tooltipModel.header;
                    _currentContent = tooltipModel.content;
                }

                OnHoverTextChanged?.Invoke(_currentHeader, _currentContent);
            }
        }

        private void OnDestroy()
        {
            OnHoverTextChanged -= HoverTextChanged;
        }
    }
}