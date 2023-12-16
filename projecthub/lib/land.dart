// ignore_for_file: library_private_types_in_public_api, avoid_print, prefer_const_constructors, non_constant_identifier_names
import 'dart:convert';
import 'dart:io';
import 'package:flutter/material.dart';
import 'package:hive/hive.dart';
import 'package:projecthub/sidenav.dart';
import 'package:projecthub/user.dart';

import 'package:url_launcher/url_launcher.dart';

void main() {
  runApp(MyApp(
    folderName: '',
    firstName: '',
  ));
}

class DragTargetData {
  bool booleanValue;
  int index;

  DragTargetData(this.booleanValue, this.index);
}

class MyApp extends StatelessWidget {
  const MyApp({Key? key, required this.folderName, required this.firstName})
      : super(key: key);
  final String folderName;
  final String firstName;
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        appBar: AppBar(
          backgroundColor: Color.fromARGB(255, 0, 0, 0),
          leading: IconButton(
            icon: const Icon(
              Icons.more_vert_rounded,
              color: Color.fromARGB(255, 133, 182, 255),
            ),
            onPressed: () async {
              // Add the functionality you want when the menu button is pressed
              // ignore: unused_local_variable
              // Open the 'user_data' Hive box
              // Open the 'user_data' Hive box
              final Box<User> userDataBox =
                  await Hive.openBox<User>('user_data');

              final User storedUser = userDataBox.values.firstWhere(
                (user) => user.firstName == firstName,
                orElse: () => NonExistingUser(),
              );
              print('the box on which the first name exist $storedUser');
              print('the first name is $firstName');

              // ignore: use_build_context_synchronously
              showDialog(
                context: context,
                builder: (BuildContext context) {
                  return CustomDialog(userBox: userDataBox);
                },
              );
            },
          ),
          title: Text(
            folderName.isNotEmpty ? folderName : 'Default Title',
            style:
                TextStyle(fontFamily: 'ReadexPro', fontWeight: FontWeight.w600),
          ),
          actions: [
            FutureBuilder(
              future: Hive.openBox<User>('user_data'),
              builder: (context, snapshot) {
                if (snapshot.connectionState == ConnectionState.done) {
                  final Box<User> userBox = Hive.box<User>('user_data');
                  final User storedUser = userBox.values.firstWhere(
                    (user) => user.firstName == firstName,
                    orElse: () => NonExistingUser(),
                  );

                  // Display the user's image in the rightmost corner of the AppBar
                  return Padding(
                    padding: const EdgeInsets.all(8.0),
                    child: ClipOval(
                      child: Image.file(
                        File(storedUser.userImage),
                        width: 32,
                        height: 32,
                        fit: BoxFit.cover,
                      ),
                    ),
                  );
                }
                // Return an empty container if user data is not loaded yet
                return Container();
              },
            ),
          ],
        ),
        body: MyDragTargets(folderName: folderName, firstName: firstName),
        bottomNavigationBar: BottomAppBar(
          child: SizedBox(
            height: 50,
            child: MyScrollableAppBar(),
          ),
        ),
      ),
    );
  }
}

class MyDragTargets extends StatefulWidget {
  const MyDragTargets(
      {Key? key, required this.folderName, required this.firstName})
      : super(key: key);
  final String folderName;
  final String firstName;
  @override
  _MyDragTargetsState createState() => _MyDragTargetsState();
}

// ignore: duplicate_ignore
class _MyDragTargetsState extends State<MyDragTargets> {
  List<DragTargetData> dragTargets = [];

  // we have introduced a map here to keep track of the image with the index of the container
  Map<int, String?> dragTargetImagePaths = {};

