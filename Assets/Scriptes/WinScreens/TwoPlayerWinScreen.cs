﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TwoPlayerWinScreen : WinScreen
{
    override public void ShowScreen(int color)
    {
        if ((color + 1 * 4) == PieceCategory.Blue)
        {
            BlueMessage.text += "\nWon";
            RedMessage.text += "\nLose";
        }
        else
        {
            RedMessage.text += "\nWon";
            BlueMessage.text += "\nLose";
        }
        GetComponent<CanvasGroup>().alpha = 1f;
        RestartBtn.interactable = true;
        MainMenuBtn.interactable = true;
    }
}
