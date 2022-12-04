using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Freshaliens.Player.Components;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CameraBoundsManager))]
public class CameraManager : MonoBehaviour
{
    
    // Singleton instance
    private static CameraManager instance = null;
    public static CameraManager Instance { get => instance; private set => instance = value; }
    
    private enum TrackingMode
    {
        Ninja, 
        Fairy, 
        Both
    }

    [Header("Camera Parameters")]
    [SerializeField] private TrackingMode _currentTrackingMode;
    public float _horizontalOffset = 2f;
    public float _verticalOffset = 2f;
    [SerializeField] private float _initialHorizontalOffset = 5f;
    public float _minCameraSize = 6f;
    public float _maxCameraSize = 12f;
    [SerializeField] private float _playerMarginBeforeZoomOut = 2f;
    [SerializeField] private float _playerMarginBeforeZoomIn = 3.5f;
    [SerializeField] private float _cameraZoomSpeed = 4f;
    [SerializeField] private bool _canIncreaseCameraSize;
    [SerializeField] private bool _canMoveCamera;
    // //Sensitivity is only for manual zoom
    // [SerializeField] private float sensitivity = 1f;

    [Header("Borders Parameters")] 
    [SerializeField] private bool _instantiateBorders = false;
    [SerializeField] private float _collisionDistanceFromBorders = 0.5f;
    [Range(0f, 1f)]
    [SerializeField] private float _ratioPlayableScreen = 0.75f;
    [SerializeField] private bool _showMessagePlayersTooDistant;
    [SerializeField] private float _timerMessagePlayersTooDistant = 5f;
    [SerializeField] private int _boundaryLayer = 11;
    
    //References
    //Transforms & Camera
    private GameObject _ninja;
    private GameObject _fairy;
    private Transform _transform;
    private Transform _ninjaTransform;
    private Transform _fairyTransform;
    private Camera _camera;
    private Transform _cameraTransform;
    private CameraBoundsManager _boundsManager;
    //Colliders
    private Collider2D _ninjaCollider;
    private Collider2D _fairyCollider;
    private Collider2D _leftBorderCollider;
    private Collider2D _rightBorderCollider;

    //CameraManager State
    private Vector3 _currentPosition;

    //Camera State
    private Vector3 _initialCameraPosition;
    private Vector3 _currentCameraPosition;
    private float _currentCameraSize;
    private float _currentScreenWidth;
    private float _currentScreenHeight;
    private bool _canMoveCameraRight;
    private bool _canMoveCameraLeft;
    private bool _mustShowMessageTooDistant;
    private float _top;
    private float _bottom;
    private float _left;
    private float _right;
    private float _rightCollision;
    private float _lastDistantMessageTime;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Set references
        //Transforms & Camera
        _ninja = PlayerMovementController.Instance.gameObject;
        _fairy = FairyMovementController.Instance.gameObject;
        _transform = transform;
        _ninjaTransform = _ninja.GetComponent<Transform>();
        _fairyTransform = _fairy.GetComponent<Transform>();
        _camera = GetComponentInChildren<Camera>();
        _cameraTransform = _camera.transform;
        _boundsManager = GetComponent<CameraBoundsManager>();
        //Colliders
        _ninjaCollider = _ninja.gameObject.GetComponent<Collider2D>();
        _fairyCollider = _fairy.gameObject.GetComponent<Collider2D>();

        //Set initial CameraManager state
        _transform.position = _ninjaTransform.position + new Vector3(_initialHorizontalOffset, _verticalOffset, _transform.position.z);
        _currentPosition = _transform.position;

        //Set initial camera state
        _canIncreaseCameraSize = true;
        _canMoveCamera = true;
        _showMessagePlayersTooDistant = true;
        _initialCameraPosition = _currentPosition;
        _currentCameraPosition = _initialCameraPosition;
        _cameraTransform.position = _currentCameraPosition;
        _currentCameraSize = _minCameraSize;
        _camera.orthographicSize = _currentCameraSize;
        _currentScreenHeight = _currentCameraSize;
        _currentScreenWidth = _currentScreenHeight * 16 / 9;
        _canMoveCameraRight = true;
        _canMoveCameraLeft = true;
        _mustShowMessageTooDistant = true;
        _lastDistantMessageTime = -1f;
        _top = _currentCameraPosition.y + _currentScreenHeight;
        _bottom = _currentCameraPosition.y - _currentScreenHeight;
        _left = _currentCameraPosition.x - _currentScreenWidth;
        _right = _currentCameraPosition.x + _currentScreenWidth;
        _rightCollision = _currentCameraPosition.x + _currentScreenWidth*_ratioPlayableScreen;

