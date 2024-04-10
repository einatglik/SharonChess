using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SharonChess
{
    public class ChessQueen : ChessPiece
    {
        public ChessQueen(Brush color, short? row = null, char? column = null) : base(color, row, column) { }

        public override List<ChessSquare> GenPath(List<ChessSquare> board)
        {
            List<ChessSquare> path = new List<ChessSquare>();
            List<ChessSquare> forwardPath = new List<ChessSquare>();
            List<ChessSquare> backwardPath = new List<ChessSquare>();
            List<ChessSquare> rightPath = new List<ChessSquare>();
            List<ChessSquare> leftPath = new List<ChessSquare>();
            List<ChessSquare> rightUpDiagonal = new List<ChessSquare>();
            List<ChessSquare> rightDownDiagonal = new List<ChessSquare>();
            List<ChessSquare> leftDownDiagonal = new List<ChessSquare>();
            List<ChessSquare> leftUpDiagonal = new List<ChessSquare>();

            foreach (ChessSquare square in board)
            {
                bool notSameSquare = $"{Column}{Row}" != $"{square.Column}{square.Row}";
                bool inDiagonal = Math.Abs((int)square.Row - (int)Row) == Math.Abs((int)square.Column - (int)Column);
                bool inPath = (Row == square.Row) || (Column == square.Column);

                if ((notSameSquare) && ((inDiagonal) || (inPath)))
                {
                    if ((square.Row > Row) && (square.Column > Column))
                    {
                        rightUpDiagonal.Add(square);
                    }

                    else if ((square.Row < Row) && (square.Column > Column))
                    {
                        rightDownDiagonal.Add(square);
                    }

                    else if ((square.Row < Row) && (square.Column < Column))
                    {
                        leftDownDiagonal.Add(square);
                    }

                    else if ((square.Row > Row) && (square.Column < Column))
                    {
                        leftUpDiagonal.Add(square);
                    }

                    else if ((square.Row > Row) && (square.Column == Column))
                    {
                        forwardPath.Add(square);
                    }

                    else if ((square.Row < Row) && (square.Column == Column))
                    {
                        backwardPath.Add(square);
                    }

                    else if ((square.Column > Column) && (square.Row == Row))
                    {
                        rightPath.Add(square);
                    }

                    else if (square.Column < Column && (square.Row == Row))
                    {
                        leftPath.Add(square);
                    }
                }
            }

            rightUpDiagonal = rightUpDiagonal.OrderBy(square => (int)square.Row).ToList();
            rightDownDiagonal = rightDownDiagonal.OrderByDescending(square => (int)square.Row).ToList();
            leftDownDiagonal = leftDownDiagonal.OrderByDescending(square => (int)square.Row).ToList();
            leftUpDiagonal = leftUpDiagonal.OrderBy(square => (int)square.Row).ToList();
            forwardPath = forwardPath.OrderBy(square => (int)square.Row).ToList();
            backwardPath = backwardPath.OrderByDescending(square => (int)square.Row).ToList();
            rightPath = rightPath.OrderBy(square => (int)square.Column).ToList();
            leftPath = leftPath.OrderByDescending(square => (int)square.Column).ToList();

            foreach (ChessSquare square in rightUpDiagonal)
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

            foreach (ChessSquare square in rightDownDiagonal)
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

            foreach (ChessSquare square in leftDownDiagonal)
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

            foreach (ChessSquare square in leftUpDiagonal)
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
