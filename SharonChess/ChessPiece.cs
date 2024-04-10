using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SharonChess
{
    public abstract class ChessPiece
    {
        public short? Row { get; set; }
        public char? Column { get; set; }
        public Brush Color { get; private set; }

        public ChessPiece(Brush color, short? row = null, char? column = null)
        {
            Color = color;

            if ((row != null) && (column != null) && (row >= 1) && (row <= 8) && (column >= 'a') && (column <= 'h'))
            {
                Row = row;
                Column = column;
            }
            else if ((row == null) && (column != null))
            {
                throw new ArgumentException("Both row and column need to be provided, not only the column.");
            }
            else if ((column == null) && (row != null))
            {
                throw new ArgumentException("Both row and column need to be provided, not only the row.");
            }
            else if ((row != null) && ((row < 1) || (row > 8)))
            {
                throw new ArgumentException($"The row needs to be in the range 1 - 8 ({row} is not in this range).");
            }
            else if ((column != null) && ((column < 'a') || (column > 'h')))
            {
                throw new ArgumentException($"The column needs to be in the range a - h ({column} is not in this range).");
            }
        }

        public abstract List<ChessSquare> GenPath(List<ChessSquare> board);
    }
}
