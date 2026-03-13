using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;


public class InputHandler
{
    private readonly float _clickDelay;
    private readonly Map _map;
    private bool _isDrugging;
    private float _lastClickTime;


    [Inject]
    private InputHandler(Map map, AppSettings appSettings)
    {
        _map = map;
        _clickDelay = appSettings.ClickPressingDelay;
    }
    
    public void OnClick(Vector2 position, bool isDown)
    {
        if (isDown)
        {
            _lastClickTime = Time.realtimeSinceStartup;
        }
        else if(Time.realtimeSinceStartup - _lastClickTime < _clickDelay)
        {
            if (IsOnlyOverMap(position))
            {
                if (FullDescription.isOpen)
                {
                    FullDescription.CloseFullDescription();
                }
                else
                {
                    _map.CreateNewPin(position);
                }
            }
        }

        if (isDown && IsOverMap(position) && IsOverPin(out var pinPrefab))
        {
            _map.SelectPin(pinPrefab);
            _isDrugging = true;
        }
        else
        {
            _isDrugging = false;
        }
        
        //- screen size pos
        //Debug.Log("CanvasToWorld " + CanvasToWorld(_map.RectTransform));
    }
    
    
    public void OnPoint(Vector2 point)
    {
        if (!_isDrugging) return;
        
        if (!IsOverMap(point))
        {
            OnMapLeave();
        }
        else
        {
            _map.DrugSelectedPin(point);
        }
    }

    private void OnMapLeave()
    {
        _isDrugging = false;
    }
    


    private bool IsOverMap(Vector2 screenPosition)
    {
        return IsOverUIElement(screenPosition, "Map");
    }
    
    private bool IsOnlyOverMap(Vector2 screenPosition)
    {
        return IsOverOneUIElement(screenPosition, "Map");
    }
    
    private bool IsOverPin(out PinPrefab pinPrefab)
    {
        foreach (var result in _results)
        {
            if (result.gameObject.TryGetComponent(out pinPrefab))
            {
                return true;
            }
        }

        pinPrefab = null;
        return false;
    }
    
    
    
    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;
    private static bool IsOverUIElement(Vector2 screenPosition, string elementName)
    {
        UpdatePointerData(screenPosition);
        return _results.Count > 0 && _results.Any(result => result.gameObject.name == elementName);
    }
    
    private static bool IsOverOneUIElement(Vector2 screenPosition, string elementName)
    {
        UpdatePointerData(screenPosition);
        return _results.Count == 1 && _results.Any(result => result.gameObject.name == elementName);
    }

    private static void UpdatePointerData(Vector2 screenPosition)
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current) {
            position = screenPosition };
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
    }

    
    
}