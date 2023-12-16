import 'dart:io';

import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:hive/hive.dart';
import 'package:projecthub/user.dart';

class SettingPage extends StatefulWidget {
  final String firstName;
  const SettingPage({Key? key, required this.firstName}) : super(key: key);
  @override
  // ignore: library_private_types_in_public_api
  _SettingPageState createState() => _SettingPageState();
}

class _SettingPageState extends State<SettingPage> {
  final TextEditingController userImageController = TextEditingController();
  PlatformFile? file;
  String filePath = '';
  Future<void> pickSingleFile() async {
    FilePickerResult? result = await FilePicker.platform.pickFiles();
    if (result != null) {
      file = result.files.first;
      if (file != null) {
        filePath = file!.path.toString();
        final Box<User> userBox = Hive.box<User>('user_data');
        // Find the user in userBox
        User? storedUser = userBox.values.firstWhere(
          (user) => user.firstName == widget.firstName,
          orElse: () => NonExistingUser(),
        );
        // Update the userImage property
        storedUser.userImage = filePath;
        // Save the changes to the userBox
        await userBox.put(storedUser.key, storedUser);
        setState(() {
          userImageController.text = filePath;
        });
      }
    }
  }

  Future<void> _showChangePasswordDialog(
      BuildContext context, Box<User> userBox) async {
    String newPassword = '';

    return showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text(
            'Change Password',
            style: TextStyle(
                fontFamily: 'ReadexPro',
                fontSize: 18,
                fontWeight: FontWeight.bold),
          ),
          content: TextField(
            onChanged: (value) {
              newPassword = value;
            },
            obscureText: true,
            decoration: InputDecoration(
              labelText: 'New Password',
              labelStyle: const TextStyle(
                fontFamily: 'ReadexPro',
                fontWeight: FontWeight.w400,
              ),
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(10),
              ),
              contentPadding:
                  const EdgeInsets.symmetric(vertical: 16, horizontal: 16),
            ),
          ),
          actions: [
            ElevatedButton(
              onPressed: () {
                Navigator.pop(context);
              },
              style: ElevatedButton.styleFrom(
                foregroundColor: Colors.white,
                backgroundColor: Colors.black,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(10),
                ),
                fixedSize: const Size(200, 40),
              ),
              child: const Text(
                'Cancel',
                style: TextStyle(
                    fontFamily: 'ReadexPro',
                    fontSize: 15,
                    fontWeight: FontWeight.w600),
              ),
            ),
            ElevatedButton(
              onPressed: () async {
                if (newPassword.isNotEmpty) {
                  User? storedUser = userBox.values.firstWhere(
                    (user) => user.firstName == widget.firstName,
                    orElse: () => NonExistingUser(),
                  );

                  String oldPassword = storedUser.password;
                  // ignore: avoid_print
                  print(
                      'the old password was this $oldPassword and new password is $newPassword');

                  storedUser.password = newPassword;

                  await userBox.put(storedUser.key, storedUser);

                  // ignore: use_build_context_synchronously
                  Navigator.pop(context);
                }
              },
              style: ElevatedButton.styleFrom(
                foregroundColor: Colors.white,
                backgroundColor: const Color.fromARGB(255, 42, 255, 49),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(10),
                ),
                fixedSize: const Size(200, 40),
              ),
              child: const Text(
                'Update',
                style: TextStyle(
                    fontFamily: 'ReadexPro',
                    fontSize: 15,
                    fontWeight: FontWeight.w600),
              ),
            ),
          ],
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    final Box<User> userBox = Hive.box<User>('user_data');

    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Settings',
          style: TextStyle(
              fontFamily: 'ReadexPro',
              fontSize: 20,
              fontWeight: FontWeight.bold),
        ),
      ),
      body: Align(
        alignment: Alignment.topLeft,
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.start,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'Welcome to the Settings Page, ${widget.firstName}!',
                style: const TextStyle(fontSize: 20, fontFamily: 'ReadexPro'),
              ),
              Row(
                children: [
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
                      'assets/default_profile_image.png',
                      height: 200,
                      width: 200,
                    ),
                  Container(
                    decoration: const BoxDecoration(
                      shape: BoxShape.circle,
                      color: Colors.black,
                    ),
                    margin: const EdgeInsets.only(top: 125, right: 500),
                    child: IconButton(
                      onPressed: pickSingleFile,
                      icon: const Icon(
                        Icons.camera_alt,
                        color: Colors.white,
                      ),
                      iconSize: 32,
                      splashColor: Colors.transparent,
                      highlightColor: Colors.transparent,
                      padding: const EdgeInsets.all(16),
                      splashRadius: 28,
                      tooltip: 'Pick Image',
                      focusColor: Colors.transparent,
                      hoverColor: Colors.transparent,
                      disabledColor: Colors.grey,
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 20),
              ElevatedButton(
                onPressed: () {
                  _showChangePasswordDialog(context, userBox);
                },
                style: ElevatedButton.styleFrom(
                  foregroundColor: Colors.white,
                  backgroundColor: Colors.black,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(10),
                  ),
                  fixedSize: const Size(250, 50),
                ),
                child: const Text(
                  'Change Password',
                  style: TextStyle(
                    fontFamily: 'ReadexPro',
                    fontSize: 16,
                    fontWeight: FontWeight.w600,
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

class NonExistingUser extends User {
  void showUserDoesNotExistDialog(BuildContext context) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('User Not Found'),
        content: const Text('The entered email does not exist.'),
        actions: [
          TextButton(
            onPressed: () {
              Navigator.pop(context);
            },
            child: const Text('OK'),
          ),
        ],
      ),
    );
  }
}
