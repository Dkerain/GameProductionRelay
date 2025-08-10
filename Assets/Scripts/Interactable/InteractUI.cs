using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractUI : MonoBehaviour
{
    private List<BaseInteractable> _activateInteract = new();

    private Image _displayNode;

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.F; // 交互按键
    
    private void Awake()
    {
        _displayNode = GetComponent<Image>();
    }

    private void Start()
    {
        _activateInteract.Clear();
    }

    private void Update()
    {
        UpdateDisplay();

        if (_activateInteract.Count > 0 && Input.GetKeyDown(interactKey))
        {
            _activateInteract[0].Interact();
        }
    }
    
    private void UpdateDisplay()
    {
        if (_activateInteract.Count > 0)
        {
            var interact = _activateInteract[0];
            var worldPos = interact.transform.position + interact.worldOffset;
            var screenPos = Camera.main.WorldToScreenPoint(worldPos);
            _displayNode.transform.position = screenPos;
            _displayNode.enabled = true;
        }
        else
        {
            _displayNode.enabled = false;
        }
    }
    
    public void AddInteract(BaseInteractable interact)
    {
        _activateInteract.Add(interact);
    }

    public void RemoveInteract(BaseInteractable interact)
    {
        _activateInteract.Remove(interact);
    }
}