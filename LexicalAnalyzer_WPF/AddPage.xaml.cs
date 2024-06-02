using System.Windows;

namespace Exp1_WPF;

public partial class AddPage : Window
{
    private string _labelContent;
    public List<string> ToAddList;
    public event EventHandler<(AddPageCaller caller, List<string> items)> OnDataAdded; // 修改事件类型
    public AddPageCaller Caller { get; set; } // 新增属性

    public AddPage(string labelContent, AddPageCaller caller)
    {
        _labelContent = labelContent;
        Caller = caller; // 设置调用来源
        InitializeComponent();
        Label.Content = _labelContent;
    }

    private void Add_Clicked(object sender, RoutedEventArgs e)
    {
        string input = InputTextBox.Text;
        if (string.IsNullOrEmpty(input))
        {
            return;
        }
        ToAddList = input.Split(',').ToList();
        OnDataAdded?.Invoke(this, (Caller, ToAddList)); // 触发事件并传递调用来源和数据
    }
}

