using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;


namespace SharonChess
{
    // This is an interface that is used in the XAML of the ChessSquare user control.
    // The interface is used to convert the width and height of the ellipse that marks the square.
    public class MultiplierConverter : IValueConverter
    {
        // This is the method that converts the value provided to it. the result is a multiplication between the value and the parameter it received.
        // If the value or the parameter are not numbers, no conversion happens.
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (double.TryParse(parameter?.ToString(), out double multiplier) && value is double initialValue)
            {
                return initialValue * multiplier;
            }

            return value;
        }

        // This is a method of the interface that didn't get implemented. If it had gotten implemented, it would have converted the width/height back to its initial value.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
