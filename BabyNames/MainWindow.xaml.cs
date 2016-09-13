using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BabyNames
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<BabyName> Names { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Names = new List<BabyName>();

            var lines =
                System.IO.File.ReadAllLines(
                    @"05-babynames.txt");

            // Add a new instance of babyname for each line
            foreach (var line in lines)
            {
                Names.Add(new BabyName(line));
            }

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            foreach (var name in Names)
            {
                ListDecadeTopNames.Items.Add(name);
            }
        }
    }
}
