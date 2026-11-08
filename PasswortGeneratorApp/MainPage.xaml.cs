using System.Security.Cryptography;

namespace PasswortGeneratorApp;

public partial class MainPage : ContentPage
{
    string aktuellesPasswort = "";

    public MainPage()
    {
        InitializeComponent();
    }

    void OnLaengeChanged(object sender, ValueChangedEventArgs e)
    {
        LaengeLabel.Text = "Länge: " + (int)e.NewValue;
    }

    void OnGenerierenClicked(object sender, EventArgs e)
    {
        string zeichen = "abcdefghijklmnopqrstuvwxyz";
        if (GrossSwitch.IsToggled) { zeichen += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
        if (ZahlenSwitch.IsToggled) { zeichen += "0123456789"; }
        if (SonderSwitch.IsToggled) { zeichen += "!?#$%&"; }

        int laenge = (int)LaengeSlider.Value;
        aktuellesPasswort = ErzeugePasswort(laenge, zeichen);

        PasswortLabel.Text = aktuellesPasswort;
        StaerkeLabel.Text = "Stärke: " + PruefeStaerke(aktuellesPasswort);
    }

    async void OnKopierenClicked(object sender, EventArgs e)
    {
        if (aktuellesPasswort == "") { return; }
        await Clipboard.Default.SetTextAsync(aktuellesPasswort);
        await DisplayAlert("Kopiert", "Dein Passwort ist in der Zwischenablage.", "OK");
    }

    static string ErzeugePasswort(int laenge, string zeichen)
    {
        string passwort = "";
        for (int i = 0; i < laenge; i++)
        {
            int position = RandomNumberGenerator.GetInt32(zeichen.Length);
            passwort += zeichen[position];
        }
        return passwort;
    }

    static string PruefeStaerke(string passwort)
    {
        int punkte = 0;
        if (passwort.Length >= 12) { punkte++; }
        if (passwort.Any(char.IsUpper)) { punkte++; }
        if (passwort.Any(char.IsLower)) { punkte++; }
        if (passwort.Any(char.IsDigit)) { punkte++; }
        if (passwort.Any(c => !char.IsLetterOrDigit(c))) { punkte++; }

        if (punkte <= 2) { return "Schwach"; }
        if (punkte <= 4) { return "Mittel"; }
        return "Stark";
    }
}