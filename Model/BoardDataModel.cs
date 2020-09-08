using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace mems_fx3lp.Model
{
    public class BoardDataModel
    {
        public BoardDataModel()
        {
            IsAttached = false;
            FlagDRDY = false;
            TriggerIsInit = false;
        }

        public string SerialNumber { get; set; }

        public bool IsAttached { get; set; }

        public bool FlagDRDY { get; set; }

        public bool TriggerIsInit { get; set;}
    }
}
