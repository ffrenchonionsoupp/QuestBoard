using System.Windows.Input;

namespace MAUI_QuestBoard.ViewModels;

public class AddEventViewModel : BaseViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Max { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Deadline { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddEventViewModel()
    {
        SaveCommand = new Command(OnSave);
        CancelCommand = new Command(OnCancel);
    }

    private async void OnSave()
    {
        if (string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Host) ||
            string.IsNullOrWhiteSpace(Location) ||
            string.IsNullOrWhiteSpace(Max) ||
            string.IsNullOrWhiteSpace(Date) ||
            string.IsNullOrWhiteSpace(Deadline))
        {
            Error = "All fields are required.";
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
