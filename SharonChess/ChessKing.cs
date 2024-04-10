using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SharonChess
{
    public class ChessKing : ChessPiece
    {

        public ChessKing(Brush color, short? row = null, char? column = null) : base(color, row, column) {}
        
        public override List<ChessSquare> GenPath(List<ChessSquare> board)
        {
            List<ChessSquare> path = new List<ChessSquare>();

            foreach (ChessSquare square in board)
            {
                bool notThisSquare = (square.Row != Row) || (square.Column != Column);
                bool notSameColor = (square.Piece == null) || (Color != square.CurrentPiece.Color);
                bool correctSquares = (Math.Abs((int)square.Row - (int)Row) <= 1) && (Math.Abs((int)square.Column - (int)Column) <= 1);

                if ((notThisSquare) && (notSameColor) && (correctSquares))
                {
                    path.Add(square);
                }
            }

            return path;
        } 
    }
}
