using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace trueAI
{
    public class Position : MonoBehaviour
    {
        private int[,] board = new int[WIDTH, HEIGHT];     // 0 if cell is empty, 1 for first player and 2 for second player.
        private int[] height = new int[WIDTH];              // number of stones per column
        private uint moves;                 // number of moves played since the beinning of the game.

        public const int WIDTH = 7;
        public const int HEIGHT = 6;



        public bool canPlay(int col)
        {
            return height[col] < HEIGHT;
        }
        public void play(int col)
        {
            board[col, height[col]] = 1 + (int)moves % 2;
            height[col]++;
            moves++;
        }

        public uint play(string seq)
        {
            for (int i = 0; i < seq.Length; i++)
            {
                int col = seq[i] - '1';
                if (col < 0 || col >= WIDTH || !canPlay(col) || isWinningMove(col)) return (uint)i; // invalid move
                play(col);
            }
            return (uint)seq.Length;
        }

        public bool isWinningMove(int col)
        {
            int current_player = 1 + (int)moves % 2;
            // check for vertical alignments
            if (height[col] >= 3
                    && board[col, height[col] - 1] == current_player
                    && board[col, height[col] - 2] == current_player
                    && board[col, height[col] - 3] == current_player)
                return true;

            for (int dy = -1; dy <= 1; dy++)          // Iterate on horizontal (dy = 0) or two diagonal directions (dy = -1 or dy = 1).
            {
                int nb = 0;                         // counter of the number of stones of current player surronding the played stone in tested direction.
                for (int dx = -1; dx <= 1; dx += 2)   // count continuous stones of current player on the left, then right of the played column.
                    for (int x = col + dx, y = height[col] + dx * dy; x >= 0 && x < WIDTH && y >= 0 && y < HEIGHT && board[x, y] == current_player; nb++)
                    {
                        x += dx;
                        y += dx * dy;
                    }
                if (nb >= 3) return true; // there is an aligment if at least 3 other stones of the current user 
                                          // are surronding the played stone in the tested direction.
            }
            return false;
        }

        public uint nbMoves()
        {
            return moves;
        }

        Position()
        {
            for(int i=0; i<WIDTH; i++)
                for(int j=0; j<HEIGHT; j++)
                    board[i, j] = 0;
            for (int i = 0; i < WIDTH; i++) height[i] = 0;
            moves = 0;
        }
        Position(Position P)
        {
            for (int i = 0; i < WIDTH; i++)
                for (int j = 0; j < HEIGHT; j++)
                    this.board[i, j] = P.board[i, j];
            for(int i= 0; i < WIDTH; i++) this.height[i] = P.height[i];
            this.moves = P.moves;
        }
    }


    public class Solver : MonoBehaviour
    {
        private ulong nodeCount;    // counter of explored nodes

        private int negamax(Position P)
        {
            nodeCount++; // increment counter of explored nodes

            if (P.nbMoves() == Position.WIDTH * Position.HEIGHT) // check for draw game
                return 0;

            for (int x = 0; x < Position.WIDTH; x++) // check if current player can win next move
                if (P.canPlay(x) && P.isWinningMove(x))
                    return (int)(Position.WIDTH * Position.HEIGHT + 1 - P.nbMoves()) / 2;

            int bestScore = -Position.WIDTH * Position.HEIGHT; // init the best possible score with a lower bound of score.

            for (int x = 0; x < Position.WIDTH; x++) // compute the score of all possible next move and keep the best one
                if (P.canPlay(x))
                {
                    Position P2 = P;
                    P2.play(x);               // It's opponent turn in P2 position after current player plays x column.
                    int score = -negamax(P2); // If current player plays col x, his score will be the opposite of opponent's score after playing col x
                    if (score > bestScore) bestScore = score; // keep track of best possible score so far.
                }

            return bestScore;
        }


        public int solve(Position P)
        {
            nodeCount = 0;
            return negamax(P);
        }
        public uint getNodeCount()
        {
            return (uint)nodeCount;
        }
    }
}
