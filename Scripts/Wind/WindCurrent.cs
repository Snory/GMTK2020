using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCurrent : MonoBehaviour
{

    [Header("WindCUrrentStats")]
    [Range(0,1)]
    public float CurrentMagnitude;

    [Header("WindCurrentObject")]
    public GameObject LinePrefab;

    private LineRenderer _lineRendered;
    private EdgeCollider2D _edgeCollier;
    private AreaEffector2D _areaEffector2D;
    private List<Vector2> _mousePositions;
    private Camera _mainCamera;
    private GameObject _currentLine;
    private float _lineLenght = 0.5f;


    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        _mousePositions = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 tempMousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            CreateCurrent(tempMousePosition, tempMousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 tempMousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            bool isSpacedEnough = Vector2.Distance(tempMousePosition, _mousePositions[_mousePositions.Count - 1]) > _lineLenght;
            if (isSpacedEnough)
            {
                bool isTimeForNewLine = _lineRendered.positionCount > 2;

                if (isTimeForNewLine)
                {
                    CreateCurrent(_lineRendered.GetPosition(_lineRendered.positionCount-1), tempMousePosition);
                }
                else
                {
                    UpdateCurrent(tempMousePosition);
                    UpdateCurrentAngle(_lineRendered.GetPosition(_lineRendered.positionCount - 2), _lineRendered.GetPosition(_lineRendered.positionCount - 1));
                }
            }
        }
    }


    void CreateCurrent(Vector2 startPoint, Vector2 endPoint)
    {
 
        _currentLine = Instantiate(LinePrefab, Vector3.zero, Quaternion.identity);
        _lineRendered = _currentLine.GetComponent<LineRenderer>();
        _edgeCollier = _currentLine.GetComponent<EdgeCollider2D>();
        _areaEffector2D = _currentLine.GetComponent<AreaEffector2D>();
        _areaEffector2D.forceMagnitude = CurrentMagnitude;
        Destroy(_currentLine, 5f);

        _mousePositions.Clear();
        _mousePositions.Add(startPoint);
        _mousePositions.Add(endPoint);
        _lineRendered.SetPosition(0, _mousePositions[0]);
        _lineRendered.SetPosition(1, _mousePositions[1]);
        _edgeCollier.points = _mousePositions.ToArray();
    }

    void UpdateCurrent(Vector2 mousePosition)
    {
        _mousePositions.Add(mousePosition);
        _lineRendered.positionCount++;
        _lineRendered.SetPosition(_lineRendered.positionCount - 1, mousePosition);
        _edgeCollier.points = _mousePositions.ToArray();
           
    }

    void UpdateCurrentAngle(Vector2 startPoint, Vector2 endPoint)
    {

        Vector2 startToEndDirection = endPoint - startPoint;
        var angle = Mathf.Atan2(startToEndDirection.y, startToEndDirection.x) * Mathf.Rad2Deg;
        _areaEffector2D.forceAngle = angle;

    }
}
