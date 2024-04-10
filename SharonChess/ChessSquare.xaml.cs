using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SharonChess
{
    /// <summary>
    /// Interaction logic for ChessSquare.xaml
    /// </summary>
    public partial class ChessSquare : UserControl
    {
        // These are the properties of the chess square. the properties represent the position of the square (row, Column), its color and the current piece on it.
        public short? Row { get; private set; }
        public char? Column { get; private set; }
        public Brush MainColor { get; private set; }
        public ChessPiece CurrentPiece { get; set; }

        // This is a dependency property that holds the source for the image of the piece that's on the square. 
        public static readonly DependencyProperty PieceProperty = DependencyProperty.Register("Piece", typeof(ImageSource), typeof(ChessSquare));

        public ImageSource Piece
        {
            get { return (ImageSource)GetValue(PieceProperty); }
            set { SetValue(PieceProperty, value); }
        }

        // This is a dependency property that holds the current color of the square.
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(ChessSquare), new PropertyMetadata(Brushes.NavajoWhite));

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        // An event handler that gets invoked by the method ChessButtonClicked.
        public event EventHandler ChessSquareClick;

        // This is the constructor of the square. It receives its color, row and column and sets their default value to null. 
        // If no color is provided, the dependency property ColorProperty sets the color of the square to the color NavajoWhite by default.
        // The constructor sets the default width and height of the square to be 40 and it subscribes the method ChessButtonClicked to the propety PreviewMouseDown of ChessButton.    
        public ChessSquare(Brush color = null, short? row = null, char? column = null)
        {
            InitializeComponent();
            ChessButton.PreviewMouseDown += ChessButtonClicked;
            Width = 40;
            Height = 40;

            if (color != null)
            {
                Color = color;
                MainColor = color;
            }
            else
            {
                MainColor = Color;
            }

            // These are conditons that check if the row and column provided are valid, and if not, a matching argument exception is thrown.
            // The row and column should either both not be provided or both be provided.
            // If both are provided, the row should be a number between 1 - 8 and the column should be a character between a - h.
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

        // This is a method that gets subscribed to the property PreviewMouseDown of ChessButton in the constructor.
        // The method checks if the pressed button on the mouse was the left button and if it is, it invokes the ChessSquareClick event handler.
        private void ChessButtonClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ChessSquareClick?.Invoke(this, EventArgs.Empty);
            }           
        }

        // This is a method that marks the square.
        // If there is no piece on the square, the mark is a circle in the middle of the square.
        // If there is a piece on the square, the mark is an empty circle with a stroke that surrounds the piece.
        public void MarkSquare()
        {
            if (Piece == null)
            {
                noPieceMark.Fill = Brushes.Gray;
            }
            else
            {
                pieceMark.Stroke = Brushes.Gray;
            }
        }

        // This is a method that unmarks the square.
        public void UnmarkSquare()
        {            
            noPieceMark.Fill = Brushes.Transparent;           
            pieceMark.Stroke = Brushes.Transparent;
        }
    }
}
