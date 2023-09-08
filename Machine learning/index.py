# import pandas as pd
# import numpy as np
# from sklearn.model_selection import train_test_split
# from sklearn.preprocessing import StandardScaler
# from sklearn.svm import SVC
# from sklearn.metrics import accuracy_score, classification_report

# # loading out the dataset in two different section 
# data = pd.read_csv("diabetes.csv")
# X = data.drop(columns=["Outcome"])
# y = data["Outcome"]


# # split the dataset in traning and testing
# X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=0)

# # Feature scaling
# scaler = StandardScaler()
# X_train_scaled = scaler.fit_transform(X_train)
# X_test_scaled = scaler.transform(X_test)

# # Initialize SVM classifier
# svm_classifier = SVC(kernel='linear', C=1.0)

# # Train the SVM classifier
# svm_classifier.fit(X_train_scaled, y_train)

# # Predict on the test set
# y_pred = svm_classifier.predict(X_test_scaled)

# # Evaluate the model
# accuracy = accuracy_score(y_test, y_pred)


# print(accuracy * (100))

# # Example test data (replace with your actual test data)
# test_data = np.array([[3, 128, 64, 23, 194, 28.7, 0.092, 30]])  # Adjust the values accordingly

# # Feature scaling (assuming you're using the same 'scaler' as before)
# test_data_scaled = scaler.transform(test_data)

# # Predict using the trained SVM classifier
# predicted_outcome = svm_classifier.predict(test_data_scaled)
# print(f"for the test dataset {test_data} the outcome value is {predicted_outcome}")


# import pandas as pd
# import numpy as np
# import matplotlib.pyplot as plt  # Import Matplotlib
# from sklearn.model_selection import train_test_split
# from sklearn.preprocessing import StandardScaler
# from sklearn.svm import SVC
# from sklearn.metrics import accuracy_score, classification_report

# # Load the dataset
# data = pd.read_csv("diabetes.csv")
# X = data.drop(columns=["Outcome"])
# y = data["Outcome"]

# # Split the dataset into training and testing
# X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=0)

# # Feature scaling
# scaler = StandardScaler()
# X_train_scaled = scaler.fit_transform(X_train)
# X_test_scaled = scaler.transform(X_test)

# # Initialize SVM classifier
# svm_classifier = SVC(kernel='linear', C=1.0)

# # Train the SVM classifier
# svm_classifier.fit(X_train_scaled, y_train)

# # Predict on the test set
# y_pred = svm_classifier.predict(X_test_scaled)

# # print the accuracy score of the result 
# accuracy = accuracy_score(y_test, y_pred)


# print(accuracy * (100))

# # Visualization
# plt.figure(figsize=(10, 6))

# # Plot original outcomes in red
# plt.scatter(X_test_scaled[:, 0], X_test_scaled[:, 1], c=y_test, cmap='RdYlBu', marker='o', label='Original Outcomes')

# # Plot SVM predicted outcomes in blue
# plt.scatter(X_test_scaled[:, 0], X_test_scaled[:, 1], c=y_pred, cmap='coolwarm', marker='x', label='SVM Predicted Outcomes')

# plt.xlabel('Feature 1')
# plt.ylabel('Feature 2')
# plt.title('Original vs. SVM Predicted Outcomes')
# plt.legend()
# plt.show()


# code to insert a column that would be counting all the words that are part of spam email and creating a new modified csv file
# import pandas as pd

# # Read the CSV file
# data = pd.read_csv("spam.csv", encoding='latin1')

# # Select the first 200 rows and columns 0 to 1
# new_dataset = data.iloc[:200, 0:2]

# # Define column names
# column_names = ["Type", "Text"]

# # Create a new DataFrame with column names
# df = pd.DataFrame(new_dataset.values, columns=column_names)

