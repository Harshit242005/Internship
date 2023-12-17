import 'package:flutter/material.dart';
import 'package:sqflite_common_ffi/sqflite_ffi.dart';
import 'package:track/singlefilepicker.dart';
import 'package:hive/hive.dart';
import 'package:path_provider/path_provider.dart' as path_provider;
import 'package:fluttertoast/fluttertoast.dart';

void main() async {
  WidgetsFlutterBinding
      .ensureInitialized(); // Ensure that Flutter is initialized.
  sqfliteFfiInit();
  databaseFactory = databaseFactoryFfi;

  // Get the application documents directory, which is where Hive stores data.
  final appDocumentDirectory =
      await path_provider.getApplicationDocumentsDirectory();

  // Set the path for Hive to use.

  Hive.init(appDocumentDirectory.path);
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      builder: FToastBuilder(),
      title: 'Flutter Demo',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.deepPurple),
        useMaterial3: true,
      ),
      home: const MyHomePage(title: 'Flutter Demo Home Page'),
    );
  }
}

class MyHomePage extends StatefulWidget {
  const MyHomePage({super.key, required this.title});

  final String title;

  @override
  State<MyHomePage> createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            const Text('TrackX',
                style: TextStyle(
                    fontFamily: 'ReadexPro',
                    fontSize: 28,
                    fontWeight: FontWeight.bold)),
            const SizedBox(
              height: 20,
            ),
            // Image.asset('assets/EnjoyLabel.png'),
            const Text(
              'A easy way to track your\nTime & Work',
              style: TextStyle(
                  fontFamily: 'ReadexPro',
                  fontSize: 20,
                  fontWeight: FontWeight.bold),
              textAlign: TextAlign.center,
            ),
            const SizedBox(
              height: 40,
            ),
            SizedBox(
              width: 200,
              height: 50,
              child: ElevatedButton(
                onPressed: () {
                  Navigator.push(
                    context,
                    MaterialPageRoute(
                        builder: (context) =>
                            const Singlefilepicker()), // Navigate to SecondScreen
                  );
                },
                style: ElevatedButton.styleFrom(
                  foregroundColor: Colors.white,
                  backgroundColor: Colors.black, // Set the text color to white
                  shape: RoundedRectangleBorder(
                    borderRadius:
                        BorderRadius.circular(5), // Decrease the border radius
                  ),
                  minimumSize: const Size(200, 60),
                ),
                child: const Text(
                  'continue',
                  style: TextStyle(
                    fontFamily: 'ReadexPro',
                    fontSize: 20,
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
