using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(RectTransform))]
public class Map : MonoBehaviour
{
    private List<PinPrefab> _pins;
    private PinPrefab _selectedPin;
    private PinFactory _pinFactory;
   
    
    [Inject]
    private void Init(PinPrefab  pinPrefab)
    {
        _pinFactory = new PinFactory(pinPrefab, GetComponent<RectTransform>());
    }

    private void Awake()
    {
        _pins = new List<PinPrefab>();
        PinPrefab.OnDeleteRequest += OnDeletePinRequest;
    }

    private void OnDestroy()
    {
        PinPrefab.OnDeleteRequest -= OnDeletePinRequest;
    }

    void Start()
    {
        LoadPins();
    }
    
    
    private void OnDeletePinRequest(PinPrefab pinPrefab)
    {
        DeletePin(pinPrefab);
    }
    
    
    
    public void SelectPin(PinPrefab pinPrefab)
    {
        _selectedPin = pinPrefab;
        if (!_pins.Contains(pinPrefab))
        {
            Debug.LogWarning("Selected pin not in the list");
        }
    }
    
    public void DrugSelectedPin(Vector2 position)
    {
        _selectedPin.MovePin(position);
    }
    
    public void DeleteSelectedPin()
    {
        DeletePin(_selectedPin);
    }

    
    public void CreateNewPin(Vector3 position)
    {
        var pin = _pinFactory.CreatePin(position);
        _pins.Add(pin);
        pin.OpenFullDescription();
    }
    
    public void SavePins()
    {
        MapData data = new MapData();
        foreach (var pin in _pins)
        {
            data.pins.Add(pin.GetPinData());
        }
        SaveSystem.SaveData(data);
    }

    public void LoadPins()
    {
        ClearMap();
        
        var data = SaveSystem.LoadData<MapData>();
        foreach (var pin in data.pins)
        {
            var pinPrefab = _pinFactory.CreatePin(pin);
            _pins.Add(pinPrefab);
        }
    }

    public void ClearMap()
    {
        for (int i = _pins.Count - 1; i >= 0; i--)
        {
            DeletePin(_pins[i]);
        }
    }

    
    private void DeletePin(PinPrefab pinPrefab)
    {
        if (!_pins.Contains(pinPrefab))
        {
            Debug.LogWarning("Selected pin not in the list");
            return;
        }
        _pins.Remove(pinPrefab);
        _pinFactory.DestroyPin(pinPrefab);
    }

    public void DeleteSaveFile()
    {
        SaveSystem.DeleteData();
    }
    
    
    
    [Serializable]
    public class MapData
    {
        public List<Pin> pins = new();
    }
}
