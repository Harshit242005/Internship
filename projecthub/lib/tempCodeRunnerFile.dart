// ignore: file_names

// ignore: unnecessary_import
import 'package:hive/hive.dart';
import 'package:hive_flutter/hive_flutter.dart';
import 'package:projecthub/user.dart';
import 'package:path_provider/path_provider.dart' as path_provider;

Future<void> clearAllHiveBoxes() async {
  try {
    // Initialize Hive
    await Hive.initFlutter();

    // Get the application documents directory

    final appDocumentDirectory =
        await path_provider.getApplicationDocumentsDirectory();

    // Set the Hive directory to the application documents directory
    Hive.init(appDocumentDirectory.path);

    // Open all registered Hive boxes
    final Box<User> userBox = await Hive.openBox<User>('user_data');
    userBox.clear();
  } catch (e) {
    print('Error clearing Hive boxes: $e');
  }
}

void main() async {
  // Register adapters and other setup if needed
  Hive.registerAdapter(UserAdapter());

  // Call the function to clear all Hive boxes
  await clearAllHiveBoxes();
}
