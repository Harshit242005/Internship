import 'dart:io';

import 'package:flutter/material.dart';
import 'package:hive/hive.dart';
import 'package:projecthub/folderpopup.dart';
import 'package:projecthub/oldprojects.dart';
import 'package:projecthub/setting.dart';
import 'package:projecthub/user.dart';

class CustomDialog extends StatelessWidget {
  final Box<User> userBox; // Pass your Hive box to the dialog
  final TextEditingController folderNameController = TextEditingController();

  CustomDialog({required this.userBox, Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    User? user = userBox.isNotEmpty ? userBox.get(0) : null;

    return Dialog(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(10.0),
      ),
      child: Container(
        width: 675,
        height: 550,
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(10),
          color: Colors.white,
        ),
        padding: const EdgeInsets.all(56.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text(
              'Profle Info',
              style: TextStyle(
                  fontSize: 40.0,
                  fontWeight: FontWeight.bold,
                  fontFamily: 'ReadexPro'),
            ),
            const SizedBox(height: 20.0),
            user != null
                ? Row(
                    children: [
                      ClipOval(
                        child: Image.file(
                          File(user
                              .userImage), // Assuming userImage is a local path
                          width: 100.0,
                          height: 100.0,
                          fit: BoxFit
                              .cover, // This ensures the image covers the oval shape
                        ),
                      ),
                      const SizedBox(width: 250.0),
                      Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Row(
                            children: [
                              Text(
                                user.firstName,
                                style: const TextStyle(
                                    fontFamily: 'ReadexPro',
                                    fontSize: 32,
                                    fontWeight: FontWeight.w600),
                              ),
                              const SizedBox(width: 5.0),
                              Text(user.lastName,
                                  style: const TextStyle(
                                      fontFamily: 'ReadexPro',
                                      fontSize: 32,
                                      fontWeight: FontWeight.w600)),
                            ],
                          ),
                          Row(
                            children: [
                              Text(
                                user.email,
                                style: const TextStyle(fontFamily: 'ReadexPro'),
                              ),
                            ],
                          ),
                        ],
                      ),
                    ],
                  )
                : const Text('No user data available'),
            // some button to deal with the user_data changes and other stuff
            const SizedBox(
              height: 25,
            ),
            Column(
              children: [
                ElevatedButton(
                  onPressed: () {
                    // this would give up the popup need to create the new project
                    showCreateFolderDialog(context, firstName: user!.firstName);
                  },
                  style: ElevatedButton.styleFrom(
                    fixedSize: const Size(550, 50), // Set the width and height
                    backgroundColor: const Color.fromARGB(255, 255, 214,
                        214), // Set background color to transparent
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(10),
                      // Set border radius to 0 for no border
                    ),
                  ),
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment
                        .center, // Center the content horizontally

                    children: [
                      Image.asset(
                        'assets/NewprojectIcon.png', // Replace with your image asset path
                        height: 25,
                        width: 25,
                      ),
                      const SizedBox(
                        width: 25,
                      ),
                      const Text(
                        'New Project',
                        style: TextStyle(
                            fontFamily: 'ReadexPro',
                            fontSize: 18,
                            color: Colors.black,
                            fontWeight: FontWeight.bold),
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: 10),
                ElevatedButton(
                  onPressed: () {
                    // when i would click this buttoni would get the show of all the other projects from the user
                    Navigator.push(
                        context,
                        MaterialPageRoute(
                            builder: (context) =>
                                ProjectListPage(firstName: user!.firstName)));
                  },
                  style: ElevatedButton.styleFrom(
                    fixedSize: const Size(550, 50), // Set the width and height
                    backgroundColor:
                        const Color.fromARGB(255, 255, 214, 214), // Set backgr
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(
                          10), // Set border radius to 0 for no border
                    ),
                  ),
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment
                        .center, // Center the content horizontally

                    children: [
                      Image.asset(
                        'assets/OpenprojectIcon.png', // Replace with your image asset path
                        height: 25,
                        width: 25,
                      ),
                      const SizedBox(
                        width: 10,
                      ),
                      const Text(
                        'Open Project',
                        style: TextStyle(
                            fontFamily: 'ReadexPro',
                            fontSize: 18,
                            color: Colors.black,
                            fontWeight: FontWeight.bold),
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: 10),
                ElevatedButton(
                  onPressed: () {
                    showDialog(
                      context: context,
                      builder: (BuildContext context) {
                        return AlertDialog(
                          title: const Text(
                            'Enter Folder Name to Delete',
                            style: TextStyle(
                                fontFamily: 'ReadexPro',
                                fontSize: 16,
                                fontWeight: FontWeight.w600),
                          ),
                          content: TextField(
                            controller: folderNameController,
                            onChanged: (value) {
                              // You can perform any live validation or updates here
                            },
                            decoration: InputDecoration(
                              contentPadding: const EdgeInsets.symmetric(
                                  horizontal: 15, vertical: 10),
                              hintText: 'Enter folder name',
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(10.0),
                                borderSide: const BorderSide(
                                    color: Colors.black, width: 2.0),
                              ),
                              focusedBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(10.0),
                                borderSide: const BorderSide(
                                    color: Colors.blue, width: 2.0),
                              ),
                            ),
                          ),
                          actions: [
                            ElevatedButton(
                              onPressed: () {
                                Navigator.of(context).pop();
                              },
                              style: ElevatedButton.styleFrom(
                                backgroundColor: Colors.black,
                                // Change the color to your preference
                                minimumSize:
                                    const Size(120, 40), // Set the minimum size
                                padding: const EdgeInsets.symmetric(
                                    horizontal: 20,
                                    vertical: 10), // Set padding
                                shape: RoundedRectangleBorder(
                                  borderRadius: BorderRadius.circular(
                                      5.0), // Set border radius
                                ),
                              ),
                              child: const Text(
                                'Cancel',
                                style: TextStyle(
                                    color: Colors.white,
                                    fontFamily: 'ReadexPro'),
                              ),
                            ),
                            ElevatedButton(
                              onPressed: () async {
                                deleteFolder(user!.firstName);
                                Navigator.of(context)
                                    .pop(); // Close the current dialog
                                Navigator.push(
                                  context,
                                  MaterialPageRoute(
                                    builder: (context) => ProjectListPage(
                                        firstName: user.firstName),
                                  ),
                                );
                              },
                              style: ElevatedButton.styleFrom(
                                backgroundColor: Colors
                                    .red, // Change the color to your preference
                                minimumSize:
                                    const Size(120, 40), // Set the minimum size
                                padding: const EdgeInsets.symmetric(
                                    horizontal: 20,
                                    vertical: 10), // Set padding
                                shape: RoundedRectangleBorder(
                                  borderRadius: BorderRadius.circular(
                                      5.0), // Set border radius
                                ),
                              ),
                              child: const Text(
                                'Delete',
                                style: TextStyle(
                                    fontFamily: 'ReadexPro',
                                    color: Colors.white),
                              ),
                            ),
                          ],
                        );
                      },
                    );
                  },
                  style: ElevatedButton.styleFrom(
                    fixedSize: const Size(550, 50), // Set the width and height
                    backgroundColor:
                        const Color.fromARGB(255, 255, 214, 214), // Set backgr
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(
                          10), // Set border radius to 0 for no border
                    ),
                  ),
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment
                        .center, // Center the content horizontally

                    children: [
                      Image.asset(
                        'assets/DeleteIcon.png', // Replace with your image asset path
                        height: 25,
                        width: 25,
                      ),
                      const SizedBox(
                        width: 10,
                      ),
                      const Text(
                        'Delete Project',
                        style: TextStyle(
                            fontFamily: 'ReadexPro',
                            fontSize: 18,
                            color: Colors.black,
                            fontWeight: FontWeight.bold),
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: 10),
                ElevatedButton(
                  onPressed: () {
                    // Setting icon where we would be going to setting page to change some data place
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (context) =>
                            SettingPage(firstName: user!.firstName),
                      ),
                    );
                  },
                  style: ElevatedButton.styleFrom(
                    fixedSize: const Size(550, 50), // Set the width and height
                    backgroundColor:
                        const Color.fromARGB(255, 255, 214, 214), // Set backgr
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(
                          10), // Set border radius to 0 for no border
                    ),
                  ),
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment
                        .center, // Center the content horizontally

                    children: [
                      Image.asset(
                        'assets/SettingIcon.png', // Replace with your image asset path
                        height: 25,
                        width: 25,
                      ),
                      const SizedBox(
                        width: 10,
                      ),
                      const Text(
                        'Settings',
                        style: TextStyle(
                            fontFamily: 'ReadexPro',
                            fontSize: 18,
                            color: Colors.black,
                            fontWeight: FontWeight.bold),
                      ),
                    ],
                  ),
                ),
              ],
            )
          ],
        ),
      ),
    );
  }

  void deleteFolder(String firstName) {
    String folderName = folderNameController.text.trim();
    String folderPath = './projectHub/$firstName/$folderName';
    deleteFolderRecursively(folderPath);
    // You can add additional logic or feedback after deletion if needed
  }

  void deleteFolderRecursively(String folderPath) {
    Directory folder = Directory(folderPath);

    if (folder.existsSync()) {
      // List all entries (files and subdirectories) in the folder
      folder.listSync(recursive: true).forEach((FileSystemEntity entity) {
        if (entity is File) {
          // Delete the file
          entity.deleteSync();
        } else if (entity is Directory) {
          // Delete the subdirectory and its contents recursively
          entity.deleteSync(recursive: true);
        }
      });

      // Delete the main folder
      folder.deleteSync();
    } else {
      print('Folder not found: $folderPath');
    }
  }
}
