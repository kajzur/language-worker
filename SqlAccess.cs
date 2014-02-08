using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;

namespace MultiLanguage
{
    class SqlAccess
    {
        SQLiteConnection con;
        SQLiteCommand cmd;
        private string DataBaseFile = AppDomain.CurrentDomain.BaseDirectory+"//datebase.s3db";
        private void init() {
            con = new SQLiteConnection("Data Source = " + DataBaseFile + ";Version=3;UseUTF8Encoding=True;");
            cmd = new SQLiteCommand();
           
            con.Open();
            cmd.Connection = con;
        }
        public SqlAccess()
        {
            con = new SQLiteConnection();
            cmd = new SQLiteCommand();
            con.ConnectionString = "Data Source = " + DataBaseFile + ";Version=3;UseUTF8Encoding=True;";
            con.Open();
            cmd.Connection = con;
            
        }
        public Language GetLanguageFromGroup(int g)
        {
            init();
            cmd.CommandText = "select * from languages l join groups g on g.ID_Language=l.ID where g.ID=@g_id ;";
            cmd.Parameters.Add(new SQLiteParameter("@g_id", g));
            SQLiteDataReader reader=  cmd.ExecuteReader();
            reader.Read();
            Language temp = new Language(reader.GetInt32(0), reader.GetString(1));
            reader.Close();
            con.Close();
            return temp;
        
        }
        public List<Group> GetGroups(bool dynamic)
        {
            init();
            if (dynamic)
            {
                MainWindow.groups.Clear();
                cmd.CommandText = "select * from groups;";
                SQLiteDataReader sqldata = cmd.ExecuteReader();
                // ObservableCollection<Group> groups = new ObservableCollection<Group>();
                while (sqldata.Read())
                {

                    MainWindow.groups.Add(new Group(sqldata.GetString(1), sqldata.GetInt32(0)));

                }
                sqldata.Close();
                con.Close();
                return null;
            }
            else {

                
                cmd.CommandText = "select * from groups;";
                SQLiteDataReader sqldata = cmd.ExecuteReader();
                List<Group> groups = new List<Group>();
                while (sqldata.Read())
                {

                    groups.Add(new Group(sqldata.GetString(1), sqldata.GetInt32(0)));

                }
                sqldata.Close();
                con.Close();
                return groups;
            
            }
            
        }

        public void DeleteWordsFromGroup(int id)
        {
            init();
            cmd.CommandText = "delete from words where ID_Groups=@g_id;";
            cmd.Parameters.Add(new SQLiteParameter("@g_id", id));
            cmd.ExecuteNonQuery();
            con.Close();

        }

        public List<Language> GetLanguages() {
            init();
            cmd.CommandText = "select * from languages;";
            SQLiteDataReader sqldata = cmd.ExecuteReader();
            List<Language> groups = new List<Language>();
            while (sqldata.Read())
            {

                groups.Add(new Language(sqldata.GetInt32(0),sqldata.GetString(1)));

            }
            sqldata.Close();
            con.Close();
            return groups;
        
        }

        public long InsertGroup(Language lg, string g, string delimeter) {
            init();
            string sqll = "CREATE TABLE IF NOT EXISTS [groups] ([ID] INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,[Name] teXT  UNIQUE NOT NULL,[ID_Language] INTEGER  NULL,[Add_date] DATE DEFAULT CURRENT_DATE NOT NULL,[Delimeter] TEXT  NOT NULL)";
            cmd.CommandText = sqll;
            cmd.ExecuteNonQuery();
            
            cmd.CommandText = "insert into groups (Name, ID_Language, Delimeter) values (@gname,@lgid,@delimiter);";
            cmd.Parameters.Add(new SQLiteParameter("@gname", g));
            cmd.Parameters.Add(new SQLiteParameter("@lgid", lg.ID));
            cmd.Parameters.Add(new SQLiteParameter("@delimiter", delimeter));
            
            int sqldata = cmd.ExecuteNonQuery();

            cmd.CommandText = "select max(ID) from groups;";
            long newid = (long)cmd.ExecuteScalar();
            con.Close();
            return newid;
        }

