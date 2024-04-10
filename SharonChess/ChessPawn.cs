using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SharonChess
{
    public class ChessPawn : ChessPiece
    {

        public ChessPawn(Brush color, short? row = null, char? column = null) : base(color, row, column) { }

        public override List<ChessSquare> GenPath(List<ChessSquare> board)
        {
            List<ChessSquare> path = new List<ChessSquare>();
            List<string> stringPath = new List<string>();
            bool isWhite = Color == Brushes.White;
            bool isNextSquareEmpty = true;

            foreach (ChessSquare square in board) {

                if ((isWhite) && (Column == square.Column) && (square.Row == Row + 1))
                {
                    if (square.Piece != null)
                    {
                        isNextSquareEmpty = false;
                        break;
                    }
                }

                else if ((!isWhite) && (Column == square.Column) && (square.Row == Row - 1))
                {
                    if (square.Piece != null)
                    {
                        isNextSquareEmpty = false;
                        break;
                    }
                }
            }
            
            if ((isWhite) && (Row == 2) && (isNextSquareEmpty))
            {
                stringPath.Add($"{Column}4");
            }

            else if ((!isWhite) && (Row == 7) && (isNextSquareEmpty))
            {
                stringPath.Add($"{Column}5");
            }

            if (isWhite)
            {
                stringPath.Add($"{Column}{Row + 1}");
                stringPath.Add($"{(char)(Column - 1)}{(char)Row + 1}");
                stringPath.Add($"{(char)(Column + 1)}{(char)Row + 1}");
            }

            else
            {
                stringPath.Add($"{Column}{Row - 1}");
                stringPath.Add($"{(char)(Column - 1)}{Row - 1}");
                stringPath.Add($"{(char)(Column + 1)}{Row - 1}");
            }

            foreach (ChessSquare square in board)
            {               
                if (stringPath.Contains($"{square.Column}{square.Row}"))
                {                   
                    if ((Column == square.Column) && (square.Piece == null))
                    {
                        path.Add(square);
                    }

                    else if ((Column != square.Column) && (square.Piece != null) && (Color != square.CurrentPiece.Color))
                    {
                        path.Add(square);
                    }
                }
            }

            return path;
        }
    }
}
