using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextMeshProBackground : MonoBehaviour
{
    private const float Tolerance = 0.00001f;

    [SerializeField] private Image _image;
    [SerializeField] private float _paddingWidth;
    [SerializeField] private float _paddingHeight;

    private TextMeshProUGUI _tmp;
    private float _preWidth;
    private float _preHeight;

    private void Start()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Math.Abs(_preWidth - _tmp.preferredWidth) < Tolerance && Math.Abs(_preHeight - _tmp.preferredHeight) < Tolerance) return;

        UpdateTMProUGUISizeDelta();
        UpdateImageSizeDelta();
    }

    /// <summary>
    /// RectTransform.sizeDeltaをテキストにぴっちりさせる
    /// </summary>
    private void UpdateTMProUGUISizeDelta()
    {
        _preWidth = _tmp.preferredWidth;
        _preHeight = _tmp.preferredHeight;
        _tmp.rectTransform.sizeDelta = new Vector2(_preWidth, _preHeight);
    }

    /// <summary>
    /// 背景のImageのRectTransform.sizeDeltaを指定したパディングで更新
    /// </summary>
    private void UpdateImageSizeDelta()
    {
        if (_preHeight == 0 || _preWidth == 0)
        {
            _image.rectTransform.sizeDelta = Vector2.zero;
            return;
        }

        _image.rectTransform.sizeDelta = new Vector2(_preWidth + _paddingWidth, _preHeight + _paddingHeight);
    }
}
