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

    public void addResults(){
        
        float tmt_res = DataTransfer.score_tmt_a+DataTransfer.score_tmt_b;
        float tmt_res_time = DataTransfer.time_tmt_a + DataTransfer.time_tmt_b;

        
            
        using(var connection = new SqliteConnection(database_name)){
            connection.Open();

            int lastId = 0;

        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT MAX(id) FROM Patient";
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    lastId = reader.GetInt32(0);

                    Debug.Log(lastId);
                }
            }
        }

            using (var command = connection.CreateCommand()) {
             command.CommandText = "UPDATE Patient SET tmt_result = @tmt_res, cdt_result = @cdt_res, bell_result = @bell_res, line_result = @line_res," +
                                  "tmt_time = @tmt_res_time, cdt_time = @cdt_time, bell_time = @bell_time, line_time = @line_time, hand_trail = @hando_trail WHERE id = @lastId" ;
            command.Parameters.AddWithValue("@tmt_res", tmt_res);
            command.Parameters.AddWithValue("@cdt_res", DataTransfer.score_cdt);
            command.Parameters.AddWithValue("@bell_res", DataTransfer.score_bell);
            command.Parameters.AddWithValue("@line_res", DataTransfer.score_line);
            command.Parameters.AddWithValue("@tmt_res_time", tmt_res_time);
            command.Parameters.AddWithValue("@cdt_time", DataTransfer.time_cdt);
            command.Parameters.AddWithValue("@bell_time", DataTransfer.time_bell);
            command.Parameters.AddWithValue("@line_time", DataTransfer.time_line);
            command.Parameters.AddWithValue("@hando_trail", DataTransfer.handoTrail);
            command.Parameters.AddWithValue("@lastId", lastId);
            
            command.ExecuteNonQuery();
        }
            connection.Close();
        }

    }   
}
