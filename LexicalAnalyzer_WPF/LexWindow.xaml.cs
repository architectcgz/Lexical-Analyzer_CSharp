using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Navigation;


namespace Exp1_WPF
{
    public partial class LexWindow : Window
    {
        private Lexer _lexer;
        private ObservableCollection<string> Keywords { get; set; }
        private ObservableCollection<string> RelationOperators { get; set; }
        private ObservableCollection<string> ArithmeticOperators { get; set; }
        private ObservableCollection<char> Separators { get; set; }

        public LexWindow(ref Lexer lexer)
        {
            InitializeComponent();
            _lexer = lexer;
            DataContext = this;
            Keywords = new ObservableCollection<string>(lexer.Keywords);
            RelationOperators = new ObservableCollection<string>(lexer.RelationOperators);
            ArithmeticOperators = new ObservableCollection<string>(lexer.ArithmeticOperators);
            Separators = new ObservableCollection<char>(lexer.Separators);
            KeywordsListBox.ItemsSource = Keywords;
            RelationOperatorsListBox.ItemsSource = RelationOperators;
            ArithmeticOperatorsListBox.ItemsSource = ArithmeticOperators;
            SeparatorsListBox.ItemsSource = Separators;
        }

        private void AddKeyword_Click(object sender, RoutedEventArgs e)
        {
            AddPage addPage = new AddPage("输入要添加到关键字,如有多个用逗号分隔",AddPageCaller.Keywords);
            // 订阅事件
            addPage.OnDataAdded += AddPage_OnDataAdded;
            addPage.Show();
        }

        private void RemoveKeyword_Click(object sender, RoutedEventArgs e)
        {
            if (KeywordsListBox.SelectedItem != null)
            {
                var selectedItem = (string)KeywordsListBox.SelectedItem;
                Keywords.Remove(selectedItem);
                _lexer.Keywords.Remove(selectedItem);
            }
        }

        private void AddRelationOperator_Click(object sender, RoutedEventArgs e)
        {
            AddPage addPage = new AddPage("输入要添加到关键字,如有多个用逗号分隔",AddPageCaller.Keywords);
            // 订阅事件
            addPage.OnDataAdded += AddPage_OnDataAdded;
            addPage.Show();
        }

        private void RemoveRelationOperator_Click(object sender, RoutedEventArgs e)
        {
            if (RelationOperatorsListBox.SelectedItem != null)
            {
                var selectedItem = (string)KeywordsListBox.SelectedItem;
                RelationOperators.Remove(selectedItem);
                _lexer.RelationOperators.Remove(selectedItem);
            }
        }

        private void AddArithmeticOperator_Click(object sender, RoutedEventArgs e)
        {
            AddPage addPage = new AddPage("输入要添加到关键字,如有多个用逗号分隔",AddPageCaller.Keywords);
            // 订阅事件
            addPage.OnDataAdded += AddPage_OnDataAdded;
            addPage.Show();
        }

        private void RemoveArithmeticOperator_Click(object sender, RoutedEventArgs e)
        {
            if (ArithmeticOperatorsListBox.SelectedItem != null)
            {
                var selectedItem = (string)KeywordsListBox.SelectedItem;
                ArithmeticOperators.Remove(selectedItem);
                _lexer.ArithmeticOperators.Remove(selectedItem);
            }
        }

        private void AddSeparator_Click(object sender, RoutedEventArgs e)
        {
            AddPage addPage = new AddPage("输入要添加到关键字,如有多个用逗号分隔",AddPageCaller.Keywords);
            // 订阅事件
            addPage.OnDataAdded += AddPage_OnDataAdded;
            addPage.Show();
        }

        private void RemoveSeparator_Click(object sender, RoutedEventArgs e)
        {
            if (SeparatorsListBox.SelectedItem != null)
            {
                var selectedItem = (string)KeywordsListBox.SelectedItem;
                Separators.Remove(selectedItem[0]);//分隔符都是单个的
                _lexer.Separators.Remove(selectedItem[0]);
            }
        }
        
        // 事件处理方法
        private void AddPage_OnDataAdded(object sender, (AddPageCaller caller, List<string> items) e)
        {
            var (caller, items) = e;

            switch (caller)
            {
                case AddPageCaller.Keywords:
                    foreach (var item in items)
                    {
                        Console.WriteLine($"向KeyWords中添加了:{item}");
                        Keywords.Add(item);
                        _lexer.Keywords.Add(item);
                    }
                    break;
                case AddPageCaller.RelationOperators:
                    foreach (var item in items)
                    {
                        RelationOperators.Add(item);
                        _lexer.Keywords.Add(item);
                    }
                    break;
                case AddPageCaller.ArithmeticOperators:
                    foreach (var item in items)
                    {
                        ArithmeticOperators.Add(item);
                        _lexer.Keywords.Add(item);
                    }
                    break;
                case AddPageCaller.Separators:
                    foreach (var item in items)
                    {
                        Separators.Add(item[0]); //分隔符都是单个的
                        _lexer.Separators.Add(item[0]);
                    }
                    break;
            }
        }
        
    }
    
    public enum AddPageCaller
    {
        Keywords,
        RelationOperators,
        ArithmeticOperators,
        Separators
    }

}
