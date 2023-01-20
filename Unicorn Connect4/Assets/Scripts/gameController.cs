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

using gameSettings;
using WinChecker;
using trueAI;


namespace GameControls
{
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

        public static bool shouldPieceBeSpawned;
        public static bool shouldAIplay;
        public static int spawnRow;
        public static int randomRow;



        public string startStop;
        public int secondFlashStart;
        public int secondFlashStop;


        private void nextPlayer() // Function switches boolean responsible for player colour
        {
            if (!activePlayer) activePlayer = true;
            else activePlayer = false;

            if (Settings.aiActive)
                shouldAIplay = !shouldAIplay;
        }
        private void showGameMatrix() //Debug function for showing the board matrix inside the console
        {
            Debug.Log(gameMatrix[0, 0] + " " + (int)gameMatrix[0, 1] + " " + (int)gameMatrix[0, 2] + " " + (int)gameMatrix[0, 3] + " " + (int)gameMatrix[0, 4] + " " + (int)gameMatrix[0, 5] + " " + (int)gameMatrix[0, 6] + "\n" +
                    gameMatrix[1, 0] + " " + (int)gameMatrix[1, 1] + " " + (int)gameMatrix[1, 2] + " " + (int)gameMatrix[1, 3] + " " + (int)gameMatrix[1, 4] + " " + (int)gameMatrix[1, 5] + " " + (int)gameMatrix[1, 6] + "\n" +
                    gameMatrix[2, 0] + " " + (int)gameMatrix[2, 1] + " " + (int)gameMatrix[2, 2] + " " + (int)gameMatrix[2, 3] + " " + (int)gameMatrix[2, 4] + " " + (int)gameMatrix[2, 5] + " " + (int)gameMatrix[2, 6] + "\n" +
                    gameMatrix[3, 0] + " " + (int)gameMatrix[3, 1] + " " + (int)gameMatrix[3, 2] + " " + (int)gameMatrix[3, 3] + " " + (int)gameMatrix[3, 4] + " " + (int)gameMatrix[3, 5] + " " + (int)gameMatrix[3, 6] + "\n" +
                    gameMatrix[4, 0] + " " + (int)gameMatrix[4, 1] + " " + (int)gameMatrix[4, 2] + " " + (int)gameMatrix[4, 3] + " " + (int)gameMatrix[4, 4] + " " + (int)gameMatrix[4, 5] + " " + (int)gameMatrix[4, 6] + "\n" +
                    gameMatrix[5, 0] + " " + (int)gameMatrix[5, 1] + " " + (int)gameMatrix[5, 2] + " " + (int)gameMatrix[5, 3] + " " + (int)gameMatrix[5, 4] + " " + (int)gameMatrix[5, 5] + " " + (int)gameMatrix[5, 6]);
        }

        private static bool checkWin(int x, int y) // Function for checking if a win condition has occured
        {
            int searchedColour; // The colour of the current piece the win algo is looking for
            if (!activePlayer) searchedColour = 2; else searchedColour = 1;
            Debug.Log(String.Format("Looking for piece of the colour {0} (1=yellow,2=red)", searchedColour));

            if (winChecker.Vertical(x, y, searchedColour, gameMatrix)) return true;
            if (winChecker.Horizontal(x, y, searchedColour, gameMatrix)) return true;
            if (winChecker.MainDiag(x, y, searchedColour, gameMatrix)) return true;
            if (winChecker.SecDiag(x, y, searchedColour, gameMatrix)) return true;

            return false;
        }
        private static bool checkTie() // Function for checking if all columns are full
        {
            for (int i = 0; i < 6; ++i)
                if (gameMatrix[0, i] != 0)
                    return true;
            return false;
        }

