using Lab2;
using System.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Lab2MAUIApp
{
    public partial class MainPage : ContentPage
    {
        API api;
        GameLibrary gameLibrary;
        private ObservableCollection<Game> _games;
        private bool isDataLoaded = false;
        private bool descendingOrder = false;
        public MainPage()
        {
            InitializeComponent();
            api = new API();
            gameLibrary = new GameLibrary();
            GamesListView.ItemSelected += OnGameSelected;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!isDataLoaded)
            {
                await LoadGames();
                isDataLoaded = true;
            }
        }

        private async Task LoadGames()
        {
            try
            {
                _games = new ObservableCollection<Game>(await gameLibrary.Games.ToListAsync());
                GamesListView.ItemsSource = _games;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load games: {ex.Message}");
                await DisplayAlert("Error", "Failed to load games", "OK");
            }
        }

        private async Task LoadSortedByTitles()
        {
            try
            {
                var gameList = await gameLibrary.Games.ToListAsync();
                if (descendingOrder)
                    _games = new ObservableCollection<Game>(gameList.OrderByDescending(g => g.title));
                else
                    _games = new ObservableCollection<Game>(gameList.OrderBy(g => g.title));
                
                GamesListView.ItemsSource = _games;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load games: {ex.Message}");
                await DisplayAlert("Error", "Failed to load games", "OK");
            }
        }

        private async Task LoadSortedByIDs()
        {
            try
            {
                var gameList = await gameLibrary.Games.ToListAsync();
                if (descendingOrder)
                    _games = new ObservableCollection<Game>(gameList.OrderByDescending(g => g.id));
                else
                    _games = new ObservableCollection<Game>(gameList.OrderBy(g => g.id));

                GamesListView.ItemsSource = _games;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load games: {ex.Message}");
                await DisplayAlert("Error", "Failed to load games", "OK");
            }
        }

        private async Task LoadSortedByPlatforms()
        {
            try
            {
                var gameList = await gameLibrary.Games.ToListAsync();
                if(descendingOrder)
                    _games = new ObservableCollection<Game>(gameList.OrderByDescending(g => g.platform));
                else
                    _games = new ObservableCollection<Game>(gameList.OrderBy(g => g.platform));

                GamesListView.ItemsSource = _games;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load games: {ex.Message}");
                await DisplayAlert("Error", "Failed to load games", "OK");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                Game game = new Game(Int32.Parse(idEntry.Text),
                                     titleEntry.Text,
                                     descEntry.Text,
                                     genreEntry.Text,
                                     platformEntry.Text,
                                     developerEntry.Text,
                                     dateEntry.Text);
                var existingGame = await gameLibrary.Games.FindAsync(game.id);
                if (existingGame != null)
                {
                    gameLibrary.Entry(existingGame).CurrentValues.SetValues(game);
                }
                else
                {
                    gameLibrary.Games.Add(game);
                }

                await gameLibrary.SaveChangesAsync();
                await LoadGames(); 
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Save failed: {ex.Message}");
                await DisplayAlert("Error", "Failed to save game", "OK");
            }
        }

        private async void OnDownloadFromAPIClicked(object sender, EventArgs e)
        {
            try
            {
                await api.download();
                if (api.gameList == null)
                {
                    Debug.WriteLine("Failed to download games or no games available.");
                    return;
                }

                foreach (var game in api.gameList)
                {
                    var existingGame = await gameLibrary.Games.FindAsync(game.id);
                    if (existingGame != null)
                    {
                        gameLibrary.Entry(existingGame).CurrentValues.SetValues(game);
                    }
                    else
                    {
                        gameLibrary.Games.Add(game);
                    }
                }

                await gameLibrary.SaveChangesAsync();
                await LoadGames();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Save failed: {ex.Message}");
                await DisplayAlert("Error", "Failed to save games", "OK");
            }
        }
        private void OnSortOrderClicked(object sender, EventArgs e)
        {
            if (descendingOrder)
            {
                descendingOrder = false;
                SortOrderButton.Text = "Sort Ascending";
            }
            else
            {
                descendingOrder = true;
                SortOrderButton.Text = "Sort Descending";
            }
        }
        private async void OnSortByTitleClicked(object sender, EventArgs e)
        {
            await LoadSortedByTitles();
        }
        private async void OnSortByIDClicked(object sender, EventArgs e)
        {
            await LoadSortedByIDs();
        }
        private async void OnSortByPlatformClicked(object sender, EventArgs e)
        {
            await LoadSortedByPlatforms();
        }

        private void OnGameSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Game selectedGame)
            {
                idEntry.Text = selectedGame.id.ToString();
                titleEntry.Text = selectedGame.title;
                descEntry.Text = selectedGame.short_description;
                genreEntry.Text = selectedGame.genre;
                platformEntry.Text = selectedGame.platform;
                developerEntry.Text = selectedGame.developer;
                dateEntry.Text = selectedGame.release_date;
            }

            GamesListView.SelectedItem = null;
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Game game)
            {
                idEntry.Text = game.id.ToString();
                titleEntry.Text = game.title;
                descEntry.Text = game.short_description;
                genreEntry.Text = game.genre;
                platformEntry.Text = game.platform;
                developerEntry.Text = game.developer;
                dateEntry.Text = game.release_date;
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Game game)
            {
                bool confirm = await DisplayAlert("Confirm", $"Delete {game.title}?", "Yes", "No");
                if (confirm)
                {
                    try
                    {
                        _games.Remove(game);
                        gameLibrary.Games.Remove(game);
                        await gameLibrary.SaveChangesAsync();
                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Delete failed: {ex.Message}");
                        await DisplayAlert("Error", "Failed to delete game", "OK");
                    }
                }
            }
        }

        private void ClearForm()
        {
            idEntry.Text = string.Empty;
            titleEntry.Text = string.Empty;
            descEntry.Text = string.Empty;
            genreEntry.Text = string.Empty;
            platformEntry.Text = string.Empty;
            developerEntry.Text = string.Empty;
            dateEntry.Text = string.Empty;
        }
    }
}