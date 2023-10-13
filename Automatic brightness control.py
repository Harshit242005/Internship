# import speech_recognition as sr
# import screen_brightness_control as sbc
# import pyttsx3

# # Initialize the text-to-speech engine
# engine = pyttsx3.init()
# current_brightness = sbc.get_brightness() 
# print(f"current brightness level {current_brightness}")
# # Initialize the recognizer
# recognizer = sr.Recognizer()

# # Open the microphone and capture audio
# with sr.Microphone() as source:
#     print("Please speak something...")
#     recognizer.adjust_for_ambient_noise(source)  # Adjust for noise levels
#     audio = recognizer.listen(source)

# # Recognize the speech
# try:
#     text = recognizer.recognize_google(audio)  # Use the Google Web Speech API
#     print(f"You said: {text}")
# except sr.UnknownValueError:
#     print("Sorry, I couldn't understand the audio.")
# except sr.RequestError as e:
#     print(f"Sorry, there was an error with the request: {e}")


# import re

# # Sample sentence
# # Define patterns for words related to brightness
# brightness_down_pattern = r'\b(down|low|decrease|downgrade|slow)\b'
# brightness_up_pattern = r'\b(up|upgrade|increase|fast)\b'

# # Search for words related to decreasing brightness
# if re.search(brightness_down_pattern, text, re.IGNORECASE):
#     if current_brightness[0] <= 0:
#         sbc.set_brightness(0)
#         new_brightness = sbc.get_brightness()
#         new_text = f"the brightness level is down to {new_brightness[0]} percent now sir"
#         engine.say(new_text)
#         engine.runAndWait()
#     sbc.set_brightness(current_brightness[0]-10)
#     new_brightness = sbc.get_brightness()
#     new_text = f"the brightness level is down to {new_brightness[0]} percent now sir"
#     engine.say(new_text)
#     engine.runAndWait()

# # Search for words related to increasing brightness
# if re.search(brightness_up_pattern, text, re.IGNORECASE):
#     if current_brightness[0] >= 100:
#         sbc.set_brightness(100)
#         new_brightness = sbc.get_brightness()
#         new_text = f"the brightness level is up to {new_brightness[0]} percent now sir"
#         engine.say(new_text)
#         engine.runAndWait()
#     else:
#         sbc.set_brightness(current_brightness[0]+10)    
#         new_brightness = sbc.get_brightness()
#         new_text = f"the brightness level is up to {new_brightness[0]} percent now sir"
#         engine.say(new_text)
#         engine.runAndWait()


import speech_recognition as sr
import screen_brightness_control as sbc
import pyttsx3
import re

# Initialize the text-to-speech engine
engine = pyttsx3.init()

while True:
    # Capture audio
    recognizer = sr.Recognizer()
    with sr.Microphone() as source:
        print("Please speak something...")
        recognizer.adjust_for_ambient_noise(source)
        audio = recognizer.listen(source)

    # Recognize the speech
    try:
        text = recognizer.recognize_google(audio)
        print(f"You said: {text}")
        
        # Get current brightness level
        current_brightness = sbc.get_brightness()
       
        
        # Define patterns for words related to brightness
        brightness_down_pattern = r'\b(down|low|decrease|downgrade|slow)\b'
        brightness_up_pattern = r'\b(up|upgrade|increase|fast)\b'
        set_brightness_pattern = r'\bset\s.*?(\d+)\b'


        # Search for words related to decreasing brightness
        if re.search(brightness_down_pattern, text, re.IGNORECASE):
            if current_brightness[0] <= 0:
                sbc.set_brightness(0)
                new_brightness = sbc.get_brightness()
                new_text = f"The brightness level is down to {new_brightness[0]} percent now, sir."
                engine.say(new_text)
                engine.runAndWait()
            else:
                sbc.set_brightness(current_brightness[0] - 10)
                new_brightness = sbc.get_brightness()
                new_text = f"The brightness level is down to {new_brightness[0]} percent now, sir."
                engine.say(new_text)
                engine.runAndWait()

        # Search for words related to increasing brightness
        if re.search(brightness_up_pattern, text, re.IGNORECASE):
            if current_brightness[0] >= 100:
                sbc.set_brightness(100)
                new_brightness = sbc.get_brightness()
                new_text = f"The brightness level is up to {new_brightness[0]} percent now, sir."
                engine.say(new_text)
                engine.runAndWait()
            else:
                sbc.set_brightness(current_brightness[0] + 10)
                new_brightness = sbc.get_brightness()
                new_text = f"The brightness level is up to {new_brightness[0]} percent now, sir."
                engine.say(new_text)
                engine.runAndWait()

        set_match = re.search(set_brightness_pattern, text, re.IGNORECASE)
        if set_match:
            brightness_level = int(set_match.group(1))
            print(brightness_level)
            sbc.set_brightness(brightness_level)
            new_text = f"the brightness level is set to {brightness_level} percent now, sir."
            engine.say(new_text)
            engine.runAndWait()


    except sr.UnknownValueError:
        print("Sorry, I couldn't understand the audio.")
    except sr.RequestError as e:
        print(f"Sorry, there was an error with the request: {e}")

    # Add an exit condition (e.g., say "exit" to stop the loop)
    if "exit" in text.lower():
        break
