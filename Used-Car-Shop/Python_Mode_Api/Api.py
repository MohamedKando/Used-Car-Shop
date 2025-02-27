from flask import Flask, request, jsonify
import joblib
import pandas as pd
import numpy as np

# Load model and encoders
model = joblib.load(r"C:\Users\user\Downloads\xgb_car_price_model.pkl")
label_encoders = joblib.load(r"C:\Users\user\Downloads\label_encoders.pkl")

# Define binary columns
binary_columns = ['Transmission', 'AirConditioner', 'PowerSteering', 'RemoteControl']

# Define categorical columns
categorical_columns = ['Make', 'Model']

# Create Flask app
app = Flask(__name__)


def prepare_sample_data(sample_dict):
    sample_df = pd.DataFrame(sample_dict)

    # Convert binary columns from 'Yes'/'No' to 1/0
    for col in binary_columns:
        sample_df[col] = sample_df[col].map({'Yes': 1, 'No': 0}).astype(int)

    # Encode categorical variables
    for col in categorical_columns:
        if col in label_encoders:
            sample_df[col] = sample_df[col].map(lambda x: label_encoders[col].transform([x])[0]
            if x in label_encoders[col].classes_ else -1)

    return sample_df


@app.route('/predict', methods=['POST'])
def predict():
    try:
        # Get request JSON data
        sample_data = request.get_json()

        # Ensure sample_data is in the correct format
        sample_df = pd.DataFrame([sample_data])  # Wrap in a list to avoid scalar error

        # Convert binary columns
        for col in ['Transmission', 'AirConditioner', 'PowerSteering', 'RemoteControl']:
            sample_df[col] = sample_df[col].map({'Yes': 1, 'No': 0}).astype(int)

        # Encode categorical variables
        for col in ['Make', 'Model']:
            if col in label_encoders:
                sample_df[col] = sample_df[col].map(
                    lambda x: label_encoders[col].transform([x])[0] if x in label_encoders[col].classes_ else -1
                )

        # Ensure correct feature order


        # Predict price
        predicted_log_price = model.predict(sample_df)
        predicted_price = np.expm1(predicted_log_price)

        return jsonify({"predicted_price": float(round(predicted_price[0], 2))})

    except Exception as e:
        return jsonify({"error": str(e)})



if __name__ == '__main__':
    app.run(debug=True)
