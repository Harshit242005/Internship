// login.dart

// ignore_for_file: avoid_print

// Import necessary packages and classes
import 'package:flutter/material.dart';
import 'package:hive/hive.dart';
import 'package:projecthub/chooseproject.dart';
import 'package:projecthub/user.dart'; // Import your User class

class LoginPage extends StatefulWidget {
  const LoginPage({Key? key}) : super(key: key);

  @override
  // ignore: library_private_types_in_public_api
  _LoginPageState createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  final TextEditingController emailController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();

  Future<void> loginUser() async {
    // Open the Hive box
    final Box<User> userBox = await Hive.openBox<User>('user_data');

    // Retrieve user data based on entered email
    // ignore: unnecessary_nullable_for_final_variable_declarations
    final User? storedUser = userBox.values.firstWhere(
      (user) => user.email == emailController.text,
      orElse: () => NonExistingUser(),
    );
    String userName = storedUser!.firstName;
    print('the userName from the stored users as the box $userName');

    // Check if user exists and password is correct
    if (storedUser.password == passwordController.text) {
      // Navigate to the next screen (replace with your screen)
      // ignore: use_build_context_synchronously
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(
            builder: (context) => LandingScreen(
                  firstName: storedUser.firstName,
                )),
      );
    } else {
      // Show a dialog to inform the user
      // ignore: use_build_context_synchronously
      showDialog(
        context: context,
        builder: (context) => AlertDialog(
          title: const Text('Invalid Login'),
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

    // Close the Hive box
    await userBox.close();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Login',
          style:
              TextStyle(fontFamily: 'ReadexPro', fontWeight: FontWeight.w600),
        ),
      ),
      body: Center(
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Image.asset(
                    'assets/ProjectHubIcon.png', // Replace with your image asset path
                    height: 100,
                    width: 100,
                  ),
                  const SizedBox(width: 16),
                  const Text(
                    'ProjectHub',
                    style: TextStyle(
                      fontSize: 24,
                      fontFamily: 'ReadexPro',
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                ],
              ),
              const Text(
                'Type credentials for login...',
                style: TextStyle(
                  fontFamily: 'ReadexPro',
                  fontWeight: FontWeight.w600,
                  fontSize: 20,
                ),
              ),
              const SizedBox(height: 20),
              // Email TextField
              Container(
                height: 50,
                width: 300,
                child: TextField(
                  controller: emailController,
                  decoration: InputDecoration(
                    labelText: 'Type email...',
                    labelStyle: const TextStyle(
                      fontFamily: 'ReadexPro',
                      fontWeight: FontWeight.w600,
                    ),
                    contentPadding: const EdgeInsets.symmetric(horizontal: 16),
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(10),
                    ),
                  ),
                ),
              ),
              const SizedBox(height: 10),
              // Password TextField
              Container(
                height: 50,
                width: 300,
                child: TextField(
                  controller: passwordController,
                  decoration: InputDecoration(
                    labelText: 'Type password...',
                    labelStyle: const TextStyle(
                      fontFamily: 'ReadexPro',
                      fontWeight: FontWeight.w600,
                    ),
                    contentPadding: const EdgeInsets.symmetric(horizontal: 16),
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(10),
                    ),
                  ),
                  obscureText: true,
                ),
              ),
              const SizedBox(height: 25),
              Container(
                width: 200,
                height: 50,
                decoration: BoxDecoration(
                  color: Colors.black,
                  borderRadius: BorderRadius.circular(5),
                ),
                child: ElevatedButton(
                  onPressed: () {
                    // Save user data to Hive when login button is pressed
                    loginUser();
                  },
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Colors.transparent,
                    shadowColor: Colors.transparent,
                  ),
                  child: const Text(
                    'Login',
                    style: TextStyle(
                      color: Colors.white,
                      fontFamily: 'ReadexPro',
                      fontWeight: FontWeight.w600,
                      fontSize: 16,
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
