using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWinScreen : WinScreen
{
    // Start is called before the first frame update
    override public void ShowScreen(int color)
    {
        if ((color + 1 * 4) == PieceCategory.Blue)
            BlueMessage.text += "\nWon";
        else
            BlueMessage.text += "\nLose";
        GetComponent<CanvasGroup>().alpha = 1f;
    }
}
