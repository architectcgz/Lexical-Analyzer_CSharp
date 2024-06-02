using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Exp1_WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window,INotifyPropertyChanged
{
    private Lexer _lexer;
    private ObservableCollection<Token> _tokens;
    private string _timeDay;
    private string _timeHM;
    private DispatcherTimer _timer;

    public ObservableCollection<Token> Tokens
    {
        get { return _tokens; }
        set
        {
            _tokens = value;
        }
    }

    public string TimeDay
    {
        get { return _timeDay; }
        set
        {
            _timeDay = value;
            OnPropertyChanged();
        }
    }

    public string TimeHM
    {
        get { return _timeHM; }
        set
        {
            _timeHM = value;
            OnPropertyChanged();
        }
    }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        _lexer = new Lexer();
        _tokens = new ObservableCollection<Token>();
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private void AnalyzeButton_Click(object sender, RoutedEventArgs e)
    {
        if (InputTextBox.Text == "")
        {
            StatusTextBlock.Text = "请先输入要分析的代码";
            StatusTextBlock.Foreground = Brushes.Red;
            return;
        }
        StatusTextBlock.Foreground = Brushes.Black;
        Tokens.Clear();//清空上次分析的结果
        _lexer.Analyze(InputTextBox.Text).ForEach(t => Tokens.Add(t));
        StatusTextBlock.Text = "分析完成";
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        TimeDay = DateTime.Now.ToString("yyyy-MM-dd");
        TimeHM = DateTime.Now.ToString("HH:mm:ss");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        LexWindow lexerWindow = new LexWindow(ref _lexer);
        foreach(var item in _lexer.Separators)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
        lexerWindow.Show();
    }
}