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
using FX3Api;
using FX3USB;
using RegMapClasses;
using mems_fx3lp.Model;
using mems_fx3lp.ViewModel;
using adisInterface;
using mems_fx3lp.Product;
using AdisApi;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Timers;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics;

namespace mems_fx3lp
{
    /// <summary>
    /// Interaction logic for UserControlPlotDUT.xaml
    /// </summary>
    public partial class UserControlPlotDUT : UserControl
    {
        //static field
        private static ADXL345EB _evalBoard;
        public static PlotModel AccelPlotModel { get; set; }
        public PlotModel FFTPlotModel { get; set; }
        private static OxyPlot.Series.LineSeries s1, s2, s3;
        private OxyPlot.Series.LineSeries s4;

        public PlotModel fFTPlot;
        public static PlotModel plot;

        private Timer aTimer;
        private bool _buttonPressed = false;

        private enum AccelerometerAxis { X_Axis = 0x00, Y_Axis = 0x01, Z_Axis = 0x02 };

        string fx3FirmwarePath;
        string fx3BootloaderPath;
        string fx3ProgrammerPath;
        adisInterface.IDutInterface Dut;

        FX3Connection fX3Connection;
        static BoardDataViewModel boardDataVM;
        BoardDataModel boardDataM;
        int j = 0;

        public UserControlPlotDUT()
        {
            InitializeComponent();
            DataContext = this;
            this.fx3FirmwarePath = "FX3_Firmware.img";
            this.fx3BootloaderPath = "boot_fw.img";
            this.fx3ProgrammerPath = "USBFlashProg.img";

            // UI initialization. Will be replace by VMMV
            //IsAttachedTextBox.Text = false.ToString();
            //ConnectToggleButton.IsChecked = false;

            // FirmwarePath, set target to "FX3_Firmware.img"
            // BootloaderPath, set target to "boot_fw.img"
            // ProgrammerPath, set target to "USBFlashProg.img"
            fX3Connection = new FX3Connection(this.fx3FirmwarePath, fx3BootloaderPath, fx3ProgrammerPath, DeviceType.IMU);
            _evalBoard = new ADXL345EB(fX3Connection);

            boardDataM = new BoardDataModel();
            boardDataVM = new BoardDataViewModel(boardDataM);

            fX3Connection.PartType = DUTType.ADcmXL3021;
            Dut = new AdcmInterface3Axis(fX3Connection);

            InitializePlot();

            RegistersComboBox.ItemsSource = Enum.GetValues(typeof(ADXL345EB.RegisterAddress));
            AxisComboBox.ItemsSource = Enum.GetValues(typeof(AccelerometerAxis));
            PresetComboBox.ItemsSource = Enum.GetValues(typeof(ADXL345EB.Presets));
            CheckIfAttached();
        }


        private void InitializePlot()
        {
            // Plot information
            plot = new PlotModel { Title = "ADI ADXL345", Subtitle = "Acceleration" };
            s1 = new LineSeries();
            s1.Title = "x Acceleration";

            s1.Color = OxyColors.Yellow;
            plot.Series.Add(s1);

            s2 = new LineSeries();
            s2.Color = OxyColors.Blue;
            s2.Title = "y Acceleration";
            plot.Series.Add(s2);

            s3 = new LineSeries();
            s3.Title = "z Acceleration";
            s3.Color = OxyColors.Green;
            plot.Series.Add(s3);
            plot.Axes.Add(new OxyPlot.Axes.LinearAxis() { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 500, Key = "Horizontal" });
            plot.Axes.Add(new OxyPlot.Axes.LinearAxis() { Position = AxisPosition.Left, Minimum = -500, Maximum = 500, Key = "Vertical" });
            AccelPlotModel = plot;

            fFTPlot = new PlotModel { Title = "FFT", Subtitle = "Real FFT" };
            s4 = new LineSeries();
            s4.Title = "Forward FFT";
            s4.Color = OxyColors.Orange;
            fFTPlot.Series.Add(s4);
            FFTPlotModel = fFTPlot;
        }

        private void ConnectDevice()
        {
            // Wait for 1 second
            fX3Connection.WaitForBoard(1);

            if (fX3Connection.AvailableFX3s.Count == 1)
            {
                // MVVM approach
                boardDataVM.SerialNumber = fX3Connection.AvailableFX3s[0];

                // Straightforward approach

                fX3Connection.Connect(boardDataVM.SerialNumber);

                CheckIfAttached();
            }

        }

