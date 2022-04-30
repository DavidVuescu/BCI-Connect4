using System;
using System.IO;
using System.Net;
using UnityEngine;
using Unity.ItemRecever;
using Intendix.Board;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;


namespace connect4
{
public class spawnPiece : MonoBehaviour
{
    public GameObject redPiece;
    public GameObject yellowPiece;

    public static string ips = "127.0.0.1";
    public static int port = 1000;
    public static IPAddress ip = IPAddress.Parse(ips);

    //BoardItem receivedItem = ReceiveItem(ip, port);

    private bool activeColour;

    private int pieceheight = 4;


    bool shouldPieceBeSpawned;
    int spawnRow;



    // Start is called before the first frame update
    void Start()
    {

        activeColour = false;
        shouldPieceBeSpawned = false;

        connection(ip,port);
        // if (receivedItem != null)
        //     Console.WriteLine("Item '{0}' received.", receivedItem.Name);

    }

    private void nextPlayer()
    {
        if(!activeColour) activeColour = true;
            else activeColour = false;
    }
    

    private void spawnListener(int spawnRow)
    {
        if(shouldPieceBeSpawned) Spawn(spawnRow);
        shouldPieceBeSpawned = false;
    }

    private void Spawn(int row)
    {
        switch (row)
        {
            case 1:  
            {
                if (activeColour)
                {
                    Instantiate(redPiece, new Vector2(-3, pieceheight), Quaternion.identity);
                    nextPlayer();
                }
                else
                {
                    Instantiate(yellowPiece, new Vector2(-3, pieceheight), Quaternion.identity);
                    nextPlayer();
                }

                break; 
            }
            case 2:
            {
                if (activeColour)
                {
                    Instantiate(redPiece, new Vector2(-2, pieceheight), Quaternion.identity);
                    nextPlayer();
                }
                else
                {
                    Instantiate(yellowPiece, new Vector2(-2, pieceheight), Quaternion.identity);
                    nextPlayer();
                }

                break;
            }
            case 3:
            {
                if (activeColour)
                {
                    Instantiate(redPiece, new Vector2(-1, pieceheight), Quaternion.identity);
                    nextPlayer();
                }
                else
                {
                    Instantiate(yellowPiece, new Vector2(-1, pieceheight), Quaternion.identity);
                    nextPlayer();
                }

                break;
            }
            case 4:
            {
                if (activeColour)
                {
                    Instantiate(redPiece, new Vector2(0, pieceheight), Quaternion.identity);
                    nextPlayer();
                }
                else
                {
                    Instantiate(yellowPiece, new Vector2(0, pieceheight), Quaternion.identity);
                    nextPlayer();
                }

                break;
            }
            case 5:
            {
                if (activeColour)
                {
                    Instantiate(redPiece, new Vector2(1, pieceheight), Quaternion.identity);
                    nextPlayer();
                }
                else
                {
                    Instantiate(yellowPiece, new Vector2(1, pieceheight), Quaternion.identity);
                    nextPlayer();
                }

                break;
            }
            case 6:
            {
                if (activeColour)
                {
                    Instantiate(redPiece, new Vector2(2, pieceheight), Quaternion.identity);
                    nextPlayer();
                }
                else
                {
                    Instantiate(yellowPiece, new Vector2(2, pieceheight), Quaternion.identity);
                    nextPlayer();
                }

                break;
            }
            case 7:
            {
                if (activeColour)
                {
                    Instantiate(redPiece, new Vector2(3, pieceheight), Quaternion.identity);
                    nextPlayer();
                }
                else
                {
                    Instantiate(yellowPiece, new Vector2(3, pieceheight), Quaternion.identity);
                    nextPlayer();
                }

                break;
            }
        }
    }

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
        if(eventArgs.BoardItem.OutputText == "Alpha7")
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
            //IP settings
            

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

  

// Update is called once per frame
void Update()
    {
        spawnListener(spawnRow);
    }
}
}
