import 'package:path/path.dart';
import 'package:sqflite_common_ffi/sqflite_ffi.dart';

void main() async {
  // Initialize the FFI loader.
  sqfliteFfiInit();

  // Define the path to the database file.
  final databasePath = join(await getDatabasesPath(), 'Tasks.db');

  // Open the database.
  final database = await databaseFactoryFfi.openDatabase(databasePath);

  // Create the "Tasks" table.
  await database.execute('''
    CREATE TABLE Tasks (
      id INTEGER PRIMARY KEY,
      name TEXT NOT NULL,
      description TEXT NOT NULL,
      label TEXT NOT NULL,
      start_time TEXT NOT NULL,
      end_time TEXT NOT NULL,
      status INTEGER DEFAULT 0,
      date DATE
    )
  ''');

  // Close the database.
  await database.close();
}
