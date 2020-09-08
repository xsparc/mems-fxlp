using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using mems_fx3lp.Model;

namespace mems_fx3lp.ViewModel
{
    /// <summary>
    /// View Model for the static state of the Board
    /// </summary>
    class BoardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private BoardModel _boardModel;

        public string SerialNumber 
        {
            get { return _boardModel.SerialNumber; }
            set { _boardModel.SerialNumber = value; OnPropertyChanged("SerialNumber"); }
        }

        public BoardViewModel(BoardModel boardModel, string serialNumber )
        {
            _boardModel = boardModel;
            _boardModel.SerialNumber = serialNumber;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
