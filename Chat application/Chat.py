from flask import Flask, render_template, request, redirect, url_for
import hashlib
import json

from datetime import datetime
import mysql.connector
db = mysql.connector.connect(
    host="localhost",
    user="root",
    password="azxcvbnmlkjhgfds",
    database="ChatX"
)
cursor = db.cursor()

app = Flask(__name__)

@app.route("/")
def landing():
    return render_template("Landing.html")

@app.route("/Signup", methods=["GET", "POST"])
def Signup():
    if request.method == "POST":
        email = request.form["email"]
        name = request.form["name"]
        password = request.form["password"]
        # hash the password
        hashed_password = hashlib.sha256(password.encode()).hexdigest()
        # Insert user data into the Users table
        query = "INSERT INTO Users (Name, Email, Password) VALUES (%s, %s, %s)"
        values = (name, email, hashed_password)
        cursor.execute(query, values)
        db.commit()

        return redirect(url_for("Login"))  # Redirect to another page after successful signup
    return render_template("Signup.html")

# Define a function to retrieve user ID based on email
def get_user_id(email):
    query = "SELECT Id FROM Users WHERE Email = %s"
    cursor.execute(query, (email,))
    user_data = cursor.fetchone()
    if user_data:
        return user_data[0]
    else:
        return None

@app.route("/Login", methods=["GET", "POST"])
def Login():
    if request.method == "POST":
        email = request.form["email"]
        name = request.form["name"]
        password = request.form["password"]
        # hash the password
        hashed_password = hashlib.sha256(password.encode()).hexdigest()
        global user_id
        user_id = get_user_id(email)  # Get the user's ID

        query = "SELECT Id, Password FROM Users WHERE Email = %s"
        cursor.execute(query, (email,))
        user_data = cursor.fetchone()

        if user_data:
            stored_password = user_data[1]  # Fetched hashed password from the database
            if hashed_password == stored_password:
                user_id = user_data[0]  # Get the user's ID
                # Here you can perform further actions, like setting up user sessions
                # or redirecting the user to the appropriate page
                return redirect(url_for("Chat"))
            else:
                return "Invalid password"
        else:
            return "User not found"
        

    return render_template("Login.html")

@app.route("/Chat", methods=["GET", "POST"])
def Chat():
    
    query = "SELECT Id, Name FROM Users WHERE Id != %s"
    cursor.execute(query, (user_id,))
    user_data = cursor.fetchall()
   
    if request.method == "POST":
       
        selected_user_id = request.form["selectedUserId"]
        message = request.form["message"]
        print(f"reciever id is {selected_user_id} and the mesage is {message}")
        current_timestamp = datetime.now()  # Get the current timestamp

        query = "INSERT INTO chat (SenderId, ReceiverId, Message, Timestamp) VALUES (%s, %s, %s, %s)"
        values = (user_id, selected_user_id, message, current_timestamp)
        
        cursor.execute(query, values)
        db.commit()
        
    elif request.method == "GET":
        selected_user_id = request.args.get("selectedUserId")
        if selected_user_id:
            print(f"selected user id is {selected_user_id} and user from which he have logged in is {user_id}")
            # Fetch messages for the selected user and your user from the database
            query = "SELECT Id, Timestamp, ReceiverId, Message, SenderId FROM chat WHERE (SenderId = %s AND ReceiverId = %s) OR (SenderId = %s AND ReceiverId = %s) ORDER BY Timestamp ASC"
            cursor.execute(query, (user_id, selected_user_id, selected_user_id, user_id))
            messages = cursor.fetchall()

# Create a list of dictionaries containing message data
            response_data = []
            for  id, timestamp, receiver_id, message, sender_id in messages:
                message_data = {
                    "timestamp": timestamp.strftime('%Y-%m-%d %H:%M:%S'),
                    "receiver_id": receiver_id,
                    "message": message,
                    "sender_id": sender_id,
                    "message_id": id,
                    "user_id": user_id  # Add the user_id to the message data
                }
                response_data.append(message_data)

# Return the messages as a JSON response
            return json.dumps(response_data)

            
    print(f"the login user id {user_id}")
    return render_template("Chat.html", Others=user_data)


@app.route("/delete-message", methods=["POST"])
def delete_message():
    if request.method == "POST":
        message_id = request.form.get("message_id")

        # Perform the deletion in the database
        delete_query = "DELETE FROM chat WHERE Id = %s"
        cursor.execute(delete_query, (message_id,))
        db.commit()

        # Return a success response
        response = {"message": "Message deleted successfully"}
        return json.dumps(response), 200



if __name__ == "__main__":
    app.run(debug=True)
