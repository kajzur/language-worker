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
    /// Interaction logic for LearningWindow.xaml
    /// </summary>
    public partial class LearningWindow : Window
    {

        private int type;
        List<Group> selectedGroups = new List<Group>();
        private static int indexOfCurrentWord = 0;
        List<Word> words;
        DateTime ts;
        List<Word> bad = new List<Word>();
        Dictionary<Group, Result> groupsWithResult = new Dictionary<Group, Result>();//used only for multiplie groups
        Word lastWord;
        int selectedType = 0;
        
        public LearningWindow(String style, int type, ListBox list, ComboBox choosenType)
        {
            InitializeComponent();
            selectedStyle.Content = style;
            ts = DateTime.Now;
            this.type = type;
            this.selectedType = choosenType.SelectedIndex;
            foreach(object ob in list.SelectedItems)
                selectedGroups.Add((Group)ob);
            if(selectedGroups.Count==1)
                basicLanguage.KeyDown += new KeyEventHandler(basicLanguage_KeyDown);
            else if (selectedGroups.Count > 1)
            {
                foreach (Group p in selectedGroups)
                    groupsWithResult.Add(p, new Result());
                basicLanguage.KeyDown += new KeyEventHandler(basicLanguageMultiple_KeyDown);
            }
            SqlAccess sql = new SqlAccess();
            
           

            if (this.type == 1) {

                Random random = new Random();
                words = sql.getWordsFromGroups(selectedGroups);
                howMuchAll.Content = words.Count;
                progressBar1.Maximum = words.Count;
                badAnsv.Content = 0;
                goodAnsv.Content = 0;
                indexOfCurrentWord = random.Next(0, words.Count);
                if(selectedType==0)
                    foreginWord.Text = words.ElementAt<Word>(indexOfCurrentWord).Basic;
                else
                    foreginWord.Text = words.ElementAt<Word>(indexOfCurrentWord).Foregin;
            
            }
            else if (this.type == 2)
            {

                Random random = new Random();
                //int randomNumber = random.Next(0, 100);
                words = sql.getWordsFromGroups(selectedGroups);
                howMuchAll.Content = words.Count;
                progressBar1.Maximum = words.Count;
                badAnsv.Content = 0;
                goodAnsv.Content = 0;
                indexOfCurrentWord = random.Next(0, words.Count);
                if (selectedType == 0)
                    foreginWord.Text = words.ElementAt<Word>(indexOfCurrentWord).Basic;
                else
                    foreginWord.Text = words.ElementAt<Word>(indexOfCurrentWord).Foregin;
            
            }
            else if (this.type == 3) {
                label5.Visibility = Visibility.Visible;
                
                Random random = new Random();
                //int randomNumber = random.Next(0, 100);
                words = sql.getWordsFromGroups(selectedGroups);
                howMuchAll.Content = words.Count;
                progressBar1.Maximum = words.Count;
                badAnsv.Content = 0;
                goodAnsv.Content = 0;
                indexOfCurrentWord = random.Next(0, words.Count);
                label5.MouseDown += new MouseButtonEventHandler(label5_MouseDown );
                if (selectedType == 0)
                    foreginWord.Text = words.ElementAt<Word>(indexOfCurrentWord).Basic;
                else
                    foreginWord.Text = words.ElementAt<Word>(indexOfCurrentWord).Foregin;
            }

        }

        //ctor end
        private void ShowResult() {

            ResultWindow rs = new ResultWindow(goodAnsv.Content, bad, howMuchAll.Content, DateTime.Now.Subtract(ts), bad);
            rs.InitializeComponent();
            rs.Show();

        }

        void label5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Word current = words.ElementAt<Word>(indexOfCurrentWord);
            if (selectedType == 0)
                basicLanguage.Text = current.Foregin.Substring(0, 1);
            else
                basicLanguage.Text = current.Basic.Substring(0, 1);
            basicLanguage.Focus();
            basicLanguage.CaretIndex = 1;
        }
        
        void basicLanguageMultiple_KeyDown(object sender, KeyEventArgs e)
        {//event for multiplie selected groups

            if (e.Key == Key.Enter)
            {
            
                //something smart to recognize which word is connected to which group and save pos/neg ansv.
                if (this.type == 1)
                {
                    if (words.Count != 0)
                    {
                        Word current = words.ElementAt<Word>(indexOfCurrentWord);
                        if ((basicLanguage.Text.Equals(current.Foregin, StringComparison.OrdinalIgnoreCase) && selectedType == 0) || (basicLanguage.Text.Equals(current.Basic, StringComparison.OrdinalIgnoreCase) && selectedType == 1))
                        {
                            words.Remove(current);
                            lastWord = current;
                            if (words.Count > 0)
                            {
                                progressBar1.Value++;
                                goodAnsv.Content = (int)goodAnsv.Content + 1;

                                Random r = new Random();
                                indexOfCurrentWord = r.Next(0, words.Count);
                                foreginWord.Text = selectedType == 0 ? words.ElementAt<Word>(indexOfCurrentWord).Basic : words.ElementAt<Word>(indexOfCurrentWord).Foregin;
                                lastForeign.Text = lastWord.Basic;
                                lastBasic.Text = lastWord.Foregin;
                                lastForeign.Visibility = Visibility.Visible;
                                lastBasic.Visibility = Visibility.Visible;
                                label6.Visibility = Visibility.Visible;
                                label7.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                //SqlAccess sql = new SqlAccess();
                                //sql.UpdateStats(new Result(selectedGroups, (int)goodAnsv.Content, (int)badAnsv.Content, DateTime.Now.Subtract(ts)));
                                this.Close();
                                ShowResult();
                            }
                        }
                        else
                        {

                            bad.Add(words.ElementAt<Word>(indexOfCurrentWord));

                            badAnsv.Content = (int)badAnsv.Content + 1;
                            Random r = new Random();
                            indexOfCurrentWord = r.Next(0, words.Count);
                            foreginWord.Text = selectedType == 0 ? words.ElementAt<Word>(indexOfCurrentWord).Basic : words.ElementAt<Word>(indexOfCurrentWord).Foregin;
                            lastForeign.Text = lastWord.Basic;
                            lastBasic.Text = lastWord.Foregin;
                            lastForeign.Visibility = Visibility.Visible;
                            lastBasic.Visibility = Visibility.Visible;
                            label6.Visibility = Visibility.Visible;
                            label7.Visibility = Visibility.Visible;
                        }
                        basicLanguage.Text = "";
                    }
                    else
                    {

                        MessageBox.Show("Wybrana grupa nie ma slowek.");

                    }
                }
                else if (this.type == 2)
                {


                    if (words.Count != 0)
                    {
                        Word current = words.ElementAt<Word>(indexOfCurrentWord);
                        if ((basicLanguage.Text.Equals(current.Foregin, StringComparison.OrdinalIgnoreCase) && selectedType == 0) || (basicLanguage.Text.Equals(current.Basic, StringComparison.OrdinalIgnoreCase) && selectedType == 1))
                        {
                            words.Remove(current);
                            lastWord = current;
                            if (words.Count > 0)
                            {
                                progressBar1.Value++;
                                goodAnsv.Content = (int)goodAnsv.Content + 1;

                                Result currResult = groupsWithResult[ selectedGroups.ElementAt<Group>(selectedGroups.IndexOf( Group.getFromID(current.Id_group)))];
                                currResult.positiveAnsw++;
                                Random r = new Random();
                                indexOfCurrentWord = r.Next(0, words.Count);
                                foreginWord.Text = selectedType == 0 ? words.ElementAt<Word>(indexOfCurrentWord).Basic : words.ElementAt<Word>(indexOfCurrentWord).Foregin;
                                lastForeign.Text = lastWord.Basic;
                                lastBasic.Text = lastWord.Foregin;
                                lastForeign.Visibility = Visibility.Visible;
                                lastBasic.Visibility = Visibility.Visible;
                                label6.Visibility = Visibility.Visible;
                                label7.Visibility = Visibility.Visible;
                                words.Remove(current);
                            }
                            else
                            {

                                SqlAccess sql = new SqlAccess();
                                sql.UpdateStats(groupsWithResult, DateTime.Now.Subtract(ts));
                                this.Close();
                                ShowResult();
                            }
                        }
                        else
                        {
                            bad.Add(words.ElementAt<Word>(indexOfCurrentWord));
                            words.Remove(current);
                            if (words.Count > 0)
                            {
                                Result currResult = groupsWithResult[selectedGroups.ElementAt<Group>(selectedGroups.IndexOf(Group.getFromID(current.Id_group)))];
                                currResult.negativeAnsw++;
                                progressBar1.Value++;
                                badAnsv.Content = (int)badAnsv.Content + 1;
                                Random r = new Random();
                                indexOfCurrentWord = r.Next(0, words.Count);

                                foreginWord.Text = selectedType == 0 ? words.ElementAt<Word>(indexOfCurrentWord).Basic : words.ElementAt<Word>(indexOfCurrentWord).Foregin;
                                lastForeign.Text = lastWord.Basic;
                                lastBasic.Text = lastWord.Foregin;
                                lastForeign.Visibility = Visibility.Visible;
                                lastBasic.Visibility = Visibility.Visible;
                                label6.Visibility = Visibility.Visible;
                                label7.Visibility = Visibility.Visible;
                            }
                            else
                            {

                                SqlAccess sql = new SqlAccess();
                                sql.UpdateStats(groupsWithResult, DateTime.Now.Subtract(ts));
                                this.Close();
                                ShowResult();
                            }
                        }
                        basicLanguage.Text = "";
                    }
                    else
                    {


                        MessageBox.Show("Wybrana grupa nie ma slowek.");

                    }
                }
                else if (this.type == 3)
                {
                    if (words.Count != 0)
                    {
                        Word current = words.ElementAt<Word>(indexOfCurrentWord);
                        if ((basicLanguage.Text.Equals(current.Foregin, StringComparison.OrdinalIgnoreCase) && selectedType == 0) || (basicLanguage.Text.Equals(current.Basic, StringComparison.OrdinalIgnoreCase) && selectedType == 1))
                        {
                            words.Remove(current);
                            lastWord = current;
                            if (words.Count > 0)
                            {
                                progressBar1.Value++;
                                goodAnsv.Content = (int)goodAnsv.Content + 1;

                                Random r = new Random();
                                indexOfCurrentWord = r.Next(0, words.Count);
                                foreginWord.Text = selectedType == 0 ? words.ElementAt<Word>(indexOfCurrentWord).Basic : words.ElementAt<Word>(indexOfCurrentWord).Foregin;
                                lastForeign.Text = lastWord.Basic;
                                lastBasic.Text = lastWord.Foregin;
                                lastForeign.Visibility = Visibility.Visible;
                                lastBasic.Visibility = Visibility.Visible;
                                label6.Visibility = Visibility.Visible;
                                label7.Visibility = Visibility.Visible;
                            }
                            else
                            {

                                //SqlAccess sql = new SqlAccess();
                                //sql.UpdateStats(new Result(selectedGroups, (int)goodAnsv.Content, (int)badAnsv.Content, DateTime.Now.Subtract(ts)));
                                this.Close();
                                ShowResult();
                            }
                        }
                        else
                        {

                            bad.Add(words.ElementAt<Word>(indexOfCurrentWord));
                            badAnsv.Content = (int)badAnsv.Content + 1;
                            Random r = new Random();
                            indexOfCurrentWord = r.Next(0, words.Count);
                            foreginWord.Text = selectedType == 0 ? words.ElementAt<Word>(indexOfCurrentWord).Basic : words.ElementAt<Word>(indexOfCurrentWord).Foregin;
                            lastForeign.Text = lastWord.Basic;
                            lastBasic.Text = lastWord.Foregin;
                            lastForeign.Visibility = Visibility.Visible;
                            lastBasic.Visibility = Visibility.Visible;
                            label6.Visibility = Visibility.Visible;
                            label7.Visibility = Visibility.Visible;
                        }
                        basicLanguage.Text = "";
                    }
                    else
                        MessageBox.Show("Wybrana grupa nie ma dodanych slow.");

                }
            }

        }


        void basicLanguage_KeyDown(object sender, KeyEventArgs e)
        {//event for single group
            
            if (e.Key == Key.Enter)
            {

                if (this.type == 1)
                {
                    if (words.Count != 0)
                    {
                        Word current = words.ElementAt<Word>(indexOfCurrentWord);
                        lastWord = current;
                        if ((basicLanguage.Text.Equals(current.Foregin, StringComparison.OrdinalIgnoreCase) && selectedType == 0) || (basicLanguage.Text.Equals(current.Basic, StringComparison.OrdinalIgnoreCase) && selectedType == 1))
                        {
                            words.Remove(current);
                            
                            if (words.Count > 0)
                            {
                                progressBar1.Value++;
                                goodAnsv.Content = (int)goodAnsv.Content + 1;

                                Random r = new Random();
                                indexOfCurrentWord = r.Next(0, words.Count);
                                foreginWord.Text = selectedType == 0 ? words.ElementAt<Word>(indexOfCurrentWord).Basic : words.ElementAt<Word>(indexOfCurrentWord).Foregin;
                                lastForeign.Text = lastWord.Basic;
                                lastBasic.Text = lastWord.Foregin;
                                lastForeign.Visibility = Visibility.Visible;
                                lastBasic.Visibility = Visibility.Visible;
                                label6.Visibility = Visibility.Visible;
                                label7.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                //SqlAccess sql = new SqlAccess();
                                //sql.UpdateStats(new Result(selectedGroups, (int)goodAnsv.Content, (int)badAnsv.Content, DateTime.Now.Subtract(ts)));
                                this.Close();
                                ShowResult();
                            }
                        }
                        else
                        {

                            bad.Add(words.ElementAt<Word>(indexOfCurrentWord));

                            badAnsv.Content = (int)badAnsv.Content + 1;
                            Random r = new Random();
                            indexOfCurrentWord = r.Next(0, words.Count);
                            foreginWord.Text = selectedType == 0 ? words.ElementAt<Word>(indexOfCurrentWord).Basic : words.ElementAt<Word>(indexOfCurrentWord).Foregin;
                            lastForeign.Text = lastWord.Basic;
                            lastBasic.Text = lastWord.Foregin;
                            lastForeign.Visibility = Visibility.Visible;
                            lastBasic.Visibility = Visibility.Visible;
                            label6.Visibility = Visibility.Visible;
                            label7.Visibility = Visibility.Visible;
                        }
                        basicLanguage.Text = "";
                    }
                    else
                    {

                       MessageBox.Show("Wybrana grupa nie ma slowek.");

                    }
                }
                else if (this.type == 2)
                {


                    if (words.Count != 0)
                    {
                        Word current = words.ElementAt<Word>(indexOfCurrentWord);
                        lastWord = current;
                        if ((basicLanguage.Text.Equals(current.Foregin, StringComparison.OrdinalIgnoreCase) && selectedType == 0) || (basicLanguage.Text.Equals(current.Basic, StringComparison.OrdinalIgnoreCase) && selectedType == 1))
                        {
                            words.Remove(current);
                            if (words.Count > 0)
                            {
                                progressBar1.Value++;
                                goodAnsv.Content = (int)goodAnsv.Content + 1;


                                Random r = new Random();
                                indexOfCurrentWord = r.Next(0, words.Count);
                                foreginWord.Text = selectedType == 0 ? words.ElementAt<Word>(indexOfCurrentWord).Basic : words.ElementAt<Word>(indexOfCurrentWord).Foregin;
                                words.Remove(current);
                                lastForeign.Text = lastWord.Basic;
                                lastBasic.Text = lastWord.Foregin;
                                lastForeign.Visibility = Visibility.Visible;
                                lastBasic.Visibility = Visibility.Visible;
                                label6.Visibility = Visibility.Visible;
                                label7.Visibility = Visibility.Visible;
                            }
                            else {
                       
                                SqlAccess sql = new SqlAccess();
                                sql.UpdateStats(new Result(selectedGroups, (int)goodAnsv.Content, (int)badAnsv.Content, DateTime.Now.Subtract(ts)));
                                this.Close();
                                ShowResult();
                            }
                        }
                        else
                        {
                            bad.Add(words.ElementAt<Word>(indexOfCurrentWord));
                            words.Remove(current);
                            if (words.Count > 0)
                            {
                                progressBar1.Value++;
                                badAnsv.Content = (int)badAnsv.Content + 1;
                                Random r = new Random();
                                indexOfCurrentWord = r.Next(0, words.Count);

                                foreginWord.Text = selectedType == 0 ? words.ElementAt<Word>(indexOfCurrentWord).Basic : words.ElementAt<Word>(indexOfCurrentWord).Foregin;
                                lastForeign.Text = lastWord.Basic;
                                lastBasic.Text = lastWord.Foregin;
                                lastForeign.Visibility = Visibility.Visible;
                                lastBasic.Visibility = Visibility.Visible;
                                label6.Visibility = Visibility.Visible;
                                label7.Visibility = Visibility.Visible;
                            }
                            else {

                                SqlAccess sql = new SqlAccess();
                                sql.UpdateStats(new Result(selectedGroups, (int)goodAnsv.Content, (int)badAnsv.Content, DateTime.Now.Subtract(ts)));
                                this.Close();
                                ShowResult();
                            }
                        }
                        basicLanguage.Text = "";
                    }
                    else
                    {


                         MessageBox.Show("Wybrana grupa nie ma slowek.");

                    }
                }
                else if (this.type == 3)
                {
                    MessageBox.Show("3");
                    if (words.Count != 0)
                    {
                        Word current = words.ElementAt<Word>(indexOfCurrentWord);
                        lastWord = current;
                        if ((basicLanguage.Text.Equals(current.Foregin, StringComparison.OrdinalIgnoreCase) && selectedType == 0) || (basicLanguage.Text.Equals(current.Basic, StringComparison.OrdinalIgnoreCase) && selectedType == 1))
                        {
                            words.Remove(current);
                            if (words.Count > 0)
                            {
                                progressBar1.Value++;
                                goodAnsv.Content = (int)goodAnsv.Content + 1;

                                Random r = new Random();
                                indexOfCurrentWord = r.Next(0, words.Count);
                                foreginWord.Text = selectedType == 0 ? words.ElementAt<Word>(indexOfCurrentWord).Basic : words.ElementAt<Word>(indexOfCurrentWord).Foregin;
                                lastForeign.Text = lastWord.Basic;
                                lastBasic.Text = lastWord.Foregin;
                                lastForeign.Visibility = Visibility.Visible;
                                lastBasic.Visibility = Visibility.Visible;
                                label6.Visibility = Visibility.Visible;
                                label7.Visibility = Visibility.Visible;
                            }
                            else {
                                
                                
                                //SqlAccess sql = new SqlAccess();
                                //sql.UpdateStats(new Result(selectedGroups, (int)goodAnsv.Content, (int)badAnsv.Content, DateTime.Now.Subtract(ts)));
                                this.Close();
                                ShowResult();
                            }
                        }
                        else
                        {

                            bad.Add(words.ElementAt<Word>(indexOfCurrentWord));
                            badAnsv.Content = (int)badAnsv.Content + 1;
                            Random r = new Random();
                            indexOfCurrentWord = r.Next(0, words.Count);
                            foreginWord.Text = selectedType == 0 ? words.ElementAt<Word>(indexOfCurrentWord).Basic : words.ElementAt<Word>(indexOfCurrentWord).Foregin;
                            lastForeign.Text = lastWord.Basic;
                            lastBasic.Text = lastWord.Foregin;
                            lastForeign.Visibility = Visibility.Visible;
                            lastBasic.Visibility = Visibility.Visible;
                            label6.Visibility = Visibility.Visible;
                            label7.Visibility = Visibility.Visible;
                        }
                        basicLanguage.Text = "";
                    }
                    else
                        MessageBox.Show("Wybrana grupa nie ma dodanych slow.");

                }

            }
        }
    }
}
