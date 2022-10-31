using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    [Header("Players&Camera")]
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
    [SerializeField] private float _playerMarginBeforeZoom = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float _ratioPlayableScreen = 0.5f;
    
    // [SerializeField] private float _cameraZoomSpeed = 30f;
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
    private bool _canIncreaseCameraSize;
    private bool _canMoveCamera;
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
        _cameraTransform.position = _currentPosition;
        _currentCameraPosition = _currentPosition;
        _currentCameraSize = _camera.orthographicSize;
        _currentScreenHeight = _currentCameraSize;
        _currentScreenWidth = _currentScreenHeight * 16 / 9;
        _canIncreaseCameraSize = true;
        _canMoveCamera = true;
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

        //to Finish: zoom
        bool zoomNeeded = NeedZoom(positionPlayer1, positionPlayer2);
        if (zoomNeeded)
        {
            //zoom
        }

        //Update CameraManager position and, if needed, camera position 
        if (_canMoveCamera)
        {
            // _currentHorizontalOffset = _defaultHorizontalOffset*(_currentCameraSize/_minCameraSize);

            float currentXCoordinate = _currentPosition.x;
            float newXCoordinate = ((positionPlayer1.x + positionPlayer2.x) / 2) + _currentHorizontalOffset;
            float horizontalDifference = newXCoordinate - currentXCoordinate;
            Vector3 positionDifference = new Vector3(horizontalDifference, 0, 0);
            _transform.position += positionDifference;
            _currentPosition += positionDifference;
            
            
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

    private bool NeedZoom(Vector3 positionPlayer1, Vector3 positionPlayer2)
    {
        bool zoomNeeded = false;

        float highestPlayerHeight = Mathf.Max(positionPlayer1.y, positionPlayer2.y);
        float distanceFromTopBorder = _top - highestPlayerHeight;
        bool tooHigh = distanceFromTopBorder < _playerMarginBeforeZoom;

        float mostLeftPlayerPosition = Mathf.Min(positionPlayer1.x, positionPlayer2.x);
        float distanceFromLeftBorder = mostLeftPlayerPosition - _left;
        bool tooLeft = distanceFromLeftBorder < _playerMarginBeforeZoom;
        
        float mostRightPlayerPosition = Mathf.Max(positionPlayer1.x, positionPlayer2.x);
        float distanceFromRightBorder = _rightCollision - mostRightPlayerPosition;
        bool tooRight = distanceFromRightBorder < _playerMarginBeforeZoom;

        zoomNeeded = (tooHigh || tooLeft || tooRight);
        return zoomNeeded;
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
    
    
    // public void OrthographicZoom()
    // {
    //     Vector3 positionPlayer1 = _player1.position;
    //     Vector3 positionPlayer2 = _player2.position;
    //     float _targetCameraSize = _currentCameraSize;
    //     
    //     // _targetCameraSize -= Input.mouseScrollDelta.y * sensitivity;
    //     _targetCameraSize = Mathf.Clamp(_targetCameraSize, _minCameraSize, _maxCameraSize);
    // float newSize = Mathf.MoveTowards(_camera.orthographicSize, _targetCameraSize, _cameraZoomSpeed * Time.deltaTime);
    //     _camera.orthographicSize = newSize;
    //     _currentCameraSize = newSize;
    //
    // }
    
}
