﻿using System;
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
using Microsoft.Win32;
using System.ComponentModel;

namespace MultiLanguage
{
    /// <summary>
    /// Interaction logic for addGroup.xaml
    /// </summary>
    public partial class addGroup : Window
    {
        int importedWords = 0;
        int failedWords = 0;
        public addGroup()
        {
            InitializeComponent();
            
            SqlAccess sql = new SqlAccess();
            comboBox1.ItemsSource = sql.GetLanguages();
            comboBox1.SelectedIndex = 0;
            List<Group> temp = sql.GetGroups(false);
            temp.Insert(0,new Group("", 0));
            comboBox2.ItemsSource = temp;
            
            comboBox2.SelectionChanged += new SelectionChangedEventHandler(comboBox2_SelectionChanged);
        }

        void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Group selected = (Group)comboBox2.SelectedItem;
            if (!selected.Name.Equals(""))
            {
                textBox1.IsEnabled = false;

                SqlAccess sql = new SqlAccess();
                Language counterpart = sql.GetLanguageFromGroup(selected.Id);
                int i = 0;
                foreach (Language l in comboBox1.ItemsSource)
                {
                    if (l.Equals(counterpart))
                    {
                        comboBox1.SelectedIndex = i;
                        comboBox1.IsEnabled = false;
                        break;
                    }
                    else
                        i++;

                }
            }
            else
            {
                textBox1.IsEnabled = true;
                comboBox1.IsEnabled = true;
            }

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.DefaultExt = "*.txt";
            op.Filter = "Pliki tekstowe (*.txt)|*.txt";
            op.AddExtension = true;
            op.Multiselect = false;
            op.Title = "Wybierz plik do importu.";
            
            Nullable<bool> result  = op.ShowDialog();
            if (result == true) {

                textBox3.Text = op.FileName;
            
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

            

            
            if (comboBox2.SelectedItem == null)
            {
                if (textBox2.Text.Equals("")) { MessageBox.Show("Podaj delimiter."); return; }
                if (textBox3.Text.Equals("")) { MessageBox.Show("Wybierz plik do wgrania."); return; }
                if (textBox1.Text.Equals("")) { MessageBox.Show("Podaj nazwę dla nowej grupy"); return; }
                button2.IsEnabled = false;
                Language lg = (Language)comboBox1.SelectedItem;
                SqlAccess sql = new SqlAccess();
                long newgroup = sql.InsertGroup(lg, textBox1.Text, textBox2.Text);

                progressBar1.Visibility = Visibility.Visible;//enable the progressbar
                BackgroundWorker bgw = new BackgroundWorker(); //new thread
                string file = textBox3.Text;
                string[] lines = System.IO.File.ReadAllLines(file, System.Text.Encoding.Default);
                List<Word> linesWithoutBlankLines = new List<Word>();
                string delimeter = textBox2.Text;
                foreach (string line in lines)
                {

                    if (line == "") continue;
                    try
                    {
                        string[] words = line.Split(new string[] { delimeter }, StringSplitOptions.None); 
                        linesWithoutBlankLines.Add(new Word(newgroup, words[0], words[1]));
                        importedWords++;
                    }
                    catch (Exception ex)
                    {

                        failedWords++;
                        continue;
                    
                    }
                    

                }

                progressBar1.Minimum = 0;
                progressBar1.Maximum = linesWithoutBlankLines.Count;

                bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
                bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
                bgw.RunWorkerAsync(linesWithoutBlankLines);
            }
            else
            {
                //pre-asserts
                
                if (textBox2.Text.Equals("")) { MessageBox.Show("Podaj delimiter."); return; }
                if (textBox3.Text.Equals("")) { MessageBox.Show("Wybierz plik do wgrania."); return; }

                button2.IsEnabled = false;
                BackgroundWorker bgw = new BackgroundWorker();
                string file = textBox3.Text;
                string[] lines = System.IO.File.ReadAllLines(file, Encoding.Default);
                List<Word> linesWithoutBlankLines = new List<Word>();
                string delimeter = textBox2.Text;
                foreach (string line in lines)
                {
                    if (line == "") continue;
                    try
                    {

                        string[] words = line.Split(new string[] { delimeter }, StringSplitOptions.None);
                        linesWithoutBlankLines.Add(new Word(((Group)comboBox2.SelectedItem).Id, words[0], words[1]));
                        importedWords++;
                    }
                    catch (Exception ex)
                    {
                        failedWords++;
                        continue;
                    
                    }
                }

                progressBar1.Minimum = 0;
                progressBar1.Maximum = linesWithoutBlankLines.Count;
                progressBar1.Visibility = Visibility.Visible;
                bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
                bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
                bgw.RunWorkerAsync(linesWithoutBlankLines);

            }

        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = progressBar1.Maximum;
            SqlAccess sql = new SqlAccess();
            sql.GetGroups(true);
            this.Close();
            MessageBox.Show("Udało się zaimportować "+importedWords+" słówek." +(failedWords>0?" Błedy wystąpiły przy "+failedWords+ " słowach.":""));
            
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Word> lines = (List<Word>)e.Argument;

            SqlAccess sql = new SqlAccess();
            foreach (Word wordsFromFile in lines)
            {
                App.Current.Dispatcher.Invoke((Action)delegate()
                {
                    bool t = sql.InsertWord(wordsFromFile.Id_group, wordsFromFile.Foregin, wordsFromFile.Basic);
                    progressBar1.Value=progressBar1.Value+1;

                });
                
            
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
