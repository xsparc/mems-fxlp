using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace mems_fx3lp.Model
{
    /// <summary>
    /// View Model for the static state of the Board
    /// </summary>
    class BoardModel
    {
        private bool _isFX3LPConnected;
        private string _name;
        private string _serialNumber;

        /// <summary>
        /// Connectivity check to the FX3LP board.
        /// </summary>
        public bool IsFX3LPConnected1 { get => _isFX3LPConnected; set => _isFX3LPConnected = value; }
        /// <summary>
        /// Name of the board. Reflected in the actual PCB.
        /// </summary>
        public string Name { get => _name; set => _name = value; }
        /// <summary>
        /// SerialNumber of the board.
        /// </summary>
        public string SerialNumber { get => _serialNumber; set => _serialNumber = value; }


        public BoardModel()
        {
            _name = "ADXL345EVB";
            _isFX3LPConnected = false;
        }

    }
}
