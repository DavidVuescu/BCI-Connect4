using System;
using System.IO;
using System.Net;
using UnityEngine;
using Unity.ItemRecever;
using Intendix.Board;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    private enum Piece
    {
        Empty = 0,
        Yellow = 1,
        Red = 2
    }
    private static int[,] gameMatrix = new int[6, 7];   //Board matrix that stores the data of all pieces present on the board 
    public static bool activePlayer; //Player colour piece: 0-yellow; 1-red


    public GameObject redPiece;     //Red piece game object
    public GameObject yellowPiece;  //Yellow piece game object

    private int pieceheight = 4;    //Height at which a piece is spawned


    //Function switches boolean responsible for player colour
    private void nextPlayer()
    {
        if (!activePlayer) activePlayer = true;
        else activePlayer = false;
    }

    //Debug function for showing the board matrix inside the console
    private void showGameMatrix()
    {
        Debug.Log(gameMatrix[0, 0] + " " + (int)gameMatrix[0, 1] + " " + (int)gameMatrix[0, 2] + " " + (int)gameMatrix[0, 3] + " " + (int)gameMatrix[0, 4] + " " + (int)gameMatrix[0, 5] + " " + (int)gameMatrix[0, 6] + "\n" +
                gameMatrix[1, 0] + " " + (int)gameMatrix[1, 1] + " " + (int)gameMatrix[1, 2] + " " + (int)gameMatrix[1, 3] + " " + (int)gameMatrix[1, 4] + " " + (int)gameMatrix[1, 5] + " " + (int)gameMatrix[1, 6] + "\n" +
                gameMatrix[2, 0] + " " + (int)gameMatrix[2, 1] + " " + (int)gameMatrix[2, 2] + " " + (int)gameMatrix[2, 3] + " " + (int)gameMatrix[2, 4] + " " + (int)gameMatrix[2, 5] + " " + (int)gameMatrix[2, 6] + "\n" +
                gameMatrix[3, 0] + " " + (int)gameMatrix[3, 1] + " " + (int)gameMatrix[3, 2] + " " + (int)gameMatrix[3, 3] + " " + (int)gameMatrix[3, 4] + " " + (int)gameMatrix[3, 5] + " " + (int)gameMatrix[3, 6] + "\n" +
                gameMatrix[4, 0] + " " + (int)gameMatrix[4, 1] + " " + (int)gameMatrix[4, 2] + " " + (int)gameMatrix[4, 3] + " " + (int)gameMatrix[4, 4] + " " + (int)gameMatrix[4, 5] + " " + (int)gameMatrix[4, 6] + "\n" +
                gameMatrix[5, 0] + " " + (int)gameMatrix[5, 1] + " " + (int)gameMatrix[5, 2] + " " + (int)gameMatrix[5, 3] + " " + (int)gameMatrix[5, 4] + " " + (int)gameMatrix[5, 5] + " " + (int)gameMatrix[5, 6]);
    }

    
    /*FUNCTIONS FOR CHECKING WIN CONDITIONS*/
    private static bool checkWinVertical(int x, int y, int searchedColour)
    {
        int pieceCount = 0;
        //Upwards
        for (int i = x + 1; i < 6; ++i)
        {
            if (gameMatrix[i, y] == searchedColour) ++pieceCount;
            else break;
            //Debug.Log(String.Format("{0} is compared with{1}", gameMatrix[i,y], searchedColour));
        }
        //Downwards
        for (int i = x; i >= 0; --i)
        {
            if (gameMatrix[i, y] == searchedColour) ++pieceCount;
            else break;
            //Debug.Log(String.Format("{0} is compared with{1}", gameMatrix[i,y], searchedColour));
        }
        if (pieceCount > 3) return true;
        // Debug.Log(String.Format("The number of pieces connected to the one you placed is: {0}", pieceCount));

        return false;
    }
    private static bool checkWinHorizontal(int x, int y, int searchedColour)
    {
        int pieceCount = -1;
        //Right
        for (int i = y; i < 7; ++i)
        {
            if (gameMatrix[x, i] == searchedColour) ++pieceCount;
            else break;
            // Debug.Log(String.Format("{0} is compared with{1}", gameMatrix[x,i], searchedColour));
        }
        //Left
        for (int i = y; i >= 0; --i)
        {
            if (gameMatrix[x, i] == searchedColour) ++pieceCount;
            else break;
            // Debug.Log(String.Format("{0} is compared with{1}", gameMatrix[x,i], searchedColour));
        }
        if (pieceCount > 3) return true;
        // Debug.Log(String.Format("The number of pieces connected to the one you placed is: {0}", pieceCount));

        return false;
    }
    private static bool checkWinMainDiag(int x, int y, int searchedColour)
    {
        int pieceCount = -1;
        int j;
        j = y;
        //Upwards and to the left (diag)
        for (int i = x; i >= 0; i--)
        {
            if (j < 0) break;
            if (gameMatrix[i, j] == searchedColour) ++pieceCount;
            else break;
            --j;
        }
        j = y;
        //Downwards and to the right (diag)
        for (int i = x; i < 6; ++i)
        {
            if (j > 6) break;
            if (gameMatrix[i, j] == searchedColour) ++pieceCount;
            else break;
            ++j;
        }
        if (pieceCount > 3) return true;
        // Debug.Log(String.Format("The number of pieces connected to the one you placed is: {0}", pieceCount));

        return false;
    }
    private static bool checkWinSecDiag(int x, int y, int searchedColour)
    {
        //Upwards and to the right (diag)
        int pieceCount = -1;
        int j;
        j = y;
        for (int i = x; i >= 0; --i)
        {
            if (j > 6) break;
            if (gameMatrix[i, j] == searchedColour) ++pieceCount;
            else break;
            ++j;
        }
        j = y;
        for (int i = x; i < 6; ++i)
        {
            if (j < 0) break;
            if (gameMatrix[i, j] == searchedColour) ++pieceCount;
            else break;
            --j;
        }
        if (pieceCount > 3) return true;
        // Debug.Log(String.Format("The number of pieces connected to the one you placed is: {0}", pieceCount));

        return false;
    }

    //main win condition check function, checks adjecent tiles for pieces of the same colour as the current one
    //if it finds 4 adjecent pieces it returns a true value used for displaying win screens
    private static bool checkWin(int x, int y)
    {
        int searchedColour; //The colour of the current piece the win algo is looking for
        if (!activePlayer) searchedColour = 2; else searchedColour = 1;
        Debug.Log(String.Format("Looking for piece of the colour {0} (1=yellow,2=red)", searchedColour));

        if (checkWinVertical(x, y, searchedColour)) return true;
        if (checkWinHorizontal(x, y, searchedColour)) return true;
        if (checkWinMainDiag(x, y, searchedColour)) return true;
        if (checkWinSecDiag(x, y, searchedColour)) return true;

        return false;
    }

    //Function for checking if all columns are full
    private static bool checkTie()
    {
        for(int i=0;i<6;++i) 
            if(gameMatrix[0,i])
                return true;
        return false;
    }

    //Function for spawning a piece on screen and in the game matrix
    public void spawnPiece(int Row, bool activePlayer) // Function for placing pieces in game logic
    {
        int x = 5;
        --Row; //row is decremented because matrix uses row numbers 0-6 instead of 1 to 7
        if (gameMatrix[x, Row] == 0)
            if (activePlayer) gameMatrix[5, Row] = (int)Piece.Red; else gameMatrix[5, Row] = (int)Piece.Yellow;
        else for (x = 0; x < 5; ++x)
            {
                if (gameMatrix[x + 1, Row] != (int)Piece.Empty)
                {
                    if (activePlayer) gameMatrix[x, Row] = (int)Piece.Red; else gameMatrix[x, Row] = (int)Piece.Yellow;
                    break;
                }
            }
        showGameMatrix();

        switch (Row)
        {
        case 0:
            {
                if (activePlayer)
                    Instantiate(redPiece, new Vector2(-3, pieceheight), Quaternion.identity);
                else
                    Instantiate(yellowPiece, new Vector2(-3, pieceheight), Quaternion.identity);

            break;
            }
        case 1:
            {
                if (activePlayer)
                    Instantiate(redPiece, new Vector2(-2, pieceheight), Quaternion.identity);
                else
                    Instantiate(yellowPiece, new Vector2(-2, pieceheight), Quaternion.identity);

            break;
            }
        case 2:
            {
                if (activePlayer)
                    Instantiate(redPiece, new Vector2(-1, pieceheight), Quaternion.identity);
                else
                    Instantiate(yellowPiece, new Vector2(-1, pieceheight), Quaternion.identity);

            break;
            }
        case 3:
            {
                if (activePlayer)
                    Instantiate(redPiece, new Vector2(0, pieceheight), Quaternion.identity);
                else
                    Instantiate(yellowPiece, new Vector2(0, pieceheight), Quaternion.identity);

            break;
            }
        case 4:
            {
                if (activePlayer)
                    Instantiate(redPiece, new Vector2(1, pieceheight), Quaternion.identity);
                else
                    Instantiate(yellowPiece, new Vector2(1, pieceheight), Quaternion.identity);

            break;
            }
        case 5:
            {
                if (activePlayer)
                    Instantiate(redPiece, new Vector2(2, pieceheight), Quaternion.identity);
                else
                    Instantiate(yellowPiece, new Vector2(2, pieceheight), Quaternion.identity);

            break;
            }
        case 6:
            {
                if (activePlayer)
                    Instantiate(redPiece, new Vector2(3, pieceheight), Quaternion.identity);
                else
                    Instantiate(yellowPiece, new Vector2(3, pieceheight), Quaternion.identity);

            break;
            }
        }
        nextPlayer();
        if (checkWin(x, Row))
        {
            Debug.Log("!!!!!!!!!!!!!!!A win condition has occured!");

            if (activePlayer == true)
            {
                //Coroutine coroutine = StartCoroutine(Wait_5_sec());
                SceneManager.LoadScene("Red Wins");
            }
            if (activePlayer == false)
            {
                //Coroutine coroutine = StartCoroutine(Wait_5_sec());
                SceneManager.LoadScene("Yellow Wins");
            }
        }
        if(checkTie())
        {
            //beni ples
        }
    }

    /*FUNCTIONS AND VARIABLES FOR BCI CONNECTION*/
    public static string ips = "127.0.0.1";
    public static int port = 1000;
    public static IPAddress ip = IPAddress.Parse(ips);

    public static bool shouldPieceBeSpawned;
    public static int spawnRow;

    private void OnItemReceived(object sender, EventArgs args)
    {
        ItemReceivedEventArgs eventArgs = (ItemReceivedEventArgs)args;
        Debug.Log(String.Format("Received BoardItem:\tName: {0}\tOutput Text: {1}", eventArgs.BoardItem.Name, eventArgs.BoardItem.OutputText));

        if (eventArgs.BoardItem.OutputText == "Alpha1")
        {
            Debug.Log("A piece was spawned on the first row");
            shouldPieceBeSpawned = true;
            spawnRow = 1;
        }
        if (eventArgs.BoardItem.OutputText == "Alpha2")
        {
            Debug.Log("A piece was spawned on the second row");
            shouldPieceBeSpawned = true;
            spawnRow = 2;
        }
        if (eventArgs.BoardItem.OutputText == "Alpha3")
        {
            Debug.Log("A piece was spawned on the third row");
            shouldPieceBeSpawned = true;
            spawnRow = 3;
        }
        if (eventArgs.BoardItem.OutputText == "Alpha4")
        {
            Debug.Log("A piece was spawned on the fourth row");
            shouldPieceBeSpawned = true;
            spawnRow = 4;
        }
        if (eventArgs.BoardItem.OutputText == "Alpha5")
        {
            Debug.Log("A piece was spawned on the fifth row");
            shouldPieceBeSpawned = true;
            spawnRow = 5;
        }
        if (eventArgs.BoardItem.OutputText == "Alpha6")
        {
            Debug.Log("A piece was spawned on the sixth row");
            shouldPieceBeSpawned = true;
            spawnRow = 6;
        }
        if (eventArgs.BoardItem.OutputText == "Alpha7")
        {
            Debug.Log("A piece was spawned on the seventh row");
            shouldPieceBeSpawned = true;
            spawnRow = 7;
        }
    }
    void connection(IPAddress ip, int port)
    {
        // Connection with Unicorn BCI
        try
        {
            //Start listening for Unicorn Speller network messages
            SpellerReceiver r = new SpellerReceiver(ip, port);

            //attach items received event
            r.OnItemReceived += OnItemReceived;

            Debug.Log(String.Format("Listening to {0} on port {1}.", ip, port));
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }
    public void bciListener(int spawnRow, bool activePlayer)
    {
        if (shouldPieceBeSpawned)
            spawnPiece(spawnRow, activePlayer);
        shouldPieceBeSpawned = false;
    }



    /*GAME LOOP FUNCTIONS*/
    // Start is called before the first frame update
    void Start()
    {
        activePlayer = false;

        //int[,] gameMatrix = new int[6,7];
        for (int x = 0; x < 6; ++x)
            for (int y = 0; y < 7; ++y)
            {
                gameMatrix[x, y] = (int)Piece.Empty;
            }
        showGameMatrix();

        connection(ip,port);
    }

    // Update is called once per frame
    void Update()
    {
        bciListener(spawnRow, activePlayer);


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Debug.Log("A piece was spawned on the first row");
            spawnPiece(1,activePlayer);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Debug.Log("A piece was spawned on the second row");
            spawnPiece(2,activePlayer);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Debug.Log("A piece was spawned on the third row");
            spawnPiece(3,activePlayer);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Debug.Log("A piece was spawned on the fourth row");
            spawnPiece(4,activePlayer);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Debug.Log("A piece was spawned on the fifth row");
            spawnPiece(5,activePlayer);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Debug.Log("A piece was spawned on the sixth row");
            spawnPiece(6,activePlayer);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Debug.Log("A piece was spawned on the seventh row");
            spawnPiece(7,activePlayer);
        }
    }
}
