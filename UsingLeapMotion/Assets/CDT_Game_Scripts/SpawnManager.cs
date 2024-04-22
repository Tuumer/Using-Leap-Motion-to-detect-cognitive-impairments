using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] _objects;
    [SerializeField] Camera _camera;
    [SerializeField] int _offsetX;
    [SerializeField] int _offsetY;
    [SerializeField] Text _timerText;

    

    void Start()
    {
        SpawnAllObjects();
    }

    void Update()
    {
        UpdateTimer();
    }

    void SpawnAllObjects()
    {
        for (int i = 0; i < 30; i++)
        {
            int randomObjectId = Random.Range(0, _objects.Length);
            Vector2 position = GetRandomCoordinates();
            GameObject spawnedObject = Instantiate(_objects[randomObjectId], position, Quaternion.identity);
        }
    }

    void UpdateTimer()
    {
        float timeElapsed = Time.time;
        if (_timerText != null)
        {
            _timerText.text = "Time Elapsed: " + Mathf.FloorToInt(timeElapsed).ToString();
        }
    }

    Vector2 GetRandomCoordinates()
    {
        int randomx = Random.Range(0 + _offsetX, Screen.width - _offsetX);
        int randomy = Random.Range(0 + _offsetY, Screen.height - _offsetY);

        Vector2 screenPoint = new Vector2(randomx, randomy);
        Vector2 worldPosition = _camera.ScreenToWorldPoint(screenPoint);

        return worldPosition;
    }
}
