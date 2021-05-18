using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected FieldController gameField;
    protected int[] gameBoard = new int[25];
    protected int center;
    protected int[] temples = { 2, 22 };

    protected HandController[] hands = new HandController[2];
    [SerializeField] protected HandController blueHand;
    [SerializeField] protected HandController redHand;

    protected CardBehaviour[] sideCards = new CardBehaviour[2];
    [SerializeField] protected CardBehaviour blueSideCard;
    [SerializeField] protected CardBehaviour redSideCard;

    [SerializeField] protected WinScreen winScreen;

    [SerializeField] protected int width;
    [SerializeField] protected int height;

    [SerializeField] protected int active;

    protected DeckBehaviour deck = new DeckBehaviour();

    [SerializeField] ConcedeButtons concedeButtons;

    virtual public void Start()
    {
        //Get center of gamefield
        center = gameField.GetCenter();
        //SetupAspect();

        //Shuffle deck and give cards
        SetupDeck();
        SetupBlueHand();
        SetupRedHand();

        //Place pieces on the field
        SetupField();

        StartGame();

        //Connect concede buttons to concede event
        concedeButtons.concede.AddListener(Concede);
    }

    public void SetupAspect()
    {

        float deltaWidth = Screen.currentResolution.height / width;
        print(Screen.currentResolution.width);
        print(Screen.safeArea.width);
        print(deltaWidth);
        float deltaHeight = Screen.currentResolution.width / height;
        print(Screen.currentResolution.height);
        print(Screen.safeArea.height);
        print(deltaHeight);
        float screenAspect = (float)Screen.width / Screen.height;
        float deltaAspect = (deltaHeight > deltaWidth) ? deltaWidth : deltaHeight;
        float aspect = (float)height / width;
        print(Camera.main.aspect);
        print(aspect);
        deltaAspect = aspect / Camera.main.aspect;
        transform.localScale = new Vector3(deltaAspect, deltaAspect, 1);
        winScreen.transform.localScale = transform.localScale;
    }

    virtual protected void SetupField()
    {
        /*
        gameField.endMoveEvent.AddListener(MoveEnd);
        gameField.par = this;
        gameField.AttemptMove.AddListener(GetMove);
        */
    }

    protected void SetupDeck()
    {
        //Start deck and shuffle it
        //print("DeckShuffled");
        deck.Start();
        deck.Shuffle();
    }
    protected void SetupBlueHand()
    {
        //Add Blue hand and Side card to array
        blueHand.Start();
        hands[0] = blueHand;
        sideCards[0] = blueSideCard;
    }

    protected void SetupRedHand()
    {
        //Add Red hand and Side card to array
        redHand.Start();
        hands[1] = redHand;
        sideCards[1] = redSideCard;
    }

    virtual protected void SetActive(int act)
    {
        //Change active player to act
        active = act;
        //Activate active player hand and disable other's
        hands[act].Activate();
        hands[1 - act].DeActivate();
        print(concedeButtons);
        //Change concede button
        concedeButtons.ChangeActive(act);
    }

    virtual protected void GetMove()
    {
        //Set active move to field
        int[] move = hands[active].GetMove();
        gameField.SetMoves(move);
    }
    protected void StartGame()
    {
        //Choose first player
        int act = Random.Range(0, 2);
        //act = 1;
        setupGame();
        SetMoves();
        StartField();
        SetActive(act);
    }

    public void setupGame()
    {
        //Fill gameBoard array
        int width = gameField.GetWidth();
        for (int i = 0; i < gameField.GetHeight(); i++)
        {
            int color = PieceCategory.None;
            //Pieces on first row colored blue on last colored red
            if (i == 0) color = PieceCategory.Blue;
            if (i == 4) color = PieceCategory.Red;

            for (int j = 0; j < width; j++)
            {
                if (color == 0)
                {
                    gameBoard[i * width + j] = color;
                    continue;
                }
                //Pieces in the middle of player line is Masters other is Students
                if (j == 2) gameBoard[i * width + j] = PieceCategory.Master | color;
                else gameBoard[i * width + j] = PieceCategory.Student | color;
            }
        }
        //Print Field
        printBoard();
    }
    virtual protected void SetMoves()
    {
        //print("SETMOVES");
        printArr((int[])deck.getCard(0).Clone());
        //printArr((int[])deck.getCard(1).Clone());
        //Blue draw two cards in hand
        hands[0].SetMove(deck.getCard(0), deck.getCard(1));
        hands[0].StartCards();
        //printArr((int[])deck.getCard(2).Clone());
        //printArr((int[])deck.getCard(3).Clone());
        //Red draw next two cards
        hands[1].SetMove(RotateMove(deck.getCard(2)), RotateMove(deck.getCard(3)));
        hands[1].StartCards();
        //printArr((int[])deck.getCard(4).Clone());
        //Fifth card goes to side
        int[] sidemove = deck.getCard(4);
        sideCards[0].Start();
        sideCards[0].SetMove(sidemove);
        //If there is red side card set same fifth card
        if (redSideCard != null)
        {
            sideCards[1].Start();
            sideCards[1].SetMove(RotateMove(sidemove));
        }
    }
    protected void printArr(int[] arr)
    {
        //Function for printing arrays
        string res = "";
        for (int i = 0; i < arr.Length; i++)
        {
            res += arr[i];
            res += ", ";
        }
        //print(res);
    }

    protected string printArr2d(int[] arr)
    {
        //Function for printing arrays
        string res = "";
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                res += arr[i * width + j];
                res += ", ";
            }
            res += "\n";
        }
        return res;
    }
    protected void StartField()
    {
        //Start gamefield and set scale then display gameBoard
        gameField.Start();
        gameField.Setup();
        gameField.SetScale(transform.localScale);
        gameField.Display(gameBoard);
        
    }
    
    virtual public void MoveEnd(int from, int to)
    {
        
    }

    protected int[] RotateMove(int[] m)
    {
        //Function for rotating move in int array
        int[] res = (int[])m.Clone();
        for (int i = 0; i < res.Length; i++)
            res[i] = 24 - res[i];
        return res;
    }

    protected int[] RotateMove(List<int> m)
    {
        //Function for rotating move in int list
        int[] res = new int[m.Count];
        for (int i = 0; i < m.Count; i++)
            res[i] = 24 - m[i];
        return res;
    }
    virtual protected void printBoard()
    {
        //Function for printing gameBoard
        int width = gameField.GetWidth();
        string boardStr = "";
        for (int i = 0; i < gameField.GetHeight(); i++)
        {
            for (int j = 0; j < width; j++)
            {
                boardStr += gameBoard[i * width + j];
                boardStr += ", ";
            }
            boardStr += "\n";
        }
    }

    public bool GetPosition(int from, int to)
    {
        //print(from);
        //print(to);
        //print(from + to);
        return (gameBoard[from] & ((active + 1) * 4)) > 0 && (gameBoard[from + to] & ((active + 1) * 4)) == 0;
    }

    protected void ShowWin(int player)
    {
        //Show WinScreen wtih text for winning player
        print(player.ToString() + " WON");
        winScreen.ShowScreen(player);
    }

    public void Concede(int act)
    {
        //ShowWin with text for other player
        ShowWin(1 - act);
    }
}
