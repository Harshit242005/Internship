import 'dart:async';

import 'package:flutter/material.dart';
import 'package:path/path.dart';
import 'package:sqflite_common_ffi/sqflite_ffi.dart';

class FifthScreen extends StatelessWidget {
  final String id;
  final String heading;
  final String description;
  final String label;
  final String status;

  const FifthScreen({
    required this.id,
    required this.heading,
    required this.description,
    required this.label,
    required this.status,
    Key? key,
  }) : super(key: key);

  // function to delete and update the code
  Future<void> deleteTask(int taskId) async {
    final databasePath = join(await getDatabasesPath(), 'Tasks.db');
    final database = await databaseFactoryFfi.openDatabase(databasePath);
    try {
      // Use the `delete` method to remove the row with the matching `id`.
      await database.delete('Tasks', where: 'id = ?', whereArgs: [taskId]);
    } catch (e) {
      print('Error deleting task: $e');
    }
    await database.close();

    // ignore: unused_element
  }

  Future<void> completeTask(int taskId) async {
    final databasePath = join(await getDatabasesPath(), 'Tasks.db');
    final database = await databaseFactoryFfi.openDatabase(databasePath);
    try {
      // Use the `update` method to set the `status` to 1 where `id` matches.
      await database.update('Tasks', {'status': 1},
          where: 'id = ?', whereArgs: [taskId]);
    } catch (e) {
      print('Error completing task: $e');
    }
    await database.close();
  }

  @override
  Widget build(BuildContext context) {
    int? checkInt = int.parse(status);
    String statusText = '';
    Color statusColor = Colors.black;
    int? integerId = int.parse(id);
    // ignore: unrelated_type_equality_checks
    if (checkInt == 0) {
      statusText = 'Incomplete';
      statusColor = Colors.red;
      // ignore: unrelated_type_equality_checks
    } else if (checkInt == 1) {
      statusText = 'Complete';
      statusColor = Colors.green;
    }

    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Task details',
          style:
              TextStyle(fontFamily: 'ReadexPro', fontWeight: FontWeight.bold),
        ),
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Image.asset(
              (label),
              height: 100, // Set the desired height for the image
              width: 100, // Set the desired width for the image
            ),
            // Display the heading
            Text(
              heading,
              style: const TextStyle(
                fontSize: 24,
                fontWeight: FontWeight.bold,
              ),
            ),

            // Display the description
            Text(
              description,
              style: const TextStyle(
                fontSize: 16,
              ),
            ),

            // Display the status (assuming it's a string)
            const Text(
              "Status: ",
              style: TextStyle(
                fontSize: 16,
                fontFamily: 'ReadexPro',
              ),
            ),
            Text(
              statusText,
              style: TextStyle(
                fontSize: 16,
                color: statusColor,
                fontFamily: 'ReadexPro',
              ),
            ),
            const SizedBox(
              height: 20,
            ),
            ElevatedButton(
              style: ElevatedButton.styleFrom(
                foregroundColor: Colors.white,
                backgroundColor: Colors.black, // Text color
                fixedSize: const Size(250, 50), // Width and height
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(10), // Border radius
                ),
              ),
              onPressed: () {
                // Add your button's action here
                deleteTask(integerId);
              },
              child: const Text(
                'Delete task',
                style: TextStyle(
                    fontFamily: 'ReadexPro',
                    fontWeight: FontWeight.w600,
                    fontSize: 20),
              ),
            ),
            const SizedBox(
              height: 20,
            ),
            ElevatedButton(
              style: ElevatedButton.styleFrom(
                foregroundColor: const Color.fromARGB(255, 0, 0, 0),
                backgroundColor:
                    const Color.fromARGB(255, 255, 255, 255), // Text color
                fixedSize: const Size(250, 50), // Width and height
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(10), // Border radius
                ),
              ),
              onPressed: () {
                // Add your button's action here
                completeTask(integerId);
              },
              child: const Text(
                'Complete task',
                style: TextStyle(
                    fontFamily: 'ReadexPro',
                    fontWeight: FontWeight.w600,
                    fontSize: 20),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
