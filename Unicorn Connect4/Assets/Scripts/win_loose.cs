using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace gameLogic
{
public class win_loose : MonoBehaviour
{
    private int[,] gameMatrix = new int[6,7];
    private enum Piece
    {
        Empty = 0,
        Yellow = 1,
        Red = 2
    }
    private bool activeColour; //0-yellow; 1-red

    private void nextPlayer()
    {
        if(!activeColour) activeColour = true;
            else activeColour = false;
    }

    private void showGameMatrix()
    {
        Debug.Log(gameMatrix[0,0] + " " + (int)gameMatrix[0,1] + " " + (int)gameMatrix[0,2] + " " + (int)gameMatrix[0,3] + " " + (int)gameMatrix[0,4] + " " + (int)gameMatrix[0,5] + " " + (int)gameMatrix[0,6] + "\n" +
                gameMatrix[1,0] + " " + (int)gameMatrix[1,1] + " " + (int)gameMatrix[1,2] + " " + (int)gameMatrix[1,3] + " " + (int)gameMatrix[1,4] + " " + (int)gameMatrix[1,5] + " " + (int)gameMatrix[1,6]  + "\n" +
                gameMatrix[2,0] + " " + (int)gameMatrix[2,1] + " " + (int)gameMatrix[2,2] + " " + (int)gameMatrix[2,3] + " " + (int)gameMatrix[2,4] + " " + (int)gameMatrix[2,5] + " " + (int)gameMatrix[2,6]  + "\n" +
                gameMatrix[3,0] + " " + (int)gameMatrix[3,1] + " " + (int)gameMatrix[3,2] + " " + (int)gameMatrix[3,3] + " " + (int)gameMatrix[3,4] + " " + (int)gameMatrix[3,5] + " " + (int)gameMatrix[3,6]  + "\n" +
                gameMatrix[4,0] + " " + (int)gameMatrix[4,1] + " " + (int)gameMatrix[4,2] + " " + (int)gameMatrix[4,3] + " " + (int)gameMatrix[4,4] + " " + (int)gameMatrix[4,5] + " " + (int)gameMatrix[4,6]  + "\n" +
                gameMatrix[5,0] + " " + (int)gameMatrix[5,1] + " " + (int)gameMatrix[5,2] + " " + (int)gameMatrix[5,3] + " " + (int)gameMatrix[5,4] + " " + (int)gameMatrix[5,5] + " " + (int)gameMatrix[5,6]);
    }

    // Start is called before the first frame update
    void Start()
    {
        activeColour = false; 

        //int[,] gameMatrix = new int[6,7];
        for(int x=0;x<6;++x)
            for(int y=0;y<7;++y)
            {
                gameMatrix[x,y] = (int)Piece.Empty;
            }
        showGameMatrix();
    }

    private bool checkWinVertical(int x, int y, int searchedColour)
    {
        int pieceCount = 0;
        //Upwards
        for(int i=x+1;i<6;++i) 
        {
            if(gameMatrix[i,y] == searchedColour) ++pieceCount;
                else break;
            //Debug.Log(String.Format("{0} is compared with{1}", gameMatrix[i,y], searchedColour));
        }
        //Downwards
        for(int i=x;i>=0;--i)
        {
            if(gameMatrix[i,y] == searchedColour) ++pieceCount;
                else break;
            //Debug.Log(String.Format("{0} is compared with{1}", gameMatrix[i,y], searchedColour));
        }
        if(pieceCount>3) return true;
        // Debug.Log(String.Format("The number of pieces connected to the one you placed is: {0}", pieceCount));

        return false;
    }
    private bool checkWinHorizontal(int x, int y, int searchedColour)
    {
        int pieceCount =-1;
        //Right
        for(int i=y;i<7;++i)
        {
            if(gameMatrix[x,i] == searchedColour) ++pieceCount;
                else break;
            // Debug.Log(String.Format("{0} is compared with{1}", gameMatrix[x,i], searchedColour));
        }
        //Left
        for(int i=y;i>=0;--i)
        {
            if(gameMatrix[x,i] == searchedColour) ++pieceCount;
                else break;
            // Debug.Log(String.Format("{0} is compared with{1}", gameMatrix[x,i], searchedColour));
        }
        if(pieceCount>3) return true;
        // Debug.Log(String.Format("The number of pieces connected to the one you placed is: {0}", pieceCount));

        return false;
    }
    private bool checkWinMainDiag(int x, int y, int searchedColour)
    {
        int pieceCount = -1;
        int j;
        j=y;
        //Upwards and to the left (diag)
        for(int i=x;i>=0;i--)
        {
            if (j<0) break;
            if(gameMatrix[i,j] == searchedColour) ++pieceCount;
                else break;
            --j;
        }
        j=y;
        //Downwards and to the right (diag)
        for(int i=x;i<6;++i)
        {
            if (j>6) break;
            if(gameMatrix[i,j] == searchedColour) ++pieceCount;
                else break;
            ++j;
        }
        if(pieceCount>3) return true;
        // Debug.Log(String.Format("The number of pieces connected to the one you placed is: {0}", pieceCount));

        return false;
    }
    private bool checkWinSecDiag(int x, int y, int searchedColour)
    {
        //Upwards and to the right (diag)
        int pieceCount = -1;
        int j;
        j=y;
        for(int i=x;i>=0;--i)
        {
            if (j>6) break;
            if(gameMatrix[i,j] == searchedColour) ++pieceCount;
                else break;
            ++j;
        }
        j=y;
        for(int i=x;i<6;++i)
        {
            if (j<0) break;
            if(gameMatrix[i,j] == searchedColour) ++pieceCount;
                else break;
            --j;
        }
        if(pieceCount>3) return true;
        // Debug.Log(String.Format("The number of pieces connected to the one you placed is: {0}", pieceCount));

        return false;
    }

    private bool checkWin(int x, int y)
    // Function checks 4 axes (2 diagonals, vertical and horizontal) for 4 pieces
    // If it finds 4 pieces of the same colour it returns true so the game may end
    {
        int searchedColour;
        if(!activeColour) searchedColour = 2; else searchedColour = 1;
        Debug.Log(String.Format("Looking for piece of the colour {0} (1=yellow,2=red)", searchedColour));

        if (checkWinVertical(x,y,searchedColour)) return true;
        if (checkWinHorizontal(x,y,searchedColour)) return true;
        if (checkWinMainDiag(x,y,searchedColour)) return true;
        if (checkWinSecDiag(x,y,searchedColour)) return true;

        return false;
    }

    public void matrixPiece(int Row, bool activeColour)
    {
        int x=5;
        --Row; // so that function can be called with normal 1-7 numbering instead of 0-6
        if(gameMatrix[x,Row]==0) 
            if(activeColour) gameMatrix[5,Row]=(int)Piece.Red; else gameMatrix[5,Row]=(int)Piece.Yellow;
        else for(x=0;x<5;++x)
        {
            if(gameMatrix[x+1,Row]!=(int)Piece.Empty) 
            {
                if(activeColour) gameMatrix[x,Row]=(int)Piece.Red; else gameMatrix[x,Row]=(int)Piece.Yellow;
                break;
            }
        }
        showGameMatrix();
        nextPlayer();
        if(checkWin(x,Row)) Debug.Log("!!!!!!!!!!!!!!!A win condition has occured!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Debug.Log("A piece was spawned on the first row");
            matrixPiece(1,activeColour);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Debug.Log("A piece was spawned on the second row");
            matrixPiece(2,activeColour);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Debug.Log("A piece was spawned on the third row");
            matrixPiece(3,activeColour);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Debug.Log("A piece was spawned on the fourth row");
            matrixPiece(4,activeColour);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Debug.Log("A piece was spawned on the fifth row");
            matrixPiece(5,activeColour);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Debug.Log("A piece was spawned on the sixth row");
            matrixPiece(6,activeColour);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Debug.Log("A piece was spawned on the seventh row");
            matrixPiece(7,activeColour);
        }
    }
}
}
