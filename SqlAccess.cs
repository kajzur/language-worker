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

        public SqlAccess()
        {
            con = new SQLiteConnection();
            cmd = new SQLiteCommand();
            con.ConnectionString = "Data Source = " + DataBaseFile + ";Version=3;UseUTF8Encoding=True;";
            con.Open();
            cmd.Connection = con;
            
        }
        public Language GetLanguageFromGroup(long g)
        {

            cmd.CommandText = "select * from languages l join groups g on g.ID_Language=l.ID where g.ID=@g_id ;";
            cmd.Parameters.Add(new SQLiteParameter("@g_id", g));
            SQLiteDataReader reader=  cmd.ExecuteReader();
            reader.Read();
            Language temp = new Language(reader.GetInt32(0), reader.GetString(1));
            reader.Close();

            return temp;
        
        }
        public List<Group> GetGroups(bool dynamic)
        {

            if (dynamic)
            {
                MainWindow.groups.Clear();
                cmd.CommandText = "select * from groups;";
                SQLiteDataReader sqldata = cmd.ExecuteReader();

                while (sqldata.Read())
                {

                    MainWindow.groups.Add(new Group(sqldata.GetString(1), sqldata.GetInt32(0)));

                }
                sqldata.Close();

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

                return groups;
            
            }
            
        }

        public void DeleteWordsFromGroup(long id)
        {

            cmd.CommandText = "delete from words where ID_Groups=@g_id;";
            cmd.Parameters.Add(new SQLiteParameter("@g_id", id));
            cmd.ExecuteNonQuery();


        }

        public List<Language> GetLanguages() {

            cmd.CommandText = "select * from languages;";
            SQLiteDataReader sqldata = cmd.ExecuteReader();
            List<Language> groups = new List<Language>();
            while (sqldata.Read())
            {

                groups.Add(new Language(sqldata.GetInt32(0),sqldata.GetString(1)));

            }
            sqldata.Close();

            return groups;
        
        }

        public long InsertGroup(Language lg, string g, string delimeter) {

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
            return newid;
        }

        public bool InsertWord(long g_id,string foregin, string basic)
        {

            string ww = "CREATE TABLE IF NOT EXISTS [words] ([ID] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,[ID_groups] INTEGER  NOT NULL,[Foregin_Lan_Word] TEXT  NOT NULL,[Base_Lan_word] TEXT  NOT NULL)";
            cmd.CommandText = ww;
            cmd.ExecuteNonQuery();
            SQLiteTransaction tran = con.BeginTransaction();
            cmd.CommandText = "insert into words values(null,@gid, @basic, @foregin)";
            cmd.Parameters.Add(new SQLiteParameter("@gid", g_id));
            cmd.Parameters.Add(new SQLiteParameter("@foregin", (foregin)));
            cmd.Parameters.Add(new SQLiteParameter("@basic", (basic)));
            int sqldata = cmd.ExecuteNonQuery();
            tran.Commit();
            return sqldata>0?true:false;
        }


        internal void DeleteGroup(long p)
        {

            cmd.CommandText = "delete from groups where ID=@g_id;";
            cmd.Parameters.Add(new SQLiteParameter("@g_id", p));
            cmd.ExecuteNonQuery();

        }


        public List<Word> getWordsFromGroups(List<Group> list)
        {

            String groups = "";

            foreach (Group p in list)
                groups += p.Id + ",";

            List<Word> words = new List<Word>();
            string finals = groups.Remove(groups.Length - 1);
            cmd.CommandText = "Select * from words where ID_groups in ("+finals+");";
            //cmd.Parameters.Add(new SQLiteParameter("@in_parameter", DbType.Int32,finals));
            SQLiteDataReader data = cmd.ExecuteReader();
            while (data.Read())
            {

                words.Add(new Word(data.GetInt64(1),( data.GetString(2)), ( data.GetString(3))));

            }
            data.Close();

            return words;

        }


        public void UpdateStats(Result result) {

            if(result.finishedGroups.Count==1){
                    
                    cmd.CommandText = "insert into stats(ID_group, PositiveAnsw,NegativeAnsw, Duration, Data) values (@id_g, @pos, @neg, @dur,datetime('now'))";
                    cmd.Parameters.Add(new SQLiteParameter("@id_g", result.finishedGroups.ElementAt<Group>(0).Id));
                    cmd.Parameters.Add(new SQLiteParameter("@pos", result.positiveAnsw));
                    cmd.Parameters.Add(new SQLiteParameter("@neg", result.negativeAnsw));
                    cmd.Parameters.Add(new SQLiteParameter("@dur", result.duration.TotalSeconds));
                    cmd.ExecuteNonQuery();

            }
        
        }

        internal List<double> GetStatsForGroup(Group g)
        {

            List<double> list = new List<double>();

            cmd.CommandText = "select round(((PositiveAnsw*1.0)/(PositiveAnsw+NegativeAnsw))*100) from stats where ID_group = @id_g group by ID order by Data asc ";
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
            cmd.CommandText = "select Attends from attends where ID_group=@id_g";
            cmd.Parameters.Add(new SQLiteParameter("@id_g", g.Id));

            return (long)cmd.ExecuteScalar();
        }

        internal TimeSpan GetAvgOfDurationForGroup(Group g)
        {
            cmd.CommandText = "select avg(Duration) from stats where ID_group=@id_g";
            cmd.Parameters.Add(new SQLiteParameter("@id_g", g.Id));
            object result = cmd.ExecuteScalar();
            try
            {

                return TimeSpan.FromSeconds((double)result);

            }
            catch (Exception x)
            { 
            
                return TimeSpan.FromSeconds(0);
            
            }


        }

        internal void UpdateStats(Dictionary<Group, Result> groupsWithResult, TimeSpan ts)
        {

            foreach (KeyValuePair<Group,Result> row in groupsWithResult)
            {

                cmd.CommandText = "insert into stats(ID_group, PositiveAnsw,NegativeAnsw, Duration, Data) values (@id_g, @pos, @neg, @dur,datetime('now'))";
                cmd.Parameters.Add(new SQLiteParameter("@id_g", row.Key.Id));
                cmd.Parameters.Add(new SQLiteParameter("@pos", row.Value.positiveAnsw));
                cmd.Parameters.Add(new SQLiteParameter("@neg", row.Value.negativeAnsw));
                cmd.Parameters.Add(new SQLiteParameter("@dur", ts.TotalSeconds));
                cmd.ExecuteNonQuery();

            }
        }
    }
}
