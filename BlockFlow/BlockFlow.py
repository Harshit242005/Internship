import mysql.connector
from flask import Flask, render_template, request, redirect, url_for, current_app
import hashlib
from datetime import datetime, timedelta
from apscheduler.schedulers.background import BackgroundScheduler
from flask_mail import Message, Mail


app = Flask(__name__)
app.config['MAIL_DEFAULT_SENDER'] = 'agreharshit610@gmail.com'  # Set your default sender email address
app.config['MAIL_SERVER'] = 'smtp.gmail.com'  # SMTP server address
app.config['MAIL_PORT'] = 587  # Port for SMTP server
app.config['MAIL_USERNAME'] = 'agreharshit610@gmail.com'
app.config['MAIL_PASSWORD'] = 'lbqxavlpxnewvczt'
app.config['MAIL_USE_TLS'] = True
app.config['MAIL_USE_SSL'] = False

mail = Mail(app)
scheduler = BackgroundScheduler()
scheduler.start()

db_config = {
    "host": "localhost",
    "user": "root",
    "password": "azxcvbnmlkjhgfds",
    "database": "BlockFlow"
}

conn = mysql.connector.connect(**db_config)
cursor = conn.cursor()
@app.route('/')
def index():
    return render_template('Landing.html')






@app.route("/update_completed_status", methods=["POST"])
def update_completed_status():
    try:
        task_number = request.form.get("taskNumber")

        # Update the "Completed" status in the database
        connection = mysql.connector.connect(**db_config)
        cursor = connection.cursor()

        update_query = "UPDATE today SET Completed = 1 WHERE Number = %s"
        cursor.execute(update_query, (task_number,))
        connection.commit()



        select_data_query = "SELECT Name, StartTime, Id, EndTime, Completed FROM today WHERE Number = %s"
        cursor.execute(select_data_query, (task_number,))
        task_data = cursor.fetchone()  # Fetch a single row of data

        if task_data:
            task_name = task_data[0]
            task_start_time = task_data[1]
            user_task_id = task_data[2]
            task_end_time = task_data[3]
            completed_value = task_data[4]
            select_email_query = "SELECT email FROM users WHERE id = %s"
            cursor.execute(select_email_query, (user_task_id,))
            task_user_email = cursor.fetchone()[0]

            timedelta_value = task_end_time

# Convert the timedelta to seconds and calculate hours, minutes, and seconds
            total_seconds = timedelta_value.total_seconds()
            hours = int(total_seconds // 3600)
            minutes = int((total_seconds % 3600) // 60)
            seconds = int(total_seconds % 60)

# Format the components as HH:MM:SS
            formatted_time = f"{hours:02d}:{minutes:02d}:{seconds:02d}"
            current_time = datetime.now().strftime('%H:%M:%S')
            
            if current_time < formatted_time:
            # Rest of your code for sending emails


                subject = "Warning: Task Completed Early"
                message = f"Your task {task_name} with start time {task_start_time} has been completed and you have completed it before the time"
                msg = Message(subject=subject, recipients=[task_user_email], body=message)
                mail.send(msg)
                return redirect(url_for("Completed"))
            # Send an email to the user
            subject = "Task Completed"
            message = f"Your task {task_name} with start time {task_start_time} has been completed."
            msg = Message(subject=subject, recipients=[task_user_email], body=message)
            mail.send(msg)




        cursor.close()
        connection.close()

        return redirect(url_for("Completed"))
    except Exception as e:
        print(str(e))
        return str(e), 500

@app.route("/Signup", methods=["GET", "POST"])
def Signup():
    if request.method == "POST":
        name = request.form["name"]
        email = request.form["email"]
        password = request.form["password"]
        hashed_password = hashlib.sha256(password.encode()).hexdigest()
        insert_query = "INSERT INTO users (name, email, password) VALUES (%s, %s, %s)"
        values = (name, email, hashed_password)
        cursor.execute(insert_query, values)
        conn.commit()  # Commit the transaction

        return redirect(url_for("login"))
    return render_template("Signup.html")

@app.route("/Login", methods=["GET", "POST"])
def Login():
    if request.method == "POST":
        global email, user_id, user_name  # Declare the user_id variable at the global level
      
        input_email = request.form["email"]
        password = request.form["password"]
        print(f"the email is {input_email} and the password is {password}")
        hash_password = hashlib.sha256(password.encode()).hexdigest()

        select_query = "SELECT * FROM Users WHERE email = %s"
        cursor.execute(select_query, (input_email,))
        user = cursor.fetchone()

        if user:
            email = user[2]
            stored_password = user[3]  # Assuming password is at index 3
            user_id = user[0]
            user_name = user[1]
            if hash_password == stored_password:
                # User is authenticated, redirect to their dashboard or task page
                return redirect(url_for('block'))  # Change to your dashboard route
            else:
                error_message = "Invalid email or password"
                return render_template("Login.html", error=error_message)
        else:
            error_message = "Invalid email or password"
            return render_template("Login.html", error=error_message)

        

    return render_template("login.html")

@app.route('/Block', methods=["GET", "POST"])
def block():
    if request.method == "POST":
        taskname = request.form["taskName"]
        start = request.form["startTime"]
        end = request.form["endTime"]

        insert_query = "INSERT INTO today (Date, Name, StartTime, EndTime, Id) VALUES (CURDATE(), %s, %s, %s, %s)"
        values = (taskname, start, end, user_id)
        cursor.execute(insert_query, values)
        conn.commit()  # Commit the transaction


    return render_template('index.html')

@app.route('/Flow', methods=["GET", "POST"])
def flow():
    # Fetch data from the table
    select_query = "SELECT * FROM today WHERE ID = %s"
    values = (user_id,)
    cursor.execute(select_query, values)
    data = cursor.fetchall()
    print(data)
    return render_template("Flow.html", data=data)

@app.route("/Completed")
def Completed():
    return render_template("Completed.html")


def send_notification(task_name, duration, user_email, user_name):
    with app.app_context():  # Use the app context from your Flask app
        subject = "Task Notification"
        recipient = user_email
        message = f"{user_name} You have a task: {task_name}. It's time to start and the duration of the task is {duration} hours"
        msg = Message(subject=subject, recipients=[recipient], body=message)
        mail.send(msg)

def check_and_send_notifications():
    with app.app_context():  # Use the app context from your Flask app
        current_time = datetime.now().time()
        query = "SELECT Name, Duration, Sent, Completed FROM today WHERE StartTime <= %s AND EndTime > %s LIMIT 1"
        cursor.execute(query, (current_time, current_time))
        tasks_to_notify = cursor.fetchall()

        for task in tasks_to_notify:
            task_name = task[0]
            duration = task[1]
            sent_flag = task[2]  # Get the Sent flag value
            completed_value = task[3]


            

            if sent_flag == 0:  # Check if the Sent flag is 0
                send_notification(task_name, duration, email, user_name)

                # Update the Sent flag for the task
                update_query = "UPDATE today SET Sent = 1 WHERE Name = %s"
                cursor.execute(update_query, (task_name,))
                conn.commit()


# Schedule the check_and_send_notifications function to run every minute
scheduler.add_job(check_and_send_notifications, 'interval', minutes=1)


if __name__ == '__main__':
    app.run(debug=True)