        public void spawnPiece(int Row, bool activePlayer) // Function for placing pieces in game logic and on screen
        // Function instantiatiates a piece on screen and adds it in the game matrix
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
            if (checkTie())
            {
                int k = 0;
                //beni ples
                for (int i = 0; i < 7; i++)
                {
                    if (gameMatrix[0, i] != 0)
                    {
                        k++;
                        Debug.Log(String.Format("Current filled top row spots: {0}", k));
                    }

                }
                if (k > 6)
                    SceneManager.LoadScene("Tie");
            }
        }

        /*-------BCI CONNECTION-------------------------------------------------------------------------------*/
        /*FUNCTIONS AND VARIABLES FOR BCI CONNECTION*/
        public static string ips = "127.0.0.1";
        public static int port = 1000;
        public static IPAddress ip = IPAddress.Parse(ips);
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
        private void OnItemReceived(object sender, EventArgs args)
        {
            ItemReceivedEventArgs eventArgs = (ItemReceivedEventArgs)args;
            Debug.Log(String.Format("Received BoardItem:\tName: {0}\tOutput Text: {1}", eventArgs.BoardItem.Name, eventArgs.BoardItem.OutputText));

            if (eventArgs.BoardItem.OutputText == "Alpha1")
            {
                Debug.Log("A piece was spawned on the first row by BCI");
                shouldPieceBeSpawned = true;
                spawnRow = 1;
            }
            if (eventArgs.BoardItem.OutputText == "Alpha2")
            {
                Debug.Log("A piece was spawned on the second row by BCI");
                shouldPieceBeSpawned = true;
                spawnRow = 2;
            }
            if (eventArgs.BoardItem.OutputText == "Alpha3")
            {
                Debug.Log("A piece was spawned on the third row by BCI");
                shouldPieceBeSpawned = true;
                spawnRow = 3;
            }
            if (eventArgs.BoardItem.OutputText == "Alpha4")
            {
                Debug.Log("A piece was spawned on the fourth row by BCI");
                shouldPieceBeSpawned = true;
                spawnRow = 4;
            }
            if (eventArgs.BoardItem.OutputText == "Alpha5")
            {
                Debug.Log("A piece was spawned on the fifth row by BCI");
                shouldPieceBeSpawned = true;
                spawnRow = 5;
            }
            if (eventArgs.BoardItem.OutputText == "Alpha6")
            {
                Debug.Log("A piece was spawned on the sixth row by BCI");
                shouldPieceBeSpawned = true;
                spawnRow = 6;
            }
            if (eventArgs.BoardItem.OutputText == "Alpha7")
            {
                Debug.Log("A piece was spawned on the seventh row by BCI");
                shouldPieceBeSpawned = true;
                spawnRow = 7;
            }


            //Start/restart 
            if (eventArgs.BoardItem.OutputText == "Alpha9")
            {
                Debug.Log("Start/restart");
                startStop = "Alpha9";
                secondFlashStart++;



            }
            //Quit
            if (eventArgs.BoardItem.OutputText == "Alpha0")
            {
                Debug.Log("Quit");
                startStop = "Alpha0";
                secondFlashStop++;


            }
        }
        public void bciListener(int spawnRow, bool activePlayer)
        {
            if (shouldPieceBeSpawned)
                spawnPiece(spawnRow, activePlayer);

            shouldPieceBeSpawned = false;
        }
        /*--^^^--BCI CONNECTION--^^^--------------------------------------------------------------------------*/


        /*GAME LOOP FUNCTIONS*/
        // Start is called before the first frame update
        void Start()
        {
            activePlayer = false;
            shouldAIplay = false;
            spawnRow = 1;

            //int[,] gameMatrix = new int[6,7];
            for (int x = 0; x < 6; ++x)
                for (int y = 0; y < 7; ++y)
                    gameMatrix[x, y] = (int)Piece.Empty;
            showGameMatrix();

            if(Settings.bciConnected) connection(ip, port);
        }

        // Update is called once per frame
        void Update()
        {
            bciListener(spawnRow, activePlayer);

            // Keyboard Inputs
            if (Input.GetKeyDown(KeyCode.Alpha1)) { spawnPiece(1, activePlayer); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { spawnPiece(2, activePlayer); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { spawnPiece(3, activePlayer); }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { spawnPiece(4, activePlayer); }
            if (Input.GetKeyDown(KeyCode.Alpha5)) { spawnPiece(5, activePlayer); }
            if (Input.GetKeyDown(KeyCode.Alpha6)) { spawnPiece(6, activePlayer); }
            if (Input.GetKeyDown(KeyCode.Alpha7)) { spawnPiece(7, activePlayer); }
            if (startStop == "Alpha9" && secondFlashStart == 2) { SceneManager.LoadScene("Game"); secondFlashStart = 0; }
            if (startStop == "Alpha0" && secondFlashStop == 2) { Application.Quit(); secondFlashStop = 0; }

            if (shouldAIplay)
            {
                pieceheight = 8;
                randomRow = UnityEngine.Random.Range(0, 7);
                if (gameMatrix[0, spawnRow] != 0 && tieBreak < 7)
                {
                    ++tieBreak;
                    randomRow = UnityEngine.Random.Range(0, 7);
                }
                spawnPiece(randomRow, !activePlayer);
                pieceheight = 4;
            }
        }
    }
}
