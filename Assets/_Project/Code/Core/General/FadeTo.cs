using _Project.Code.Core.General;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeTo : Singleton<FadeTo>
{
    public event Action OnFadeComplete;

    private Image panel;
    private float _fadeValue;
    private bool _timeToFade;
    private bool _isFadeTo;

    [SerializeField] private bool isFadeToBlack = true;
    [SerializeField, Range(0, 5.0f)] private float dividerToSlowFade = 3.0f;

    private void Start()
    {
        panel = GetComponentInChildren<Image>();

        if (isFadeToBlack)
            panel.color = Color.black;
        else
            panel.color = Color.white;

        _fadeValue = 0.0f;
        SetOpacity(_fadeValue);
        _timeToFade = false;
    }

    public void Fade(bool isFadeTo)
    {
        _timeToFade = true;

        _isFadeTo = isFadeTo;
    }

    private void SetOpacity(float alphaValue)
    {
        Color tempColor = panel.color;

        tempColor.a = Mathf.Clamp01(alphaValue);

        panel.color = tempColor;
    }

    private void Update()
    {
        if (_timeToFade)
        {
            if (_isFadeTo)
            {
                _fadeValue += Time.deltaTime / dividerToSlowFade;
                SetOpacity(_fadeValue);

                if (_fadeValue >= 1.0f)
                {
                    _timeToFade = false;
                    OnFadeComplete?.Invoke();
                }
            }
            else
            {
                _fadeValue -= Time.deltaTime / dividerToSlowFade;
                SetOpacity(_fadeValue);

                if (_fadeValue <= 0.0f)
                {
                    _timeToFade = false;
                    OnFadeComplete?.Invoke();
                }
            }
        }
    }
}