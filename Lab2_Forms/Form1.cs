using Lab2;
using FastMember;
using System.Data;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab2_Forms
{
    public partial class Form1 : Form
    {
        API api;
        DataTable dataTable;
        GameLibrary gameLibrary;

        private void loadDataTableView()
        {
            using (var reader = ObjectReader.Create(gameLibrary.Games.ToList<Game>()))
            {
                dataTable.Load(reader);
            }

            #pragma warning disable CS8602
            if (dataTable.Columns["id"] != null)
                dataTable.Columns["id"].SetOrdinal(0);
            if (dataTable.Columns["title"] != null)
                dataTable.Columns["title"].SetOrdinal(1);
            if (dataTable.Columns["short_description"] != null)
                dataTable.Columns["short_description"].SetOrdinal(2);
            if (dataTable.Columns["genre"] != null)
                dataTable.Columns["genre"].SetOrdinal(3);
            if (dataTable.Columns["platform"] != null)
                dataTable.Columns["platform"].SetOrdinal(4);
            if (dataTable.Columns["developer"] != null)
                dataTable.Columns["developer"].SetOrdinal(5);
            if (dataTable.Columns["release_date"] != null)
                dataTable.Columns["release_date"].SetOrdinal(6);
            #pragma warning restore CS8602

            dataGridView.DataSource = dataTable;
        }
        public Form1()
        {
            InitializeComponent();
            api = new API();
            dataTable = new DataTable();
            gameLibrary = new GameLibrary();
            loadDataTableView();
        }

        private async void downloadButton_Click(object sender, EventArgs e)
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
                    gameLibrary.Entry(existingGame).CurrentValues.SetValues(game);
                else
                    gameLibrary.Games.Add(game);
                await gameLibrary.SaveChangesAsync();
            }

            loadDataTableView();
        }
    }
}
