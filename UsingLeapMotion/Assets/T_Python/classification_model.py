import UnityEngine
import os
import pandas as pd
import joblib

def model_load(model_name):
   model = joblib.load(f'Assets/Models/{model_name}.pkl')

   return model

def get_inputs(file_path):
    with open(file_path, "r") as file:
        inputs = [float(line.strip().replace(',', '.')) for line in file.readlines()]
    return inputs

def make_prediction(model_name, params):
    
    model = model_load(model_name)

    params_df = pd.DataFrame([params])

    prediction = model.predict(params_df)

    return prediction

file_path = os.path.join("Assets","T_Python", "numberInput.txt")

inputs = get_inputs(file_path)

for i in inputs:
    UnityEngine.Debug.Log(f"Param {i}")

params_df = pd.DataFrame(inputs)

for i in params_df:
    UnityEngine.Debug.Log(i)


prediction_0 = make_prediction("SVM_diploma", inputs)
prediction_1 = make_prediction("LR_diploma", inputs)

final_prediction = (prediction_0+prediction_1)/2

UnityEngine.Debug.Log(final_prediction)

file_path = os.path.join("Assets", "T_Python", "status_prediction.txt")

# Write true_count to the file
with open(file_path, "w") as file:
    file.write(str(float(final_prediction[0])))