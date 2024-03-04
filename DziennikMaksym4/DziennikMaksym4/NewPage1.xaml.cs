namespace DziennikMaksym4;

public partial class NewPage1 : ContentPage
{
    public string _filePath;
    public int studentIdCounter = 0;
    public NewPage1(string filePath)
    {
        InitializeComponent();
        _filePath = filePath;

        List<string> lines = ReadTextFile(filePath);

        foreach (string line in lines)
        {
            Button button = new Button
            {
                Text = line,
                Margin = new Thickness(10),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            button.Clicked += (sender, e) =>
            {
                OpenStudent(filePath);
                OpenStudent(line);
            };
            stackLayout.Children.Add(button);
        }

    }

    public async void OpenStudent(string studentName)
    {
        try
        {
            string selectedOption = await DisplayActionSheet("Wybierz Dzia?anie", "Anuluj", null, "Usu? ucznia", "Zaznacz obecno??");

            switch (selectedOption)
            {
                case "Usu? ucznia":
                    DeleteStudent(studentName);
                    break;
                case "Zaznacz obecno??":
                    //ShowRandomStudent();
                    break;
                default:
                    //await DisplayAlert("ERROR!", "Zaznaczy?e? z?? opcj?", "OK");
                    break;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Opis b³êdu: {ex.Message}", "OK");
        }
    }
    private async void ShowRandomStudent(object sender, EventArgs e)
    {
        List<string> lines = File.ReadAllLines(_filePath).ToList();  //Sprawdzenie wszystkich linijek w kodzie

        if (lines.Count > 0)
        {
            Random random = new Random();
            int randomIndex = random.Next(0, lines.Count);
            string randomStudent = lines[randomIndex];

            await DisplayAlert("Ucze? do pytania", randomStudent, "OK");
        }
        else
        {
            await DisplayAlert("Brak uczniów", "W klasie nie ma uczniów do losowania!", "OK");
        }
    }

    private List<Student> _students = new List<Student>();

    public class Student
    {
        public int LineNumber { get; set; }
        public string Name { get; set; }
    }

    public void DeleteStudent(string studentName)
    {
        List<string> lines = File.ReadAllLines(_filePath).ToList();

        int index = lines.FindIndex(line => line.Contains(studentName));

        if (index != -1)
        {
            lines.RemoveAt(index);
            File.WriteAllLines(_filePath, lines);

            DisplayAlert("Komunikat", "Ucze? zosta? pomy?lnie usuni?ty", "OK");
        }
        else
        {
            DisplayAlert("Komunikat", "Nie mo?na odnale?? ucznia", "OK");
        }
    }

    private List<string> ReadTextFile(string filePath)
    {
        List<string> lines = new List<string>();
        if (File.Exists(filePath))
        {
            lines.AddRange(File.ReadAllLines(filePath));
        }
        return lines;
    }

    public async void OnNewStudent(object sender, EventArgs e)
    {
        try
        {
            string entry = await DisplayPromptAsync("Nowy Ucze?", "Wpisz imi? i nazwisko:", "OK", "Cancel");

            if (!string.IsNullOrEmpty(entry))
            {
                string[] lines = File.ReadAllLines(_filePath);

                int lineNumber = 1;
                while (lines.Any(line => line.StartsWith($"{lineNumber}.")))
                {
                    lineNumber++;
                }

                string newStudentLine = $"{lineNumber}. {entry}";
                File.AppendAllText(_filePath, newStudentLine + Environment.NewLine);

                await DisplayAlert("Komunikat", "Dodano nowego ucznia", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Niepoprawna nazwa ucznia!", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Opis b³êdu: {ex.Message}", "OK");
        }
    }
    public void Wroc(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
}
