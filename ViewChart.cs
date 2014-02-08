using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyPlot;
using OxyPlot.Series;
using System.ComponentModel;
using System.Windows;
using OxyPlot.Axes;
namespace MultiLanguage
{
    class ViewChart : INotifyPropertyChanged
    {
        public ViewChart(Group current)
        {
            this.model = new PlotModel("Postęp nauki dla "+current.Name);
            LineSeries basicSerie =  new LineSeries("Postęp nauki");
            LinearAxis ax = new LinearAxis();
            ax.Unit = "%";
            ax.Title = "Poprawne odpowiedzi";
            model.Axes.Add(ax);

            SqlAccess sql = new SqlAccess();
            basicSerie.Smooth = true;
            List<double> list = sql.GetStatsForGroup(current);
            basicSerie.Points.Add(new DataPoint(0,0));
            int i = 1;

            foreach (double d in list)
            {

                basicSerie.Points.Add(new DataPoint(i,d));
                i++;
            }

            this.model.Series.Add(basicSerie);
        }


        private PlotModel plotModel;
        public PlotModel model
        {
            get { return plotModel; }
            set { plotModel = value; OnPropertyChanged("PlotModel"); }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
