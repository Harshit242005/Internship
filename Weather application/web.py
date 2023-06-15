from flask import Flask, render_template, request, redirect, url_for
import requests
import json
import mysql.connector
from urllib.parse import urlencode
from datetime import datetime
import base64


import matplotlib.pyplot as plt
import numpy as np
import io
import base64
# Creation of the MySQL connector and connecting to the database
connection = mysql.connector.connect(
    host='localhost',
    user='root',
    password='azxcvbnmlkjhgfds',
    database='Weather'
)

app = Flask(__name__)

location = ""

image_today = ""
temperature_today = 0
humidity_today = 0

@app.route('/', methods=['GET'])
def home():
    return render_template('index.html')


@app.route('/get_weather', methods=['POST'])
def get_weather():
    global location
    location = request.form.get('location')
    api_key = "d0269a671c2a8d437f801421d9dd3acf"
    url = f'http://api.weatherstack.com/current?access_key={api_key}&query={location}'
    response = requests.get(url)
    if response.status_code == 200:
        data = response.json()
        weather_icon_url = data['current']['weather_icons'][0]
        temperature = data['current']['temperature']
        humidity = data['current']['humidity']
        city = data["location"]["name"]
        country = data["location"]["country"]
        weather_type = data["current"]["weather_descriptions"][0]
        cursor = connection.cursor()

        current_date = datetime.now().date()
        str_date = str(current_date)
        insert_query = "INSERT INTO Weather_data (Temperature, Humidity, Image, RecordTime, Country, City, WeatherType, Date, Years, Months, Dates) VALUES (%s, %s, %s, CURTIME(), %s, %s, %s, %s, YEAR(CURDATE()), MONTHNAME(CURDATE()), DAY(CURDATE()))"
        data = (temperature, humidity, weather_icon_url, country, city, weather_type, str_date)
        try:
            # Execute the INSERT query
            cursor.execute(insert_query, data)
            connection.commit()
            

            # Create the query parameters
            query_parameters = {
                'temperature': temperature,
                'humidity': humidity,
                'city': city,
                'country': country,
                'WeatherType': weather_type,
                'weather_icon_url': weather_icon_url,
                'Date': str_date
            }

            query_string = urlencode(query_parameters)

            # Redirect to the weather_details route with the updated query string
            return redirect(url_for('weather_details') + '?' + query_string)

        except mysql.connector.Error as error:
            return 'Error inserting data into MySQL table: {}'.format(error)
    else:
        return 'Error fetching weather data'


@app.route('/weather_details', methods=['GET'])
def weather_details():
    temperature = request.args.get('temperature')
    humidity = request.args.get('humidity')
    country = request.args.get('country')
    city = request.args.get('city')
    weather_type = request.args.get('WeatherType')
    weather_icon_url = request.args.get('weather_icon_url')
    Date = request.args.get('Date')

    global image_today
    global temperature_today
    global humidity_today

    image_today = get_image(city)
    temperature_today = fetch_today_temperatures(city)
    humidity_today = fetch_today_humidity(city)


    return render_template('weather_details.html', temperature=temperature, humidity=humidity, country=country,
                           city=city, weather_type=weather_type, weather_icon_url=weather_icon_url, Date=Date)


def fetch_temperatures():
    cursor = connection.cursor()
    select_query = "SELECT Temperature FROM Weather_data WHERE City = %s"
    try:
        cursor.execute(select_query, (location,))
        result = cursor.fetchall()
        temperatures = [row[0] for row in result]  # Extract temperatures from the result
        return temperatures
    except mysql.connector.Error as error:
        print('Error fetching temperatures from MySQL table:', error)
        return []

def fetch_humidity():
    cursor = connection.cursor()
    select_query = "SELECT Humidity FROM Weather_data WHERE City = %s"
    try:
        cursor.execute(select_query, (location,))
        result = cursor.fetchall()
        humidities = [row[0] for row in result]
        return humidities
    except mysql.connector.Error as error:
        print('Error fetching humidities from MySql table: ', error)
        return []

def fetch_time():
    cursor = connection.cursor()
    select_query = "SELECT RecordTime FROM Weather_data WHERE City = %s"
    try:
        cursor.execute(select_query, (location,))
        result = cursor.fetchall()
        recordTime = [row[0] for row in result]
        return recordTime
    except mysql.connector.Error as error:
        print('Error fetching record time from MySql table: ', error)
        return []



def save_plot_image():
    temperatures = fetch_temperatures()
    humidities = fetch_humidity()
    time_of_record = fetch_time()
    record_hours = []

    for td in time_of_record:
        total_seconds = td.total_seconds()
        record_hours.append(int(total_seconds // 3600))
    error = f"Not enough data to get analyse the weather of {location}"

    if temperatures is None or humidities is None or time_of_record is None:
        return None
    
    plt.subplot(1, 2, 1)
    plt.scatter(temperatures, record_hours, color='red', label='Temperature')
    plt.xlabel('Temperature')
    plt.ylabel('Record Hours')

    plt.subplot(1, 2, 2)
    plt.scatter(humidities, record_hours, color='hotpink', label='Humidity')
    plt.xlabel('Humidity')
    plt.ylabel('Record Hours')

    plt.suptitle('Temperature and Humidity vs Record Hours')
    plt.legend()
    plt.tight_layout()

    buffer = io.BytesIO()
    plt.savefig(buffer, format='png')
    buffer.seek(0)
    image_png = buffer.getvalue()
    buffer.close()

    return image_png



# finding out the average temprature of today till the data is recored 
# an image of today


today_date = datetime.now().day
def fetch_today_temperatures(City):
    cursor = connection.cursor()
    select_query = f"SELECT Temperature FROM Weather_data WHERE City = %s AND Dates = {today_date}"
    try:
        cursor.execute(select_query, (City,))
        result = cursor.fetchall()
        temperatures = [row[0] for row in result]  # Extract temperatures from the result
        size = len(temperatures)
        return sum(temperatures) // size
    except mysql.connector.Error as error:
        print('Error fetching temperatures from MySQL table:', error)
        return []

def fetch_today_humidity(City):
    cursor = connection.cursor()
    select_query = f"SELECT Humidity FROM Weather_data WHERE City = %s AND Dates = {today_date}"
    try:
        cursor.execute(select_query, (City,))
        result = cursor.fetchall()
        humidities = [row[0] for row in result]
        size = len(humidities)
        return sum(humidities) // size
    except mysql.connector.Error as error:
        print('Error fetching humidities from MySql table: ', error)
        return []


def get_image(City):
    cursor = connection.cursor()
    select_query = f"SELECT Image FROM Weather_data WHERE City = %s AND Dates = {today_date} LIMIT 1"
    try:
        cursor.execute(select_query, (City,))
        result = cursor.fetchall()
        image = [row[0] for row in result]
        return image[0]
    except mysql.connector.Error as error:
        print('Error fetching image from mysql table :', error)
        return []





@app.route('/analysis')
def analysis():
    image_png = save_plot_image()

    if image_png is None:
        return render_template('Error.html', error="Not enough data to analyze the weather of {}".format(location))

    # Converting the image to a base64-encoded string
    graph = base64.b64encode(image_png).decode()
    print(image_today)
    print(temperature_today)
    print(humidity_today)
    return render_template('analysis.html', location=location, graph=graph, today_image=image_today, today_temprature=temperature_today, today_humidity=humidity_today)



if __name__ == '__main__':
    app.run(debug=True)
    

