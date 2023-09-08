

from bardapi import Bard
import pyttsx3
import test_1
text = ""
import speech_recognition as sr

def convert_speech_to_text(readed_text):
    machine_text = "ask you're question sir"
    engine = pyttsx3.init()

    # Set the properties of the voice
    engine.setProperty('rate', 200)  # Speed of speech
    engine.setProperty('volume', 1)  # Volume (0.0 to 1.0)

    # Convert text to speech
    engine.say(machine_text)

    # Play the speech
    engine.runAndWait()
    # Create a recognizer object
    recognizer = sr.Recognizer()

    # Use the default microphone as the audio source
    with sr.Microphone() as source:
        print("Speak now...")
        # Adjust for ambient noise levels
        recognizer.adjust_for_ambient_noise(source)
        
        # Listen for audio input
        audio = recognizer.listen(source)

    try:
        print("Recognizing speech...")
        # Use Google Speech Recognition to convert speech to text
        global text
        text = recognizer.recognize_google(audio)
        text += readed_text
        token = 'Xwh4jh22XDywdOaTcN9GTKY9evVg3fsWh5KsLvF4w2hthTJtX6ks9YYv6iwMLmce1kyqTg.'
        bard = Bard(token=token)

        if text is not None:
            
            result = bard.get_answer(text)['content']
            
            engine = pyttsx3.init()

            engine.setProperty('rate', 200)
            voices = engine.getProperty('voices') 
            engine.setProperty('voice', voices[0].id) 
            modified_result = result.replace('*', '')
            engine.say(modified_result)
            engine.runAndWait()
            check_call()
        else:
            print("no question has been asked")  

    except sr.UnknownValueError:
        print("Could not understand speech")
    except sr.RequestError as e:
        print("Error occurred while requesting speech recognition service: ", str(e))

# Call the function to convert speech to text

def check_call():
    # Create a recognizer object
    recognizer = sr.Recognizer()

    # Use the default microphone as the audio source
    with sr.Microphone() as source:
        print("Speak now...")
        # Adjust for ambient noise levels
        recognizer.adjust_for_ambient_noise(source)

        # Listen for audio input
        audio = recognizer.listen(source)

    try:
        print("Recognizing speech...")
        # Use the Google Speech Recognition API to convert speech to text
        text_value = recognizer.recognize_google(audio)
        
        if text_value.lower() == "jarvis":

            read_text = test_1.extract_text_from_screenshot()
            convert_speech_to_text(read_text)

    except sr.UnknownValueError:
        print("Could not understand speech")
        check_call()
    except sr.RequestError as e:
        print("Error occurred while requesting speech recognition service: ", str(e))

# Call the function to convert speech to text
check_call()