import mysql.connector
from datetime import timedelta
from datetime import datetime


db_config = {
    "host": "localhost",
    "user": "root",
    "password": "azxcvbnmlkjhgfds",
    "database": "BlockFlow"
}

conn = mysql.connector.connect(**db_config)
cursor = conn.cursor()

select_query = "SELECT EndTime FROM today LIMIT 1";
cursor.execute(select_query)
data = cursor.fetchone()

# Extract the timedelta value from the fetched data
timedelta_value = data[0]

# Convert the timedelta to seconds and calculate hours, minutes, and seconds
total_seconds = timedelta_value.total_seconds()
hours = int(total_seconds // 3600)
minutes = int((total_seconds % 3600) // 60)
seconds = int(total_seconds % 60)

# Format the components as HH:MM:SS
formatted_time = f"{hours:02d}:{minutes:02d}:{seconds:02d}"

current_time = datetime.now().strftime('%H:%M:%S')
print(current_time, formatted_time)
if current_time > formatted_time:
    print("current time is greater")
else:
    print("current time is not greater")    
