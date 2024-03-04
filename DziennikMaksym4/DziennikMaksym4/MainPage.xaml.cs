namespace DziennikMaksym4
{
    public partial class MainPage : ContentPage
    {
        private readonly string folderPath = "C:\\Maksym";

        public MainPage()
        {
            InitializeComponent();
            CheckAndCreateFolder();
            LoadTextFiles();
        }

        private void CheckAndCreateFolder() //Tworzenie folderu "Maksym"
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        private void LoadTextFiles()
        {

            string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in txtFiles)
            {
                string fileName = Path.GetFileName(filePath);
                Button fileButton = new Button
                {
                    Text = fileName,
                    Margin = new Thickness(5)
                };
                fileButton.Clicked += (sender, e) =>
                {
                    OpenClass(filePath);
                };

                fileStackLayout.Children.Add(fileButton);
            }
        }
        public async void OnNewClass(object sender, EventArgs e)
        {
            string className = await DisplayPromptAsync("Nowa klasa", "Wpisz nazwę klasy: ", "Dodaj", "Anuluj");

            if (className != null)
            {
                string driveletter = "C:\\Maksym";
                string filename = className + ".txt";
                string filePath = Path.Combine(driveletter, filename);
                DisplayAlert("Utworzono klasę ", $"{filename}", "OK");
                File.WriteAllText(filePath, ""); //Useless
            }
        }

        public void OnReloadClicked(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            Navigation.PushAsync(new MainPage());
        }

        private async void OpenClass(string filePath)
        {
            string text = File.ReadAllText(filePath);

            string selectedOption = await DisplayActionSheet("Wybierz Działanie", "Anuluj", null, "Wyświetl klasę", "Edytuj klasę", "Usuń klasę");

            switch (selectedOption)
            {
                case "Wyświetl klasę":
                    DisplayAlert("Lista uczniów", $"Uczniowie klasy '{Path.GetFileName(filePath)}':\n{text}", "OK");
                    break;
                case "Edytuj klasę":
                    await Navigation.PushAsync(new NewPage1(filePath));
                    break;
                case "Usuń klasę":
                    Usun(filePath);

                    break;
                default:
                    //await DisplayAlert("ERROR!", "Zaznaczyłeś złą opcję", "OK");
                    break;
            }
        }
        private void Usun(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                DisplayAlert("Komunikat", "Plik został usunięty pomyślnie!", "OK");
            }
            else
            {
                DisplayAlert("ERROR!", "Żądany plik nie został znaleziony!", "OK");
            }
        }


    }

}