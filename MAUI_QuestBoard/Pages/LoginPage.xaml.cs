using MAUI_QuestBoard.ViewModels;
namespace MAUI_QuestBoard.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel();
    }
}