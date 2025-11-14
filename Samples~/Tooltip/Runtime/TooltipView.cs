using System.Collections;
using TMPro;
using UnityEngine;

namespace GamingXRCore.Tooltip
{
    internal class TooltipView : MonoBehaviour
    {
        [SerializeField] private TMP_Text txt_header;
        [SerializeField] private TMP_Text txt_content;

        [SerializeField] private RectTransform _canvasRect;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private float _animationDuration = 0.15f;

        private RectTransform _rectTransform;
        private Vector3 _targetScale;
        private bool _isVisible;
        private IEnumerator OnScaleRoutine;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _targetScale = _rectTransform.localScale;
        }

        private void Update()
        {
            if (!_isVisible)
                return;

            FollowMouse();
        }

        public void SetHoverText(string header, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                Hide();
                return;
            }

            txt_header.text = header;
            txt_content.text = content;
            Show();
        }

        private void Show()
        {
            _isVisible = true;
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = false;

            FollowMouse(); //Force update
            Animate();
        }

        private void Hide()
        {
            _isVisible = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }

        private void Animate()
        {
            _rectTransform.localScale = Vector3.zero;

            if (OnScaleRoutine != null)
            {
                StopCoroutine(OnScaleRoutine);
            }

            OnScaleRoutine = ScaleCoroutine();
            StartCoroutine(OnScaleRoutine);
        }

        private IEnumerator ScaleCoroutine()
        {
            float elapsedTime = 0f;
            Vector3 startScale = _rectTransform.localScale;

            while (elapsedTime < _animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / _animationDuration;

                // OutBack easing formula
                float c1 = 1.70158f;
                float c3 = c1 + 1f;
                float easedT = 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);

                _rectTransform.localScale = Vector3.Lerp(startScale, _targetScale, easedT);

                yield return null;
            }

            _rectTransform.localScale = _targetScale;
        }

        private void FollowMouse()
        {
            // Convert mouse position to UI local position
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRect,
                Input.mousePosition,
                _canvasRect.GetComponent<Canvas>().worldCamera,
                out mousePos
            );

            // Apply offset
            _rectTransform.anchoredPosition = mousePos + _offset;

            ClampToCanvas();
        }

        private void ClampToCanvas()
        {
            Vector2 pos = _rectTransform.anchoredPosition;
            Vector2 size = _rectTransform.sizeDelta;
            Vector2 canvasSize = _canvasRect.sizeDelta;

            float halfWidth = size.x / 2;
            float halfHeight = size.y / 2;

            pos.x = Mathf.Clamp(pos.x, -canvasSize.x / 2 + halfWidth, canvasSize.x / 2 - halfWidth);
            pos.y = Mathf.Clamp(pos.y, -canvasSize.y / 2 + halfHeight, canvasSize.y / 2 - halfHeight);

            _rectTransform.anchoredPosition = pos;
        }

        private void OnDestroy()
        {
            if (OnScaleRoutine != null)
                StopCoroutine(OnScaleRoutine);
        }
    }
}