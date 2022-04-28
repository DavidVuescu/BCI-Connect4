using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using Unity.ItemRecever;
using System;

public class spawnPiece : MonoBehaviour
{
    public GameObject redPiece;
    public GameObject yellowPiece;

    private bool activeColour;

    private int pieceheight = 4;

    // Start is called before the first frame update
    void Start()
    {
        activeColour = false;

        // Connection with Unicorn BCI
        try
        {
            //IP settings
            string ips = "127.0.0.1";
            int port = 1000;
            IPAddress ip = IPAddress.Parse(ips);

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

    private void nextPlayer()
    {
        if(!activeColour) activeColour = true;
            else activeColour = false;
    }

    private void Spawn(uint row)
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

        //Do something...
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("A piece was spawned on the first row");
            Spawn(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("A piece was spawned on the second row");
            Spawn(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("A piece was spawned on the third row");
            Spawn(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("A piece was spawned on the fourth row");
            Spawn(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("A piece was spawned on the fifth row");
            Spawn(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("A piece was spawned on the sixth row");
            Spawn(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log("A piece was spawned on the seventh row");
            Spawn(7);
        }
    }
}
