using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shockwave : MonoBehaviour
{
    [SerializeField] private Image _shockwaveImage;
    [HideInInspector] public Animator animator;

    [SerializeField] private float _lerpAlphaTime;

    [SerializeField] private float _minScaleX, _minScaleY;
    [SerializeField] private float _maxScaleX, _maxScaleY;

    [SerializeField] private float _minAlpha, _maxAlpha;

    private void Start()
    {
        animator = GetComponent<Animator>();

        _shockwaveImage.rectTransform.localScale = new Vector3(_minScaleX, _minScaleY, _shockwaveImage.rectTransform.localScale.z);
    }

    // Lerping both shockwave's scale and alpha
    public IEnumerator LerpShockwave()
    {
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
        animator.Play("ButtonBounce");
    }
}
