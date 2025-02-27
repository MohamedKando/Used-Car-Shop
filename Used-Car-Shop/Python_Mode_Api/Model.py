import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns
from sklearn.preprocessing import LabelEncoder
from sklearn.model_selection import train_test_split
from sklearn.metrics import mean_squared_error, r2_score
from xgboost import XGBRegressor
import joblib

# Load data
data = pd.read_csv('/content/hatla2ee_scraped_data.csv')

# Preprocessing: Handle missing values and format features
data.dropna(subset=["Mileage", "Price"], inplace=True)
data["Year"] = data["Name"].apply(lambda x: int(x.split()[-1]) if isinstance(x, str) and x.split()[-1].isdigit() else None)
data.drop(columns=["Item URL", "Date Displayed", "Name", "Color", "City"], inplace=True)

# Clean 'Mileage' and 'Price' columns
data["Mileage"] = data["Mileage"].str.replace(" Km", "").str.replace(",", "").astype(int)
data["Price"] = data["Price"].str.replace(" EGP", "").str.replace(",", "").astype(int)

# Convert binary columns to numeric (1/0)
binary_columns = ['Transmission', 'AirConditioner', 'PowerSteering', 'RemoteControl']
for col in binary_columns:
    data[col] = data[col].map({'Yes': 1, 'No': 0}).astype(int)

# Label encode remaining categorical features
label_encoders = {}
categorical_columns = ['Make', 'Model']  # Only encode non-binary categorical columns
for col in categorical_columns:
    if data[col].dtype == 'object':
        le = LabelEncoder()
        data[col] = le.fit_transform(data[col])
        label_encoders[col] = le

# Prepare features and target
X = data.drop(["Price"], axis=1)
y = data["Price"]
y = np.log1p(y)  # Apply log transformation

# Split data into training and testing sets
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42, shuffle=True)

# Train the model
xgb_reg = XGBRegressor(random_state=42)
xgb_reg.fit(X_train, y_train)

# Make predictions and calculate performance metrics
y_pred = xgb_reg.predict(X_test)
y_pred_original = np.expm1(y_pred)
y_test_original = np.expm1(y_test)

# Calculate MSE and R² on the original scale
mse_original = mean_squared_error(y_test_original, y_pred_original)
r2 = r2_score(y_test_original, y_pred_original)

print(f"MSE in Original Scale: {mse_original}")
print(f"R² Score: {r2}")

# Save the model and label encoders
joblib.dump(xgb_reg, "xgb_car_price_model.pkl")
joblib.dump(label_encoders, "label_encoders.pkl")

# Function to prepare sample data
def prepare_sample_data(sample_dict, label_encoders, feature_columns):
    sample_df = pd.DataFrame(sample_dict)

    # Convert binary columns to numeric
    binary_cols = ['Transmission', 'AirConditioner', 'PowerSteering', 'RemoteControl']
    for col in binary_cols:
        sample_df[col] = sample_df[col].map({'Yes': 1, 'No': 0}).astype(int)

    # Encode categorical variables
    for col in ['Make', 'Model']:
        if col in label_encoders:
            sample_df[col] = sample_df[col].map(lambda x: label_encoders[col].transform([x])[0]
                                              if x in label_encoders[col].classes_ else -1)

    # Ensure correct feature order
    return sample_df[feature_columns]

# Test the model with a sample input
sample_data = {
    "Year": [2010],
    "Mileage": [200100],
    "Make": ["Kia"],
    "Model": ["Cerato"],
    "Transmission": ['Yes'],
    "AirConditioner": ['Yes'],
    "PowerSteering": ['Yes'],
    "RemoteControl": ['Yes']
}

# Prepare sample data using the new function
sample_df = prepare_sample_data(sample_data, label_encoders, X_train.columns)

# Predict price
predicted_log_price = xgb_reg.predict(sample_df)
predicted_price = np.expm1(predicted_log_price)

print(f"Predicted Price: {predicted_price[0]:,.2f} EGP")