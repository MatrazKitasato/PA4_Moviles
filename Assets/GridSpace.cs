using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    private gameManager gameManager;
    
    public Button button;
    public TextMeshProUGUI buttontext;
    public string playerSide;
    void Start()
    {
        button = GetComponent<Button>();
        buttontext = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetSpace()
    {
        buttontext.text = gameManager.GetPlayerSide();
        button.interactable = false;
        gameManager.EndTurn();
    }
    public void SetController(gameManager _gameManager)
    {
        gameManager = _gameManager;
    }
}