  @override
  void initState() {
    super.initState();
    // Add an initial DragTarget with booleanValue set to false
    dragTargets.add(DragTargetData(false, 1));
    dragTargets.add(DragTargetData(false, 2));
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        for (int i = 0; i < dragTargets.length; i += 4)
          Column(
            children: [
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  for (int j = i; j < i + 4 && j < dragTargets.length; j++)
                    buildDragTarget(
                      dragTargets[j].booleanValue,
                      dragTargets[j].index,
                      (int index, String imagePath) {
                        print('the index $index has the image $imagePath');
                        setState(() {
                          dragTargetImagePaths[index] = imagePath;
                        });
                        print(dragTargetImagePaths);
                        String pathData = dragTargetImagePaths[j] ?? '';
                        print('the data of the image from dict $pathData');
                      },
                    ),
                ],
              ),
              const SizedBox(height: 20), // Adjust the height as needed
            ],
          ),
      ],
    );
  }

  Widget buildDragTarget(
    bool booleanValue,
    int index,
    Function(int, String) onImageChanged,
  ) {
    return DragTarget<String>(
      builder: (BuildContext context, List<String?> accepted,
          List<dynamic> rejected) {
        String? imagePath = accepted.isNotEmpty ? accepted.first : null;
        print('the image path is $imagePath');
        return Container(
            margin: const EdgeInsets.only(left: 25, top: 25),
            width: 200,
            height: 200,
            decoration: BoxDecoration(
                color: booleanValue
                    ? const Color.fromARGB(138, 214, 210, 210)
                    : const Color.fromARGB(99, 244, 244, 244),
                borderRadius: BorderRadius.circular(10),
                border: Border.all(
                    width: 0.5, color: const Color.fromARGB(51, 0, 0, 0)),
                // here we would place the image directly by myself using the Map
                image: DecorationImage(
                    image: AssetImage(dragTargetImagePaths[index] ?? ''),
                    alignment: Alignment.center)
                // image: const DecorationImage(
                //     image: AssetImage(
                //         'assets/VScodeIcon.png'), // Replace with your image asset path
                //     alignment: Alignment.center),
                ),
            child: Tooltip(
              message: 'DragTarget $index',
              child: Stack(
                children: [
                  Center(
                    child: Container(
                      decoration: const BoxDecoration(
                        shape: BoxShape.circle,
                        color: Color.fromARGB(255, 255, 255, 255),
                      ),
                      child: booleanValue
                          ? const SizedBox.shrink()
                          : IconButton(
                              onPressed: () {
                                bool secondLastBooleanValue =
                                    dragTargets.isEmpty
                                        ? false
                                        : dragTargets[dragTargets.length - 2]
                                            .booleanValue;
                                print(
                                    'Second last drag target boolean value is $secondLastBooleanValue');
                                if (dragTargets.isEmpty ||
                                    secondLastBooleanValue) {
                                  setState(() {
                                    int newIndex = dragTargets.length + 1;
                                    print(
                                        'The new index for the new drag target $newIndex');
                                    dragTargets.add(
                                      DragTargetData(false, newIndex),
                                    );
                                  });
                                }
                              },
                              icon: const Icon(
                                Icons.add,
                                size: 40,
                                color: Colors.black,
                              ),
                            ),
                    ),
                  ),
                  if (booleanValue)
                    Positioned(
                      top: 5,
                      right: 5,
                      child: Tooltip(
                        message: 'More',
                        child: IconButton(
                          icon: Row(
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: const [
                              Icon(
                                Icons.more_vert,
                                color: Colors.black,
                              ),
                              SizedBox(width: 4),
                              Text(
                                'More',
                                style: TextStyle(
                                    fontFamily: 'ReadexPro',
                                    fontWeight: FontWeight.w600),
                              ),
                            ],
                          ),
                          onPressed: () {
                            showDialog(
                              context: context,
                              builder: (BuildContext context) {
                                return Dialog(
                                  shape: const RoundedRectangleBorder(
                                    borderRadius:
                                        BorderRadius.all(Radius.circular(20)),
                                  ),
                                  child: ClipRRect(
                                    borderRadius:
                                        BorderRadius.all(Radius.circular(20)),
                                    child: Container(
                                      width: 200,
                                      color: const Color.fromARGB(255, 0, 0, 0),
                                      padding: EdgeInsets.zero,
                                      child: Column(
                                        mainAxisSize: MainAxisSize.min,
                                        children: [
                                          IconButton(
                                            onPressed: () {
                                              // here the open button would open the file
                                              // we would just have one function that would be called with index
                                              OpenFile(index);
                                            },
                                            icon: Row(
                                              mainAxisAlignment:
                                                  MainAxisAlignment.center,
                                              children: const [
                                                Icon(Icons.folder_open),
                                                SizedBox(width: 15),
                                                Text(
                                                  'Open',
                                                  style: TextStyle(
                                                      fontFamily: 'ReadexPro',
                                                      fontSize: 20,
                                                      fontWeight:
                                                          FontWeight.w600,
                                                      color: Color.fromARGB(
                                                          255, 227, 225, 225)),
                                                ),
                                              ],
                                            ),
                                          ),
                                          IconButton(
                                            onPressed: () {
                                              // Add functionality for the Details button here we would open up the file as text of the ide and show it
                                              showIdea(index);
                                            },
                                            icon: Row(
                                              mainAxisAlignment:
                                                  MainAxisAlignment.center,
                                              children: const [
                                                Icon(Icons.info),
                                                SizedBox(width: 15),
                                                Text(
                                                  'Details',
                                                  style: TextStyle(
                                                      fontFamily: 'ReadexPro',
                                                      fontSize: 20,
                                                      fontWeight:
                                                          FontWeight.w600,
                                                      color: Color.fromARGB(
                                                          255, 227, 225, 225)),
                                                ),
                                              ],
                                            ),
                                          ),
                                          IconButton(
                                            onPressed: () {
                                              // function calling for deleting the key value pair at the index from JSON file
                                              DeleteKeyValue(index);
                                            },
                                            icon: Row(
                                              mainAxisAlignment:
                                                  MainAxisAlignment.center,
                                              children: const [
                                                Icon(Icons.delete),
                                                SizedBox(width: 15),
                                                Text(
                                                  'Delete',
                                                  style: TextStyle(
                                                      fontFamily: 'ReadexPro',
                                                      fontSize: 20,
                                                      fontWeight:
                                                          FontWeight.w600,
                                                      color: Color.fromARGB(
                                                          255, 227, 225, 225)),
                                                ),
                                              ],
                                            ),
                                          ),
                                        ],
                                      ),
                                    ),
                                  ),
                                );
                              },
                            );
                          },
                        ),
                      ),
                    )
                ],
              ),
            ));
      },
      onAccept: (String? data) {
        print('Data accepted on DragTarget $index: $data');
        if (data != null) {
          setState(() {
            dragTargets[index - 1].booleanValue = true;
            onImageChanged(index, data);
          });

          // Check if the data path contains the word "notepad" (case insensitive)
          if (RegExp(r'notepad', caseSensitive: false).hasMatch(data)) {
            // Open Notepad using Process.run
            //openNotepad();
            showCreateFileDialog(context, index);
          } else if (RegExp(r'figma', caseSensitive: false).hasMatch(data)) {
            // Open Figma (replace the comment with the actual code to open Figma)
            openFigma();
          } else if (RegExp(r'glitch', caseSensitive: false).hasMatch(data)) {
            // Open Glitch (replace the comment with the actual code to open Glitch)
            openGlitch();
          } else if (RegExp(r'codesandbox', caseSensitive: false)
              .hasMatch(data)) {
            // Open Codesandbox (replace the comment with the actual code to open Codesandbox)
            openCodesandbox();
          } else if (RegExp(r'onenote', caseSensitive: false).hasMatch(data)) {
            // Open Codesandbox (replace the comment with the actual code to open Codesandbox)
            openOnenote();
          } else if (RegExp(r'photoshop', caseSensitive: false)
              .hasMatch(data)) {
            // Open Codesandbox (replace the comment with the actual code to open Codesandbox)
            openPhotoShop();
          }
          // this will update the Json file with the index and data which is asset image for
          // later opening the file
          updateJsonFile(index, data);
          showTextDialog(index);
          // for creating the direectory and Json file related to it
          //createDirectoryAndJsonFile(widget.folderName);
        }
      },
    );
  }

  // }

  Future<void> showTextDialog(int index) async {
    String textFieldValue = ''; // This will store the text from the text field

    return showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: Text(
            'Type idea here...',
            style:
                TextStyle(fontFamily: 'ReadexPro', fontWeight: FontWeight.bold),
          ),
          content: Container(
            decoration: BoxDecoration(
              border:
                  Border.all(color: Colors.black), // Add border to TextField
            ),
            child: TextField(
              onChanged: (value) {
                textFieldValue = value;
              },
              decoration: InputDecoration(
                hintText: 'Type...',
                hintStyle: TextStyle(fontFamily: 'ReadexPro'),
                contentPadding:
                    EdgeInsets.symmetric(horizontal: 16), // Adjust padding
              ),
            ),
          ),
          actions: [
            ElevatedButton(
              onPressed: () {
                Navigator.pop(context); // Close the dialog on Cancel
              },
              style: ElevatedButton.styleFrom(
                  foregroundColor: Colors.white,
                  backgroundColor:
                      Color.fromARGB(255, 0, 0, 0), // Set text color
                  padding: EdgeInsets.all(12), // Adjust padding
                  fixedSize: Size(200, 40)),
              child: Text('Cancel', style: TextStyle(fontFamily: 'ReadexPro')),
            ),
            ElevatedButton(
              onPressed: () {
                // Save the text to the JSON file for the corresponding index
                saveTextToJson(index, textFieldValue);
                Navigator.pop(context); // Close the dialog on Save
              },
              style: ElevatedButton.styleFrom(
                  foregroundColor: Colors.white,
                  backgroundColor:
                      Color.fromARGB(255, 82, 255, 85), // Set text color
                  padding: EdgeInsets.all(12), // Adjust padding
                  fixedSize: Size(200, 40)),
              child: Text(
                'Save',
                style: TextStyle(fontFamily: 'ReadexPro'),
              ),
            ),
          ],
        );
      },
    );
  }

  // Function to save the entered text to the JSON file for the given index
  Future<void> saveTextToJson(int index, String text) async {
    Map<String, dynamic> jsonData =
        loadJsonFile(widget.folderName, widget.firstName);
    print('json data is $jsonData');

    // Update the data for the current index
    if (jsonData.containsKey(index.toString())) {
      jsonData[index.toString()]?.add(text);
    } else {
      jsonData[index.toString()] = [text];
    }

    // Save the updated JSON data to the file
    saveJsonFile(widget.folderName, jsonData, widget.firstName);
  }

  void showIdea(int index) {
    String jsonFileIndex = index.toString();
    Map<String, dynamic> jsonData =
        loadJsonFile(widget.folderName, widget.firstName);

    // Check if the key exists in the jsonData map
    if (jsonData.containsKey(jsonFileIndex)) {
      // Check if the value at the key is a List
      if (jsonData[jsonFileIndex] is List) {
        // Cast the dynamic value to a List<dynamic>
        List dynamicList = jsonData[jsonFileIndex];

        // Check if the list is not empty
        if (dynamicList.isNotEmpty) {
          // Get the last value in the list
          dynamic lastValue = dynamicList.last;

          // Convert the dynamic value to String or handle it as needed
          String lastStringValue = lastValue.toString();

          // Now you have the last value stored in the list
          print('Last value in the list: $lastStringValue');

          // Open a dialog with a text field, cancel, and edit buttons
          showDialog(
            context: context,
            builder: (BuildContext context) {
              String textFieldValue =
                  lastStringValue; // Initial value for the text field

              return AlertDialog(
                title: Text('Edit Idea'),
                content: TextField(
                  controller: TextEditingController(text: textFieldValue),
                ),
                actions: [
                  ElevatedButton(
                    onPressed: () {
                      Navigator.pop(context); // Close the dialog on Cancel
                    },
                    child: Text('Cancel'),
                  ),
                  ElevatedButton(
                    onPressed: () {
                      // Handle the edit action with the text field value
                      saveTextToJson(index, textFieldValue);

                      Navigator.pop(context); // Close the dialog on Edit
                    },
                    child: Text('Edit'),
                  ),
                ],
              );
            },
          );
        } else {
          print('The list is empty.');
        }
      } else {
        print('The value at key $jsonFileIndex is not a List.');
      }
    } else {
      print('Key $jsonFileIndex does not exist in the JSON data.');
    }
  }

  void OpenFile(int Index) async {
    String jsonFileIndex = Index.toString();
    Map<String, dynamic> jsonData =
        // ignore: await_only_futures
        await loadJsonFile(widget.folderName, widget.firstName);

    // Check if the key exists in the jsonData map
    if (jsonData.containsKey(jsonFileIndex)) {
      // Check if the value at the key is a List
      if (jsonData[jsonFileIndex] is List) {
        // Cast the dynamic value to a List<dynamic>
        List dynamicList = jsonData[jsonFileIndex];

        // Check if the list is not empty
        if (dynamicList.isNotEmpty) {
          // Get the last value in the list
          dynamic lastValue = dynamicList.last;

          // Convert the dynamic value to String or handle it as needed
          String lastStringValue = lastValue.toString();

          // Now you have the last value stored in the list
          print('Last value in the list: $lastStringValue');
          if (lastStringValue.endsWith(".txt")) {
            // Call the openTextFile function with the File object
            openTextFile(File(lastStringValue));
          } else {
            print('The last value does not have a ".txt" extension.');
          }
        } else {
          print('The list is empty.');
        }
      } else {
        print('The value at key $jsonFileIndex is not a List.');
      }
    } else {
      print('Key $jsonFileIndex does not exist in the JSON data.');
    }
  }

  void DeleteKeyValue(int Index) async {
    String jsonFileIndex = Index.toString();
    Map<String, dynamic> jsonData =
        await loadJsonFile(widget.folderName, widget.firstName);

    // Check if the key exists in the jsonData map
    if (jsonData.containsKey(jsonFileIndex)) {
      // Remove the key-value pair from the map
      jsonData.remove(jsonFileIndex);

      // Save the updated jsonData back to the file
      saveJsonFile(widget.folderName, jsonData, widget.firstName);

      print('Key $jsonFileIndex and its value deleted successfully.');
    } else {
      print('Key $jsonFileIndex does not exist in the JSON data.');
    }
  }

  // to add data in the JSON file
  void updateJsonFile(int index, String data) {
    print("called this function for adding the data on the JSON file");
    // Get the current data from the JSON file
    Map<String, dynamic> jsonData =
        loadJsonFile(widget.folderName, widget.firstName);
    print('inital json data $json');
    // Update the data for the current index
    jsonData[index.toString()] = [data];
    String jsondataindex = index.toString();
    print('the json data is getitng loaded $jsondataindex with the data $data');
    // Save the updated data back to the JSON file
    saveJsonFile(widget.folderName, jsonData, widget.firstName);
  }

  Map<String, dynamic> loadJsonFile(String folderName, String firstName) {
    try {
      // Read the content of the JSON file
      File jsonFile =
          File('projectHub/$firstName/$folderName/$folderName.json');
      String jsonContent = jsonFile.readAsStringSync();
      print('Json file content is $jsonContent');
      // Parse the JSON content
      Map<String, dynamic> jsonData = json.decode(jsonContent);

      return jsonData;
    } catch (e) {
      print('Error loading JSON file: $e');
      return {};
    }
  }

  void saveJsonFile(
      String folderName, Map<String, dynamic> data, String firstName) {
    // Create a JSON file with the same name as the directory
    File jsonFile = File('projectHub/$firstName/$folderName/$folderName.json');

    // Convert the data to JSON format
    String jsonData = jsonEncode(data);
    print('the json data $jsonData');
    // Write the JSON data to the file
    jsonFile.writeAsStringSync(jsonData);

    print('JSON file saved: $jsonFile');
  }

  void openFigma() async {
    const String figmaUrl = 'https://www.figma.com/';

    try {
      // ignore: deprecated_member_use
      if (await canLaunch(figmaUrl)) {
        // ignore: deprecated_member_use
        await launch(figmaUrl);
      } else {
        print('Could not launch $figmaUrl');
      }
    } catch (e) {
      print('Error opening Figma: $e');
    }
  }

  void openPhotoShop() async {
    const String onenoteUlr =
        'https://www.adobe.com/in/products/photoshop.html';
    try {
      // ignore: deprecated_member_use
      if (await canLaunch(onenoteUlr)) {
        // ignore: deprecated_member_use
        await launch(onenoteUlr);
      } else {
        print('Could not launch $onenoteUlr');
      }
    } catch (e) {
      print('Error opening Figma: $e');
    }
  }

  void openOnenote() async {
    const String onenoteUlr = 'https://www.onenote.com/hrd';
    try {
      // ignore: deprecated_member_use
      if (await canLaunch(onenoteUlr)) {
        // ignore: deprecated_member_use
        await launch(onenoteUlr);
      } else {
        print('Could not launch $onenoteUlr');
      }
    } catch (e) {
      print('Error opening Figma: $e');
    }
  }

  // function for the glitch open
  void openGlitch() async {
    const String glitchUrl = 'https://glitch.com/';
    try {
      // ignore: deprecated_member_use
      if (await canLaunch(glitchUrl)) {
        // ignore: deprecated_member_use
        await launch(glitchUrl);
      } else {
        print('Could not launch $glitchUrl');
      }
    } catch (e) {
      print('Error opening Figma: $e');
    }
  }

  void openCodesandbox() async {
    const String sandboxUrl = 'https://codesandbox.io/';
    try {
      // ignore: deprecated_member_use
      if (await canLaunch(sandboxUrl)) {
        // ignore: deprecated_member_use
        await launch(sandboxUrl);
      } else {
        print('Could not launch $sandboxUrl');
      }
    } catch (e) {
      print('Error opening Figma: $e');
    }
  }

  void showCreateFileDialog(BuildContext context, int Index) {
    String fileName = '';

    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: Text('Create Text File in ${widget.folderName}'),
          content: TextField(
            onChanged: (value) {
              fileName = value;
            },
            decoration: InputDecoration(labelText: 'Type file name...'),
          ),
          actions: <Widget>[
            TextButton(
              onPressed: () {
                Navigator.of(context).pop(); // Close the dialog
              },
              child: Text('Cancel'),
            ),
            TextButton(
              onPressed: () {
                if (fileName.isNotEmpty) {
                  createAndOpenTextFile(
                      widget.folderName, fileName, Index, widget.firstName);
                  Navigator.of(context).pop(); // Close the dialog
                }
              },
              child: Text('Create'),
            ),
          ],
        );
      },
    );
  }

  Future<void> createAndOpenTextFile(
      String folderName, String fileName, int Index, String firstName) async {
    // Create the text file inside the directory
    File file = File('projectHub/$firstName/$folderName/$fileName.txt');
    file.writeAsStringSync('');
    UpdateJsonTextFileAdd(folderName, fileName, Index, firstName);
    // Open the text file
    openTextFile(file);
  }

  void UpdateJsonTextFileAdd(
      String folderNAME, String filenName, int Index, String firstName) async {
    String indexValue = Index.toString();
    Map<String, dynamic> jsonData =
        loadJsonFile(widget.folderName, widget.firstName);
    String filePath = 'projectHub/$firstName/$folderNAME/$filenName.txt';

    // Check if the key exists in the jsonData map
    if (jsonData.containsKey(indexValue)) {
      // Check if the value at the key is a List
      if (jsonData[indexValue] is List) {
        // Cast the dynamic value to a List<dynamic>
        List<dynamic> dynamicList = jsonData[indexValue];

        // Add the filePath to the end of the list
        dynamicList.add(filePath);

        // Update the value in the jsonData map
        jsonData[indexValue] = dynamicList;

        // Save the updated jsonData to the file
        saveJsonFile(widget.folderName, jsonData, widget.firstName);
      }
    }
  }

  void openTextFile(File file) async {
    try {
      await Process.run('notepad.exe', [file.path]);
    } catch (e) {
      print('Error opening text file: $e');
    }
  }
}