# # Define keywords to count
# keywords = [
#     "free", "win", "Hey there darling", "guaranteed", "limited time", "special offer",
#     "exclusive deal", "urgent", "act now", "call now", "cash bonus", "money back",
#     "click below", "unsubscribe", "credit card offers", "earn money", "work from home",
#     "amazing results", "winning notification", "prize", "discount", "earn extra cash",
#     "increase sales", "debt relief", "invest now", "no credit check", "lose weight",
#     "miracle", "lowest price", "hidden charges", "one-time", "million dollars",
#     "bulk email", "online marketing", "multi-level marketing", "stop snoring", "reverses aging",
#     "double your", "as seen on", "apply now", "order now", "meet singles", "meet girls",
#     "meet women", "cialis", "viagra", "low interest", "meet the girl of your dreams",
#     "earn per week", "no hidden costs", "risk-free", "get out of debt", "mortgage rates"
# ]

# # Create a new column "Count" and initialize with zeros
# df["Count"] = 0

# # Iterate through the keywords and update "Count" column
# for keyword in keywords:
#     df["Count"] += df["Text"].str.lower().str.count(keyword)

# # Print the modified DataFrame
# df.to_csv("modified_spam.csv", index=False)


# import re
# import numpy as np
# import pandas as pd
# from sklearn.model_selection import train_test_split
# from sklearn.preprocessing import StandardScaler
# from sklearn.linear_model import LogisticRegression
# from sklearn.metrics import accuracy_score, classification_report

# # Read the modified CSV file
# data = pd.read_csv("modified_spam.csv")

# # Separate features (Count) and labels (Type)
# X = data["Count"].values.reshape(-1, 1)
# y = data["Type"]

# # Split the data into training and testing sets
# X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=0)

# # Standardize the features
# scaler = StandardScaler()
# X_train_scaled = scaler.fit_transform(X_train)
# X_test_scaled = scaler.transform(X_test)

# # Initialize and train the Logistic Regression model
# logreg_model = LogisticRegression()
# logreg_model.fit(X_train_scaled, y_train)

# # Predict on the test set
# y_pred = logreg_model.predict(X_test_scaled)

# # Evaluate the model
# accuracy = accuracy_score(y_test, y_pred)


# print("Accuracy:", accuracy * 100)

# # predicting the type of the newly generated text of an email
# # New email text
# new_email_text = "win a car"

# # List of keywords
# keywords = ["free", "win", "Hey there darling"]

# # Count keyword occurrences in the new email text
# keyword_count = sum(1 for keyword in keywords if re.search(keyword, new_email_text, re.IGNORECASE))

# # Standardize the keyword count
# keyword_count_scaled = scaler.transform(np.array([[keyword_count]]))

# # Make a prediction using the trained Logistic Regression model
# predicted_type = logreg_model.predict(keyword_count_scaled)

# # Print the prediction
# print("Predicted Type:", predicted_type)






# code to check if the text is spam or ham
import re
import numpy as np
import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler
from sklearn.svm import SVC
from sklearn.metrics import accuracy_score

# Read the modified CSV file
data = pd.read_csv("modified_spam.csv")

# Separate features (Count) and labels (Type)
X = data["Count"].values.reshape(-1, 1)
y = data["Type"]

# Split the data into training and testing sets
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=0)

# Standardize the features
scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

# Initialize and train the Support Vector Machine (SVM) model
svm_model = SVC(kernel='linear')
svm_model.fit(X_train_scaled, y_train)

# Predict on the test set
y_pred = svm_model.predict(X_test_scaled)

# Evaluate the model
accuracy = accuracy_score(y_test, y_pred)

print("Accuracy:", accuracy * 100)

# Predicting the type of a newly generated email text
# New email text
new_email_text = "can you call me"