        //Set boundaries
        if (_instantiateBorders)
        {
            InitializeBorders();
        }
    }

    private void InitializeBorders()
    {
        //Instantiate
        Vector3 sideBordersScale = new Vector3(0.1f, _maxCameraSize*2, 0f);
        Vector3 topBorderScale = new Vector3(_maxCameraSize*2*16f/9, 0.1f, 0f);
        GameObject leftBorder = InstantiateBorder("LeftBorder", sideBordersScale);
        GameObject rightBorder = InstantiateBorder("RightBorder", sideBordersScale);
        GameObject topBorder = InstantiateBorder("TopBorder", topBorderScale);
        
        //Set positions
        float leftBorderDistance = (_maxCameraSize * 16 / 9) - _collisionDistanceFromBorders;
        float rightBorderDistance = (_maxCameraSize * 16 / 9) * _ratioPlayableScreen;
        float topBorderDistance = _maxCameraSize - _collisionDistanceFromBorders;
        Vector3 leftBorderPosition = new Vector3(_currentCameraPosition.x - leftBorderDistance, _currentCameraPosition.y, _currentCameraPosition.z);
        Vector3 rightBorderPosition = new Vector3(_currentCameraPosition.x + rightBorderDistance, _currentCameraPosition.y, _currentCameraPosition.z);
        Vector3 topBorderPosition = new Vector3(_currentCameraPosition.x , _currentCameraPosition.y + topBorderDistance, _currentCameraPosition.z);
        leftBorder.transform.SetPositionAndRotation(leftBorderPosition, Quaternion.identity);
        rightBorder.transform.SetPositionAndRotation(rightBorderPosition, Quaternion.identity);
        topBorder.transform.SetPositionAndRotation(topBorderPosition, Quaternion.identity);
        
        //Saves colliders
        _leftBorderCollider = leftBorder.GetComponent<Collider2D>();
        _rightBorderCollider = rightBorder.GetComponent<Collider2D>();
    }

    private GameObject InstantiateBorder(String name, Vector3 scale)
    {
        GameObject newBorder = new GameObject(name);
        newBorder.layer = _boundaryLayer;
        newBorder.transform.parent = _camera.transform;
        newBorder.transform.localScale = scale;
        newBorder.AddComponent<BoxCollider2D>();
        return newBorder;
    }


    //better fixedUpdate?
    void LateUpdate()
    {
        Vector3 positionNinja = _ninjaTransform.position;
        Vector3 positionFairy = _fairyTransform.position;

        //Manage zoom
        if (_canIncreaseCameraSize)
        {
            bool zoomNeeded = NeedToIncreaseCameraSize(positionNinja, positionFairy);
            if (zoomNeeded)
            {
                ChangeCameraSize(_maxCameraSize);
            }
            else if (CanDecreaseCameraSize(positionNinja, positionFairy))
            {
                ChangeCameraSize(_minCameraSize);
            }
        }

        //Manage CameraManager and MainCamera position 
        if (_canMoveCamera)
        {
            //Manage CameraManager position 
            float currentXCoordinate = _currentPosition.x;
            float currentYCoordinate = _currentPosition.y;
            float newXCoordinate;
            float newYCoordinate = currentYCoordinate;

            switch (_currentTrackingMode)
            {
                case TrackingMode.Ninja:
                    newXCoordinate = positionNinja.x + _horizontalOffset;
                    newYCoordinate = positionNinja.y + _verticalOffset;
                    break;
                case TrackingMode.Fairy:
                    newXCoordinate = positionFairy.x + _horizontalOffset;
                    break;
                case TrackingMode.Both:
                    newXCoordinate = ((positionNinja.x + positionFairy.x) / 2) + _horizontalOffset;
                    break;
                default:
                    newXCoordinate = positionNinja.x + _horizontalOffset;
                    newYCoordinate = positionNinja.y + _verticalOffset;
                    break;
            }

            //newXCoordinate = Mathf.Max(newXCoordinate, _initialCameraPosition.x);
            //newYCoordinate = Mathf.Max(newYCoordinate, _boundsManager.GetLowerBoundAtXCoord(newXCoordinate).y + _currentCameraSize);
            _boundsManager.ClampOrthographicCamera(ref newXCoordinate, ref newYCoordinate, _currentCameraSize, _camera.aspect);
            
            float horizontalDifference = newXCoordinate - currentXCoordinate;
            float verticalDifference = newYCoordinate - currentYCoordinate;
            Vector3 positionDifference = new Vector3(horizontalDifference, verticalDifference, 0);
            _transform.position += positionDifference;
            _currentPosition += positionDifference;

            //Manage MainCamera position 
            CheckCameraCanMoveSideWay();
            if ((horizontalDifference > 0) && !_canMoveCameraRight)
            {
                _cameraTransform.position -= positionDifference;
                return;
            }
            if ((horizontalDifference < 0) && !_canMoveCameraLeft)
            {
                _cameraTransform.position -= positionDifference;
                return;
            }
            _currentCameraPosition = _cameraTransform.position;
            _top = _currentCameraPosition.y + _currentScreenHeight;
            _bottom = _currentCameraPosition.y - _currentScreenHeight;
            _left = _currentCameraPosition.x - _currentScreenWidth;
            _right = _currentCameraPosition.x + _currentScreenWidth;
            _rightCollision = _currentCameraPosition.x + _currentScreenWidth*_ratioPlayableScreen;
        }

        Debug.DrawLine(_cameraTransform.position, _boundsManager.GetLowerBoundAtXCoord(_cameraTransform.position.x));
    }

    private bool NeedToIncreaseCameraSize(Vector3 positionPlayer1, Vector3 positionPlayer2)
    {
        bool needToIncreaseCameraSize = false;

        float highestPlayerHeight = Mathf.Max(positionPlayer1.y, positionPlayer2.y);
        float distanceFromTopBorder = _top - highestPlayerHeight;
        bool tooHigh = distanceFromTopBorder < _playerMarginBeforeZoomOut;
        
        float lowestPlayerHeight = Mathf.Min(positionPlayer1.y, positionPlayer2.y);
        float distanceFromBottomBorder = lowestPlayerHeight - _bottom;
        bool tooLow = distanceFromBottomBorder < _playerMarginBeforeZoomOut;

        float mostLeftPlayerPosition = Mathf.Min(positionPlayer1.x, positionPlayer2.x);
        float distanceFromLeftBorder = mostLeftPlayerPosition - _left;
        bool tooLeft = distanceFromLeftBorder < _playerMarginBeforeZoomOut;
        
        float mostRightPlayerPosition = Mathf.Max(positionPlayer1.x, positionPlayer2.x);
        float distanceFromRightBorder = _rightCollision - mostRightPlayerPosition;
        bool tooRight = distanceFromRightBorder < _playerMarginBeforeZoomOut;

        needToIncreaseCameraSize = (tooHigh || tooLow || tooLeft || tooRight);
        return needToIncreaseCameraSize;
    }
    
    private bool CanDecreaseCameraSize(Vector3 positionPlayer1, Vector3 positionPlayer2)
    {
        bool canDecreaseCameraSize = false;

        float highestPlayerHeight = Mathf.Max(positionPlayer1.y, positionPlayer2.y);
        float distanceFromTopBorder = _top - highestPlayerHeight;
        bool notTooHigh = distanceFromTopBorder > _playerMarginBeforeZoomIn;
        
        float lowestPlayerHeight = Mathf.Min(positionPlayer1.y, positionPlayer2.y);
        float distanceFromBottomBorder = lowestPlayerHeight - _bottom;
        bool notTooLow = distanceFromBottomBorder > _playerMarginBeforeZoomIn;

        float mostLeftPlayerPosition = Mathf.Min(positionPlayer1.x, positionPlayer2.x);
        float distanceFromLeftBorder = mostLeftPlayerPosition - _left;
        bool notTooLeft = distanceFromLeftBorder > _playerMarginBeforeZoomIn;
        
        float mostRightPlayerPosition = Mathf.Max(positionPlayer1.x, positionPlayer2.x);
        float distanceFromRightBorder = _rightCollision - mostRightPlayerPosition;
        bool notTooRight = distanceFromRightBorder > _playerMarginBeforeZoomIn;

        canDecreaseCameraSize = (notTooHigh && notTooLow && notTooLeft && notTooRight);
        return canDecreaseCameraSize;
    }

    private void ChangeCameraSize(float targetSize)
    {
        float newCameraSize = Mathf.MoveTowards(_currentCameraSize, targetSize, _cameraZoomSpeed * Time.deltaTime);
        _camera.orthographicSize = newCameraSize;
        _currentCameraSize = newCameraSize;
        _currentScreenHeight = newCameraSize;
        _currentScreenWidth = newCameraSize * 16 / 9;
        
        _cameraTransform.position = _currentCameraPosition;
        
        _top = _currentCameraPosition.y + _currentScreenHeight;
        _bottom = _currentCameraPosition.y - _currentScreenHeight;
        _left = _currentCameraPosition.x - _currentScreenWidth;
        _right = _currentCameraPosition.x + _currentScreenWidth;
        _rightCollision = _currentCameraPosition.x + _currentScreenWidth*_ratioPlayableScreen;
    }

    public void CheckCameraCanMoveSideWay()
    {

        if (!_instantiateBorders)
        {
            return;
        }
        
        _canMoveCameraRight = true;
        _canMoveCameraLeft = true;

        bool leftColliderIsTouched = _leftBorderCollider.IsTouching(_ninjaCollider) || _leftBorderCollider.IsTouching(_fairyCollider);
        bool rightColliderIsTouched = _rightBorderCollider.IsTouching(_ninjaCollider) || _rightBorderCollider.IsTouching(_fairyCollider);
        if(leftColliderIsTouched)
            _canMoveCameraRight = false;
        if(rightColliderIsTouched)
            _canMoveCameraLeft = false;

        if (_showMessagePlayersTooDistant)
        {
            if (leftColliderIsTouched && rightColliderIsTouched)
            {
                float currentTime = Time.time;
                if (_mustShowMessageTooDistant)
                {
                    bool canShowMessageTooDistant = (currentTime - _lastDistantMessageTime) > _timerMessagePlayersTooDistant;
                    if (canShowMessageTooDistant)
                    {
                        _mustShowMessageTooDistant = false;
                        //Show Message Players Too Distant
                        //
                        //
                        //
                    }
                }
                _lastDistantMessageTime = currentTime;
            }
            else
            {
                _mustShowMessageTooDistant = true;
            }
        }
    }

}