        private void ResetAllFX3s()
        {
            fX3Connection.ResetAllFX3s();
            CheckIfAttached();
        }

        private void DisconnectDevice()
        {
            fX3Connection.Disconnect(boardDataVM.SerialNumber);
            CheckIfAttached();
        }

        private void CheckIfAttached()
        {
            // VMMV
            boardDataVM.IsAttached = fX3Connection.FX3BoardAttached;

            // Delete after VMMV is properly running
            //IsAttachedTextBox.Text = boardDataVM.IsAttached.ToString();
            ConnectToggleButton.IsChecked = boardDataVM.IsAttached;
            if(boardDataVM.IsAttached)
            {
                SerialNumTextBox.Text = boardDataVM.SerialNumber;
            }
            else
            {
                SerialNumTextBox.Text = "";
            }
        }

        /// <summary>
        /// Checks if DUT is ready for register read.
        /// </summary>
        private void CheckDUTDataReady()
        {
            // VMMV 
            boardDataVM.FlagDRDY = fX3Connection.DrActive;

            // Delete after VMMV is properly running
            //DRDYTextBox.Text = boardDataVM.FlagDRDY.ToString();

        }

        private void ResetBtnClick(object sender, RoutedEventArgs e)
        {
            fX3Connection.ResetAllFX3s();
        }

        private void FFTBtnClick(object sender, RoutedEventArgs e)
        {
            var axisSelected = (AccelerometerAxis)AxisComboBox.SelectedItem;
            FFTDisplay(axisSelected);
        }

        private void MeasurementBtnClick(object sender, RoutedEventArgs e)
        {
            byte[] MOSI = new byte[] { (byte)ADXL345EB.SpiInstByte.SPI_WRITE | (byte)ADXL345EB.RegisterAddress.POWER_CTL, 0x08 };
            _evalBoard.Configure(MOSI);
        }

        private void SelectionChangedRegisterComboBox(object sender, SelectionChangedEventArgs e)
        {

            ADXL345EB.RegisterAddress registerAddress =(ADXL345EB.RegisterAddress)RegistersComboBox.SelectedItem;
            RegisterRdWrTextBox.Text = _evalBoard.ReadRegister(registerAddress).ToString();
        }

        private void RegisterWriteBtnClick(object sender, RoutedEventArgs e)
        {
            ADXL345EB.RegisterAddress registerAddress = (ADXL345EB.RegisterAddress)RegistersComboBox.SelectedItem;
            _evalBoard.WriteRegister(registerAddress, Convert.ToByte(RegisterRdWrTextBox.Text));
        }

        private void TimedPlotBtnClick(object sender, RoutedEventArgs e)
        {
            //PlotPlayButton.    
            if(_buttonPressed == false)
            {
                aTimer = new Timer();

                // Hook up the Elapsed event for the timer.
                aTimer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
                
                // Set the Interval to 2 seconds (2000 milliseconds).
                aTimer.Interval = Convert.ToDouble(PlotRefreshPeriodlTextBox.Text);
                aTimer.Enabled = true;
                _buttonPressed = true;
                TimedPlotButton.Content = "Stop";
            }
            else 
            {
                aTimer.Enabled = false;
                _buttonPressed = false;
                TimedPlotButton.Content = "Plot";
            }
        }

        public static void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            UpdatePlot();
        }

        private void CaptureBtnClick(object sender, RoutedEventArgs e)
        {
            _evalBoard.GetXYZFIFO();

            // Test Code
            //_evalBoard.GetXYZFIFO_Interrupt(boardDataVM.TriggerIsInit);
            //if (boardDataVM.TriggerIsInit == false)
            //{
            //    boardDataVM.TriggerIsInit = true;
            //}

            AccelXTextBox.Text = _evalBoard.DataX_S32.ToString();
            AccelYTextBox.Text = _evalBoard.DataY_S32.ToString();
            AccelZTextBox.Text = _evalBoard.DataZ_S32.ToString();

        }

        private void PopulateBtnClick(object sender, RoutedEventArgs e)
        {
            UpdatePlot();
        }

        private static void UpdatePlot()
        {
            int xAxisCount = s1.Points.Count;
            int yAxisCount = s2.Points.Count;
            int zAxisCount = s3.Points.Count;
            
            _evalBoard.GetXYZFIFO();

            s1.Points.Add(new DataPoint(xAxisCount, _evalBoard.DataX_S32));
            s2.Points.Add(new DataPoint(yAxisCount, _evalBoard.DataY_S32));
            s3.Points.Add(new DataPoint(zAxisCount, _evalBoard.DataZ_S32));

            AccelPlotModel = plot;
            AccelPlotModel.InvalidatePlot(true);
        }

