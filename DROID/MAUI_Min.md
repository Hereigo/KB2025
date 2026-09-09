```bash
dotnet new maui -n MinimalMauiApp
```
### Step 2: Update MainPage.xaml

Replace the content with:

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MinimalMauiApp.MainPage">

    <VerticalStackLayout Padding="30" Spacing="20">
        <Entry x:Name="inputField"
               Placeholder="Type something..."
               FontSize="18"/>

        <Button Text="Submit"
                Clicked="OnButtonClicked"
                FontSize="18"/>

        <Label x:Name="outputLabel"
               Text="Result will appear here"
               FontSize="18"
               TextColor="Black"/>
    </VerticalStackLayout>
</ContentPage>
```

### Step 3: Update MainPage.xaml.cs

```csharp
namespace MinimalMauiApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnButtonClicked(object sender, EventArgs e)
    {
        outputLabel.Text = $"You typed: {inputField.Text}";
    }
}
```

### Step 4: Run on Android

Connect your Android device or start an emulator.

```bash
dotnet build -t:Run -f net8.0-android
```
This will deploy and run the app on your Android device/emulator.