        public bool InsertWord(long g_id,string foregin, string basic)
        {
            init();
            string ww = "CREATE TABLE IF NOT EXISTS [words] ([ID] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,[ID_groups] INTEGER  NOT NULL,[Foregin_Lan_Word] TEXT  NOT NULL,[Base_Lan_word] TEXT  NOT NULL)";
            cmd.CommandText = ww;
            cmd.ExecuteNonQuery();
            SQLiteTransaction tran = con.BeginTransaction();
            cmd.CommandText = "insert into words values(null,@gid, @basic, @foregin)";
            cmd.Parameters.Add(new SQLiteParameter("@gid", g_id));
            cmd.Parameters.Add(new SQLiteParameter("@foregin", ToUTF8(foregin)));
            cmd.Parameters.Add(new SQLiteParameter("@basic", ToUTF8(basic)));
            int sqldata = cmd.ExecuteNonQuery();
            tran.Commit();
            
            con.Close();
            return sqldata>0?true:false;
        }


        internal void DeleteGroup(int p)
        {
            init();
            cmd.CommandText = "delete from groups where ID=@g_id;";
            cmd.Parameters.Add(new SQLiteParameter("@g_id", p));
            cmd.ExecuteNonQuery();
            con.Close();
            this.DeleteWordsFromGroup(p);

        }
        private String ToUTF8(string input) {

            //return Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(input)));
            return input;
        }

        private String FromUTF8(string input)
        {
            //return Encoding.Default.GetString(Encoding.Convert(Encoding.UTF8, Encoding.Default, Encoding.UTF8.GetBytes(input)));
            return input;
        }


        public List<Word> getWordsFromGroups(List<Group> list)
        {
            init();
            String groups = "";
            foreach (Group p in list)
                groups += p.Id + ",";

            List<Word> words = new List<Word>();
            cmd.CommandText = "Select * from words where ID_Groups in ("+groups.Remove(groups.Length-1)+");";
            SQLiteDataReader data = cmd.ExecuteReader();
            while (data.Read())
            {

                words.Add(new Word(data.GetInt64(1),FromUTF8( data.GetString(2)), FromUTF8( data.GetString(3))));

            }
            data.Close();
            con.Close();
            return words;

        }


        public void UpdateStats(Result result) {


            init();
            if(result.finishedGroups.Count==1){
                    
                    cmd.CommandText = "insert into stats(ID_group, PositiveAnsw,NegativeAnsw, Duration, Data) values (@id_g, @pos, @neg, @dur,datetime('now'))";
                    cmd.Parameters.Add(new SQLiteParameter("@id_g", result.finishedGroups.ElementAt<Group>(0).Id));
                    cmd.Parameters.Add(new SQLiteParameter("@pos", result.positiveAnsw));
                    cmd.Parameters.Add(new SQLiteParameter("@neg", result.negativeAnsw));
                    cmd.Parameters.Add(new SQLiteParameter("@dur", result.duration.TotalSeconds));
                    cmd.ExecuteNonQuery();
                    con.Close();
                
            }
        
        }

        internal List<double> GetStatsForGroup(Group g)
        {
            init();
            List<double> list = new List<double>();

            cmd.CommandText = "select ((PositiveAnsw*1.0)/(PositiveAnsw+NegativeAnsw))*100 from stats where ID_group = @id_g group by ID order by Data asc ";
            cmd.Parameters.Add(new SQLiteParameter("@id_g", g.Id));
            SQLiteDataReader data = cmd.ExecuteReader();
            while (data.Read()) {

                list.Add((double)data.GetDecimal(0));
            
            }
            data.Close();
            return list;

        }



        internal long GetAttendsForGroup(Group g)
        {
            init();

            cmd.CommandText = "select Attends from attends where ID_group=@id_g";
            cmd.Parameters.Add(new SQLiteParameter("@id_g", g.Id));

            return (long)cmd.ExecuteScalar();
        }

        internal TimeSpan GetAvgOfDurationForGroup(Group g)
        {
            init();

            cmd.CommandText = "select avg(Duration) from stats where ID_group=@id_g";
            cmd.Parameters.Add(new SQLiteParameter("@id_g", g.Id));
            TimeSpan ts = TimeSpan.FromSeconds((double)cmd.ExecuteScalar());
            return ts;
        }
    }
}
