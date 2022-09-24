using CommunityToolkit.Maui.Views;
using MauiApp1.Services;

namespace MauiApp1.Views;

public partial class LibraryPage : ContentPage
{
	public LibraryPage()
	{
		InitializeComponent();
		GlobalData.LibraryPage = this;
	}

	public void ShowPopupPl()
	{
        var popup = new AddPlaylistPage();
        this.ShowPopup(popup);
    }
}