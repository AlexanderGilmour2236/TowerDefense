using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenView : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI gameOverText;
    [SerializeField] 
    private TextMeshProUGUI buttonText;
    [SerializeField]
    private Button button;
    
    public event Action ButtonClick;

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            ButtonClick?.Invoke();
        });
    }

    public void SetText(string text)
    {
        gameOverText.text = text;
    }

    public void SetButtonText(string text)
    {
        buttonText.text = text;
    }
}
