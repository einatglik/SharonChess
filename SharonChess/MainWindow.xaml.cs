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
using System.Runtime.InteropServices;
using System.Threading;


using ChessServer;

namespace SharonChess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // This is a window in which the game of chess happens. In this window the game is played between 2 people on the same computer.
    public partial class MainWindow : Window
    {
        private double prevWidth;
        private double prevHeight;
        private double aspectRatio;
        private bool isLoaded;
        private string texturesResource;
        private List<ChessSquare> board;
        private ChessSquare clickedSquare;
        private ChessSquare[] prevMove;
        private Brush nextTurnColor;
        private List<ChessSquare> path;
        private Player me;
        private Player oponent;
        // private Image piece;

        // This is the constructor of the window. it sets the prevWidth and prevHeight fields to be the initial size of the window. these fields are used to resize the controls in the window properly when the window gets resized. 
        // The field isLoaded is being set to false to make sure not to resize the controls of the window before it gets loaded.
        // The field texturesResource is being set to the directory in which the textures for the pieces are stored.
        // The constructor uses the method GenBoard to show the board with the pieces on the screen and store all the squares in the list chessSquaresList.
        // The field nextTurnColor is being set to white. This field holds the color of the pieces which need to move next.
        // The field prevMove gets initialized. This field is an array with 2 members that holds the 2 squares in which the last move happened. 
       
        public MainWindow()
        {
            InitializeComponent();
            prevWidth = Width;
            prevHeight = Height;
            isLoaded = false;
            texturesResource = "pack://application:,,,/SharonChess;component/Textures/";

       
            board = GenBoard();
            nextTurnColor = Brushes.White;
            prevMove = new ChessSquare[2];
            path = new List<ChessSquare>();

           
        }

        public void SetPlayers(Player me, Player oponent)
        {
            this.me = me;
            this.oponent = oponent;

        }

        // This is a method that occurs when the window gets loaded. it sets the field isLoaded to be true so that when the window gets resized, the controls change their size and position too.
        // The method also declares the asspect ratio of the window so that when the window gets resized, it always keeps its asspect ratio.
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
            aspectRatio = Width / Height;
        }

        // This method handles the event of a chess square click. 
        // The method checks if the clicked control is a chess square and only if it is, the click should be handled in this method.
        // The method handles the click based on the piece on the square, its color and the square that has been clicked before.
        // If the clicked square has a piece of the color of the next turn, the squares in its path get marked and the square of the piece gets colored. 
        // The previously clicked square gets replaced by this one.
        // If an empty square or a square with a piece of the opposite color of the next turn color gets clicked and there is a clicked square from before, 
        // the piece on the clicked square from before moves to the current clicked square, 
        // the marked squares get unmarked, the squares in which the piece moved get colored and saved to prevMove and the value of nextTurnColor switches to the opposite color.   
        private void ChessSquareClick(object sender, EventArgs e)
        {
            if (sender is ChessSquare chessSquare)
            {
                if (chessSquare.Piece != null)
                {
                    if ((clickedSquare == null) && (chessSquare.CurrentPiece.Color == nextTurnColor))
                    {
                        chessSquare.Color = Brushes.Green;                       
                        clickedSquare = chessSquare;
                        path = chessSquare.CurrentPiece.GenPath(board);
                        MarkSquares(path);
                    }

                    else if ((clickedSquare != null) && (chessSquare == clickedSquare))
                    {
                        clickedSquare.Color = clickedSquare.MainColor;                       
                        clickedSquare = null;
                        UnMarkSquares(path);
                        path.Clear();
                    }

                    else if ((clickedSquare != null) && (chessSquare.CurrentPiece.Color == nextTurnColor))
                    {
                        clickedSquare.Color = clickedSquare.MainColor;
                        chessSquare.Color = Brushes.Green;
                        clickedSquare = chessSquare;
                        UnMarkSquares(path);
                        path = chessSquare.CurrentPiece.GenPath(board);
                        MarkSquares(path);
                        /*MovePiece(chessSquare, path);*/
                    }

                    else if ((clickedSquare != null) && (chessSquare.CurrentPiece.Color != nextTurnColor) && (path.Contains(chessSquare)))
                    {
                        nextTurnColor = chessSquare.CurrentPiece.Color;
                        UpdatePieceLocation(clickedSquare.Piece, clickedSquare.CurrentPiece, chessSquare);                    
                        MarkMove(chessSquare, clickedSquare, prevMove);
                        clickedSquare.Piece = null;
                        clickedSquare.CurrentPiece = null;
                        clickedSquare = null;
                        UnMarkSquares(path);
                        path.Clear();
                    }
                }

                else if ((clickedSquare != null) && (path.Contains(chessSquare)))
                {
                    UpdatePieceLocation(clickedSquare.Piece, clickedSquare.CurrentPiece, chessSquare);
                    MarkMove(chessSquare, clickedSquare, prevMove);
                    clickedSquare.Piece = null;
                    clickedSquare.CurrentPiece = null;
                    clickedSquare = null;
                    UnMarkSquares(path);
                    path.Clear();

                    if (nextTurnColor == Brushes.White)
                    {
                        nextTurnColor = Brushes.Black;
                    }

                    else
                    {
                        nextTurnColor = Brushes.White;
                    }
                }
            }
        }

        // This method handles a size change in the window.
        // The method waits until the window gets loaded.
        // If there is a change in the aspect ratio, the width or height of the window get changed to fit the aspect ratio.
        // The method updates the position and size of every control according to the size change in the window.
        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!isLoaded)
            {
                return;
            }

            double newWidth = e.NewSize.Width;
            double newHeight = e.NewSize.Height;

            double newAspectRatio = newWidth / newHeight;

            if (Math.Abs(newAspectRatio - aspectRatio) > double.Epsilon)
            {
                if (Math.Abs(newWidth - prevWidth) >= Math.Abs(newHeight - prevHeight))
                {
                    Height = newWidth / aspectRatio;
                    Width = newWidth;
                }
                else
                {
                    Width = newHeight * aspectRatio;
                    Height = newHeight;
                }
            }
            else
            {
                Width = newWidth;
                Height = newHeight;
            }

            foreach (var control in MainCanvas.Children)
            {
                if (control is FrameworkElement frameworkElement)
                {
                    double xCenter = Canvas.GetLeft(frameworkElement) + frameworkElement.ActualWidth / 2;
                    double yCenter = Canvas.GetTop(frameworkElement) + frameworkElement.ActualHeight / 2;
                    double xRatio = xCenter / prevWidth;
                    double yRatio = yCenter / prevHeight;
                    double widthRatio = frameworkElement.ActualWidth / prevWidth;
                    double heightRatio = frameworkElement.ActualHeight / prevHeight;
                    frameworkElement.Width = widthRatio * Width;
                    frameworkElement.Height = heightRatio * Height;
                    frameworkElement.UpdateLayout();
                    Canvas.SetLeft(frameworkElement, xRatio * Width);
                    Canvas.SetLeft(frameworkElement, Canvas.GetLeft(frameworkElement) - frameworkElement.ActualWidth / 2);
                    Canvas.SetTop(frameworkElement, yRatio * Height);
                    Canvas.SetTop(frameworkElement, Canvas.GetTop(frameworkElement) - frameworkElement.ActualHeight / 2);
                }                
            }

            prevWidth = Width;
            prevHeight = Height;
        }

        // This is a method that generates the board with the pieces on it.
        // The method generates the board row by row, starting from square a8 in the Coordinates (240, 50) on the canvas.
        // After a square gets generated, the method adds it to the list of all the squares, chessSquaresList.
        // The method also places the pieces on the right squares. There are conditions in the method that check which piece needs to be placed on the next square.  
        public List<ChessSquare> GenBoard() {
            short nextRow = 8;
            char nextColumn = 'a';
            Brush nextColor;
            int xPos = 240;
            int yPos = 50;
            List<ChessSquare> chessSquaresList = new List<ChessSquare>();
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (nextRow % 2 != nextColumn % 2)
                    {
                        nextColor = Brushes.NavajoWhite;
                    }
                    else
                    {
                        nextColor = Brushes.Brown;
                    }

                    ChessSquare nextChessSquare = new ChessSquare(nextColor, nextRow, nextColumn);
                    MainCanvas.Children.Add(nextChessSquare);
                    Canvas.SetLeft(nextChessSquare, xPos);
                    Canvas.SetTop(nextChessSquare, yPos);
                    nextChessSquare.ChessSquareClick += ChessSquareClick;

                    if ((nextRow == 8) && ((nextColumn == 'a') || (nextColumn == 'h')))
                    {
                        nextChessSquare.Piece = new BitmapImage(new Uri(texturesResource + "BlackRook.png"));
                        nextChessSquare.CurrentPiece = new ChessRook(Brushes.Black, 8, nextChessSquare.Column);
                    }
                    else if ((nextRow == 8) && ((nextColumn == 'b') || (nextColumn == 'g')))
                    {
                        nextChessSquare.Piece = new BitmapImage(new Uri(texturesResource + "BlackKnight.png"));
                        nextChessSquare.CurrentPiece = new ChessKnight(Brushes.Black, 8, nextChessSquare.Column);
                    }
                    else if ((nextRow == 8) && ((nextColumn == 'c') || (nextColumn == 'f')))
                    {
                        nextChessSquare.Piece = new BitmapImage(new Uri(texturesResource + "BlackBishop.png"));
                        nextChessSquare.CurrentPiece = new ChessBishop(Brushes.Black, 8, nextChessSquare.Column);
                    }
                    else if ((nextRow == 8) && (nextColumn == 'd'))
                    {
                        nextChessSquare.Piece = new BitmapImage(new Uri(texturesResource + "BlackQueen.png"));
                        nextChessSquare.CurrentPiece = new ChessQueen(Brushes.Black, 8, 'd');
                    }
                    else if ((nextRow == 8) && (nextColumn == 'e'))
                    {
                        nextChessSquare.Piece = new BitmapImage(new Uri(texturesResource + "BlackKing.png"));
                        nextChessSquare.CurrentPiece = new ChessKing(Brushes.Black, 8, 'e');
                    }
                    else if (nextRow == 7)
                    {
                        nextChessSquare.Piece = new BitmapImage(new Uri(texturesResource + "BlackPawn.png"));
                        nextChessSquare.CurrentPiece = new ChessPawn(Brushes.Black, 7, nextChessSquare.Column);
                    }
                    else if ((nextRow == 1) && ((nextColumn == 'a') || (nextColumn == 'h')))
                    {
                        nextChessSquare.Piece = new BitmapImage(new Uri(texturesResource + "WhiteRook.png"));
                        nextChessSquare.CurrentPiece = new ChessRook(Brushes.White, 1, nextChessSquare.Column);
                    }
                    else if ((nextRow == 1) && ((nextColumn == 'b') || (nextColumn == 'g')))
                    {
                        nextChessSquare.Piece = new BitmapImage(new Uri(texturesResource + "WhiteKnight.png"));
                        nextChessSquare.CurrentPiece = new ChessKnight(Brushes.White, 1, nextChessSquare.Column);
                    }
                    else if ((nextRow == 1) && ((nextColumn == 'c') || (nextColumn == 'f')))
                    {
                        nextChessSquare.Piece = new BitmapImage(new Uri(texturesResource + "WhiteBishop.png"));
                        nextChessSquare.CurrentPiece = new ChessBishop(Brushes.White, 1, nextChessSquare.Column);
                    }
                    else if ((nextRow == 1) && (nextColumn == 'd'))
                    {
                        nextChessSquare.Piece = new BitmapImage(new Uri(texturesResource + "WhiteQueen.png"));
                        nextChessSquare.CurrentPiece = new ChessQueen(Brushes.White, 1, 'd');
                    }
                    else if ((nextRow == 1) && (nextColumn == 'e'))
                    {
                        nextChessSquare.Piece = new BitmapImage(new Uri(texturesResource + "WhiteKing.png"));
                        nextChessSquare.CurrentPiece = new ChessKing(Brushes.White, 1, 'e');
                    }
                    else if (nextRow == 2)
                    {
                        nextChessSquare.Piece = new BitmapImage(new Uri(texturesResource + "WhitePawn.png"));
                        nextChessSquare.CurrentPiece = new ChessPawn(Brushes.White, 2, nextChessSquare.Column);
                    }

                    chessSquaresList.Add(nextChessSquare);
                    nextColumn++;
                    xPos += 40;
                }
                nextRow--;
                nextColumn = 'a';
                xPos = 240;
                yPos += 40;
            }
            return chessSquaresList;
        }

        // This is a method that receives 2 chess squares and an array of chess squares with 2 members
        // The 2 chess squares represent squares in which a piece moved and the array contains the squares in which the previous move happened.
        // The method sets the color of the squares in the array to their regular color, colors the 2 new squares and puts them in the array.
        public void MarkMove(ChessSquare chessSquare1, ChessSquare chessSquare2, ChessSquare[] prevMove)
        {                     
            if (prevMove[0] != null)
            {
                prevMove[0].Color = prevMove[0].MainColor;
            }

            if (prevMove[1] != null)
            {
                prevMove[1].Color = prevMove[1].MainColor;
            }

            chessSquare1.Color = Brushes.LightGreen;
            chessSquare2.Color = Brushes.LightGreen;
            prevMove[0] = chessSquare1;
            prevMove[1] = chessSquare2;
        }

        public void MarkSquares(List<ChessSquare> squares)
        {
            foreach (ChessSquare square in squares)
            {
                square.MarkSquare();
            }
        }

        public void UnMarkSquares(List<ChessSquare> squares)
        {
            foreach (ChessSquare square in squares)
            {
                square.UnmarkSquare();
            }
        }

        public void UpdatePieceLocation(ImageSource source, ChessPiece piece, ChessSquare square)
        {
            square.Piece = source;
            square.CurrentPiece = piece;
            piece.Row = square.Row;
            piece.Column = square.Column;
        }             
    }
}
