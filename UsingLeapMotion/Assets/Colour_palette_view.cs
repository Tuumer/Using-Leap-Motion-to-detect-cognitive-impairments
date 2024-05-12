using UnityEngine;
using UnityEngine.UI;

public class ColourPaletteView : MonoBehaviour
{
    [SerializeField]
    private MouseDraw MouseDrawComponent;

    [SerializeField]
    private Toggle penToggle; // Кнопка для включения режима рисования

    [SerializeField]
    private Toggle eraserToggle; // Кнопка для включения режима стирания

    private void OnEnable()
    {
        penToggle.onValueChanged.AddListener(OnPenToggled);
        eraserToggle.onValueChanged.AddListener(OnEraserToggled);
    }

    private void OnDisable()
    {
        penToggle.onValueChanged.RemoveListener(OnPenToggled);
        eraserToggle.onValueChanged.RemoveListener(OnEraserToggled);
    }

    private void OnPenToggled(bool value)
    {
        if (value)
        {
            // Включить режим рисования и отключить режим стирания
            MouseDrawComponent.IsEraser = false;
            Debug.Log("false");
        }
    }

    private void OnEraserToggled(bool value)
    {
        if (value)
        {
            // Включить режим стирания и отключить режим рисования
            MouseDrawComponent.IsEraser = true;
            Debug.Log("true");
        }
    }
}
