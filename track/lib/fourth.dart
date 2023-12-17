import 'package:path/path.dart';
import 'package:sqflite/sqflite.dart';
// import 'dart:io';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class FourthScreen extends StatefulWidget {
  const FourthScreen({super.key});

  @override
  // ignore: library_private_types_in_public_api
  _FourthScreenState createState() => _FourthScreenState();
}

class _FourthScreenState extends State<FourthScreen> {
  TextEditingController headingController = TextEditingController();
  TextEditingController descriptionController = TextEditingController();
  String selectedImage = 'assets/WorkLabel.png'; // Default image
  TimeOfDay? selectedTime; // This will store the selected time
  TimeOfDay? endedTime;
  bool isHovered_1 = false; // Define isHovered and initialize it to fal
  bool isHovered_2 = false;
  bool isHovered_3 = false;
  bool isHovered_4 = false;
  bool isHovered_5 = false;
  bool isHovered_6 = false;

  Future<void> _selectTime(BuildContext context) async {
    final TimeOfDay? picked = await showTimePicker(
      context: context,
      initialTime: selectedTime ?? TimeOfDay.now(),
    );

    if (picked != null && picked != selectedTime) {
      setState(() {
        selectedTime = picked;
      });
    }
  }

  Future<void> _endedTime(BuildContext context) async {
    final TimeOfDay? picked = await showTimePicker(
      context: context,
      initialTime: endedTime ?? TimeOfDay.now(),
    );

    if (picked != null && picked != endedTime) {
      setState(() {
        endedTime = picked;
      });
    }
  }

  // function to add new rows in the database
  Future<void> insertTask(Database database, String name, String description,
      String label, String startTime, String endTime) async {
    final formattedDate = DateFormat('dd/MM/yyyy').format(
        DateTime.now().toLocal()); // Get// Get the current date and time.

    // Create the "Tasks" table if it doesn't exist.
    await database.execute('''
      CREATE TABLE IF NOT EXISTS Tasks (
        id INTEGER PRIMARY KEY,
        name TEXT NOT NULL,
        description TEXT NOT NULL,
        label TEXT NOT NULL,
        start_time TEXT NOT NULL,
        end_time TEXT NOT NULL,
        status INTEGER DEFAULT 0,
        date DATE
      )
    ''');

    final task = {
      'name': name,
      'description': description,
      'label': label,
      'start_time': startTime,
      'end_time': endTime,
      'date': formattedDate,
    };

    await database.insert('Tasks', task);
  }

  final formattedDate = DateFormat('dd/MM/yyyy')
      .format(DateTime.now().toLocal()); // Get the current date and time.
  @override
  void dispose() {
    headingController.dispose();
    descriptionController.dispose();
    super.dispose();
  }

