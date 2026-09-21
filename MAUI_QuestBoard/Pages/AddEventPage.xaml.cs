using MAUI_QuestBoard.Services;

namespace MAUI_QuestBoard.Pages;

public partial class AddEventPage : ContentPage
{
    public AddEventPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Safety net: guests should never actually land on this page (the
        // "Add Event" button already redirects them), but if it's ever
        // reached another way, bounce to account creation instead of
        // letting a guest host an event.
        if (!SessionService.IsLoggedIn)
        {
            await Shell.Current.GoToAsync(nameof(AddUserPage));
        }
    }
}
