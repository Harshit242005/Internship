import 'dart:io';
import 'package:intl/intl.dart';
import 'package:flutter/material.dart';
import 'package:path/path.dart';
import 'package:sqflite_common_ffi/sqflite_ffi.dart';
import 'package:track/fifth.dart';

import 'fourth.dart';

class ThirdScreen extends StatefulWidget {
  final String name;
  final String imagePath;

  const ThirdScreen({required this.name, required this.imagePath, Key? key})
      : super(key: key);

  @override
  // ignore: library_private_types_in_public_api
  _ThirdScreenState createState() => _ThirdScreenState();
}

class _ThirdScreenState extends State<ThirdScreen> {
  List<Task> tasks = [];
  bool noTasksFound = false; // Flag to indicate whether no tasks are found.
  @override
  void initState() {
    super.initState();
    // Open the database and fetch tasks when the widget initializes.
    openDatabaseAndFetchTasks();
  }

  Future<void> openDatabaseAndFetchTasks() async {
    final databasePath = join(await getDatabasesPath(), 'Tasks.db');
    final database = await databaseFactoryFfi.openDatabase(databasePath);
    try {
      final fetchedTasks = await fetchTasks(database);
      setState(() {
        tasks = fetchedTasks;
      });
    } catch (e) {
      // Handle the error when the "Tasks" table doesn't exist.
      setState(() {
        noTasksFound = true;
      });
    }
    await database.close();
  }

  // Future<List<Task>> fetchTasks(Database database) async {
  //   final customDateFormat =
  //       DateFormat('dd/MM/yyyy'); // Define the custom date format
  //   final formattedDate = customDateFormat
  //       .format(DateTime.now()); // Get the current date in the custom format

  //   try {
  //     final List<Map<String, dynamic>> maps = await database.query(
  //       'Tasks',
  //       where: 'date = ?',
  //       whereArgs: [formattedDate],
  //     );

  //     return List.generate(maps.length, (i) {
  //       return Task(
  //           id: maps[i]['id'],
  //           name: maps[i]['name'],
  //           description: maps[i]['description'],
  //           label: maps[i]['label'],
  //           startTime: maps[i]['start_time'],
  //           endTime: maps[i]['end_time'],
  //           status: maps[i]['status'],
  //           date: customDateFormat.parse(maps[i]['date']),
  //           cardColor: const Color.fromARGB(255, 177, 177, 177));
  //     });
  //   } catch (e) {
  //     print("Error fetching tasks: $e");
  //     return []; // Return an empty list in case of an error.
  //   }
  // }

  Future<List<Task>> fetchTasks(Database database) async {
    final customDateFormat = DateFormat('dd/MM/yyyy');
    final formattedDate = customDateFormat.format(DateTime.now());

    try {
      final List<Map<String, dynamic>> maps = await database.query(
        'Tasks',
        where: 'date = ?',
        whereArgs: [formattedDate],
      );

      final now = TimeOfDay.now();

      return List.generate(maps.length, (i) {
        String startTimeString = maps[i]['start_time'];
        String endTimeString = maps[i]['end_time'];

        // Extract the hour and minute components from the strings
        String startTimeFormatted =
            startTimeString.replaceAll('TimeOfDay(', '').replaceAll(')', '');
        String endTimeFormatted =
            endTimeString.replaceAll('TimeOfDay(', '').replaceAll(')', '');

        final startTimeParts = startTimeFormatted.split(':');
        final endTimeParts = endTimeFormatted.split(':');
        print(startTimeParts);
        print(endTimeParts);

        if (startTimeParts.length == 2 && endTimeParts.length == 2) {
          // Parse startTime and endTime strings into TimeOfDay objects
          final startTime = TimeOfDay(
            hour: int.parse(startTimeParts[0]),
            minute: int.parse(startTimeParts[1]),
          );
          final endTime = TimeOfDay(
            hour: int.parse(endTimeParts[0]),
            minute: int.parse(endTimeParts[1]),
          );

          // Compare current time with start and end times
          Color cardColor;

          if (now.hour >= startTime.hour && now.hour <= endTime.hour) {
            cardColor = Colors.transparent;
          } else {
            cardColor = const Color.fromARGB(255, 255, 255, 255);
          }
// You can change this color as needed.
          if (maps[i]['status'] == 1) {
            cardColor = const Color.fromARGB(255, 126, 240, 130);
          }

          return Task(
            id: maps[i]['id'],
            name: maps[i]['name'],
            description: maps[i]['description'],
            label: maps[i]['label'],
            startTime: startTimeString,
            endTime: endTimeString,
            status: maps[i]['status'],
            date: customDateFormat.parse(maps[i]['date']),
            cardColor: cardColor,
          );
        } else {
          // Handle invalid time format by setting cardColor to a default color.
          return Task(
            id: maps[i]['id'],
            name: maps[i]['name'],
            description: maps[i]['description'],
            label: maps[i]['label'],
            startTime: startTimeString,
            endTime: endTimeString,
            status: maps[i]['status'],
            date: customDateFormat.parse(maps[i]['date']),
            cardColor: const Color.fromARGB(255, 177, 177, 177),
          );
        }
      });
    } catch (e) {
      print("Error fetching tasks: $e");
      return []; // Return an empty list in case of an error.
    }
  }

  // we would check if the start time is less then or equal to the current time hour

