// dialog_utils.dart

// ignore_for_file: prefer_const_constructors, sized_box_for_whitespace, avoid_print

import 'dart:io';

import 'package:flutter/material.dart';
import 'package:projecthub/mindmap.dart';

Future<void> showCreateFolderDialog(BuildContext context,
    {required String firstName}) async {
  String folderName = '';

  return showDialog<void>(
    context: context,
    builder: (BuildContext context) {
      return AlertDialog(
        backgroundColor:
            Color.fromARGB(255, 225, 225, 225), // Change the background color
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(10.0), // Add border radius
        ),
        content: Container(
          width: 425.0, // Set the width of the AlertDialog
          height: 200,
          child: Column(
            mainAxisAlignment: MainAxisAlignment.start,
            children: <Widget>[
              Text(
                'Creating project for $firstName...',
                style: TextStyle(
                    fontFamily: 'ReadexPro',
                    fontSize: 24,
                    color: Color.fromARGB(65, 0, 0, 0)),
              ),
              const SizedBox(
                height: 25,
              ),
              Container(
                padding: const EdgeInsets.all(0.0),
                decoration: BoxDecoration(
                  color: const Color.fromARGB(
                      255, 216, 215, 215), // Set background color for TextField
                  borderRadius: BorderRadius.circular(8.0),
                ),
                child: TextField(
                  onChanged: (value) {
                    folderName = value;
                  },
                  decoration: const InputDecoration(
                      contentPadding: EdgeInsets.only(left: 15),
                      labelText: 'Type project name...',
                      border: InputBorder.none, // Remove TextField border
                      labelStyle: TextStyle(
                          fontFamily: 'ReadexPro',
                          fontWeight: FontWeight.w600)),
                ),
              ),
              SizedBox(height: 16.0),
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceAround,
                children: <Widget>[
                  ElevatedButton(
                    style: ElevatedButton.styleFrom(
                      fixedSize: const Size(200, 50),
                      backgroundColor: const Color.fromARGB(255, 167, 166, 166),
                      shape: RoundedRectangleBorder(
                        borderRadius:
                            BorderRadius.circular(8.0), // Add border radius
                      ),
                    ),
                    onPressed: () {
                      print(
                          'Creating folder: $folderName for the user $firstName');
                      createDirectoryAndJsonFile(folderName, firstName);
                      Navigator.push(
                        context,
                        MaterialPageRoute(
                            builder: (context) => MindMap(
                                folderName: folderName, firstName: firstName)),
                      );
                    },
                    child: const Text(
                      'Create',
                      style: TextStyle(
                          fontFamily: 'ReadexPro',
                          fontSize: 16,
                          fontWeight: FontWeight.w500,
                          color: Colors.black),
                    ),
                  ),
                  const SizedBox(
                    width: 25,
                  ),
                  ElevatedButton(
                    style: ElevatedButton.styleFrom(
                      fixedSize: const Size(200, 50),
                      backgroundColor: const Color.fromARGB(255, 167, 166,
                          166), // Set background color for Cancel button
                      shape: RoundedRectangleBorder(
                        borderRadius:
                            BorderRadius.circular(8.0), // Add border radius
                      ),
                    ),
                    onPressed: () {
                      Navigator.of(context).pop();
                    },
                    child: const Text(
                      'Cancel',
                      style: TextStyle(
                          fontFamily: 'ReadexPro',
                          fontSize: 16,
                          fontWeight: FontWeight.w500,
                          color: Colors.black),
                    ),
                  ),
                ],
              ),
            ],
          ),
        ),
      );
    },
  );
}

void createDirectoryAndJsonFile(String folderName, String firstName) {
  String projectHubDirectory = 'projectHub';
  // Create the projectHub directory if it doesn't exist
  Directory projectHubDir = Directory(projectHubDirectory);
  if (!projectHubDir.existsSync()) {
    projectHubDir.createSync();
    print('Newly created directory is $projectHubDir');
  }

  // Create the subdirectory
  Directory firstNameDirectory = Directory('$projectHubDirectory/$firstName');
  if (!firstNameDirectory.existsSync()) {
    firstNameDirectory.createSync();
    print('Newly created directory is $firstNameDirectory');
  }

  // Create the subdirectory
  Directory directory =
      Directory('$projectHubDirectory/$firstName/$folderName');
  if (!directory.existsSync()) {
    directory.createSync();
    print('Newly created directory is $directory');
  }

  // Create the JSON file if it doesn't exist
  File jsonFile =
      File('$projectHubDirectory/$firstName/$folderName/$folderName.json');
  if (!jsonFile.existsSync()) {
    jsonFile.writeAsStringSync('{}');
  }
}
