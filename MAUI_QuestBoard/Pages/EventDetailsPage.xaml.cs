using MAUI_QuestBoard.ViewModels;

namespace MAUI_QuestBoard.Pages;

public partial class EventDetailsPage : ContentPage
{
    public EventDetailsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is EventDetailsViewModel vm)
            await vm.LoadAsync();
    }
}
