import 'package:flutter/material.dart';

import 'package:projecthub/folderpopup.dart';
import 'package:projecthub/oldprojects.dart'; // Update with the correct path

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return const MaterialApp(
      home: LandingScreen(
        firstName: '',
      ),
    );
  }
}

class LandingScreen extends StatelessWidget {
  final String firstName;

  const LandingScreen({Key? key, required this.firstName}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      // appBar: AppBar(
      //   title: const Text('Landing Screen'),
      // ),

      body: Container(
        decoration: const BoxDecoration(
          image: DecorationImage(
            image: AssetImage(
                'assets/Background.png'), // Replace with your image asset path
            fit: BoxFit.cover,
          ),
        ),
        child: Center(
          child: Column(
            children: [
              // here i will pass the text and the buttons for navigation
              Container(
                margin:
                    const EdgeInsets.only(right: 750.0, top: 50.0, left: 25),
                child: Row(
                  children: [
                    // Add your image widget here, for example:
                    Image.asset(
                      'assets/ProjectHubIcon.png',
                      height: 100, // Adjust the height as needed
                      width: 100, // Adjust the width as needed
                    ),
                    const SizedBox(
                        width: 8), // Adjust the spacing between image and text
                    const Text(
                      'ProjectHub',
                      style: TextStyle(
                        fontFamily: 'ReadexPro',
                        fontWeight: FontWeight.bold,
                        fontSize: 32,
                      ),
                    ),
                  ],
                ),
              ),

              Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Container(
                    margin: const EdgeInsets.only(top: 100),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        const Text(
                          'What would you like to do today?',
                          style: TextStyle(
                              fontWeight: FontWeight.w400,
                              fontSize: 24,
                              fontFamily: 'ReadexPro'),
                        ),
                        const SizedBox(height: 50),
                        Row(
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: [
                            ElevatedButton(
                              onPressed: () {
                                // Add functionality for the new project button
                                // in this we would first offer a pop up dialog first and then use that to create a folder
                                showCreateFolderDialog(context,
                                    firstName: firstName);
                              },
                              style: ElevatedButton.styleFrom(
                                fixedSize: const Size(
                                    300, 100), // Set the width and height
                                backgroundColor: const Color.fromARGB(
                                    148,
                                    255,
                                    255,
                                    255), // Set background color to transparent
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
                                    height: 50,
                                    width: 50,
                                  ),
                                  const SizedBox(
                                    width: 25,
                                  ),
                                  const Text(
                                    'New Project',
                                    style: TextStyle(
                                        fontFamily: 'ReadexPro',
                                        fontSize: 20,
                                        color: Colors.black,
                                        fontWeight: FontWeight.w400),
                                  ),
                                ],
                              ),
                            ),
                            const SizedBox(width: 50),
                            ElevatedButton(
                              onPressed: () {
                                // with this i would get to see a page where all the prject woud be shown
                                Navigator.push(
                                    context,
                                    MaterialPageRoute(
                                        builder: (context) => ProjectListPage(
                                            firstName: firstName)));
                              },
                              style: ElevatedButton.styleFrom(
                                fixedSize: const Size(300, 100),
                                backgroundColor:
                                    const Color.fromARGB(148, 255, 255, 255),
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
                                    height: 50,
                                    width: 50,
                                  ),
                                  const SizedBox(
                                    width: 10,
                                  ),
                                  const Text(
                                    'Open Project',
                                    style: TextStyle(
                                        fontFamily: 'ReadexPro',
                                        fontSize: 20,
                                        color: Colors.black,
                                        fontWeight: FontWeight.w400),
                                  ),
                                ],
                              ),
                            ),
                          ],
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }
}
