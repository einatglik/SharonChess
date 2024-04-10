using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SharonChess
{
    public class ChessRook : ChessPiece
    {
        public ChessRook(Brush color, short? row = null, char? column = null) : base(color, row, column) { }

        public override List<ChessSquare> GenPath(List<ChessSquare> board)
        {
            List<ChessSquare> path = new List<ChessSquare>();
            List<ChessSquare> forwardPath = new List<ChessSquare>();
            List<ChessSquare> backwardPath = new List<ChessSquare>();
            List<ChessSquare> rightPath = new List<ChessSquare>();
            List<ChessSquare> leftPath = new List<ChessSquare>();

            foreach (ChessSquare square in board)
            {
                bool notSameSquare = $"{Column}{Row}" != $"{square.Column}{square.Row}";
                bool inPath = (Row == square.Row) || (Column == square.Column);

                if ((notSameSquare) && (inPath))
                {                    
                    if (square.Row > Row)
                    {
                        forwardPath.Add(square);
                    }

                    else if (square.Row < Row)
                    {
                        backwardPath.Add(square);
                    }

                    else if (square.Column > Column)
                    {
                        rightPath.Add(square);
                    }

                    else if (square.Column < Column)
                    {
                        leftPath.Add(square);
                    }
                }
            }

            forwardPath = forwardPath.OrderBy(square => (int)square.Row).ToList();
            backwardPath = backwardPath.OrderByDescending(square => (int)square.Row).ToList();
            rightPath = rightPath.OrderBy(square => (int)square.Column).ToList();
            leftPath = leftPath.OrderByDescending(square => (int)square.Column).ToList();

            foreach (ChessSquare square in forwardPath)
            {
                if (square.Piece == null)
                {
                    path.Add(square);
                }

                else if (square.CurrentPiece.Color == Color)
                {
                    break;
                }

                else
                {
                    path.Add(square);
                    break;
                }
            }

            foreach (ChessSquare square in backwardPath)
            {
                if (square.Piece == null)
                {
                    path.Add(square);
                }

                else if (square.CurrentPiece.Color == Color)
                {
                    break;
                }

                else
                {
                    path.Add(square);
                    break;
                }
            }

            foreach (ChessSquare square in rightPath)
            {
                if (square.Piece == null)
                {
                    path.Add(square);
                }

                else if (square.CurrentPiece.Color == Color)
                {
                    break;
                }

                else
                {
                    path.Add(square);
                    break;
                }
            }

            foreach (ChessSquare square in leftPath)
            {
                if (square.Piece == null)
                {
                    path.Add(square);
                }

                else if (square.CurrentPiece.Color == Color)
                {
                    break;
                }

                else
                {
                    path.Add(square);
                    break;
                }
            }

            return path;
        }
    }
}
