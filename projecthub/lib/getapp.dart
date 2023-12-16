// import 'dart:io';

// void main() async {
//   try {
//     ProcessResult result = await Process.run('where', ['*.exe']);
//     if (result.exitCode == 0) {
//       String executables = result.stdout;
//       print('List of executables:\n$executables');

//       // Check if 'notepad.exe' exists in the list
//       if (executables.toLowerCase().contains('notepad.exe')) {
//         print('Notepad is installed.');

//         // Open Notepad
//         await Process.run('notepad.exe', []);
//       } else {
//         print('Notepad is not installed.');
//       }
//     } else {
//       print('Error: ${result.stderr}');
//     }
//   } catch (e) {
//     print('Error: $e');
//   }
// }

// import 'dart:io';

// void main() async {
//   try {
//     await Process.run('notepad.exe', []);
//   } catch (e) {
//     print('Error: $e');
//   }
// }

// ignore_for_file: avoid_print

// import 'dart:io';

// void main() {
//   Directory current = Directory.current;
//   print("Current Directory: $current");
// }

// import 'dart:io';

// void openApp(String appName) async {
//   try {
//     // Use the 'where' command to find the executable with the specified name
//     var result = await Process.run('where', ['$appName.exe']);

//     // Extract the path from the result
//     String executablePath = result.stdout.toString().trim();

//     if (executablePath.isNotEmpty) {
//       // Open the executable using the 'start' command
//       await Process.run('start', [executablePath]);
//       print('$appName opened successfully.');
//     } else {
//       print('$appName not found.');
//     }
//   } catch (e) {
//     print('Error: $e');
//   }
// }

// void main() {
//   String imagePath = "assets/FigmaIcon.png";

//   // Define the regex pattern
//   RegExp regex = RegExp(r'assets\/(.*?)Icon\.png');

//   // Match the pattern against the image path
//   Match? match = regex.firstMatch(imagePath);

//   // Extract the name and remove "Icon" if there is a match
//   String appName =
//       match != null ? match.group(1)!.replaceAll("Icon", "") : "No match";

//   print("Application Name: $appName");

//   // Open the app with the extracted name
//   openApp(appName);
// }




