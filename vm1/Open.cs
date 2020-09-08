using System;
using OxyPlot;

namespace vm1
{
    internal class Open : IPlotModel
    {
        private string v;

        public Open(string v)
        {
            this.v = v;
        }

        OxyColor IPlotModel.Background
        {
            get
            {
                IPlotModel model = new vm1.Open(@"C:\Users\Caron\Source\Repos\mems-fx3lp4\RiverFlow.svg");
                OxyColor d = default(OxyColor);
                return d;

            }
        }
        void IPlotModel.AttachPlotView(IPlotView plotView)
        {
            IPlotModel model = new vm1.Open(@"C:\Users\Caron\Source\Repos\mems-fx3lp4\RiverFlow.svg");
            OxyColor d = default(OxyColor);
            return;
        }

        void IPlotModel.Render(IRenderContext rc, double width, double height)
        {
            IPlotModel model = new vm1.Open(@"C:\Users\Caron\Source\Repos\mems-fx3lp4\RiverFlow.svg");
            OxyColor d = default(OxyColor);
            return;
        }

        void IPlotModel.Update(bool updateData)
        {
            IPlotModel model = new vm1.Open(@"C:\Users\Caron\Source\Repos\mems-fx3lp4\RiverFlow.svg");
            OxyColor d = default(OxyColor);
            return;
        }
    }
    
}