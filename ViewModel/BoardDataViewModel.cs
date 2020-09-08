using mems_fx3lp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mems_fx3lp.ViewModel
{
    class BoardDataViewModel : INotifyPropertyChanged
    {
        private BoardDataModel boardDataModel;

        public  BoardDataViewModel(BoardDataModel pBoardDataModel)
        {
            boardDataModel = pBoardDataModel;
        }

        public string SerialNumber
        {
            get { return boardDataModel.SerialNumber;}
            set { boardDataModel.SerialNumber = value; OnPropertyChanged("SerialNumber"); }
        }

        public bool IsAttached
        {
            get { return boardDataModel.IsAttached; }
            set { boardDataModel.IsAttached = value; OnPropertyChanged("IsAttached"); }
        }

        public bool FlagDRDY
        {
            get { return boardDataModel.FlagDRDY;  }
            set { boardDataModel.FlagDRDY = value; OnPropertyChanged("FlagDRDY"); }
        }

        public bool TriggerIsInit
        {
            get { return boardDataModel.TriggerIsInit; }
            set { boardDataModel.TriggerIsInit = value; OnPropertyChanged("TriggerIsInit"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }

    }
}
