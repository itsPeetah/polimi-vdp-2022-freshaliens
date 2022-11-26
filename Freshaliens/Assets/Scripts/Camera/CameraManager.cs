using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private GameObject _ninja;
    [SerializeField] private GameObject _fairy;
    
    private enum TrackingMode
    {
        Ninja, 
        Fairy, 
        Both
    }

    [Header("Camera Parameters")]
    [SerializeField] private TrackingMode _currentTrackingMode;
    [SerializeField] private float _defaultHorizontalOffset = 2f;
    [SerializeField] private float _defaultVerticalOffset = 2f;
    [SerializeField] private float _minCameraSize = 5f;
    [SerializeField] private float _maxCameraSize = 10f;
    [SerializeField] private float _playerMarginBeforeZoomOut = 1f;
    [SerializeField] private float _playerMarginBeforeZoomIn = 2.5f;
    [SerializeField] private float _cameraZoomSpeed = 5f;
    [SerializeField] private bool _canIncreaseCameraSize;
    [SerializeField] private bool _canMoveCamera;
    [SerializeField] private bool _dynamicHorizontalOffset;
    // //Sensitivity is only for manual zoom
    // [SerializeField] private float sensitivity = 1f;

    [Header("Borders Parameters")] 
    [SerializeField] private bool _instantiateBorders = false;
    [SerializeField] private float _collisionDistanceFromBorders = 0.5f;
    [Range(0f, 1f)]
    [SerializeField] private float _ratioPlayableScreen = 0.5f;
    [SerializeField] private bool _showMessagePlayersTooDistant;
    [SerializeField] private float _timerMessagePlayersTooDistant = 5f;
    [SerializeField] private int _boundaryLayer = 11;
    
    //References
    //Transforms & Camera
    private Transform _transform;
    private Transform _ninjaTransform;
    private Transform _fairyTransform;
    private Camera _camera;
    private Transform _cameraTransform;
    //Colliders
    private Collider2D _ninjaCollider;
    private Collider2D _fairyCollider;
    private Collider2D _leftBorderCollider;
    private Collider2D _rightBorderCollider;

    //CameraManager State
    private Vector3 _currentPosition;
    private float _currentHorizontalOffset;
    private float _currentVerticalOffset;

    //Camera State
    private Vector3 _initialCameraPosition;
    private float _initialCameraPositionXCoordinate;
    private Vector3 _currentCameraPosition;
    private float _currentCameraSize;
    private float _currentScreenWidth;
    private float _currentScreenHeight;
    private bool _canMoveCameraRight;
    private bool _canMoveCameraLeft;
    private bool _mustShowMessageTooDistant;
    private float _top;
    private float _left;
    private float _right;
    private float _rightCollision;
    private float _lastDistantMessageTime;

    private void Start()
    {
        //Set references
        //Transforms & Camera
        _transform = transform;
        _ninjaTransform = _ninja.GetComponent<Transform>();
        _fairyTransform = _fairy.GetComponent<Transform>();
        _camera = GetComponentInChildren<Camera>();
        _cameraTransform = _camera.transform;
        //Colliders
        _ninjaCollider = _ninja.gameObject.GetComponent<Collider2D>();
        _fairyCollider = _fairy.gameObject.GetComponent<Collider2D>();

        //Set initial CameraManager state
        _transform.position += new Vector3(0, _defaultVerticalOffset, 0);
        _currentPosition = _transform.position;
        
        _currentHorizontalOffset = _defaultHorizontalOffset;
        _currentVerticalOffset = _defaultVerticalOffset;

        //Set initial camera state
        _canIncreaseCameraSize = true;
        _canMoveCamera = true;
        _showMessagePlayersTooDistant = true;
        _cameraTransform.position = _currentPosition;
        _currentCameraPosition = _currentPosition;
        // _initialCameraPosition = _currentPosition;
        // _initialCameraPositionXCoordinate = _currentPosition.x;
        _currentCameraSize = _minCameraSize;
        _camera.orthographicSize = _currentCameraSize;
        _currentScreenHeight = _currentCameraSize;
        _currentScreenWidth = _currentScreenHeight * 16 / 9;
        _canMoveCameraRight = true;
        _canMoveCameraLeft = true;
        _mustShowMessageTooDistant = true;
        _lastDistantMessageTime = -1f;
        _top = _currentCameraPosition.y + _currentScreenHeight;
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
            if (_dynamicHorizontalOffset)
            {
                _currentHorizontalOffset = _defaultHorizontalOffset*(_currentCameraSize/_minCameraSize);
            }
            float currentXCoordinate = _currentPosition.x;
            float newXCoordinate;

            switch (_currentTrackingMode)
            {
                case TrackingMode.Ninja:
                    newXCoordinate = positionNinja.x + _currentHorizontalOffset;
                    break;
                case TrackingMode.Fairy:
                    newXCoordinate = positionFairy.x + _currentHorizontalOffset;
                    break;
                case TrackingMode.Both:
                    newXCoordinate = ((positionNinja.x + positionFairy.x) / 2) + _currentHorizontalOffset;
                    break;
                default:
                    newXCoordinate = ((positionNinja.x + positionFairy.x) / 2) + _currentHorizontalOffset;
                    break;
            }
            
            float horizontalDifference = newXCoordinate - currentXCoordinate;
            Vector3 positionDifference = new Vector3(horizontalDifference, 0, 0);
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
            _left = _currentCameraPosition.x - _currentScreenWidth;
            _right = _currentCameraPosition.x + _currentScreenWidth;
            _rightCollision = _currentCameraPosition.x + _currentScreenWidth*_ratioPlayableScreen;
        }
    }

    private bool NeedToIncreaseCameraSize(Vector3 positionPlayer1, Vector3 positionPlayer2)
    {
        bool needToIncreaseCameraSize = false;

        float highestPlayerHeight = Mathf.Max(positionPlayer1.y, positionPlayer2.y);
        float distanceFromTopBorder = _top - highestPlayerHeight;
        bool tooHigh = distanceFromTopBorder < _playerMarginBeforeZoomOut;

        float mostLeftPlayerPosition = Mathf.Min(positionPlayer1.x, positionPlayer2.x);
        float distanceFromLeftBorder = mostLeftPlayerPosition - _left;
        bool tooLeft = distanceFromLeftBorder < _playerMarginBeforeZoomOut;
        
        float mostRightPlayerPosition = Mathf.Max(positionPlayer1.x, positionPlayer2.x);
        float distanceFromRightBorder = _rightCollision - mostRightPlayerPosition;
        bool tooRight = distanceFromRightBorder < _playerMarginBeforeZoomOut;

        needToIncreaseCameraSize = (tooHigh || tooLeft || tooRight);
        return needToIncreaseCameraSize;
    }
    
    private bool CanDecreaseCameraSize(Vector3 positionPlayer1, Vector3 positionPlayer2)
    {
        bool canDecreaseCameraSize = false;

        float highestPlayerHeight = Mathf.Max(positionPlayer1.y, positionPlayer2.y);
        float distanceFromTopBorder = _top - highestPlayerHeight;
        bool notTooHigh = distanceFromTopBorder > _playerMarginBeforeZoomIn;

        float mostLeftPlayerPosition = Mathf.Min(positionPlayer1.x, positionPlayer2.x);
        float distanceFromLeftBorder = mostLeftPlayerPosition - _left;
        bool notTooLeft = distanceFromLeftBorder > _playerMarginBeforeZoomIn;
        
        float mostRightPlayerPosition = Mathf.Max(positionPlayer1.x, positionPlayer2.x);
        float distanceFromRightBorder = _rightCollision - mostRightPlayerPosition;
        bool notTooRight = distanceFromRightBorder > _playerMarginBeforeZoomIn;

        canDecreaseCameraSize = (notTooHigh && notTooLeft && notTooRight);
        return canDecreaseCameraSize;
    }

    private void ChangeCameraSize(float targetSize)
    {
        float newCameraSize = Mathf.MoveTowards(_currentCameraSize, targetSize, _cameraZoomSpeed * Time.deltaTime);
        _camera.orthographicSize = newCameraSize;
        _currentCameraSize = newCameraSize;
        _currentScreenHeight = newCameraSize;
        _currentScreenWidth = newCameraSize * 16 / 9;

        _currentVerticalOffset = _defaultVerticalOffset + (newCameraSize - _minCameraSize);
        _currentCameraPosition = new Vector3(_currentCameraPosition.x, _currentVerticalOffset, _currentCameraPosition.z);
        _cameraTransform.position = _currentCameraPosition;
        
        _top = _currentCameraPosition.y + _currentScreenHeight;
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
                        Debug.Log("PLAYERS TOO DISTANT!");
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
    
    //This method can be used to set zoom with mouse
    // public void OrthographicZoom()
    // {
    //     float _targetCameraSize = _currentCameraSize;
    //     _targetCameraSize -= Input.mouseScrollDelta.y * sensitivity;
    //     _targetCameraSize = Mathf.Clamp(_targetCameraSize, _minCameraSize, _maxCameraSize);
    //     float newCameraSize = Mathf.MoveTowards(_currentCameraSize, _targetCameraSize, _cameraZoomSpeed * Time.deltaTime);
    //     _camera.orthographicSize = newCameraSize;
    //     _currentCameraSize = newCameraSize;
    //     _currentScreenHeight = newCameraSize;
    //     _currentScreenWidth = newCameraSize * 16 / 9;
    //     _top = _currentCameraPosition.y + _currentScreenHeight;
    //     _left = _currentCameraPosition.x - _currentScreenWidth;
    //     _right = _currentCameraPosition.x + _currentScreenWidth;
    //     _rightCollision = _currentCameraPosition.x + _currentScreenWidth*_ratioPlayableScreen;
    // }
    
}
