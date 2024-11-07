using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Bingo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int grid_count = 25;
        private bool[] table_state = new bool[grid_count];
        private readonly Random rnd_gen = new();
        private readonly List<int> table_history = [];
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < grid_count; i++)
            {
                var child = VisualTreeHelper.GetChild(gdBingo, i);
                if (child != null && child is Button button)
                {
                    button.Content = button.Name;
                }
                lbHash.Content = "Hash";
            }

        }

        private void Shuffle(object sender, RoutedEventArgs e)
        {
            int[] bingo = new int[grid_count];
            int bingo_hash;

            while (true)
            {
                for (int i = 0; i < bingo.Length; i++)
                {
                    int rnd;
                    do
                    {
                        rnd = (int)rnd_gen.NextInt64(1, 75 + 1);
                    } while (bingo.Contains(rnd));
                    bingo[i] = rnd;
                }
                bingo_hash = bingo.GetHashCode();
                if (!table_history.Contains(bingo_hash)) break;
            }

            table_history.Add(bingo_hash);
            lbHash.Content = bingo_hash;

            for (int i = 0; i < grid_count; i++)
            {
                var child = VisualTreeHelper.GetChild(gdBingo, i);
                if (child != null && child is Button button)
                {
                    button.Content = bingo[i];
                    button.Background = new BrushConverter().ConvertFrom("#FFDDDDDD") as SolidColorBrush;
                }
            }

            table_state = new bool[grid_count];
            lvHistory.Items.Clear();
        }

        private void TileClicked(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is Button button)
            {
                int row = int.Parse(button.Name[2].ToString());
                int col = int.Parse(button.Name[3].ToString());
                int number = row * 5 + col;

                if (!table_state[number])
                {
                    table_state[number] = true;

                    button.Background = Brushes.Aqua;

                    lvHistory.Items.Add(number.ToString());
                }

                bool bingo = false;
                // 縦方向に探索
                for (int i = 0; i < 5; i++)
                {
                    int punched = 0;
                    for (int j = 0; j < 5; j++)
                    {
                        if (table_state[i + j * 5]) { punched++; }
                        else { break; }
                    }
                    if (punched == 5) { bingo = true; }
                }
                //横方向に探索
                for (int i = 0; i < 5; i++)
                {
                    int punched = 0;
                    for (int j = 0; j < 5; j++)
                    {
                        if (table_state[i * 5 + j]) { punched++; }
                        else { break; }
                    }
                    if (punched == 5) { bingo = true; }
                }

                //斜め方向に探索
                int punchedAngle = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (table_state[i * 6]) { punchedAngle++; }
                    else { break; }
                }
                if (punchedAngle == 5) { bingo = true; }
                
                punchedAngle = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (table_state[(i + 1) * 4]) { punchedAngle++; }
                    else { break; }
                }
                if (punchedAngle == 5) { bingo = true; }

                if (bingo)
                {
                    MessageBox.Show("BINGO!");
                }
            }
        }
    }
}