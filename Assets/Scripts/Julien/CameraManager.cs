using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;

    public static CameraManager Instance;

    [SerializeField] private Camera _camera = null;

    private Vector3 _lastMousePosition;
    private Vector3 _startClickPosition;

    private Vector2 centerTouchPos;  
    private Vector2 centerTouchDelta; 

    private float _timeSinceClick = 0;

    private bool _canMove = true;

    private Vector3 _velocity = Vector3.zero; // Vélocité de la caméra
    public float deceleration = 5f; // La vitesse à laquelle la caméra décélère après la fin du mouvement
    public float moveSpeed = 1f;

    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }

    void Update()
    {
        CheckClick();

        if(Application.isMobilePlatform) HandleTouchMovement();
        else                             HandlePCMovement();;

        ApplyVelocity();
    }

    private void HandlePCMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastMousePosition = Input.mousePosition;
            _canMove = !IsMouseOverUI();
        }

        if (Input.GetMouseButton(0) && _canMove)
        {
            Vector3 mouseWorldPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lastWorldPoint = _camera.ScreenToWorldPoint(_lastMousePosition);

            Vector3 delta = mouseWorldPoint - lastWorldPoint;
            _lastMousePosition = Input.mousePosition;

            _camera.transform.position -= delta;

            _velocity = delta * moveSpeed;
        }
    }

    private void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _timeSinceClick = 0;
            _startClickPosition = Input.mousePosition;
        }

        if (!Input.GetMouseButtonUp(0)) return;

        if (_timeSinceClick > 0.25f) return;

        const float similarityThreshold = 10;
        float distance = Vector3.Distance(_startClickPosition, Input.mousePosition);

        if (distance > similarityThreshold) return;
    }

    private void ApplyVelocity()
    {
        if (!Input.GetMouseButton(0)) 
        {
            _velocity = Vector3.Lerp(_velocity, Vector3.zero, deceleration * Time.deltaTime);

            _camera.transform.position -= _velocity * Time.deltaTime;
        }
    }

    private bool IsMouseOverUI()
    {
        // Return true if the mouse is hovering a UI element
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void HandleTouchMovement()
    {
        if(Input.touchCount >0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                _canMove = !IsMouseOverUI();
            }

            if(touch.phase == TouchPhase.Moved && _canMove)
            {
                Vector3 touchPosPrev = touch.position - touch.deltaPosition;

                Vector3 currentWorld = _camera.ScreenToWorldPoint(touch.position);
                Vector3 lastWorld = _camera.ScreenToWorldPoint(touchPosPrev);

                Vector3 delta = lastWorld - currentWorld;

                _camera.transform.position -= delta;

                _velocity = delta * moveSpeed;
            }
        }
    }

    public void SetCameraPoint()
    {
        _camera.transform.position = Vector3.zero;
    }
}
