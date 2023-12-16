// // ignore_for_file: avoid_print

// import 'dart:io';
// import 'package:flutter/material.dart';
// import 'package:projecthub/folderpopup.dart';
// import 'package:projecthub/mindmap.dart';

// class ProjectListPage extends StatelessWidget {
//   final String firstName;
//   const ProjectListPage({super.key, required this.firstName});

//   @override
//   Widget build(BuildContext context) {
//     final projects = getProjectFolders(firstName);

//     return Scaffold(
//       appBar: AppBar(
//         title: const Text(
//           'Project List',
//           style:
//               TextStyle(fontFamily: 'ReadexPro', fontWeight: FontWeight.w600),
//         ),
//         actions: [
//           Container(
//             margin: const EdgeInsets.only(right: 25),
//             child: ElevatedButton(
//               onPressed: () {
//                 // Your button logic here
//                 showCreateFolderDialog(context, firstName: firstName);
//               },
//               style: ElevatedButton.styleFrom(
//                 fixedSize: const Size(200, 40),

//                 foregroundColor: Colors.white,
//                 backgroundColor: Colors.black, // Text color
//                 shape: RoundedRectangleBorder(
//                   borderRadius: BorderRadius.circular(5), // Border radius
//                 ),
//               ),
//               child: const Text(
//                 'Add Project',
//                 style: TextStyle(
//                     fontFamily: 'ReadexPro',
//                     fontSize: 16,
//                     fontWeight: FontWeight.w600),
//               ),
//             ),
//           )
//         ],
//       ),
//       body: ListView.builder(
//         itemCount: projects.length,
//         itemBuilder: (context, index) {
//           return ListTile(
//             title: Text(projects[index].name),
//             subtitle: Text('Last Modified: ${projects[index].lastModified}'),
//             onTap: () {
//               Navigator.push(
//                 context,
//                 MaterialPageRoute(
//                   builder: (context) => MindMap(
//                       folderName: projects[index].name, firstName: firstName),
//                 ),
//               );
//             },
//           );
//         },
//       ),
//     );
//   }
// }

// class ProjectInfo {
//   final String name;
//   final DateTime lastModified;

//   ProjectInfo({required this.name, required this.lastModified});
// }

// List<ProjectInfo> getProjectFolders(String firstName) {
//   List<ProjectInfo> projects = [];

//   // Directory path where your projects are stored
//   String projectHubDirectory = 'projectHub/$firstName';

//   // Fetch project folders
//   Directory projectHub = Directory(projectHubDirectory);
//   List<FileSystemEntity> entities = projectHub.listSync();

//   for (var entity in entities) {
//     if (entity is Directory) {
//       String projectName =
//           entity.uri.pathSegments[entity.uri.pathSegments.length - 2];
//       DateTime lastModified = File(entity.path).statSync().modified;
//       print(projectName);
//       projects.add(ProjectInfo(name: projectName, lastModified: lastModified));
//     }
//   }

//   return projects;
// }

// ignore_for_file: avoid_print

import 'dart:io';
import 'package:flutter/material.dart';
import 'package:projecthub/folderpopup.dart';
import 'package:projecthub/mindmap.dart';

class ProjectListPage extends StatelessWidget {
  final String firstName;
  const ProjectListPage({super.key, required this.firstName});

  @override
  Widget build(BuildContext context) {
    List<ProjectInfo> projects;

    try {
      projects = getProjectFolders(firstName);

      if (projects.isEmpty) {
        return Scaffold(
          appBar: buildAppBar(context),
          body: const Center(
            child: Text(
              'No projects yet',
              style: TextStyle(fontFamily: 'ReadexPro', fontSize: 18),
            ),
          ),
        );
      }
    } catch (e) {
      print('Error fetching projects: $e');

      return Scaffold(
        appBar: buildAppBar(context),
        body: const Center(
          child: Text(
            'An error occurred while fetching projects...',
            style: TextStyle(
                fontFamily: 'ReadexPro',
                fontSize: 20,
                fontWeight: FontWeight.w600),
          ),
        ),
      );
    }

    return Scaffold(
      appBar: buildAppBar(context),
      body: ListView.builder(
        itemCount: projects.length,
        itemBuilder: (context, index) {
          return ListTile(
            title: Text(projects[index].name),
            subtitle: Text('Last Modified: ${projects[index].lastModified}'),
            onTap: () {
              print(
                  'we had the first now for the clicked project folder is $firstName');
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => MindMap(
                    folderName: projects[index].name,
                    firstName: firstName,
                  ),
                ),
              );
            },
          );
        },
      ),
    );
  }

  AppBar buildAppBar(BuildContext context) {
    return AppBar(
      title: const Text(
        'Project List',
        style: TextStyle(fontFamily: 'ReadexPro', fontWeight: FontWeight.w600),
      ),
      actions: [
        Container(
          margin: const EdgeInsets.only(right: 25),
          child: ElevatedButton(
            onPressed: () {
              // Your button logic here
              showCreateFolderDialog(context, firstName: firstName);
            },
            style: ElevatedButton.styleFrom(
              fixedSize: const Size(200, 40),
              foregroundColor: Colors.white,
              backgroundColor: Colors.black, // Text color
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(5), // Border radius
              ),
            ),
            child: const Text(
              'Add Project',
              style: TextStyle(
                  fontFamily: 'ReadexPro',
                  fontSize: 16,
                  fontWeight: FontWeight.w600),
            ),
          ),
        )
      ],
    );
  }
}

class ProjectInfo {
  final String name;
  final DateTime lastModified;

  ProjectInfo({required this.name, required this.lastModified});
}

List<ProjectInfo> getProjectFolders(String firstName) {
  List<ProjectInfo> projects = [];
  print('fetching the projects of the users $firstName');
  // Directory path where your projects are stored
  String projectHubDirectory = 'projectHub/$firstName';

  // Fetch project folders
  Directory projectHub = Directory(projectHubDirectory);
  List<FileSystemEntity> entities = projectHub.listSync();

  for (var entity in entities) {
    if (entity is Directory) {
      String projectName =
          entity.uri.pathSegments[entity.uri.pathSegments.length - 2];
      DateTime lastModified = File(entity.path).statSync().modified;
      print(projectName);
      projects.add(ProjectInfo(name: projectName, lastModified: lastModified));
    }
  }

  return projects;
}