class MyScrollableAppBar extends StatelessWidget {
  MyScrollableAppBar({Key? key}) : super(key: key);

  final List<String> imagePaths = [
    'assets/FigmaIcon.png',
    'assets/VScodeIcon.png',
    'assets/NotepadIcon.png',
    'assets/PhotoShopIcon.png',
    'assets/OnenoteIcon.png',
    'assets/NotionIcon.png',
  ];

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
      scrollDirection: Axis.horizontal,
      itemCount: imagePaths.length,
      itemBuilder: (context, index) {
        return Draggable<String>(
          data: imagePaths[index],
          feedback: Image.asset(imagePaths[index], height: 100, width: 100),
          child: Container(
            margin: const EdgeInsets.only(left: 25),
            decoration: BoxDecoration(
                border: Border.all(
                    width: 1, color: const Color.fromARGB(25, 0, 0, 0)),
                borderRadius: BorderRadius.circular(10)),
            padding: const EdgeInsets.all(8.0),
            child: Image.asset(imagePaths[index], height: 50, width: 50),
          ),
        );
      },
    );
  }
}

class NonExistingUser extends User {
  // You can add any additional fields or methods specific to a non-existing user

  void showUserDoesNotExistDialog(BuildContext context) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('User Not Found'),
        content: const Text('The entered email does not exist.'),
        actions: [
          TextButton(
            onPressed: () {
              Navigator.pop(context); // Close the dialog
            },
            child: const Text('OK'),
          ),
        ],
      ),
    );
  }
}
