using System;
using System.Collections.Generic;
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
    public Color32 backroundColour = new Color32(0, 0, 0, 0);

    [Tooltip("Toggles between Pen and Eraser.")]
    public bool IsEraser = false;

    private bool _isInFocus = false;
    public bool IsInFocus
    {
        get => _isInFocus;
        private set
        {
            if (value != _isInFocus)
            {
                _isInFocus = value;
            }
        }
    }

    private float m_scaleFactor = 10;
    private RawImage m_image;
    private Vector2? m_lastPos;

    public int eraserRadius = 20; // Larger eraser radius

    void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        m_image = transform.GetComponent<RawImage>();
    }

    void Update()
    {
        var pos = Input.mousePosition;

        if (IsInFocus)
        {
            if (Input.GetMouseButton(0))
            {
                if (IsEraser)
                    ErasePixels(pos);
                else
                    WritePixels(pos);
            }
        }

        if (Input.GetMouseButtonUp(0))
            m_lastPos = null;
    }

    private void Init()
    {
        m_scaleFactor = HostCanvas.scaleFactor * 2;

        var tex = new Texture2D(Convert.ToInt32(Screen.width / m_scaleFactor), Convert.ToInt32(Screen.height / m_scaleFactor), TextureFormat.RGBA32, false);
        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                tex.SetPixel(i, j, backroundColour);
            }
        }

        tex.Apply();
        m_image.texture = tex;
    }

    private void WritePixels(Vector2 pos)
    {
        pos /= m_scaleFactor;
        var mainTex = m_image.texture;
        var tex2d = new Texture2D(mainTex.width, mainTex.height, TextureFormat.RGBA32, false);

        var curTex = RenderTexture.active;
        var renTex = new RenderTexture(mainTex.width, mainTex.height, 32);

        Graphics.Blit(mainTex, renTex);
        RenderTexture.active = renTex;

        tex2d.ReadPixels(new Rect(0, 0, mainTex.width, mainTex.height), 0, 0);

        var positions = m_lastPos.HasValue ? GetLinearPositions(m_lastPos.Value, pos) : new List<Vector2>() { pos };

        foreach (var position in positions)
        {
            var pixels = GetNeighbouringPixels(new Vector2(mainTex.width, mainTex.height), position, 2);

            if (pixels.Count > 0)
                foreach (var p in pixels)
                    tex2d.SetPixel((int)p.x, (int)p.y, penColour);
        }

        tex2d.Apply();

        RenderTexture.active = curTex;
        renTex.Release();
        Destroy(renTex);
        Destroy(mainTex);
        curTex = null;
        renTex = null;
        mainTex = null;

        m_image.texture = tex2d;
        m_lastPos = pos;
    }

    private void ErasePixels(Vector2 pos)
    {
        pos /= m_scaleFactor;
        var mainTex = m_image.texture;
        var tex2d = new Texture2D(mainTex.width, mainTex.height, TextureFormat.RGBA32, false);

        var curTex = RenderTexture.active;
        var renTex = new RenderTexture(mainTex.width, mainTex.height, 32);

        Graphics.Blit(mainTex, renTex);
        RenderTexture.active = renTex;

        tex2d.ReadPixels(new Rect(0, 0, mainTex.width, mainTex.height), 0, 0);

        var positions = GetNeighbouringPixels(new Vector2(mainTex.width, mainTex.height), pos, eraserRadius);

        foreach (var position in positions)
        {
            var pixels = GetNeighbouringPixels(new Vector2(mainTex.width, mainTex.height), position, 1);

            if (pixels.Count > 0)
                foreach (var p in pixels)
                    tex2d.SetPixel((int)p.x, (int)p.y, backroundColour);
        }

        tex2d.Apply();

        RenderTexture.active = curTex;
        renTex.Release();
        Destroy(renTex);
        Destroy(mainTex);
        curTex = null;
        renTex = null;
        mainTex = null;

        m_image.texture = tex2d;
        m_lastPos = pos;
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
}
