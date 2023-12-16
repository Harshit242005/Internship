// ignore_for_file: prefer_const_constructors

import 'dart:io';

import 'package:flutter/material.dart';
import 'package:hive/hive.dart';
import 'package:projecthub/chooseproject.dart';
import 'user.dart'; // Import your User class
import 'package:file_picker/file_picker.dart';

class SignUpPage extends StatefulWidget {
  const SignUpPage({super.key});

  @override
  // ignore: library_private_types_in_public_api
  _SignUpPageState createState() => _SignUpPageState();
}

class _SignUpPageState extends State<SignUpPage> {
  final TextEditingController userImageController = TextEditingController();
  final TextEditingController firstNameController = TextEditingController();
  final TextEditingController lastNameController = TextEditingController();
  final TextEditingController emailController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();
  PlatformFile? file;
  String filePath = '';

  Future<void> picksinglefile() async {
    FilePickerResult? result = await FilePicker.platform.pickFiles();
    if (result != null) {
      file = result.files.first;
      if (file != null) {
        filePath = file!.path.toString();
        setState(() {
          userImageController.text = filePath;
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Sign Up',
          style:
              TextStyle(fontFamily: 'ReadexPro', fontWeight: FontWeight.w600),
        ),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              //crossAxisAlignment: CrossAxisAlignment.baseline,
              children: [
                // Display User Image or Default Image
                if (userImageController.text.isNotEmpty)
                  ClipOval(
                    child: Image.file(
                      File(userImageController.text),
                      width: 250,
                      height: 250,
                      fit: BoxFit.cover,
                    ),
                  )
                else
                  Image.asset(
                    'assets/default_profile_image.png', // Replace with your default image asset path
                    height: 200,
                    width: 200,
                  ),

                // Image Picker Button
                // IconButton with Camera Icon
                Container(
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    color: Colors.black,
                  ),
                  margin: EdgeInsets.only(top: 125, right: 500),
                  child: IconButton(
                    onPressed: picksinglefile,
                    icon: Icon(
                      Icons.camera_alt,
                      color: Colors.white,
                    ),
                    iconSize: 32, // Adjust the size as needed
                    splashColor: Colors.transparent, // Remove splash effect
                    highlightColor:
                        Colors.transparent, // Remove highlight effect
                    padding: EdgeInsets.all(16),
                    splashRadius: 28, // Adjust the splash radius as needed
                    tooltip: 'Pick Image',
                    focusColor: Colors.transparent,
                    hoverColor: Colors.transparent,
                    disabledColor: Colors.grey,
                  ),
                ),
              ],
            ),
            const SizedBox(height: 10),
            Row(
              children: [
                // First Name
                Container(
                  width: 250,
                  height: 75,
                  decoration: BoxDecoration(
                    border: Border.all(color: Colors.grey),
                    borderRadius: BorderRadius.circular(5),
                  ),
                  child: Padding(
                    padding: const EdgeInsets.only(left: 10.0, top: 15),
                    child: TextFormField(
                      controller: firstNameController,
                      decoration: const InputDecoration(
                        hintText: 'First Name',
                        hintStyle: TextStyle(
                            fontFamily: 'ReadexPro',
                            color: Color.fromARGB(100, 0, 0, 0),
                            fontSize: 18,
                            fontWeight: FontWeight.w600),
                        border: InputBorder.none,
                      ),
                      style: TextStyle(
                        fontFamily: 'ReadexPro',
                        fontWeight: FontWeight.w600,
                      ),
                    ),
                  ),
                ),
                const SizedBox(width: 16),

                // Last Name
                Container(
                  width: 250,
                  height: 75,
                  decoration: BoxDecoration(
                    border: Border.all(color: Colors.grey),
                    borderRadius: BorderRadius.circular(5),
                  ),
                  child: Padding(
                    padding: const EdgeInsets.only(left: 10.0, top: 10),
                    child: TextFormField(
                      controller: lastNameController,
                      decoration: const InputDecoration(
                        hintText: 'Last Name',
                        hintStyle: TextStyle(
                            fontFamily: 'ReadexPro',
                            color: Color.fromARGB(100, 0, 0, 0),
                            fontSize: 18,
                            fontWeight: FontWeight.w600),
                        border: InputBorder.none,
                      ),
                      style: TextStyle(
                        fontFamily: 'ReadexPro',
                        fontWeight: FontWeight.w600,
                      ),
                    ),
                  ),
                ),
              ],
            ),
            const SizedBox(
              height: 10,
            ),
            Column(
              children: [
                // Email
                Container(
                  width: 600,
                  height: 60,
                  decoration: BoxDecoration(
                    border: Border.all(color: Colors.grey),
                    borderRadius: BorderRadius.circular(5),
                  ),
                  child: Padding(
                    padding: const EdgeInsets.only(left: 10.0, top: 5),
                    child: TextFormField(
                      controller: emailController,
                      decoration: const InputDecoration(
                        hintText: 'Email',
                        hintStyle: TextStyle(
                            fontFamily: 'ReadexPro',
                            color: Color.fromARGB(100, 0, 0, 0),
                            fontSize: 18,
                            fontWeight: FontWeight.w600),
                        border: InputBorder.none,
                      ),
                      style: TextStyle(
                        fontFamily: 'ReadexPro',
                        fontWeight: FontWeight.w600,
                      ),
                    ),
                  ),
                ),
                const SizedBox(height: 10),

                // Password
                Container(
                  width: 600,
                  height: 60,
                  decoration: BoxDecoration(
                    border: Border.all(color: Colors.grey),
                    borderRadius: BorderRadius.circular(5),
                  ),
                  child: Padding(
                    padding: const EdgeInsets.only(left: 10.0, top: 5),
                    child: TextFormField(
                      controller: passwordController,
                      obscureText: true,
                      decoration: const InputDecoration(
                        hintText: 'Password',
                        hintStyle: TextStyle(
                            fontFamily: 'ReadexPro',
                            color: Color.fromARGB(100, 0, 0, 0),
                            fontSize: 18,
                            fontWeight: FontWeight.w600),
                        border: InputBorder.none,
                      ),
                      style: TextStyle(
                        fontFamily: 'ReadexPro',
                        fontWeight: FontWeight.w600,
                      ),
                    ),
                  ),
                ),
              ],
            ),
            const SizedBox(
              height: 10,
            ),
            Container(
              width: 200,
              height: 50,
              decoration: BoxDecoration(
                color: Colors.black,
                borderRadius: BorderRadius.circular(5),
              ),
              child: ElevatedButton(
                onPressed: () async {
                  // Save user data to Hive when signup button is pressed
                  await saveUserDataToHive();
                },
                style: ElevatedButton.styleFrom(
                  backgroundColor: Colors
                      .transparent, // Set the background color to transparent
                  shadowColor: Colors.transparent, // Remove shadow
                ),
                child: const Text(
                  'Sign Up',
                  style: TextStyle(
                      color: Colors.white,
                      fontFamily: 'ReadexPro',
                      fontWeight: FontWeight.w600,
                      fontSize: 16),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Future<void> saveUserDataToHive() async {
    // Open the Hive box
    final Box<User> userBox = await Hive.openBox<User>('user_data');

    // Create a User object with the entered data
    final User user = User()
      ..userImage = userImageController.text
      ..firstName = firstNameController.text
      ..lastName = lastNameController.text
      ..email = emailController.text
      ..password = passwordController.text;

    // Save the user data to Hive
    await userBox.add(user);

    // Close the Hive box
    await userBox.close();
    // Navigate to the LandingScreen
    // ignore: use_build_context_synchronously
    Navigator.pushReplacement(
      context,
      MaterialPageRoute(
          builder: (context) => LandingScreen(
                firstName: firstNameController.text,
              )),
    );
  }
}
