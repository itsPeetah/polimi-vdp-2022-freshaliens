using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private GameObject _player1;
    [SerializeField] private GameObject _player2;

    [Header("CollisionBorders")]
    [SerializeField] private GameObject _leftBorder;
    [SerializeField] private GameObject _rightBorder;
    [SerializeField] private GameObject _topBorder;
    
    [Header("Camera Parameters")] 
    [SerializeField] private float _defaultHorizontalOffset = 2f;
    [SerializeField] private float _defaultVerticalOffset = 2f;
    [SerializeField] private float _minCameraSize = 5f;
    [SerializeField] private float _maxCameraSize = 10f;
    [SerializeField] private float _collisionDistanceFromBorders = 0.5f;
    [SerializeField] private float _playerMarginBeforeZoomOut = 1f;
    [SerializeField] private float _playerMarginBeforeZoomIn = 2.5f;
    [SerializeField] private float _cameraZoomSpeed = 5f;
    [Range(0f, 1f)]
    [SerializeField] private float _ratioPlayableScreen = 0.5f;
    [SerializeField] private bool _canIncreaseCameraSize;
    [SerializeField] private bool _canMoveCamera;
    [SerializeField] private bool _dynamicHorizontalOffset;
    // //Sensitivity is only for manual zoom
    // [SerializeField] private float sensitivity = 1f;
    
    //References
    //Transforms & Camera
    private Transform _transform;
    private Transform _player1Transform;
    private Transform _player2Transform;
    private Camera _camera;
    private Transform _cameraTransform;
    //Colliders
    private Collider2D _player1Collider;
    private Collider2D _player2Collider;
    private Collider2D _leftBorderCollider;
    private Collider2D _rightBorderCollider;
    //RigidBodies
    private Rigidbody2D _player1Rigidbody2D;
    private Rigidbody2D _player2Rigidbody2D;

    //CameraManager State
    private Vector3 _currentPosition;
    private float _currentHorizontalOffset;
    private float _currentVerticalOffset;

    //Camera State
    private Vector3 _currentCameraPosition;
    private float _currentCameraSize;
    private float _currentScreenWidth;
    private float _currentScreenHeight;
    private bool _canMoveCameraRight;
    private bool _canMoveCameraLeft;
    private float _top;
    private float _left;
    private float _right;
    private float _rightCollision;

    private void Start()
    {
        //Set references
        //Transforms & Camera
        _transform = transform;
        _player1Transform = _player1.GetComponent<Transform>();
        _player2Transform = _player2.GetComponent<Transform>();
        _camera = GetComponentInChildren<Camera>();
        _cameraTransform = _camera.transform;
        //Colliders
        _player1Collider = _player1.gameObject.GetComponent<Collider2D>();
        _player2Collider = _player2.gameObject.GetComponent<Collider2D>();
        _leftBorderCollider = _leftBorder.GetComponent<Collider2D>();
        _rightBorderCollider = _rightBorder.GetComponent<Collider2D>();
        //RigidBodies
        _player1Rigidbody2D = _player1.GetComponent<Rigidbody2D>();
        _player2Rigidbody2D = _player2.GetComponent<Rigidbody2D>();

        //Set initial CameraManager state
        _transform.position += new Vector3(0, _defaultVerticalOffset, 0);
        _currentPosition = _transform.position;
        _currentHorizontalOffset = _defaultHorizontalOffset;
        _currentVerticalOffset = _defaultVerticalOffset;

        //Set initial camera state
        // _canIncreaseCameraSize = true;
        // _canMoveCamera = true;
        _cameraTransform.position = _currentPosition;
        _currentCameraPosition = _currentPosition;
        _currentCameraSize = _camera.orthographicSize;
        _currentScreenHeight = _currentCameraSize;
        _currentScreenWidth = _currentScreenHeight * 16 / 9;
        _canMoveCameraRight = true;
        _canMoveCameraLeft = true;
        _top = _currentCameraPosition.y + _currentScreenHeight;
        _left = _currentCameraPosition.x - _currentScreenWidth;
        _right = _currentCameraPosition.x + _currentScreenWidth;
        _rightCollision = _currentCameraPosition.x + _currentScreenWidth*_ratioPlayableScreen;

        //Set boundaries
        float leftBorderDistance = (_maxCameraSize * 16 / 9) - _collisionDistanceFromBorders;
        float rightBorderDistance = (_maxCameraSize * 16 / 9) * _ratioPlayableScreen;
        float topBorderDistance = _maxCameraSize - _collisionDistanceFromBorders;
        
        Vector3 leftBorderPosition = new Vector3(_currentCameraPosition.x - leftBorderDistance, _currentCameraPosition.y, _currentCameraPosition.z);
        _leftBorder.transform.SetPositionAndRotation(leftBorderPosition, Quaternion.identity);
        
        Vector3 rightBorderPosition = new Vector3(_currentCameraPosition.x +rightBorderDistance, _currentCameraPosition.y, _currentCameraPosition.z);
        _rightBorder.transform.SetPositionAndRotation(rightBorderPosition, Quaternion.identity);
        
        Vector3 topBorderPosition = new Vector3(_currentCameraPosition.x , _currentCameraPosition.y + topBorderDistance, _currentCameraPosition.z);
        _topBorder.transform.SetPositionAndRotation(topBorderPosition, Quaternion.identity);
    }


    //better fixedUpdate?
    void LateUpdate()
    {
        //The horizontal position of the camera is the average of the two players + offset
        Vector3 positionPlayer1 = _player1Transform.position;
        Vector3 positionPlayer2 = _player2Transform.position;

        //Manage zoom
        if (_canIncreaseCameraSize)
        {
            bool zoomNeeded = NeedToIncreaseCameraSize(positionPlayer1, positionPlayer2);
            if (zoomNeeded)
            {
                ChangeCameraSize(_maxCameraSize);
            }
            else if (CanDecreaseCameraSize(positionPlayer1, positionPlayer2))
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
            float newXCoordinate = ((positionPlayer1.x + positionPlayer2.x) / 2) + _currentHorizontalOffset;
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
        _canMoveCameraRight = true;
        _canMoveCameraLeft = true;
        if(_leftBorderCollider.IsTouching(_player1Collider) || _leftBorderCollider.IsTouching(_player2Collider))
            _canMoveCameraRight = false;
        if(_rightBorderCollider.IsTouching(_player1Collider) || _rightBorderCollider.IsTouching(_player2Collider))
            _canMoveCameraLeft = false;
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
