// ignore_for_file: prefer_const_constructors

import 'package:flutter/material.dart';
import 'package:projecthub/login.dart';
import 'package:projecthub/signup.dart';
import 'package:path_provider/path_provider.dart' as path_provider;
import 'package:hive_flutter/hive_flutter.dart';
import 'user.dart'; // Import your User class

void main() async {
  runApp(MyApp());
  await Hive.initFlutter();
  WidgetsFlutterBinding.ensureInitialized();

  final appDocumentDirectory =
      await path_provider.getApplicationDocumentsDirectory();
  Hive.init(appDocumentDirectory.path);

  Hive.registerAdapter(UserAdapter()); // Register the adapter
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: LandingPage(),
    );
  }
}

class LandingPage extends StatelessWidget {
  const LandingPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            // Image and Text Row
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Image.asset(
                  'assets/ProjectHubIcon.png', // Replace with your image asset path
                  height: 100,
                  width: 100,
                ),
                SizedBox(width: 16),
                Text(
                  'ProjectHub',
                  style: TextStyle(
                    fontSize: 24,
                    fontFamily: 'ReadexPro',
                    fontWeight: FontWeight.bold,
                  ),
                ),
              ],
            ),
            SizedBox(height: 32),

            // Welcome Text
            // Welcome Text
            // ignore: sized_box_for_whitespace
            Container(
              width: 450, // Set the desired width
              child: Text(
                'Welcome to ProjectHub for creating interactive mind maps to give your ideas life.',
                textAlign: TextAlign.center,
                style: TextStyle(
                    fontSize: 20,
                    fontFamily: 'ReadexPro',
                    fontWeight: FontWeight.w600),
              ),
            ),
            SizedBox(height: 32),

            // Signup and Login Row
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                ElevatedButton(
                  onPressed: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => SignUpPage()),
                    );
                  },
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color.fromARGB(
                        255, 224, 221, 221), // Background color
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(5),
                    ),
                    minimumSize: Size(250, 75), // Button size
                  ),
                  child: Text(
                    'Sign Up',
                    style: TextStyle(
                        fontSize: 18,
                        color: Colors.black,
                        fontFamily: 'ReadexPro',
                        fontWeight: FontWeight.w600),
                  ),
                ),
                SizedBox(width: 16),
                ElevatedButton(
                  onPressed: () {
                    // Add login button functionality
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => LoginPage()),
                    );
                  },
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color.fromARGB(
                        255, 224, 221, 221), // Background color
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(5),
                    ),
                    minimumSize: Size(250, 75), // Button size
                  ),
                  child: Text(
                    'Login',
                    style: TextStyle(
                        fontSize: 18,
                        color: Colors.black,
                        fontFamily: 'ReadexPro',
                        fontWeight: FontWeight.w600),
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
