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

    string database_name = "URI=file:" + Application.dataPath + "/Resources/leapmotion.db";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void createDB(){
    //     using(var connection = new SqliteConnection(database_name)){
            
    //         connection.Open();

    //     }
    // }

    public void addEntry(){

        using(var connection = new SqliteConnection(database_name)){
            connection.Open();

            using(var command = connection.CreateCommand()){
                command.CommandText = "INSERT INTO Patient (Full_name, Sex) VALUES(@fullName, @sex)";
                command.Parameters.AddWithValue("@fullName", full_name.text);
                command.Parameters.AddWithValue("@sex", sex.text);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

    }
}
