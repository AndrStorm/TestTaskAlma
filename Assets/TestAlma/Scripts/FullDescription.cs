using System;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



public class FullDescription : MonoBehaviour
{
    public GameObject fullDescriptionWindow;
    public TMP_InputField nameInputField;
    public TMP_InputField textInputField;
    public RawImage image;
    public Image editButtonImage;
    public Texture2D defaultImage;

    public Color editModeColor = new(0.27f, 0.93f, 0.27f);

    private Texture2D _descriptionImageTexture;
    private PinPrefab _pinPrefab;
    private Pin _pinData;

    private Color _defModeColor;
    private bool _isEditMode;

    public static bool isOpen { get; private set; }
    
    public static event Action OnCloseFullDescriptionRequest;


    
    private void Awake()
    {
        PinPrefab.OnOpenFullDescriptionRequest += OpenPinDescription;
        OnCloseFullDescriptionRequest += ClosePinDescription;
    }

    private void OnDestroy()
    {
        PinPrefab.OnOpenFullDescriptionRequest -= OpenPinDescription;
        OnCloseFullDescriptionRequest -= ClosePinDescription;
    }
    
    public static void CloseFullDescription()
    {
        OnCloseFullDescriptionRequest?.Invoke();
    }
    
    private void Start()
    {
        _descriptionImageTexture = new Texture2D(4, 4);
        _defModeColor = editButtonImage.color;
        ClosePinDescription();
    }


    private void OpenPinDescription(Pin pinData, PinPrefab pinPrefab)
    {
        isOpen = true;
        _pinData = pinData;
        _pinPrefab = pinPrefab;
        
        fullDescriptionWindow.SetActive(true);
        UpdatePinData();
    }

    
    private void UpdatePinData()
    {
        nameInputField.text = _pinData.name;
        textInputField.text = _pinData.text;

        if (_pinData.Bytes != null && _pinData.Bytes.Length > 0)
        {
            _descriptionImageTexture.LoadImage(_pinData.Bytes);
            image.texture = _descriptionImageTexture;
        }
        else
        {
            image.texture = defaultImage;
        }
    }



    public void ChangeImage()
    {
        string path = EditorUtility.OpenFilePanel("Выбор изображения",
            Application.persistentDataPath, "png,jpg,jpeg");

        if (path.Length == 0) return;
        
        var bytes = File.ReadAllBytes(path);
        _descriptionImageTexture.LoadImage(bytes);
        image.texture = _descriptionImageTexture;
        _pinData.Bytes = bytes;
    }
    
    public void ClosePinDescription()
    {
        isOpen = false;
        fullDescriptionWindow.SetActive(false);
    }
    
    public void DeletePin()
    {
        ClosePinDescription();
        _pinPrefab.DeletePin();
    }

    
    public void EditPin()
    {
        _isEditMode = !_isEditMode;
        textInputField.interactable = _isEditMode;
        nameInputField.interactable = _isEditMode;
        editButtonImage.color = _isEditMode ? editModeColor :  _defModeColor;
    }

    public void SavePin()
    {
        SavePinData();
        _pinPrefab.UpdatePinData();
    }

    public void RevertChange()
    {
        UpdatePinData();
    }

    private void SavePinData()
    {
        _pinData.name = nameInputField.text;
        _pinData.text = textInputField.text;
    }
}
