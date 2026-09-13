using System.Windows.Input;

namespace MAUI_QuestBoard.ViewModels;

public class AddUserViewModel : BaseViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Password1 { get; set; } = string.Empty;
    public string Password2 { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;

    public ICommand AddCommand { get; }
    public ICommand CancelCommand { get; }

    public AddUserViewModel()
    {
        AddCommand = new Command(OnAdd);
        CancelCommand = new Command(OnCancel);
    }

    private async void OnAdd()
    {
        if (string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Phone) ||
            string.IsNullOrWhiteSpace(Password1) ||
            string.IsNullOrWhiteSpace(Password2))
        {
            Error = "All fields are required.";
            OnPropertyChanged(nameof(Error));
            return;
        }

        if (Password1 != Password2)
        {
            Error = "Passwords do not match.";
            OnPropertyChanged(nameof(Error));
            return;
        }

        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
