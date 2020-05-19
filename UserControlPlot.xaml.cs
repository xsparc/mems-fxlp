using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for UserControlPlot.xaml
    /// </summary>
    public partial class UserControlPlot : UserControl
    {
        public PlotModel ScatterModel { get; set; }
        public UserControlPlot()
        {
            InitializeComponent();
            DataContext = this;
            var tmp = new PlotModel { Title = "Sample Plot", Subtitle = "Barnsley fern (IFS)"};
            var s1 = new LineSeries
            {
                
                Background = OxyColors.White,
                StrokeThickness = 0,
                MarkerSize = 3,
                MarkerStroke = OxyColors.ForestGreen,
                MarkerType = MarkerType.Plus,
            };

            /*
            foreach (var pt in Fern.Generate(2000))
            {
                s1.Points.Add(new DataPoint(pt.X, -pt.Y));
            }
            */
            

            double DummyFunc(double d)
            {
                var test = new Random((int)d);
                return test.NextDouble();
            }
            
            /*
            for (int i = 0; i < 3000; i++)
            {
                //s1.Points.Add(new DataPoint(i, test.NextDouble()));
                tmp.Series.Add(new FunctionSeries(DummyFunc,0,10,.1, "random"));
            }
            */

            tmp.Series.Add(new FunctionSeries(DummyFunc, 0, 100, .1, "random"));
            //tmp.Series.Add(s1);
            this.ScatterModel = tmp;
        }
    }
}
