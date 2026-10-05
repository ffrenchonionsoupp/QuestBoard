using MAUI_QuestBoard.Services;
using MAUI_QuestBoard.ViewModels;

namespace MAUI_QuestBoard.Pages;

public partial class AdminPage : ContentPage
{
    public AdminPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Safety net: the Admin tab is hidden for everyone except the
        // administrator, but never show this list to anyone else even if
        // the page is reached some other way.
        if (!SessionService.IsAdmin)
        {
            await Shell.Current.GoToAsync("//QuestBoardPage");
            return;
        }

        if (BindingContext is AdminViewModel vm)
            await vm.LoadAsync();
    }
}
