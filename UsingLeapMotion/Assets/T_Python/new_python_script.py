import UnityEngine
import cv2
import numpy as np
import matplotlib.pyplot as plt
import tensorflow as tf
from tensorflow.keras.models import load_model

model = load_model('model_simple_save.h5')



def process_triangle(img, number):
    x1, y1 = 690, 40
    x3, y3 = 1690, 1040
    x2, y2 = int(x1 + (x3 - x1) / 2), int(y1 + (y3 - y1) / 2)

    triangles = [
        [[x2+134, y1], [x2, y2], [x3, y1]],
        [[x3, y1], [x2, y2], [x3, y2-134]],
        [[x3, y2+134], [x2, y2], [x3, y3]],
        [[x3, y3], [x2, y2], [x2+134, y3]],
        [[x2-134, y3], [x2, y2], [x1, y3]],
        [[x1, y3], [x2, y2], [x1, y2+134]],
        [[x1, y2-134], [x2, y2], [x1, y1]],
        [[x1, y1], [x2, y2], [x2-134, y1]]
    ]
    
    cropped_regions = [
        [y1+15, y1+15+270, x2+80, x2+80+270],
        [y2-80-270, y2-80, x3-15-270, x3-15],
        [y2+80, y2+80+270, x3-15-270, x3-15],
        [y3-15-270, y3-15, x2+80, x2+80+270],
        [y3-15-270, y3-15, x2-80-270, x2-80],
        [y2+80, y2+80+270, x1+15, x1+15+270],
        [y2-80-270, y2-80, x1+15, x1+15+270],
        [y1+15, y1+15+270, x2-80-270, x2-80]
    ]
    
    img_background = 255 * np.ones(img.shape, dtype=img.dtype)
    img_background = 255 - img_background

    r = cv2.boundingRect(np.array(triangles[number], dtype=np.float32))

    triangle_cropped = []
    for j in range(3):
        triangle_cropped.append(((triangles[number][j][0] - r[0]), (triangles[number][j][1] - r[1])))
    
    print(f"Triangle cropped coordinates: {triangle_cropped}")
    
    img_cropped = img[r[1]:r[1] + r[3], r[0]:r[0] + r[2]]
    
    warp_mat = cv2.getAffineTransform(np.float32(triangle_cropped), np.float32(triangle_cropped))
    
    img_background_cropped = cv2.warpAffine(img_cropped, warp_mat, (r[2], r[3]), None, flags=cv2.INTER_LINEAR, borderMode=cv2.BORDER_REFLECT_101)

    mask = np.zeros((r[3], r[2], 3), dtype=np.float32)
    cv2.fillConvexPoly(mask, np.int32(triangle_cropped), (1.0, 1.0, 1.0), 16, 0)
    
    img_background_cropped = img_background_cropped * mask

    img_background[r[1]:r[1] + r[3], r[0]:r[0] + r[2]] = img_background[r[1]:r[1] + r[3], r[0]:r[0] + r[2]] * (1 - mask)
    img_background[r[1]:r[1] + r[3], r[0]:r[0] + r[2]] = img_background[r[1]:r[1] + r[3], r[0]:r[0] + r[2]] + img_background_cropped
    
    img_rgb = cv2.cvtColor(img_background, cv2.COLOR_BGR2RGB)
    x1 = cropped_regions[number][0]
    x2 = cropped_regions[number][1] 
    y1 = cropped_regions[number][2]
    y2 = cropped_regions[number][3]
    sliced_img = img_rgb[x1:x2, y1:y2]
    resized_img_64 = cv2.resize(sliced_img, (64, 64))
    
    return resized_img_64



def predict_processed_image(img_tri):
    img_array = np.expand_dims(img_tri, axis=0)
    result = model.predict(img_array)

    class_indices = {'0': 0, '1': 1, '2': 2, '3': 3, '4': 4, '5': 5, '6': 6, '7': 7, '8': 8, '9': 9}
    ind_to_class = {v: k for k, v in class_indices.items()}
    predicted_class = ind_to_class[result[0].argmax()]

    return predicted_class



def img_tri_process_triangle (img, number):
    if number < 0 or number > 7:
        raise ValueError("Number must be in the range 0-7")
        
    img_tri = process_triangle(img, number)
    return img_tri



def count_predicted_classes(predicted_classes):
    count = 0
    if predicted_classes[0] == '1':
        count += 1
    if predicted_classes[1] == '2':
        count += 1
    if predicted_classes[2] == '4':
        count += 1
    if predicted_classes[3] == '5':
        count += 1
    if predicted_classes[4] == '7':
        count += 1
    if predicted_classes[5] == '8':
        count += 1
    if predicted_classes[6] == '1' or predicted_classes[6] == '0':
        count += 1
    if predicted_classes[7] == '1':
        count += 1
    return count



img = cv2.imread("screenshot-2024-05-17-08-31-30.png")
img = 255 - img
predicted_classes = []
for number in range(8):
    predicted_class = predict_processed_image(img_tri_process_triangle(img, number))
    predicted_classes.append(predicted_class)
    print("For number:", number, "Predicted class:", predicted_class)
    
print("Predicted classes array:", predicted_classes)
true_count = count_predicted_classes(predicted_classes)
print("Number of true conditions:", true_count)


UnityEngine.Debug.Log(true_count)