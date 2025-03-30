using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    [Header("First Seleceted Button")]
    [SerializeField] private Button firstSelected;

    protected virtual void OnEnable()
    {
        if (firstSelected != null)
            SetFirstSelected(firstSelected);
    }

    public void SetFirstSelected(Button firstSelectedButton)
    {
        firstSelectedButton.Select();
    }
}