keywords = [
    "free", "win", "Hey there darling", "guaranteed", "limited time", "special offer",
    "exclusive deal", "urgent", "act now", "call now", "cash bonus", "money back",
    "click below", "unsubscribe", "credit card offers", "earn money", "work from home",
    "amazing results", "winning notification", "prize", "discount", "earn extra cash",
    "increase sales", "debt relief", "invest now", "no credit check", "lose weight",
    "miracle", "lowest price", "hidden charges", "one-time", "million dollars",
    "bulk email", "online marketing", "multi-level marketing", "stop snoring", "reverses aging",
    "double your", "as seen on", "apply now", "order now", "meet singles", "meet girls",
    "meet women", "cialis", "viagra", "low interest", "meet the girl of your dreams",
    "earn per week", "no hidden costs", "risk-free", "get out of debt", "mortgage rates"
]

# Count keyword occurrences in the new email text
keyword_count = sum(1 for keyword in keywords if re.search(keyword, new_email_text, re.IGNORECASE))

# Standardize the keyword count
keyword_count_scaled = scaler.transform(np.array([[keyword_count]]))

# Make a prediction using the trained SVM model
predicted_type = svm_model.predict(keyword_count_scaled)

# Print the prediction
print("Predicted Type:", predicted_type)



# let's build more complex model now

import os
import imapclient
import re
import numpy as np
import pandas as pd
from sklearn.preprocessing import StandardScaler
from sklearn.linear_model import LogisticRegression

# Read the modified CSV file containing spam keywords and counts
data = pd.read_csv("modified_spam.csv")

# Separate features (Count) and labels (Type)
X = data["Count"].values.reshape(-1, 1)
y = data["Type"]

# Standardize the features
scaler = StandardScaler()
X_scaled = scaler.fit_transform(X)

# Initialize and train the Logistic Regression model
logreg_model = LogisticRegression()
logreg_model.fit(X_scaled, y)

# Get email and password from environment variables
EMAIL = "youre gmail account"
PASSWORD = "youre dual app password for the email app from google"

if EMAIL is None or PASSWORD is None:
    print("Please set the GMAIL_EMAIL and GMAIL_PASSWORD environment variables.")
    exit(1)

# Connect to Gmail IMAP server
server = imapclient.IMAPClient('imap.gmail.com')
server.login(EMAIL, PASSWORD)
server.select_folder('INBOX')

unseen_emails = server.search(['UNSEEN'])

if unseen_emails:
    first_unseen_email = unseen_emails[0]
    email_data = server.fetch([first_unseen_email], ['BODY[TEXT]'])
    email_body = email_data[first_unseen_email][b'BODY[TEXT]'].decode('utf-8')
    # Rest of your code for processing the email body


    # List of keywords for spam classification
    keywords = [
    "free", "win", "Hey there darling", "guaranteed", "limited time", "special offer",
    "exclusive deal", "urgent", "act now", "call now", "cash bonus", "money back",
    "click below", "unsubscribe", "credit card offers", "earn money", "work from home",
    "amazing results", "winning notification", "prize", "discount", "earn extra cash",
    "increase sales", "debt relief", "invest now", "no credit check", "lose weight",
    "miracle", "lowest price", "hidden charges", "one-time", "million dollars",
    "bulk email", "online marketing", "multi-level marketing", "stop snoring", "reverses aging",
    "double your", "as seen on", "apply now", "order now", "meet singles", "meet girls",
    "meet women", "cialis", "viagra", "low interest", "meet the girl of your dreams",
    "earn per week", "no hidden costs", "risk-free", "get out of debt", "mortgage rates"]

    # Count keyword occurrences in the email body
    keyword_count = sum(1 for keyword in keywords if re.search(keyword, email_body, re.IGNORECASE))

    # Standardize the keyword count
    keyword_count_scaled = scaler.transform(np.array([[keyword_count]]))

    # Make a prediction using the trained Logistic Regression model
    predicted_type = logreg_model.predict(keyword_count_scaled)

    # Print the email body and predicted classification
  
    print("Predicted Type:", predicted_type)
  
else:
    print("No unseen emails found.")    

# Logout from the server
server.logout()
