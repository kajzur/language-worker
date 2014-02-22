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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using SQLiteUTF8CIComparison;
using System.Windows.Controls.Primitives;

namespace MultiLanguage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ObservableCollection<Group> groups = new ObservableCollection<Group>();
        public MainWindow()
        {
            SQLiteFunction.RegisterFunction(typeof(SQLiteCaseInsensitiveCollation));
            InitializeComponent();
            SqlAccess sql = new SqlAccess();
            sql.GetGroups(true);
            listBox1.ItemsSource = groups;
            listBox2.ItemsSource = groups;
            listBox3.ItemsSource = groups;
            if (groups.Count > 0)
            {
                listBox2.SelectedIndex = 0;
                listBox3.SelectedIndex = 0;
                listBox1.SelectedIndex = 0;
            }
                
            listBox3.MouseDoubleClick+=new MouseButtonEventHandler(listBox3_MouseDoubleClick);

        }

        private void listBox3_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;

            while (obj != null && obj != listBox3)
            {
                if (obj.GetType() == typeof(ListBoxItem))
                {
                    ChartWindow chart = new ChartWindow((Group)listBox3.SelectedItem);
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void listBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            addGroup adg = new addGroup();

            adg.InitializeComponent();
            adg.ShowDialog();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            SqlAccess sql = new SqlAccess();
            foreach (Group g in listBox1.SelectedItems)
            {
                sql.DeleteWordsFromGroup(g.Id);
            }
            MessageBox.Show("Grupa została wyczyszczona.");
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            SqlAccess sql = new SqlAccess();
            Queue<Group> temporary = new Queue<Group>();
            foreach (Group g in listBox1.SelectedItems)
            {
                sql.DeleteGroup(g.Id);
                temporary.Enqueue(g);
                
            }
            foreach(Group p in temporary)
                MainWindow.groups.Remove(p);
            MessageBox.Show("Grupa została usunięta.");
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            LearningWindow lw;
            if (radioButton1.IsChecked==true)//nauka do bolu
            {
                lw = new LearningWindow("Wybrano tryb: Nauka do bólu",1, listBox2, comboBox1);
                lw.ShowDialog();
            }
            else if (radioButton2.IsChecked==true)//nauka w formie testu
            {
                lw = new LearningWindow("Wybrano tryb: Nauka w formie testu", 2, listBox2, comboBox1);
                lw.ShowDialog();
            }
            else if (radioButton3.IsChecked == true)//nauka z pomoca
            {
                lw = new LearningWindow("Wybrano tryb: Nauka dla początkujących", 3, listBox2, comboBox1);
                lw.ShowDialog();
            }
        }
    }
}
