using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using mems_fx3lp.Model;
using mems_fx3lp.ViewModel;

namespace mems_fx3lp
{
    /// <summary>
    /// Interaction logic for the basic User Control of the board
    /// </summary>
    public partial class UserControlBasic : UserControl
    {
        private BoardModel _boardModel;
        private BoardViewModel _boardViewModel;

        public UserControlBasic()
        {
            InitializeComponent();
            _boardModel = new BoardModel();
            _boardViewModel = new BoardViewModel(_boardModel, "1234");

            SerialNumLbl.DataContext = _boardViewModel;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            _boardViewModel.SerialNumber = "4321";
        }
    }
}
