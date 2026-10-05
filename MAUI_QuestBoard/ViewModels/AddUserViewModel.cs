using System.Windows.Input;
using MAUI_QuestBoard.DataAccess;
using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.Services;

namespace MAUI_QuestBoard.ViewModels;

public class AddUserViewModel : BaseViewModel
{
    private readonly UserData _userData = new();

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Password1 { get; set; } = string.Empty;
    public string Password2 { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(Error);

    public ICommand AddCommand { get; }
    public ICommand CancelCommand { get; }

    public AddUserViewModel()
    {
        AddCommand = new Command(OnAdd);
        CancelCommand = new Command(OnCancel);
    }

    private void SetError(string message)
    {
        Error = message;
        OnPropertyChanged(nameof(Error));
        OnPropertyChanged(nameof(HasError));
    }

    private async void OnAdd()
    {
        // Collect every problem so the person can fix them all in one pass.
        // Nothing the person typed is cleared - the fields keep their values.
        var problems = new List<string>();

        var missing = new List<string>();
        if (string.IsNullOrWhiteSpace(Name)) missing.Add("Name");
        if (string.IsNullOrWhiteSpace(Email)) missing.Add("Email");
        if (string.IsNullOrWhiteSpace(Phone)) missing.Add("Phone");
        if (string.IsNullOrWhiteSpace(Password1)) missing.Add("Password");
        if (string.IsNullOrWhiteSpace(Password2)) missing.Add("Confirm Password");
        if (missing.Count > 0)
        {
            problems.Add($"Please fill in: {string.Join(", ", missing)}.");
        }

        if (!string.IsNullOrWhiteSpace(Email) && !ValidationHelper.IsValidEmail(Email))
        {
            problems.Add("Enter a valid email address (for example, name@example.com).");
        }

        if (!string.IsNullOrWhiteSpace(Phone) && !ValidationHelper.IsValidPhone(Phone))
        {
            problems.Add("Phone number can only contain digits, spaces, dashes, dots, parentheses and +, with at least 7 digits.");
        }

        if (!string.IsNullOrWhiteSpace(Password1) &&
            !string.IsNullOrWhiteSpace(Password2) &&
            Password1 != Password2)
        {
            problems.Add("Passwords do not match.");
        }

        if (problems.Count > 0)
        {
            SetError(string.Join(Environment.NewLine, problems));
            return;
        }

        var email = Email.Trim();

        try
        {
            var existing = await _userData.GetUserAsync(email);
            if (existing is not null)
            {
                SetError("An account with that email address already exists.");
                return;
            }

            await _userData.SaveUserAsync(new User
            {
                Email = email,
                Password = Password1,
                Name = Name.Trim(),
                Phone = Phone.Trim(),
                Role = "User"
            });
        }
        catch (Exception ex)
        {
            SetError($"Could not create the account: {ex.Message}");
            return;
        }

        SetError(string.Empty);
        await Shell.Current.DisplayAlert("Account created", "Your account was created. You can now log in.", "OK");
        await NavigationHelper.GoBackAsync();
    }

    private async void OnCancel()
    {
        await NavigationHelper.GoBackAsync();
    }
}
