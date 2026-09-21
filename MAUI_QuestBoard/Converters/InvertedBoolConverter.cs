using System.Globalization;

namespace MAUI_QuestBoard.Converters;

// Lets a single bool (e.g. IsGuest) drive the visibility of two opposite
// UI blocks without needing a second property on every ViewModel.
public class InvertedBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool b && !b;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool b && !b;
    }
}
