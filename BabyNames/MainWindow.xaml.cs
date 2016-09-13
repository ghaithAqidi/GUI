using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        
        private ObservableCollection<int> DecadesList { get; set;}

        private Dictionary<int, List<BabyName>> TopNamesForYear { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Names = new List<BabyName>();
            DecadesList = new ObservableCollection<int>();

            var lines =
                System.IO.File.ReadAllLines(
                    @"05-babynames.txt");

            // Add a new instance of babyname for each line
            foreach (var line in lines)
            {
                Names.Add(new BabyName(line));
            }
            
            // Initialise decades
            var decs = new List<int> {1900, 1910, 1920, 1930, 1940, 1950, 1960, 1970, 1980, 1990, 2000};
            foreach (var dec in decs)
            {
                DecadesList.Add(dec);
            }

            GenerateTopYearNames(Names);

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            foreach (var name in Names)
            {
                ListDecadeTopNames.Items.Add(name);
            }

            foreach (var decade in DecadesList)
            {
                Decades.Items.Add(decade);
            }
        }

        /// <summary>
        /// Generates teh dictionary blablabla
        /// </summary>
        /// <param name="names"></param>
        private void GenerateTopYearNames(List<BabyName> names)
        {
            TopNamesForYear = new Dictionary<int, List<BabyName>>();
            foreach (var decade in DecadesList)
            {
                TopNamesForYear.Add(decade, new List<BabyName>());
                var listForYear = TopNamesForYear[decade];

                foreach (var babyName in Names)
                {
                    if (babyName.Rank(decade) <= 10 && babyName.Rank(decade) != 0)
                        listForYear.Add(babyName);
                }

                // Sort list according to rank
                TopNamesForYear[decade] = listForYear.OrderBy(x => x.Rank(decade)).ToList();
            }
        }

        private void Decades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (int) Decades.SelectedItems[0];

            ListDecadeTopNames.Items.Clear();

            var topNames = TopNamesForYear[selected];
            foreach (var babyName in topNames)
            {
                ListDecadeTopNames.Items.Add(babyName);
            }
            ListDecadeTopNames.UpdateLayout();
        }

        private void SerachButton_Click(object sender, RoutedEventArgs e)
        {
            var name = NameBox.Text;

            var babyName = Names.FirstOrDefault(x => x.Name.ToLower().Equals(name.ToLower()));

            if (babyName == null)
                return;

            // Set labels for avg and trend
            TrendResLabel.Content = babyName.Trend();
            RankResLabel.Content = babyName.AverageRank();

            YearRankBox.Items.Clear();
            foreach (var decade in DecadesList)
            {
                YearRankBox.Items.Add($"{decade} - {babyName.Rank(decade)}");
            }
            YearRankBox.UpdateLayout();
        }
        
    }
}
