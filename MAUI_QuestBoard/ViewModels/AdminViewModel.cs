using System.Collections.ObjectModel;
using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.Services;

namespace MAUI_QuestBoard.ViewModels;

public class AdminViewModel : BaseViewModel
{
    public ObservableCollection<User> Users { get; set; } = new();

    public AdminViewModel()
    {
        LoadUsers();
    }

    private void LoadUsers()
    {
        // Fetch all users from the DataService
        var allUsers = DataService.GetAllUsers();
        foreach (var user in allUsers)
        {
            Users.Add(user);
        }
    }
}