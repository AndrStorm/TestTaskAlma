using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputService : IInitializable, IDisposable
{
    private InputAction _clickAction;
    private InputAction _pointAction;

    private InputHandler _inputHandler;

    [Inject]
    private InputService(InputHandler  inputHandler)
    {
        _inputHandler = inputHandler;
    }
    
    
    public void Initialize()
    {
        _clickAction = InputSystem.actions["Click"];
        _pointAction = InputSystem.actions["Point"];

        _clickAction.performed += OnClick;
        _pointAction.performed += OnPoint;
    }

    public void Dispose()
    {
        _clickAction.performed -= OnClick;
        _pointAction.performed -= OnPoint;
    }
    
    private void OnClick(InputAction.CallbackContext context)
    {
        Vector2 position = _pointAction.ReadValue<Vector2>();
        _inputHandler.OnClick(position, context.ReadValueAsButton());
    }
    
    
    private void OnPoint(InputAction.CallbackContext context)
    {
        _inputHandler.OnPoint(context.ReadValue<Vector2>());
    }
}