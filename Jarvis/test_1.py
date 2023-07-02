import pytesseract
from PIL import Image
import pyautogui
import os

def extract_text_from_screenshot():
    counter_file = 'counter.txt'
    screenshot_folder = 'screenshots'

    # Create the screenshots folder if it doesn't exist
    os.makedirs(screenshot_folder, exist_ok=True)

    # Check if the counter file exists
    try:
        with open(counter_file, 'r') as file:
            counter = int(file.read())
    except FileNotFoundError:
        # If the counter file doesn't exist, create it with initial value 0
        counter = 0
        with open(counter_file, 'w') as file:
            file.write('0')

    # Increment the counter by 1
    counter += 1

    # Write the updated counter value back to the file
    with open(counter_file, 'w') as file:
        file.write(str(counter))

    # Set the Tesseract executable path
    pytesseract.pytesseract.tesseract_cmd = r'C:\Program Files (x86)\Tesseract-OCR\tesseract.exe'

    # Capture the screenshot
    screenshot = pyautogui.screenshot()

    # Save the screenshot with an incremented filename in the screenshots folder
    screenshot_path = os.path.join(screenshot_folder, f'screenshot{counter}.png')
    screenshot.save(screenshot_path)

    # Open the screenshot image
    image = Image.open(screenshot_path)

    # Perform OCR on the screenshot image
    text = pytesseract.image_to_string(image)

    # Return the extracted text
    return text

