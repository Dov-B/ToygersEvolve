using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointAndClickButtonHandler : MonoBehaviour
{
    [Space(5), Header("Shockwave")]

    [SerializeField] private Image _shockwaveImage;
    private bool _shockwaveImageInitialized = false;

    [SerializeField] private float _lerpAlphaTime;

    [SerializeField] private float _minScaleX, _minScaleY;
    [SerializeField] private float _maxScaleX, _maxScaleY;

    [SerializeField] private float _minAlpha, _maxAlpha;

    [Space(5), Header("Button")]

    private Image _buttonImage;
    private Animator _buttonAnimator;

    private void Start()
    {
        _buttonImage = GetComponent<Image>();
        _buttonAnimator = GetComponent<Animator>();
    }

    // Lerping both shockwave's scale and alpha
    public IEnumerator LerpShockwave()
    {
        CheckShockwaveImageInitialization();
        float timeElapsed = 0f;

        Color color = _shockwaveImage.color;

        Vector3 imageScale = _shockwaveImage.rectTransform.localScale;
        Vector3 minScale = new Vector3(_minScaleX, _minScaleY, imageScale.z);
        Vector3 maxScale = new Vector3(_maxScaleX, _maxScaleY, imageScale.z);

        while (timeElapsed < _lerpAlphaTime)
        {
            color.a = Mathf.Lerp(_maxAlpha, _minAlpha, timeElapsed / _lerpAlphaTime);
            _shockwaveImage.color = color;

            imageScale = Vector3.Lerp(minScale, maxScale, timeElapsed / _lerpAlphaTime);
            _shockwaveImage.rectTransform.localScale = imageScale;

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        _shockwaveImage.color = new Color(_shockwaveImage.color.r, _shockwaveImage.color.g, _shockwaveImage.color.b, _minAlpha);

        _shockwaveImage.rectTransform.localScale = maxScale;
    }

    public void PlayButtonAnimation()
    {
        _buttonAnimator.Play("ButtonBounce");
    }

    public void DisableButton()
    {
        _shockwaveImage.enabled = false;    
        _buttonImage.enabled = false;    
    }

    // Enable the shockwave image once so that it scales well during the first iteration
    private void CheckShockwaveImageInitialization()
    {
        if(!_shockwaveImageInitialized)
        {
            _shockwaveImage.enabled = true;
            _shockwaveImageInitialized = true;  
        }

    }
}
