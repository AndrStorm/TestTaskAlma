using UnityEngine;


public class PinFactory
{
    private readonly PinPrefab _pinPrefab;
    private readonly RectTransform _mapTransform;
    private readonly AppSettings _appSettings;
    
    
    public PinFactory(PinPrefab pinPrefab, RectTransform mapTransform,  AppSettings appSettings)
    {
        _pinPrefab = pinPrefab;
        _mapTransform = mapTransform;
        _appSettings = appSettings;
    }
    
    public PinPrefab CreatePin(Vector3 position)
    {
        Pin pinData = new Pin();
        var pin = Object.Instantiate(_pinPrefab, position, Quaternion.identity, _mapTransform);
        pin.Init(pinData, _appSettings);
        return pin;
    }
    
    public PinPrefab CreatePin(Pin pinData)
    {
        var pin = Object.Instantiate(_pinPrefab, Vector3.zero, Quaternion.identity, _mapTransform);
        pin.SetUpAnchoredPosition(pinData);
        pin.Init(pinData, _appSettings);
        return pin;
    }
    
    public void DestroyPin(PinPrefab pinPrefab)
    {
        Object.Destroy(pinPrefab.gameObject);
    }
   
    
}
