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
using OxyPlot;
using OxyPlot.Series;

namespace mems_fx3lp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlotModel ScatterModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            var tmp = new PlotModel { Title = "SToDo Application", Subtitle = "Barnsley fern (IFS)" };
            var s1 = new LineSeries
            {
                StrokeThickness = 0,
                MarkerSize = 3,
                MarkerStroke = OxyColors.ForestGreen,
                MarkerType = MarkerType.Plus
            };

            foreach (var pt in Fern.Generate(2000))
            {
                s1.Points.Add(new DataPoint(pt.X, -pt.Y));
            }

            tmp.Series.Add(s1);
            this.ScatterModel = tmp;
        }     
    }
}
