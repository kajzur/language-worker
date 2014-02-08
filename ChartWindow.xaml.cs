using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Series;
using System.IO;
namespace MultiLanguage
{
    /// <summary>
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {

        private ViewChart plotModel;

        public ChartWindow(Group g)
        {

            plotModel = new ViewChart(g);
            DataContext = plotModel;
            InitializeComponent();
            SqlAccess sql = new SqlAccess();
            label2.Content = sql.GetAttendsForGroup(g);
            TimeSpan timeSpan = sql.GetAvgOfDurationForGroup(g);
            avgduration.Content = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds);
            Show();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "graph"; // Default file name
            dlg.DefaultExt = ".svg"; // Default file extension
            dlg.Filter = "Plik SVG (.svg)|*.svg"; // Filter files by extension

            
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                using (StreamWriter writer = new StreamWriter(filename, false))
                {
                    writer.Write(plotModel.model.ToSvg(400,300,true));
                    writer.Close();
                }
                
            }
            
        }


        
    }
}
