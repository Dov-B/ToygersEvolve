using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointAndClickButtonHandler : MonoBehaviour
{
    [Space(5), Header("Shockwave")]

    [SerializeField] private Image _shockwaveImage;

    [Space(5), Header("Selection Button")]

    [SerializeField] private Image _selectionButtonImage;

    [Space(5), Header("Action Button")]

    [SerializeField] private GameObject _actionButtonGameobject;
    [SerializeField] private float _lerpActionButtonTime;
    [SerializeField] private float _minAlphaActionButton, _maxAlphaActionButton;
    [SerializeField] private float _buttonTransitionOffset;

    private Image _actionButtonImage;
    private TextMeshProUGUI _actionButtonText;

    private void Start()
    {
        _actionButtonImage = _actionButtonGameobject.GetComponent<Image>();
        _actionButtonText = _actionButtonGameobject.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        CheckInputOutsideThisButton();
    }

    public void DisableSelectionButton()
    {
        _shockwaveImage.enabled = false;
        _selectionButtonImage.enabled = false;
    }

    public void EnableSelectionButton()
    {
        _shockwaveImage.enabled = true;
        _selectionButtonImage.enabled = true;
    }

    // Lerping Image and Text's alphas and gameobject's position
    public void ShowActionButton()
    {
        StartCoroutine(LerpActionButton(_minAlphaActionButton,_maxAlphaActionButton,_buttonTransitionOffset));
    }

    public void HideActionButton()
    {
        StartCoroutine(LerpActionButton(_maxAlphaActionButton, _minAlphaActionButton,-_buttonTransitionOffset));
    }

    public IEnumerator LerpActionButton(float minAlpha, float maxAlpha, float buttonTransitionOffset)
    {
        // if action button is appearing
        if (buttonTransitionOffset > 0)
        {
            DisableSelectionButton();
            _actionButtonGameobject.SetActive(true);
        }

        float timeElapsed = 0f;

        Color colorImage = _actionButtonImage.color;
        Color colorText = _actionButtonText.color;

        Vector3 initialTransform = _actionButtonImage.rectTransform.localPosition;
        Vector3 finalTransform = new Vector3(initialTransform.x, initialTransform.y + buttonTransitionOffset, initialTransform.z);

        while (timeElapsed < _lerpActionButtonTime)
        {
            colorImage.a = Mathf.Lerp(minAlpha, maxAlpha, timeElapsed / _lerpActionButtonTime);
            colorText.a = Mathf.Lerp(minAlpha, maxAlpha, timeElapsed / _lerpActionButtonTime);
            _actionButtonImage.rectTransform.localPosition = Vector3.Lerp(initialTransform, finalTransform, timeElapsed / _lerpActionButtonTime);

            _actionButtonImage.color = colorImage;
            _actionButtonText.color = colorText;

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        _actionButtonImage.color = new Color(_actionButtonImage.color.r,
                                             _actionButtonImage.color.g,
                                             _actionButtonImage.color.b,
                                             maxAlpha);

        _actionButtonText.color = new Color(_actionButtonText.color.r,
                                            _actionButtonText.color.g,
                                            _actionButtonText.color.b,
                                            maxAlpha);

        _actionButtonImage.rectTransform.localPosition = new Vector3(finalTransform.x, finalTransform.y, finalTransform.z);

        // if action button is disappearing
        if (buttonTransitionOffset < 0)
        {
            EnableSelectionButton();
            _actionButtonGameobject.SetActive(false);
        }
    }

    private void CheckInputOutsideThisButton()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began && _actionButtonGameobject.activeInHierarchy)
            {
                //_actionButtonGameobject.SetActive(false);
                EnableSelectionButton();

                _selectionButtonImage.GetComponent<Shockwave>().PlayButtonAnimation();
            }
        }

        if (Input.GetMouseButtonDown(0) && _actionButtonGameobject.activeInHierarchy)
        {
            //_actionButtonGameobject.SetActive(false);
            EnableSelectionButton();
            HideActionButton();
            _selectionButtonImage.GetComponent<Shockwave>().PlayButtonAnimation();
        }
    }
}
