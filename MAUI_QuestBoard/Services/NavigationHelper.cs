namespace MAUI_QuestBoard.Services;

public static class NavigationHelper
{
    // Pops the current page directly instead of asking Shell to resolve a
    // ".." route, and reports a problem on screen instead of crashing the
    // app (an exception escaping an async void handler takes the whole
    // app down).
    public static async Task GoBackAsync()
    {
        try
        {
            await Shell.Current.Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Navigation error", ex.Message, "OK");
        }
    }
}