  // Function to update selectedImage
  void updateSelectedImage(String imagePath) {
    setState(() {
      selectedImage = imagePath;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Create Task',
          style:
              TextStyle(fontWeight: FontWeight.bold, fontFamily: 'ReadexPro'),
        ),
      ),
      body: SingleChildScrollView(
        child: Center(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.center,
            children: <Widget>[
              Text(
                'Today date - $formattedDate',
                style: const TextStyle(
                    fontFamily: 'ReadexPro',
                    fontSize: 20,
                    fontWeight: FontWeight.bold),
              ),
              Container(
                margin: const EdgeInsets.only(
                  right: 550.0,
                  top: 50,
                ),
                child: const Text(
                  'Type heading of the task',
                  style: TextStyle(
                    fontFamily: 'ReadexPro',
                    fontSize: 20,
                    fontWeight: FontWeight.w400,
                  ),
                ),
              ),
              const SizedBox(
                height: 10,
              ),
              Container(
                width: 800,
                padding: const EdgeInsets.all(
                  8.0,
                ),
                decoration: BoxDecoration(
                  border: Border.all(
                    color: Colors.grey,
                  ),
                  borderRadius: BorderRadius.circular(4.0),
                ),
                child: TextField(
                  controller: headingController,
                  style: const TextStyle(
                    fontFamily: 'ReadexPro',
                    fontWeight: FontWeight.w500,
                    fontSize: 16,
                  ),
                  decoration: const InputDecoration(
                    hintText: 'Type heading',
                    border: InputBorder.none,
                    hintStyle: TextStyle(fontFamily: 'ReadexPro'),
                  ),
                ),
              ),
              const SizedBox(
                height: 20,
              ),
              Container(
                margin: const EdgeInsets.only(
                  right: 520,
                  top: 25,
                ),
                child: const Text(
                  'Type description of the task',
                  style: TextStyle(
                    fontFamily: 'ReadexPro',
                    fontSize: 20,
                    fontWeight: FontWeight.w400,
                  ),
                ),
              ),
              const SizedBox(
                height: 10,
              ),
              Container(
                width: 800,
                height: 250,
                padding: const EdgeInsets.only(top: 10, left: 10),
                decoration: BoxDecoration(
                  border: Border.all(
                    color: Colors.grey,
                    width: 1.0,
                  ),
                  borderRadius: BorderRadius.circular(8.0),
                ),
                child: TextField(
                  controller: descriptionController,
                  maxLines: 4,
                  style: const TextStyle(
                    fontSize: 16.0,
                  ),
                  decoration: const InputDecoration(
                    hintText: 'Type your text here',
                    border: InputBorder.none,
                  ),
                ),
              ),
              const SizedBox(
                height: 20,
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: <Widget>[
                  GestureDetector(
                    onTap: () {
                      updateSelectedImage('assets/WorkLabel.png');
                    },
                    child: MouseRegion(
                      onEnter: (_) {
                        // Add a border when hovered.
                        setState(() {
                          isHovered_1 = true;
                        });
                      },
                      onExit: (_) {
                        // Remove the border when not hovered.
                        setState(() {
                          isHovered_1 = false;
                        });
                      },
                      child: AnimatedContainer(
                        duration: const Duration(
                            milliseconds:
                                2000), // Adjust the duration as needed

                        decoration: isHovered_1
                            ? BoxDecoration(
                                shape:
                                    BoxShape.circle, // Set the shape to circle
                                border: Border.all(
                                  color: const Color.fromARGB(
                                      50, 0, 0, 0), // Border color on hover
                                  width: 2.0, // Border width on hover
                                ),
                              )
                            : null, // No border when not hovered
                        child: Image.asset(
                          'assets/WorkLabel.png',
                        ),
                      ),
                    ),
                  ),

// Repeat the same pattern for other images

                  const SizedBox(
                    width: 25,
                  ),
                  GestureDetector(
                    onTap: () {
                      updateSelectedImage('assets/StudyLabel.png');
                    },
                    child: MouseRegion(
                      onEnter: (_) {
                        // Add a border when hovered.
                        setState(() {
                          isHovered_2 = true;
                        });
                      },
                      onExit: (_) {
                        // Remove the border when not hovered.
                        setState(() {
                          isHovered_2 = false;
                        });
                      },
                      child: Container(
                        decoration: isHovered_2
                            ? BoxDecoration(
                                shape:
                                    BoxShape.circle, // Set the shape to circle
                                border: Border.all(
                                  color: const Color.fromARGB(50, 0, 0, 0),
                                  width: 2.0, // Border width on hover
                                ),
                              )
                            : null, // No border when not hovered
                        child: Image.asset(
                          'assets/StudyLabel.png',
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(
                    width: 25,
                  ),
                  GestureDetector(
                    onTap: () {
                      updateSelectedImage('assets/EnjoyLabel.png');
                    },
                    child: MouseRegion(
                      onEnter: (_) {
                        // Add a border when hovered.
                        setState(() {
                          isHovered_3 = true;
                        });
                      },
                      onExit: (_) {
                        // Remove the border when not hovered.
                        setState(() {
                          isHovered_3 = false;
                        });
                      },
                      child: Container(
                        decoration: isHovered_3
                            ? BoxDecoration(
                                shape:
                                    BoxShape.circle, // Set the shape to circle
                                border: Border.all(
                                  color: const Color.fromARGB(50, 0, 0, 0),
                                  width: 2.0, // Border width on hover
                                ),
                              )
                            : null, // No border when not hovered
                        child: Image.asset(
                          'assets/EnjoyLabel.png',
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(
                    width: 25,
                  ),
                  GestureDetector(
                    onTap: () {
                      updateSelectedImage('assets/FamilyLabel.png');
                    },
                    child: MouseRegion(
                      onEnter: (_) {
                        // Add a border when hovered.
                        setState(() {
                          isHovered_4 = true;
                        });
                      },
                      onExit: (_) {
                        // Remove the border when not hovered.
                        setState(() {
                          isHovered_4 = false;
                        });
                      },
                      child: Container(
                        decoration: isHovered_4
                            ? BoxDecoration(
                                shape:
                                    BoxShape.circle, // Set the shape to circle
                                border: Border.all(
                                  color: const Color.fromARGB(50, 0, 0, 0),
                                  width: 2.0, // Border width on hover
                                ),
                              )
                            : null, // No border when not hovered
                        child: Image.asset(
                          'assets/FamilyLabel.png',
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(
                    width: 25,
                  ),
                  GestureDetector(
                    onTap: () {
                      updateSelectedImage('assets/FoodLabel.png');
                    },
                    child: MouseRegion(
                      onEnter: (_) {
                        // Add a border when hovered.
                        setState(() {
                          isHovered_5 = true;
                        });
                      },
                      onExit: (_) {
                        // Remove the border when not hovered.
                        setState(() {
                          isHovered_5 = false;
                        });
                      },
                      child: Container(
                        decoration: isHovered_5
                            ? BoxDecoration(
                                shape:
                                    BoxShape.circle, // Set the shape to circle
                                border: Border.all(
                                  color: const Color.fromARGB(50, 0, 0, 0),
                                  width: 2.0, // Border width on hover
                                ),
                              )
                            : null, // No border when not hovered
                        child: Image.asset(
                          'assets/FoodLabel.png',
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(
                    width: 25,
                  ),
                  GestureDetector(
                    onTap: () {
                      updateSelectedImage('assets/ExcersiceLabel.png');
                    },
                    child: MouseRegion(
                      onEnter: (_) {
                        // Add a border when hovered.
                        setState(() {
                          isHovered_6 = true;
                        });
                      },
                      onExit: (_) {
                        // Remove the border when not hovered.
                        setState(() {
                          isHovered_6 = false;
                        });
                      },
                      child: Container(
                        decoration: isHovered_6
                            ? BoxDecoration(
                                shape:
                                    BoxShape.circle, // Set the shape to circle
                                border: Border.all(
                                  color: const Color.fromARGB(50, 0, 0, 0),
                                  width: 2.0, // Border width on hover
                                ),
                              )
                            : null, // No border when not hovered
                        child: Image.asset(
                          'assets/ExcersiceLabel.png',
                        ),
                      ),
                    ),
                  ),
                  // You can add more GestureDetector widgets for other images
                ],
              ),
              const SizedBox(
                height: 20,
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  SizedBox(
                    width: 400,
                    child: TextFormField(
                      onTap: () {
                        _selectTime(context);
                      },
                      readOnly: true, // Prevents direct text input
                      controller: TextEditingController(
                          text: selectedTime?.format(context) ?? ''),
                      decoration: const InputDecoration(
                        labelText: 'Select Start Time',
                        labelStyle: TextStyle(
                            fontFamily: 'ReadexPro',
                            fontWeight: FontWeight.w600),
                        prefixIcon: Icon(Icons.access_time),
                        border: OutlineInputBorder(
                          borderSide: BorderSide(), // Remove the bottom border
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(
                    width: 25,
                  ),
                  SizedBox(
                    width: 400,
                    child: TextFormField(
                      onTap: () {
                        _endedTime(context);
                      },
                      readOnly: true, // Prevents direct text input
                      controller: TextEditingController(
                          text: endedTime?.format(context) ?? ''),
                      decoration: const InputDecoration(
                        labelText: 'Select End Time',
                        labelStyle: TextStyle(
                            fontFamily: 'ReadexPro',
                            fontWeight: FontWeight.w600),
                        prefixIcon: Icon(Icons.access_time),
                        border: OutlineInputBorder(
                          borderSide: BorderSide(), // Remove the bottom border
                        ),
                      ),
                    ),
                  ),
                ],
              ),
              const SizedBox(
                height: 20,
              ),
              SizedBox(
                width: 400,
                height: 60,
                child: ElevatedButton(
                  onPressed: () async {
                    // Access the entered data and the selected image
                    final heading = headingController.text;
                    final description = descriptionController.text;
                    final label = selectedImage;
                    final startTime = selectedTime.toString();
                    final endTime = endedTime.toString();

                    // Get the path to the database file.
                    final databasePath =
                        join(await getDatabasesPath(), 'Tasks.db');

                    // Open the database.
                    final database = await openDatabase(databasePath);

                    // Insert the data into the database.
                    await insertTask(database, heading, description, label,
                        startTime, endTime);

                    // ignore: use_build_context_synchronously
                    _showDataAddedDialog(context, heading, description, label,
                        startTime, endTime, formattedDate);

                    // Close the database.
                    // You can use 'selectedImage' for the image selection
                    // Do something with the data, like submitting it
                  },
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Colors.black,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(10),
                    ),
                  ),
                  child: const Text(
                    'Create',
                    style: TextStyle(
                      fontFamily: 'ReadexPro',
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                  ),
                ),
              )
            ],
          ),
        ),
      ),
    );
  }

  void _showDataAddedDialog(
      BuildContext context,
      String heading,
      String description,
      String label,
      String startTime,
      String endTime,
      String formattedDate) {
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text('Data Added'),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Image.asset(
                label, // Assuming 'label' contains the image asset path.
              ),
              Row(
                children: [
                  Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text('Heading: $heading'),
                      Text('Description: $description'),
                    ],
                  ),
                  Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text('Start Time: $startTime'),
                      Text('End Time: $endTime'),
                    ],
                  ),
                ],
              ),
            ],
          ),
          actions: <Widget>[
            TextButton(
              onPressed: () {
                Navigator.of(context).pop(); // Close the dialog
              },
              child: const Text('OK'),
            ),
          ],
        );
      },
    );
  }
}
