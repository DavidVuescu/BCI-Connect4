using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main win condition check function, checks adjecent tiles for pieces of the same colour as the current one
// If they finds 4 adjecent pieces it returns a true value used for displaying win screens
namespace WinChecker
{
    public class winChecker : MonoBehaviour
    {
        public static bool Vertical(int x, int y, int searchedColour, int[,] gameMatrix)
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
        public static bool Horizontal(int x, int y, int searchedColour, int[,] gameMatrix)
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
        public static bool MainDiag(int x, int y, int searchedColour, int[,] gameMatrix)
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
        public static bool SecDiag(int x, int y, int searchedColour, int[,] gameMatrix)
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
        /*-----WIN CONDITIONS-------------------------------------------------------------------------------------------*/
    }
}