        private void ClearBtnClick(object sender, RoutedEventArgs e)
        {
            ClearPlot();
            AccelPlotModel.InvalidatePlot(true);
            FFTPlotModel.InvalidatePlot(true);

        }

        private void SelectionChangedPresetComboBox(object sender, SelectionChangedEventArgs e)
        {
            ADXL345EB.Presets preset = (ADXL345EB.Presets)PresetComboBox.SelectedItem;
            _evalBoard.SetPreset(preset);
        }


        private void ConntectToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            ConnectDevice();
        }

        private void ConnectToggleButtonUnchecked(object sender, RoutedEventArgs e)
        {
            DisconnectDevice();
        }

        private void ClearPlot()
        {
            s1.Points.Clear();
            s2.Points.Clear();
            s3.Points.Clear();
            s4.Points.Clear();
        }

        private void FFTDisplay(AccelerometerAxis axis)
        {
            // Clear points
            s4.Points.Clear();

            List<DataPoint> pointsList;
            List<DataPoint> pointsList2; //Additional for Imaginary , Added by JP
            switch (axis)
            {
                case AccelerometerAxis.X_Axis:
                    pointsList = s1.Points;
                    pointsList2 = s1.Points; //Additional for Imaginary , Added by JP
                    break;
                case AccelerometerAxis.Y_Axis:
                    pointsList = s2.Points;
                    pointsList2 = s2.Points;//Additional for Imaginary , Added by JP
                    break;
                case AccelerometerAxis.Z_Axis:
                    pointsList = s3.Points;
                    pointsList2 = s3.Points;//Additional for Imaginary , Added by JP
                    break;
                default:
                    // This should not happen
                    pointsList = null;
                    pointsList2 = null;
                    break;
            }

            //var pointsList = s1.Points;
            var pointsRealList = new List<double>();
            var pointsImagList = new List<double>(); //Additional for Imaginary , Added by JP
            foreach (var dataPoint in pointsList)
            {
                pointsRealList.Add(dataPoint.Y);
            }

            foreach (var dataPoint in pointsList2) //Additional for Imaginary , Added by JP
            {
                pointsImagList.Add(dataPoint.Y * 0); // Multiply by 0, all imaginary values are 0. 
            }

            int numSamples = pointsRealList.Count;
            int numSamples2 = pointsImagList.Count; //Additional for Imaginary , Added by JP

            if (pointsRealList.Count.IsEven())
            {
                pointsRealList.Add(0);
                pointsRealList.Add(0);
            }
            else
            {
                pointsRealList.Add(0);
            }

            if (pointsImagList.Count.IsEven()) //Additional for Imaginary , Added by JP
            {
                pointsImagList.Add(0);
                pointsImagList.Add(0);
            }
            else
            {
                pointsImagList.Add(0);
            }


            var pointsRealArray = pointsRealList.ToArray();
            var pointsImagArray = pointsImagList.ToArray(); //Additional for Imaginary , Added by JP

            var FFT_Count = pointsRealArray.Count();


            int ctr = 0; //Additional for Imaginary , Added by JP

            Complex32[] For_FFTval = new Complex32[FFT_Count]; //Additional for Imaginary , Added by JP
            foreach (var data_points in pointsRealArray) //Additional for Imaginary , Added by JP

            {
                For_FFTval[ctr] = new Complex32((float)pointsRealArray[ctr], (float)pointsImagArray[ctr]);
                ctr = ctr + 1;
            }

            Fourier.Forward(For_FFTval); //Additional for Imaginary , Added by JP


            ctr = 0;
            double[] FFT_plot = new double[FFT_Count]; //Additional for Imaginary , Added by JP
            foreach (var data_points in For_FFTval)

            {
                FFT_plot[ctr] = Complex32.Abs(For_FFTval[ctr]); //Additional for Imaginary , Added by JP
                ctr = ctr + 1;
            }

            var x = 0;

            foreach (var realPoint in FFT_plot)
            {
                var count = (double)s4.Points.Count;
                s4.Points.Add(new DataPoint(count, realPoint));
            }

            FFTPlotModel = fFTPlot;
            FFTPlotModel.InvalidatePlot(true);
        }

    }
}
