using UnityEngine;


public class PinFactory
{
    private readonly PinPrefab _pinPrefab;
    private readonly RectTransform _mapTransform;
    
    public PinFactory(PinPrefab pinPrefab, RectTransform mapTransform)
    {
        _pinPrefab = pinPrefab;
        _mapTransform = mapTransform;
    }
    
    public PinPrefab CreatePin(Vector3 position)
    {
        Pin pinData = new Pin
        {
            position = position
        };
        var pin = CreatePin(pinData);
        return pin;
    }
    
    public PinPrefab CreatePin(Pin pinData)
    {
        var pin = Object.Instantiate(_pinPrefab, pinData.position, Quaternion.identity, _mapTransform);
        pin.SetPinData(pinData);
        return pin;
    }
    
    public void DestroyPin(PinPrefab pinPrefab)
    {
        Object.Destroy(pinPrefab.gameObject);
    }
   
    
}
