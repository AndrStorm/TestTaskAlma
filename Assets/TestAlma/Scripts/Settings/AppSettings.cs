using UnityEngine;

[CreateAssetMenu(fileName = "AppSettings", menuName = "ScriptableObjects/AppSettings")]
public class AppSettings : ScriptableObject
{
    
    [Header("Input settings")]
    [SerializeField] private float _clickPressingDelay = 0.5f;
    
    [Header("Map settings")]
    [SerializeField] private float _descriptionCloseDelay = 0.8f;
    
    
    
    public float ClickPressingDelay => _clickPressingDelay;
    public float DescriptionCloseDelay => _descriptionCloseDelay;
    
}
