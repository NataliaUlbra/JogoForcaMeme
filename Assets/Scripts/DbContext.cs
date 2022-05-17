using Assets.Scripts.Model;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class DbContext : MonoBehaviour
{
    public static DbContext Instance { get; protected set; }
    public string conn, sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    private IDataReader reader;
    string DatabaseName = "LocalDatabase4.s3db";
    public string filepath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        filepath = Application.persistentDataPath + "/" + DatabaseName;
        DbContext.Instance.conn = $"URI=file:{filepath}";
        InitDatabase();
    }
    /// <summary>
    /// Criar banco de dados
    /// </summary>
    public void InitDatabase()
    {
        string filepath = Application.persistentDataPath + "/" + DatabaseName;
        try
        {
            Debug.Log($"SQLite starting...: {filepath}");
            if (!File.Exists(filepath))
            {
                Debug.LogWarning("File \"" + filepath + "\" does not exist. Attempting to create from \"" + Application.dataPath + "!/assets/LocalDatabase4");
                // UNITY_ANDROID
                var loadDB = new WWW($"jar:file://{Application.dataPath}!/assets/LocalDatabase4.s3db");
                while (!loadDB.isDone) { }
                File.WriteAllBytes(filepath, loadDB.bytes);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        DbContext.Instance.conn = $"URI=file:{filepath}";
        dbconn = new SqliteConnection(DbContext.Instance.conn);
        dbconn.Open();
        try
        {
            IDataReader reader;
            //TODO: Futuramente MAC, Prize, Extraball
            var queryDDL1 = "CREATE TABLE IF NOT EXISTS Category(RowId INTEGER PRIMARY KEY ASC,CategoryValue TEXT UNIQUE)";
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = queryDDL1;
            reader = dbcmd.ExecuteReader();

            var queryDDL2 = "CREATE TABLE IF NOT EXISTS Words(RowId INTEGER PRIMARY KEY ASC,WordsValue TEXT UNIQUE,  CategoryId INTEGER,FOREIGN KEY(CategoryId) REFERENCES Category(RowId))";
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = queryDDL2;
            reader = dbcmd.ExecuteReader();

            var queryDDL3 = "CREATE TABLE IF NOT EXISTS Tips(RowId INTEGER PRIMARY KEY ASC,TipsValue TEXT,WordsId INTEGER,FOREIGN KEY(WordsId) REFERENCES Words(RowId))";
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = queryDDL3;
            reader = dbcmd.ExecuteReader();

            var queryDDL4 = "CREATE TABLE IF NOT EXISTS Leaderboard(RowId INTEGER PRIMARY KEY ASC,UserName TEXT,Score INTEGER)";
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = queryDDL4;
            reader = dbcmd.ExecuteReader();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        dbconn.Close();
    }
    #region Insert
    /// <summary>
    /// Inserir categoria 
    /// </summary>
    public void InsertCategory(CategoryViewModel model)
    {
        try
        {
            using (dbconn = new SqliteConnection(DbContext.Instance.conn))
            {
                dbconn.Open();
                dbcmd = dbconn.CreateCommand();
                sqlQuery = string.Format($"INSERT INTO Category (CategoryValue) VALUES(\"{model.CategoryValue}\")");
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteScalar();
                dbconn.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    /// <summary>
    /// Inserir dicas 
    /// </summary>
    public void InsertTips(TipsViewModel model)
    {
        try
        {
            using (dbconn = new SqliteConnection(DbContext.Instance.conn))
            {
                dbconn.Open();
                dbcmd = dbconn.CreateCommand();
                sqlQuery = string.Format($"INSERT INTO Tips (TipsValue,WordsId)VALUES(\"{model.TipsValue}\", {model.WordsId})");
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteScalar();
                dbconn.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    /// <summary>
    /// Inserir rank 
    /// </summary>
    public void InsertLeaderboard(LeaderboardViewModel model)
    {
        try
        {
            using (dbconn = new SqliteConnection(DbContext.Instance.conn))
            {
                dbconn.Open();
                dbcmd = dbconn.CreateCommand();
                sqlQuery = string.Format($"INSERT INTO Leaderboard (UserName,Score)VALUES(\"{model.UserName}\", {model.Score})");
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteScalar();
                dbconn.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    /// <summary>
    /// Inserir palavras 
    /// </summary>
    public void InsertWords(WordsViewModel model)
    {
        try
        {
            using (dbconn = new SqliteConnection(DbContext.Instance.conn))
            {
                dbconn.Open();
                dbcmd = dbconn.CreateCommand();
                sqlQuery = string.Format($"INSERT INTO Words (WordsValue,CategoryId)VALUES(\"{model.WordsValue}\", {model.CategoryId})");
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteScalar();
                dbconn.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    #endregion
    #region Get
    /// <summary>
    /// Consultar categoria 
    /// </summary>
    public List<CategoryViewModel> GetCategory()
    {
        var categories = new List<CategoryViewModel>();

        using (dbconn = new SqliteConnection(DbContext.Instance.conn))
        {
            dbconn.Open();
            dbcmd = dbconn.CreateCommand();
            sqlQuery = string.Format($"SELECT * FROM Category;");
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    categories.Add(new CategoryViewModel(Convert.ToInt32(reader.GetInt32(0)), reader.GetString(1)));
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;

            dbconn.Close();
            return categories;
        }
    }
    #endregion
}
