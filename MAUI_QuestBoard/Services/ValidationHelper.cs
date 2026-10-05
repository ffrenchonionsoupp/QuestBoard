using System.Text.RegularExpressions;

namespace MAUI_QuestBoard.Services;

public static class ValidationHelper
{
    // Something@something.something with no spaces.
    private static readonly Regex EmailPattern =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    // Digits plus the usual separators - no letters.
    private static readonly Regex PhonePattern =
        new(@"^[\d\s\-\.\(\)\+]+$", RegexOptions.Compiled);

    public static bool IsValidEmail(string value)
    {
        return EmailPattern.IsMatch(value.Trim());
    }

    public static bool IsValidPhone(string value)
    {
        var trimmed = value.Trim();
        return PhonePattern.IsMatch(trimmed) && trimmed.Count(char.IsDigit) >= 7;
    }
}
