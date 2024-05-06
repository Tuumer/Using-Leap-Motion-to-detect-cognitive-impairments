using System.Collections.Generic;
using UnityEngine;

namespace _Project
{
    public class ConnectionManager : MonoBehaviour
    {
        public List<GameObject> connectedObjects = new List<GameObject>();
        public List<Vector3> drawPositions = new List<Vector3>();
        public LineRenderer lineRen;
        public LayerMask targetLayerMask;
        public Material correct;
        public Material incorrect;
        private Camera _mainCamera;
        private bool _isDrawing;
        private int currentSphereIndex = 0;
        private GameObject lastHoveredObject;

        private void Start()
        {
            _mainCamera = Camera.main;
            HideAllSpheresExceptFirst();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = GetRayOnMousePosition();
                if (Physics.Raycast(ray, out var raycastHit, 1920f, targetLayerMask))
                {
                    if (raycastHit.transform.CompareTag("tmtstart"))
                    {
                        _isDrawing = true;
                        connectedObjects.Add(raycastHit.transform.gameObject);
                        lineRen.gameObject.SetActive(true);
                    }
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
                        if (targetObject.CompareTag("tmtstart"))
                        {
                            if (connectedObjects.Count == 0 || !connectedObjects[connectedObjects.Count - 1].CompareTag("endtmt"))
                            {
                                connectedObjects.Add(targetObject);
                            }
                        }
                        else
                        {
                            connectedObjects.Add(targetObject);
                        }
                    }
                }
                DrawLine();
            }

            if (Input.GetMouseButtonUp(0) && _isDrawing)
            {
                float correctRatio = CountCorrectObjects();

                string[] expectedTags = { "tmtstart", "2", "3", "4", "5", "6", "7", "endtmt" };

                Debug.Log("Ratio of correct objects selected: " + correctRatio + "/" + expectedTags.Length);
                _isDrawing = false;
                connectedObjects.Clear();
                DeActiveDrawing();
            }

            // Обновляем позицию линии каждый кадр
            UpdateLinePosition();

            // Additional logic for selecting next sphere based on tag order
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentSphereIndex++;
                if (currentSphereIndex < connectedObjects.Count)
                {
                    connectedObjects[currentSphereIndex].SetActive(true);
                }
            }
        }

        private void OnMouseEnter()
        {
            if (lastHoveredObject != null)
            {
                lastHoveredObject.GetComponent<Renderer>().material = correct;
            }
            lastHoveredObject = gameObject;
            GetComponent<Renderer>().material = incorrect;
        }

        private void OnMouseExit()
        {
            if (lastHoveredObject != null)
            {
                lastHoveredObject.GetComponent<Renderer>().material = correct;
            }
            lastHoveredObject = null;
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

        private void HideAllSpheresExceptFirst()
        {
            for (int i = 0; i < connectedObjects.Count; i++)
            {
                if (i != 0)
                {
                    connectedObjects[i].SetActive(false);
                }
            }
        }

        private float CountCorrectObjects()
        {
            string[] expectedTags = { "tmtstart", "2", "3", "4", "5", "6", "7", "endtmt" };
            int correctCount = 0;

            int minLength = Mathf.Min(connectedObjects.Count, expectedTags.Length);
            for (int i = 0; i < minLength; i++)
            {
                if (connectedObjects[i].CompareTag(expectedTags[i]))
                {
                    correctCount++;
                }
                else
                {
                    break; // Прерываем цикл, если порядок нарушен
                }
            }

            return (float)correctCount;
        }

        private void UpdateLinePosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 88f; // Здесь мы используем заранее заданное расстояние от камеры до объекта
            Vector3 worldMousePosition = _mainCamera.ScreenToWorldPoint(mousePosition);

            if (connectedObjects.Count > 0)
            {
                drawPositions.Clear();
                foreach (var targetObject in connectedObjects)
                {
                    drawPositions.Add(targetObject.transform.position);
                }

                drawPositions.Add(worldMousePosition);

                lineRen.positionCount = drawPositions.Count;
                lineRen.SetPositions(drawPositions.ToArray());
            }
        }
    }
}
