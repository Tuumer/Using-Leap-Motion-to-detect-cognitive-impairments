using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace _Project
{
    public class ConnectionManager : MonoBehaviour
    {
        public LineRenderer lineRen;
        public LayerMask targetLayerMask;
        private Camera _mainCamera;
        private bool _isDrawing;

        public List<GameObject> connectedObjects = new List<GameObject>();
        public List<Vector3> drawPositions = new List<Vector3>();

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = GetRayOnMousePosition();
                if (Physics.Raycast(ray, out var raycastHit, 1920f, targetLayerMask))
                {
                    _isDrawing = true;
                    connectedObjects.Add(raycastHit.transform.gameObject);
                    lineRen.gameObject.SetActive(true);
                }
            }

            if (Input.GetMouseButton(0) && _isDrawing)
            {
                var ray = GetRayOnMousePosition();
                if (Physics.Raycast(ray, out var raycastHit, 1080f, targetLayerMask))
                {
                    var targetObject = raycastHit.transform.gameObject;
                    if (!connectedObjects.Contains(targetObject))
                    {
                        connectedObjects.Add(raycastHit.transform.gameObject);
                    }
                }
                DrawLine();
            }

            if (Input.GetMouseButtonUp(0) && _isDrawing)
            {
                _isDrawing = false;
                connectedObjects.Clear();
                DeActiveDrawing();
            }
        }


        public Ray GetRayOnMousePosition()
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            return ray;
        }


        public void DrawLine()
        {
            drawPositions.Clear();
            if (connectedObjects.Count > 0)
            {
                foreach (var targetObject in connectedObjects)
                {
                    drawPositions.Add(targetObject.transform.position);
                }

                var inputDrawPosition = GetMouseWorldInputPosition();


                drawPositions.Add(inputDrawPosition);

                lineRen.positionCount = drawPositions.Count;
                lineRen.SetPositions(drawPositions.ToArray());
            }
        }

        public Vector3 GetMouseWorldInputPosition()
        {
            var targetInputPos = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 88f));
            return targetInputPos;
        }

        public void DeActiveDrawing()
        {
            lineRen.positionCount = 0;
            drawPositions.Clear();
            lineRen.gameObject.SetActive(false);
        }


    }
}