using System.Collections.ObjectModel;
using System.Windows.Input;
using MAUI_QuestBoard.DataAccess;
using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.Services;

namespace MAUI_QuestBoard.ViewModels;

public class AdminViewModel : BaseViewModel
{
    private readonly UserData _userData = new();

    // ----- Registered accounts -----
    public ObservableCollection<User> Users { get; } = new();

    // ----- Database details -----
    public string FileName { get; private set; } = string.Empty;
    public string FilePath { get; private set; } = string.Empty;
    public string FolderPath { get; private set; } = string.Empty;
    public string FileExists { get; private set; } = string.Empty;
    public string FileSize { get; private set; } = string.Empty;
    public string LastModified { get; private set; } = string.Empty;
    public string OpenFlags { get; private set; } = string.Empty;
    public ObservableCollection<string> Tables { get; } = new();
    public ObservableCollection<string> Connections { get; } = new();

    // ----- Confirmations and problems, shown on the page itself -----
    public string InfoMessage { get; private set; } = string.Empty;
    public bool HasInfo => !string.IsNullOrEmpty(InfoMessage);

    public string ErrorMessage { get; private set; } = string.Empty;
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public ICommand RefreshCommand { get; }
    public ICommand CopyPathCommand { get; }
    public ICommand OpenFolderCommand { get; }
    public ICommand BackCommand { get; }
    public ICommand LogoutCommand { get; }

    public AdminViewModel()
    {
        RefreshCommand = new Command(OnRefresh);
        CopyPathCommand = new Command(OnCopyPath);
        OpenFolderCommand = new Command(OnOpenFolder);
        BackCommand = new Command(OnBack);
        LogoutCommand = new Command(OnLogout);
    }

    private void SetInfo(string message)
    {
        InfoMessage = message;
        ErrorMessage = string.Empty;
        OnPropertyChanged(nameof(InfoMessage));
        OnPropertyChanged(nameof(HasInfo));
        OnPropertyChanged(nameof(ErrorMessage));
        OnPropertyChanged(nameof(HasError));
    }

    private void SetError(string message)
    {
        ErrorMessage = message;
        InfoMessage = string.Empty;
        OnPropertyChanged(nameof(ErrorMessage));
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(InfoMessage));
        OnPropertyChanged(nameof(HasInfo));
    }

    // Called from the page's OnAppearing and from the Refresh button. Any
    // failure is shown on the page instead of silently leaving it blank.
    public async Task LoadAsync()
    {
        try
        {
            // Reading the users first also makes sure the database file has
            // been created and seeded before we describe it below.
            var allUsers = await _userData.GetUsersAsync();

            Users.Clear();
            foreach (var user in allUsers.OrderBy(u => u.Name))
                Users.Add(user);

            var info = await DatabaseDiagnostics.GetInfoAsync();

            FileName = info.FileName;
            FilePath = info.FilePath;
            FolderPath = info.FolderPath;
            FileExists = info.FileExists;
            FileSize = info.FileSize;
            LastModified = info.LastModified;
            OpenFlags = info.OpenFlags;

            Tables.Clear();
            foreach (var table in info.Tables)
                Tables.Add(table);

            Connections.Clear();
            foreach (var connection in info.Connections)
                Connections.Add(connection);

            OnPropertyChanged(nameof(FileName));
            OnPropertyChanged(nameof(FilePath));
            OnPropertyChanged(nameof(FolderPath));
            OnPropertyChanged(nameof(FileExists));
            OnPropertyChanged(nameof(FileSize));
            OnPropertyChanged(nameof(LastModified));
            OnPropertyChanged(nameof(OpenFlags));

            SetError(info.Error ?? string.Empty);
        }
        catch (Exception ex)
        {
            SetError($"Could not load the admin data: {ex.Message}");
        }
    }

    private async void OnRefresh()
    {
        await LoadAsync();
    }

    private async void OnCopyPath()
    {
        try
        {
            await Microsoft.Maui.ApplicationModel.DataTransfer.Clipboard.Default.SetTextAsync(FilePath);
            SetInfo("Database path copied to the clipboard.");
        }
        catch (Exception ex)
        {
            SetError($"Could not copy the path: {ex.Message}");
        }
    }

    private void OnOpenFolder()
    {
#if WINDOWS
        try
        {
            if (Directory.Exists(FolderPath))
            {
                System.Diagnostics.Process.Start(
                    new System.Diagnostics.ProcessStartInfo("explorer.exe", $"\"{FolderPath}\"")
                    {
                        UseShellExecute = true
                    });
            }
            else
            {
                SetError("That folder doesn't exist yet.");
            }
        }
        catch (Exception ex)
        {
            SetError($"Could not open the folder: {ex.Message}");
        }
#else
        SetInfo("Opening the folder is only supported on Windows - use Copy Path instead.");
#endif
    }

    private async void OnBack()
    {
        await NavigationHelper.GoBackAsync();
    }

    private async void OnLogout()
    {
        SessionService.SignOut();
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
