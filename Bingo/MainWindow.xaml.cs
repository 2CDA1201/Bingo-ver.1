using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bingo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int grid_count = 25;
        private readonly List<int> history = [];
        private readonly Random rnd_gen = new();
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < grid_count; i++)
            {
                var child = VisualTreeHelper.GetChild(gdBingo, i);
                if (child != null && child is Label label)
                {
                    label.Content = label.Name;
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int[] bingo = new int[grid_count];

            for (int i = 0; i < bingo.Length; i++)
            {
                int rnd;
                do
                {
                    rnd = (int)rnd_gen.NextInt64(1, 75 + 1);
                } while (bingo.Contains(rnd));
                bingo[i] = rnd;
            }

            for (int i = 0; i < grid_count; i++)
            {
                var child = VisualTreeHelper.GetChild(gdBingo, i);
                if (child != null && child is Label label)
                {
                    label.Content = bingo[i];
                }
            }

            history.Clear();
        }
        private void LabelClicked(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is Label label)
            {
                int row = int.Parse(label.Name[2].ToString());
                int col = int.Parse(label.Name[3].ToString());
                int number = row * 5 + col;

                history.Add(number);

                string history_str = "";
                foreach (int item in history)
                {
                    history_str += item.ToString() + ", ";
                }

                MessageBox.Show(history_str);
            }
        }
    }
}