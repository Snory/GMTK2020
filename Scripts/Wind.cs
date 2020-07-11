using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public GameObject LinePrefab;
    private GameObject _currentLine;
    private LineRenderer _lineRendered;
    private EdgeCollider2D _edgeCollier;
    private List<Vector2> _mousePositions;
    private Camera _mainCamera;

    private float _lineLenght = 0.1f;


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
            CreateLine();
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 tempMousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            bool isSpacedEnough = Vector2.Distance(tempMousePosition, _mousePositions[_mousePositions.Count - 1]) > _lineLenght;

            if (isSpacedEnough)
            {
                UpdateLine(tempMousePosition);
            }
        }
    }


    void CreateLine()
    {
        _currentLine = Instantiate(LinePrefab, Vector3.zero, Quaternion.identity);
        _lineRendered = _currentLine.GetComponent<LineRenderer>();
        _edgeCollier = _currentLine.GetComponent<EdgeCollider2D>();
        _mousePositions.Clear();
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _mousePositions.Add(mousePosition);
        _mousePositions.Add(mousePosition);
        _lineRendered.SetPosition(0, _mousePositions[0]);
        _lineRendered.SetPosition(1, _mousePositions[1]);
        _edgeCollier.points = _mousePositions.ToArray();
    }

    void UpdateLine(Vector2 mousePosition)
    {
        _mousePositions.Add(mousePosition);
        _lineRendered.positionCount++;
        _lineRendered.SetPosition(_lineRendered.positionCount - 1, mousePosition);
        _edgeCollier.points = _mousePositions.ToArray();
    }
}
