using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using TMPro;

public class DataManagement : MonoBehaviour
{
    // public Text full_name;
    // public Text sex;
    public TextMeshProUGUI full_name;
    public TextMeshProUGUI sex;
    public TextMeshProUGUI birthDate;
    public Toggle togl;
    public TextMeshProUGUI buttonText;


    string database_name = "URI=file:" + Application.dataPath + "/Resources/leapmotion.db";
    
    void Start()
    {
    }

    void Update()
    {
        
    }

    // public void createDB(){
    //     using(var connection = new SqliteConnection(database_name)){
            
    //         connection.Open();

    //     }
    // }

    public void addEntry(){
            
            string gender = togl.isOn ? "Male" : "Female";

            Debug.Log(gender);

        using(var connection = new SqliteConnection(database_name)){
            connection.Open();

            using(var command = connection.CreateCommand()){
                command.CommandText = "INSERT INTO Patient (Full_name, birth_date, sex) VALUES(@fullName, @birthDate, @gender)";
                command.Parameters.AddWithValue("@fullName", full_name.text);
                command.Parameters.AddWithValue("@birthDate", birthDate.text);
                command.Parameters.AddWithValue("@gender", gender);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

    }

    public void addResultsTMT(){
            
        using(var connection = new SqliteConnection(database_name)){
            connection.Open();

            using(var command = connection.CreateCommand()){
                command.CommandText = "INSERT INTO Patient (tmt_result) VALUES(@score_tmt)";
                command.Parameters.AddWithValue("@fullName", full_name.text);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

    }   
}