using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseDraw : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Canvas HostCanvas;

    [Tooltip("The Pens Colour.")]
    public Color32 penColour = new Color32(0, 0, 0, 255);

    [Tooltip("The Drawing Background Colour.")]
    public Color32 backgroundColour = new Color32(0, 0, 0, 0);

    [Tooltip("Toggles between Pen and Eraser.")]
    public bool IsEraser = false;
    public GameObject hourArrow;
    private bool _isInFocus = false;
    public bool IsInFocus
    {
        get => _isInFocus;
        private set => _isInFocus = value;
    }

    private float m_scaleFactor = 10;
    private RawImage m_image;
    private Vector2? m_lastPos;
    [SerializeField] UnityEngine.UI.Text _timerText; // Specify UnityEngine.UI.Text

    public int eraserRadius = 20;

    public class ObjectBoundary
    {
        public Rect Boundary;
        public Color32 ObjectColor;
        public float Rotation;
        public int TotalPixels;
        public int DrawnPixels;

        public ObjectBoundary(Rect boundary, Color32 objectColor, float rotation = 0)
        {
            Boundary = boundary;
            ObjectColor = objectColor;
            Rotation = rotation;
            TotalPixels = CalculateTotalPixels(boundary, rotation);
            DrawnPixels = 0;
        }

        public float GetDrawnPercentage()
        {
            return ((float)DrawnPixels * 100) / TotalPixels;
        }

        private static int CalculateTotalPixels(Rect boundary, float rotation)
        {
            float rad = rotation * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            float newWidth = Mathf.Abs(boundary.width * cos) + Mathf.Abs(boundary.height * sin);
            float newHeight = Mathf.Abs(boundary.width * sin) + Mathf.Abs(boundary.height * cos);

            return Mathf.RoundToInt(newWidth * newHeight);
        }
    }

    public List<ObjectBoundary> ObjectBoundaries = new List<ObjectBoundary>();

    void Start()
    {
        Init();
        // Initialize object boundaries independently
        ObjectBoundaries.Add(new ObjectBoundary(new Rect(585, 288, 165, 50), Color.red, 30));
        ObjectBoundaries.Add(new ObjectBoundary(new Rect(0, 0, 165, 50), Color.blue, 0));
    }

    private void OnEnable()
    {
        m_image = GetComponent<RawImage>();
    }

    void Update()
    {
        UpdateTimer();
        var pos = Input.mousePosition;

        if (IsInFocus)
        {
            if (Input.GetMouseButton(0))
            {
                if (IsEraser)
                {
                    ErasePixels(pos);
                }
                else
                {
                    WritePixels(pos);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
            m_lastPos = null;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayPercentages();
        }

        DrawObjectBoundaries(); // Ensure boundaries are drawn
    }

    void UpdateTimer()
    {
        float timeElapsed = Time.time;
        if (_timerText != null)
        {
            _timerText.text = "Time Elapsed: " + Mathf.FloorToInt(timeElapsed).ToString();
        }
    }

    private void Init()
    {
        m_scaleFactor = HostCanvas.scaleFactor * 2;

        var tex = new Texture2D(Convert.ToInt32(Screen.width / m_scaleFactor), Convert.ToInt32(Screen.height / m_scaleFactor), TextureFormat.RGBA32, false);
        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                tex.SetPixel(i, j, backgroundColour);
            }
        }

        tex.Apply();
        m_image.texture = tex;
    }

    private void WritePixels(Vector2 pos)
    {
        pos /= m_scaleFactor;
        var tex2d = CreateWritableTexture(m_image.texture);

        var positions = m_lastPos.HasValue ? GetLinearPositions(m_lastPos.Value, pos) : new List<Vector2> { pos };

        foreach (var position in positions)
        {
            var pixels = GetNeighbouringPixels(new Vector2(tex2d.width, tex2d.height), position, 2);
            foreach (var p in pixels)
            {
                if (tex2d.GetPixel((int)p.x, (int)p.y) != penColour)
                {
                    tex2d.SetPixel((int)p.x, (int)p.y, penColour);
                    UpdateDrawnPixels(p, true);
                }
            }
        }

        tex2d.Apply();
        m_image.texture = tex2d;
        m_lastPos = pos;
    }

    private void ErasePixels(Vector2 pos)
    {
        pos /= m_scaleFactor;
        var tex2d = CreateWritableTexture(m_image.texture);

        var positions = GetNeighbouringPixels(new Vector2(tex2d.width, tex2d.height), pos, eraserRadius);

        foreach (var position in positions)
        {
            var pixels = GetNeighbouringPixels(new Vector2(tex2d.width, tex2d.height), position, 1);
            foreach (var p in pixels)
            {
                if (tex2d.GetPixel((int)p.x, (int)p.y) != backgroundColour)
                {
                    tex2d.SetPixel((int)p.x, (int)p.y, backgroundColour);
                    UpdateDrawnPixels(p, false);
                }
            }
        }

        tex2d.Apply();
        m_image.texture = tex2d;
        m_lastPos = pos;
    }

    private void UpdateDrawnPixels(Vector2 p, bool isDrawing)
    {
        foreach (var obj in ObjectBoundaries)
        {
            if (IsPointInRotatedRect(p, obj.Boundary, obj.Rotation))
            {
                if (isDrawing)
                {
                    obj.DrawnPixels++;
                }
                else
                {
                    obj.DrawnPixels--;
                }
            }
        }
    }

    private Texture2D CreateWritableTexture(Texture mainTex)
    {
        var tex2d = new Texture2D(mainTex.width, mainTex.height, TextureFormat.RGBA32, false);
        var curTex = RenderTexture.active;
        var renTex = new RenderTexture(mainTex.width, mainTex.height, 32);

        Graphics.Blit(mainTex, renTex);
        RenderTexture.active = renTex;

        tex2d.ReadPixels(new Rect(0, 0, mainTex.width, mainTex.height), 0, 0);
        tex2d.Apply();

        RenderTexture.active = curTex;
        renTex.Release();
        Destroy(renTex);

        return tex2d;
    }

    private List<Vector2> GetNeighbouringPixels(Vector2 textureSize, Vector2 position, int brushRadius)
    {
        var pixels = new List<Vector2>();

        for (int i = -brushRadius; i < brushRadius; i++)
        {
            for (int j = -brushRadius; j < brushRadius; j++)
            {
                var pxl = new Vector2(position.x + i, position.y + j);
                if (pxl.x > 0 && pxl.x < textureSize.x && pxl.y > 0 && pxl.y < textureSize.y)
                    pixels.Add(pxl);
            }
        }

        return pixels;
    }

    private List<Vector2> GetLinearPositions(Vector2 firstPos, Vector2 secondPos, int spacing = 2)
    {
        var positions = new List<Vector2>();

        var dir = secondPos - firstPos;

        if (dir.magnitude <= spacing)
        {
            positions.Add(secondPos);
            return positions;
        }

        for (int i = 0; i < dir.magnitude; i += spacing)
        {
            var v = Vector2.ClampMagnitude(dir, i);
            positions.Add(firstPos + v);
        }

        positions.Add(secondPos);
        return positions;
    }

    public void OnPointerEnter(PointerEventData eventData) => IsInFocus = true;

    public void OnPointerExit(PointerEventData eventData) => IsInFocus = false;

    public void DisplayPercentages()
    {
        UnityEngine.Debug.Log("Hour: " + ObjectBoundaries[0].GetDrawnPercentage());
        UnityEngine.Debug.Log("Minutes: " + ObjectBoundaries[1].GetDrawnPercentage());
    }

    public float GetHourResult()
    {
        if (ObjectBoundaries.Count > 0)
            return ObjectBoundaries[0].GetDrawnPercentage();
        else
            return 10f; // Or handle this situation appropriately
    }

    public float GetMinuteResult()
    {
        if (ObjectBoundaries.Count > 1)
            return ObjectBoundaries[1].GetDrawnPercentage();
        else
            return 10f; // Or handle this situation appropriately
    }

    private bool IsPointInRotatedRect(Vector2 point, Rect rect, float rotation)
    {
        Vector2 localPoint = point - new Vector2(rect.x + rect.width / 2, rect.y + rect.height / 2);

        float rad = -rotation * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        float localX = cos * localPoint.x - sin * localPoint.y;
        float localY = sin * localPoint.x + cos * localPoint.y;

        return Mathf.Abs(localX) <= rect.width / 2 && Mathf.Abs(localY) <= rect.height / 2;
    }

    private void DrawObjectBoundaries()
    {
        var tex2d = CreateWritableTexture(m_image.texture);

        foreach (var obj in ObjectBoundaries)
        {
            DrawBoundary(tex2d, obj);
        }

        tex2d.Apply();
        m_image.texture = tex2d;
    }

    private void DrawBoundary(Texture2D tex2d, ObjectBoundary obj)
    {
        Vector2 center = new Vector2(obj.Boundary.x + obj.Boundary.width / 2, obj.Boundary.y + obj.Boundary.height / 2);
        float rad = obj.Rotation * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        for (int x = 0; x < obj.Boundary.width; x++)
        {
            for (int y = 0; y < obj.Boundary.height; y++)
            {
                Vector2 localPoint = new Vector2(x - obj.Boundary.width / 2, y - obj.Boundary.height / 2);
                float rotatedX = cos * localPoint.x - sin * localPoint.y;
                float rotatedY = sin * localPoint.x + cos * localPoint.y;
                Vector2 worldPoint = center + new Vector2(rotatedX, rotatedY);
                if (worldPoint.x >= 0 && worldPoint.x < tex2d.width && worldPoint.y >= 0 && worldPoint.y < tex2d.height)
                {
                    tex2d.SetPixel((int)worldPoint.x, (int)worldPoint.y, obj.ObjectColor);
                }
            }
        }
    }
}
