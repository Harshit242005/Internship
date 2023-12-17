import 'package:file_picker/file_picker.dart';
// import 'package:open_app_file/open_app_file.dart';
import 'package:flutter/material.dart';
import 'package:track/third.dart';

// import 'package:windows_notification/notification_message.dart';
// import 'package:windows_notification/windows_notification.dart';

class Singlefilepicker extends StatefulWidget {
  const Singlefilepicker({super.key});

  @override
  State<Singlefilepicker> createState() => _SinglefilepickerState();
}

class _SinglefilepickerState extends State<Singlefilepicker> {
  PlatformFile? file;
  String name = '';
  String filePath = '';

  Future<void> picksinglefile() async {
    FilePickerResult? result = await FilePicker.platform.pickFiles();
    if (result != null) {
      file = result.files.first;
      if (file != null) {
        filePath = file!.path.toString();
        // OpenAppFile.open(file!.path.toString());
      }
    }
  }

  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        flexibleSpace: Container(
          decoration: const BoxDecoration(
            color: Color.fromARGB(255, 255, 255, 255),
          ),
        ),
        title: const Text(
          'Tell us something about you',
          style: TextStyle(
            color: Color.fromARGB(255, 0, 0, 0),
            fontWeight: FontWeight.bold,
            fontFamily: 'ReadexPro',
            fontSize: 25,
          ),
        ),
      ),
      body: SingleChildScrollView(
        child: Center(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Container(
                margin: const EdgeInsets.only(
                  top: 0.0,
                  right: 25.0,
                  bottom: 0.0,
                  left: 0.0,
                ),
                alignment: Alignment.topRight,
                child: const Text(
                  'TrackX',
                  style: TextStyle(
                    fontFamily: 'ReadexPro',
                    fontSize: 24,
                    fontWeight: FontWeight.bold,
                  ),
                ),
              ),
              const SizedBox(
                height: 200,
              ),
              SizedBox(
                width: 400,
                height: 50,
                child: TextField(
                  decoration: const InputDecoration(
                    labelText: 'Type name here...',
                    labelStyle: TextStyle(
                      fontFamily: 'ReadexPro',
                      color: Color.fromARGB(255, 0, 0, 0),
                      fontSize: 16,
                      fontWeight: FontWeight.bold,
                    ),
                    enabledBorder: OutlineInputBorder(
                      borderSide: BorderSide(
                        color: Colors.grey,
                      ),
                    ),
                    focusedBorder: OutlineInputBorder(
                      borderSide: BorderSide(
                        color: Colors.green,
                      ),
                    ),
                  ),
                  onChanged: (value) {
                    setState(() {
                      name = value;
                    });
                  },
                  style: const TextStyle(
                    color: Colors.black,
                    fontSize: 16,
                    fontWeight: FontWeight.normal,
                    fontFamily: 'ReadexPro',
                  ),
                ),
              ),
              const SizedBox(
                height: 25,
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  const Text(
                    'Pick an image',
                    style: TextStyle(
                      fontSize: 20,
                      fontFamily: 'ReadexPro',
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(
                    width: 90,
                  ),
                  SizedBox(
                    height: 50,
                    width: 175,
                    child: ElevatedButton.icon(
                      onPressed: picksinglefile,
                      icon: const Icon(Icons.insert_drive_file_sharp),
                      label: const Text(
                        'Pick File',
                        style: TextStyle(
                          fontSize: 16,
                          fontFamily: 'ReadexPro',
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      style: ElevatedButton.styleFrom(
                        backgroundColor: Colors.white,
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(10),
                        ),
                        padding: const EdgeInsets.all(16),
                      ),
                    ),
                  ),
                ],
              ),
              const SizedBox(
                height: 100,
              ),
              SizedBox(
                width: 200,
                height: 50,
                child: ElevatedButton(
                  onPressed: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                          builder: (context) => ThirdScreen(
                              name: name,
                              imagePath: filePath)), // Navigate to SecondScreen
                    );
                  },
                  style: ElevatedButton.styleFrom(
                    foregroundColor: Colors.white,
                    backgroundColor: Colors.black,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(5),
                    ),
                    minimumSize: const Size(200, 60),
                  ),
                  child: const Text(
                    'continue',
                    style: TextStyle(
                      fontFamily: 'ReadexPro',
                      fontSize: 20,
                    ),
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
