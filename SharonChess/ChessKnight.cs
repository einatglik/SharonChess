using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SharonChess
{
    public class ChessKnight : ChessPiece
    {

        public ChessKnight(Brush color, short? row = null, char? column = null) : base(color, row, column) { }

        public override List<ChessSquare> GenPath(List<ChessSquare> board)
        {
            List<ChessSquare> path = new List<ChessSquare>();

            foreach (ChessSquare square in board)
            {
                bool horizontalLShape = (Math.Abs((int)square.Row - (int)Row) == 2) && (Math.Abs((int)square.Column - (int)Column) == 1);
                bool verticalLShape = (Math.Abs((int)square.Row - (int)Row) == 1) && (Math.Abs((int)square.Column - (int)Column) == 2);

                if (((horizontalLShape) || (verticalLShape)) && ((square.Piece == null) || (Color != square.CurrentPiece.Color)))
                {
                    path.Add(square);
                }
            }

            return path;
        }
    }
}