  final customDateFormat =
      DateFormat('dd/MM/yyyy'); // Define the custom date format

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: Container(
          decoration: BoxDecoration(
            shape: BoxShape.circle,
            border: Border.all(
                color: Colors.white, width: 2), // Customize border properties
          ),
          child: CircleAvatar(
            backgroundImage: FileImage(File(widget.imagePath)),
            radius: 20, // Adjust the size of the circular image
          ),
        ), // Load the image from the provided imagePath
        title: Text(
          widget.name,
          style: const TextStyle(
              fontFamily: 'ReadexPro', fontWeight: FontWeight.bold),
        ), // Display the name as the title
        // ... Your existing app bar code ...
        actions: [
          // Add a refresh button/icon in the app bar
          IconButton(
            icon: const Icon(Icons.refresh), // You can choose a different icon
            onPressed: () {
              // Implement the logic to refresh the data here
              openDatabaseAndFetchTasks(); // This will refresh the data
            },
          ),
        ],
      ),
      body: Center(
        child: Column(
          children: [
            if (noTasksFound)
              const Text(
                'No tasks found. Create a task to get started.',
                style: TextStyle(
                  fontSize: 18,
                  fontFamily: 'ReadexPro',
                  color: Color.fromARGB(
                      136, 0, 0, 0), // You can choose a different color.
                ),
              ),

            if (tasks.isNotEmpty)
              Container(
                margin: const EdgeInsets.only(right: 1290),
                child: const Text(
                  'Today task',
                  textAlign: TextAlign.left,
                  style: TextStyle(
                      fontFamily: 'ReadexPro',
                      fontWeight: FontWeight.bold,
                      fontSize: 20),
                ),
              ),
            const SizedBox(
              height: 20,
            ),
            SingleChildScrollView(
              child: Column(
                children: [
                  // Display the task-related data
                  for (final task in tasks)
                    Card(
                      elevation: 5,
                      margin: const EdgeInsets.only(bottom: 20),
                      child: Container(
                        color: task
                            .cardColor, // Set the background color based on cardColor
                        child: SizedBox(
                          // Set the background color based on cardColor
                          width: 800, // Set the desired width
                          child: GestureDetector(
                            onTap: () {
                              Navigator.push(
                                context,
                                MaterialPageRoute(
                                  builder: (context) => FifthScreen(
                                    id: task.id.toString(),
                                    heading: task.name,
                                    description: task.description,
                                    label: task.label,
                                    status: task.status.toString(),
                                    // Use an empty string if label is null.
                                  ),
                                ),
                              );
                            },
                            child: Padding(
                              padding: const EdgeInsets.all(16.0),
                              child: Row(
                                children: [
                                  // Image based on label
                                  Image.asset(
                                    task.label, // Assuming task.label is in the format 'assets/WorkLabel.png'
                                    height: 80,
                                    width: 80,
                                  ),
                                  const SizedBox(
                                    width:
                                        16, // Add some spacing between image and text
                                  ),
                                  // Task details
                                  Row(
                                    crossAxisAlignment:
                                        CrossAxisAlignment.start,
                                    children: [
                                      Column(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.start,
                                        children: [
                                          Text(
                                            task.name,
                                            style: const TextStyle(
                                              fontWeight: FontWeight.bold,
                                              fontSize: 24,
                                              fontFamily: 'ReadexPro',
                                            ),
                                          ),
                                        ],
                                      ),
                                      const SizedBox(
                                        width: 200,
                                      ),
                                      Column(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.center,
                                        children: [
                                          Text(
                                            task.startTime,
                                            style: const TextStyle(
                                              fontFamily: 'ReadexPro',
                                              fontWeight: FontWeight.bold,
                                              fontSize: 16,
                                            ),
                                          ),
                                          Text(
                                            task.endTime,
                                            style: const TextStyle(
                                              fontFamily: 'ReadexPro',
                                              fontWeight: FontWeight.bold,
                                              fontSize: 16,
                                            ),
                                          ),
                                          Text(
                                            customDateFormat.format(task.date),
                                            style: const TextStyle(
                                                fontFamily: 'ReadexPro',
                                                fontWeight: FontWeight.bold,
                                                fontSize: 18),
                                          )
                                          // here you can create a button to perform some action
                                        ],
                                      ),
                                    ],
                                  ),
                                ],
                              ),
                            ),
                          ),
                        ),
                      ),
                    ),
                  const SizedBox(
                    height: 20,
                  ),
                  SizedBox(
                    width: 400,
                    height: 60,
                    child: ElevatedButton(
                      onPressed: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (context) => const FourthScreen(),
                          ),
                        );
                      },
                      style: ElevatedButton.styleFrom(
                        backgroundColor: Colors.black,
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(10),
                        ),
                      ),
                      child: const Text(
                        'Create Task',
                        style: TextStyle(
                          fontSize: 20,
                          fontWeight: FontWeight.bold,
                          fontFamily: 'ReadexPro',
                          color: Colors.white,
                        ),
                      ),
                    ),
                  )
                ],
              ),
            )

            // Create a button to navigate to the FourthScreen
          ],
        ),
      ),
    );
  }
}

class Task {
  final int id;
  final String name;
  final String description;
  final String label;
  final String startTime;
  final String endTime;
  final int status;
  final DateTime date;
  final Color cardColor;

  Task(
      {required this.id,
      required this.name,
      required this.description,
      required this.label,
      required this.startTime,
      required this.endTime,
      required this.status,
      required this.date,
      required this.cardColor});
}
