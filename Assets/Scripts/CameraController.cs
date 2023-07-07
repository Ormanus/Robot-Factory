using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public Tilemap tileMap;
    public float movementSpeed = 5f;

    Camera _cam;
    float _scale;

    Bounds _camBounds;
    Bounds _mapBounds;

    private void Awake()
    {
        _cam = GetComponent<Camera>();

        var x0 = _cam.ScreenToWorldPoint(Vector3.zero).x;
        var x1 = _cam.ScreenToWorldPoint(Vector3.right).x;
        _scale = x1 - x0;

        var h = _cam.orthographicSize * 2f;
        var v = h * _cam.aspect;
        _camBounds = new Bounds(_cam.transform.position, new Vector3(v, h));
        //_mapBounds = tileMap.localBounds;

        //MoveToMapBounds();
    }

    private void Update()
    {
        Vector2 pos = Input.mousePosition;
        Vector2 halfScreen = new Vector2(Screen.width, Screen.height) * 0.5f;
        pos -= halfScreen;
        Vector2 halfGameArea = halfScreen * 0.9f;

        if (pos.x > halfGameArea.x)
        {
            transform.position += Vector3.right * movementSpeed * Time.deltaTime;
        }
        if (pos.x < -halfGameArea.x)
        {
            transform.position += Vector3.left * movementSpeed * Time.deltaTime;
        }
        if (pos.y > halfGameArea.y)
        {
            transform.position += Vector3.up * movementSpeed * Time.deltaTime;
        }
        if (pos.y < -halfGameArea.y)
        {
            transform.position += Vector3.down * movementSpeed * Time.deltaTime;
        }


        //MoveToMapBounds();
    }

    void MoveToMapBounds()
    {
        _camBounds.center = _cam.transform.position;
        var minDelta = _camBounds.min - _mapBounds.min;
        var maxDelta = _camBounds.max - _mapBounds.max;

        if (minDelta.x < 0)
        {
            transform.position += Vector3.right * -minDelta.x;
        }
        if (minDelta.y < 0)
        {
            transform.position += Vector3.up * -minDelta.y;
        }
        if (maxDelta.x > 0)
        {
            transform.position += Vector3.right * -maxDelta.x;
        }
        if (maxDelta.y > 0)
        {
            transform.position += Vector3.up * -maxDelta.y;
        }
    }
}
