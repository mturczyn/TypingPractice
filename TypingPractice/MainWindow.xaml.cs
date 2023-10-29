using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace TypingPractice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BitmapImage _piesioImage = new BitmapImage(new Uri(@"piesio.JPEG", UriKind.Relative));

        private static char[] _letters = Enumerable.Range('a', 'z' - 'a').Select(x => (char)x).ToArray();
        private static SpeechRecognitionEngine _recognizer;
        private readonly SpeechSynthesizer _synthesizer;
        private volatile bool _isSpeaking = false;

        public static RoutedCommand UpdateCommand { get; } = new RoutedCommand();

        public MainWindow()
        {
            // Create an in-process speech recognizer for the en-US locale.  
            _recognizer = new SpeechRecognitionEngine(CultureInfo.CurrentCulture);

            _synthesizer = new SpeechSynthesizer();
            _synthesizer.SetOutputToDefaultAudioDevice();

            InitializeComponent();

            UpdateCommand.InputGestures.Add(new KeyGesture(Key.U, ModifierKeys.Control));

            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
            _synthesizer.SpeakCompleted += _synthesizer_SpeakCompleted;
        }

        private void _synthesizer_SpeakCompleted(object? sender, SpeakCompletedEventArgs e)
        {
            _isSpeaking = false;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _recognizer?.Dispose();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Installed speech recognizers: " +
                string.Join(
                    ',',
                    SpeechRecognitionEngine.InstalledRecognizers()
                        .Select(x => $"{x.Name} {x.Description}")));

            if (false) StartSpeechRecognition();
        }

        private void TypedText_KeyDown(object sender, KeyEventArgs e)
        {
            var prevLetterToType = TextToType.Text;

            var rnd = new Random();

            var idx = rnd.Next(0, _letters.Length);

            var nextLetter = char.ToUpper(_letters[idx]);

            TextToType.Text = nextLetter.ToString();
            var typedLetter = e.Key.ToString();
            TypedText.Text = typedLetter;

            if (!_isSpeaking)
            {
                _synthesizer.SpeakAsync(typedLetter);
                _isSpeaking = true;
            }

            // Correct type
            if (e.Key.ToString().ToLower() == prevLetterToType.ToString().ToLower())
            {
                var image = new Image()
                {
                    Source = _piesioImage,
                    Height = 150,
                    Margin = new Thickness(5),
                };
                image.MouseDown += (s, e) => piesie.Children.Clear();
                piesie.Children.Add(image);
            }
        }

        private static void StartSpeechRecognition()
        {
            // Create and load a dictation grammar.  
            _recognizer.LoadGrammar(new DictationGrammar());

            // Add a handler for the speech recognized event.  
            _recognizer.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

            // Configure input to the speech recognizer.  
            _recognizer.SetInputToDefaultAudioDevice();

            // Start asynchronous, continuous speech recognition.  
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        // Handle the SpeechRecognized event.  
        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            MessageBox.Show("Recognized text: " + e.Result.Text);
            Console.WriteLine("RECOGNIZED TEXT");
            System.Diagnostics.Debug.WriteLine("RECOGNIZED TEXT");
            Console.WriteLine("Recognized text: " + e.Result.Text);
            System.Diagnostics.Debug.WriteLine("Recognized text: " + e.Result.Text);
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var publisher_name = "TypingPractice";
            var product_name = "TypingPractice";
            var shortcutName = string.Concat(
                Environment.GetFolderPath(Environment.SpecialFolder.Programs),
                "\\",
                publisher_name, 
                "\\", 
                product_name, 
                ".appref-ms");
            var process = new Process();
            process.StartInfo = new ProcessStartInfo(shortcutName)
            {
                UseShellExecute = true,
            };

            process.Start();

            Application.Current.Shutdown();

            return;

            var sb = new StringBuilder("App deploymewnt values");
            sb.AppendLine();
            var getters = typeof(ApplicationDeployment).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);

            foreach (var getter in getters)
            {
                var value = getter.GetValue(null);
                sb.Append(getter.Name);
                sb.Append(" = ");
                sb.AppendLine($"{value}");
            }
            MessageBox.Show(sb.ToString());

            if (!bool.TryParse(
                Environment.GetEnvironmentVariable(ApplicationDeploymentEnvVarNames.isNetworkDeployedEnvVar),
                out _))
            {
                MessageBox.Show($"Could not recognize {ApplicationDeploymentEnvVarNames.isNetworkDeployedEnvVar} environment variable");
                return;
            }

            MessageBox.Show($"{ApplicationDeploymentEnvVarNames.isNetworkDeployedEnvVar} found, value = {ApplicationDeployment.IsNetworkDeployed}");

            if (!ApplicationDeployment.IsNetworkDeployed)
            {
                MessageBox.Show($"{ApplicationDeploymentEnvVarNames.isNetworkDeployedEnvVar} was false");
                return;
            }

            var activationUriRaw = Environment.GetEnvironmentVariable(ApplicationDeploymentEnvVarNames.activationUriEnvVar);
            if (string.IsNullOrEmpty(activationUriRaw))
            {
                MessageBox.Show($"{ApplicationDeploymentEnvVarNames.activationUriEnvVar} environment variable was emtpy");
                return;
            }

            var activationUri = new Uri(activationUriRaw);

            var nvt = HttpUtility.ParseQueryString(activationUri.Query);
            MessageBox.Show($"{ApplicationDeploymentEnvVarNames.activationUriEnvVar} environment variable successfully read, value = {activationUri}");
        }
    }
}
