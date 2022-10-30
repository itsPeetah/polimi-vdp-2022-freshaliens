using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private Transform Player1;
    [SerializeField] private Transform Player2;
    
    [Header("PlayerBorders")]
    [SerializeField] private GameObject BorderLeft;
    [SerializeField] private GameObject BorderRight;
    [SerializeField] private GameObject BorderTop;
    
    [Header("Camera Parameters")] 
    [SerializeField] private float _horizontalOffsetRight = 0;
    [SerializeField] private float _horizontalOffsetLeft = 0;
    [SerializeField] private float _verticalOffset = 2f;
    [SerializeField] private float _minCameraSize = 5f;
    [SerializeField] private float _maxCameraSize = 10f;
    [SerializeField] private float _limitDistanceFromBorders = 0.5f;
    [SerializeField] private float _playerMarginBeforeZoom = 1f;
    [SerializeField] private float _cameraZoomSpeed = 30f;
    //Sensitivity is only for manual zoom
    [SerializeField] private float sensitivity = 1f;

    //State
    private Vector3 _currentPosition = Vector3.zero;
    private Transform _transform;
    private Camera _cam;
    private float _currentCameraSize;
    private float _currentWidth;
    private float _currentHeight;
    

    private void Start()
    {
        //Set initial state
        _transform = transform;
        _currentPosition = _transform.position;
        _transform.position = new Vector3(_currentPosition.x, _verticalOffset, _currentPosition.z);
        _currentPosition = _transform.position;
        
        _cam = GetComponentInChildren<Camera>();
        _currentCameraSize = _cam.orthographicSize;
        _currentHeight = _currentCameraSize;
        _currentWidth = _currentHeight * 16 / 9;
        
        //Set boundaries
        float sideBorderDistance, topBorderDistance;
        sideBorderDistance = (_maxCameraSize * 16 / 9) - _limitDistanceFromBorders;
        topBorderDistance = _maxCameraSize - _limitDistanceFromBorders;
        
        Vector3 leftBorderPosition = new Vector3(_currentPosition.x - sideBorderDistance, _currentPosition.y, _currentPosition.z);
        BorderLeft.transform.SetPositionAndRotation(leftBorderPosition, Quaternion.identity);
        
        Vector3 rightBorderPosition = new Vector3(_transform.position.x + sideBorderDistance, _currentPosition.y, _currentPosition.z);
        BorderRight.transform.SetPositionAndRotation(rightBorderPosition, Quaternion.identity);
        
        Vector3 topBorderPosition = new Vector3(_transform.position.x , _currentPosition.y + topBorderDistance, _currentPosition.z);
        BorderTop.transform.SetPositionAndRotation(topBorderPosition, Quaternion.identity);

    }


    void LateUpdate()
    {
        //The horizontal position of the camera is the average of the two players
        //The vertical position of the camera is fixed
        Vector3 positionPlayer1 = Player1.position;
        Vector3 positionPlayer2 = Player2.position;

        
        float newX = -_horizontalOffsetLeft + ((positionPlayer1.x + positionPlayer2.x) / 2) + _horizontalOffsetRight;

        Vector3 newCameraTrackerPosition = new Vector3(newX, _currentPosition.y, _currentPosition.z);
        _transform.position = newCameraTrackerPosition;
        _currentPosition = newCameraTrackerPosition;

        //to Finish: zoom
        bool tooHigh, tooFar;
        tooHigh = (Mathf.Max(positionPlayer1.y, positionPlayer2.y) - _currentHeight) < _playerMarginBeforeZoom;
        tooFar = (Mathf.Max(positionPlayer1.x, positionPlayer2.x) - _currentWidth) < _playerMarginBeforeZoom;

    }
    
    
    public void OrthographicZoom()
    {
        Vector3 positionPlayer1 = Player1.position;
        Vector3 positionPlayer2 = Player2.position;
        float _targetCameraSize = _currentCameraSize;
        
        // _targetCameraSize -= Input.mouseScrollDelta.y * sensitivity;
        _targetCameraSize = Mathf.Clamp(_targetCameraSize, _minCameraSize, _maxCameraSize);
        float newSize = Mathf.MoveTowards(_cam.orthographicSize, _targetCameraSize, _cameraZoomSpeed * Time.deltaTime);
        _cam.orthographicSize = newSize;
        _currentCameraSize = newSize;

    }
}