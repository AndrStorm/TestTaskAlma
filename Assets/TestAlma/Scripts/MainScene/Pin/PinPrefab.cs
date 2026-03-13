using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class PinPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject popUpDescription;
    public TMP_Text nameLabel;
    public TMP_Text shortTextLabel;
    //public Image image;
    
    private Animator _animator;
    private RectTransform _rectTransform;
    private Pin _pinData;
    
    private float _popUpTimeDelay;
    private float _lastDeactivationTime;
    private bool _isDescriptionNeedToClose;
    
    
    public static event Action<PinPrefab> OnDeleteRequest;
    public static event Action<Pin, PinPrefab> OnOpenFullDescriptionRequest;
    
    
    
    private void Awake()
    {
        _animator =  GetComponent<Animator>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_isDescriptionNeedToClose)
        {
            CloseDescription();
        }
    }
    
    private void CloseDescription()
    {
        if (Time.realtimeSinceStartup - _lastDeactivationTime > _popUpTimeDelay)
        {
            _animator.Play("HideDescription");
            _isDescriptionNeedToClose = false;
        }
    }


    public void MovePin(Vector2 position)
    {
        _rectTransform.position = position;
        _pinData.position = position;
    }

    public Pin GetPinData()
    {
        return _pinData;
    }

    public void Init(Pin pinData, AppSettings settings)
    {
        _popUpTimeDelay = settings.DescriptionCloseDelay;
        _pinData =  pinData;
        UpdatePinData();
    }
    
    public void UpdatePinData()
    {
        nameLabel.text = _pinData.name;
        shortTextLabel.text = _pinData.text;
    }

    public void DeletePin()
    {
        OnDeleteRequest?.Invoke(this);
    }
    
    public void OpenFullDescription()
    {
        OnOpenFullDescriptionRequest?.Invoke(_pinData, this);
    }

    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _animator.Play("ShowDescription");
        _isDescriptionNeedToClose = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isDescriptionNeedToClose = true;
        _lastDeactivationTime = Time.realtimeSinceStartup;
    }

    
}
