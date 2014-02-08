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

namespace MultiLanguage
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        private object p;
        private List<Word>  p_2;
        private object p_3;
        private TimeSpan timeSpan;
        private List<Word> p_4;


        public ResultWindow(object p, List<Word>  p_2, object p_3, TimeSpan timeSpan, List<Word> p_4)
        {
            InitializeComponent();
            this.p = p;
            this.p_2 = p_2;
            this.p_3 = p_3;
            this.timeSpan = timeSpan;
            this.p_4 = p_4;
            allWords.Content = p_3;
            posAnsw.Content = p;
            negAnsw.Content = p_2.Count;
            time.Content = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds);
            listBox1.ItemsSource = p_4;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
