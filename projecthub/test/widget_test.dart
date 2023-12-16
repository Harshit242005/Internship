// ignore_for_file: avoid_print

import 'package:flutter/material.dart';

void main() {
  runApp(const MyApp());
}

class DragTargetData {
  bool booleanValue;
  int index;

  DragTargetData(this.booleanValue, this.index);
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        appBar: AppBar(
          title: const Text('Dynamic DragTargets'),
        ),
        body: const MyDragTargets(),
      ),
    );
  }
}

class MyDragTargets extends StatefulWidget {
  const MyDragTargets({super.key});

  @override
  // ignore: library_private_types_in_public_api
  _MyDragTargetsState createState() => _MyDragTargetsState();
}

class _MyDragTargetsState extends State<MyDragTargets> {
  List<DragTargetData> dragTargets = [];

  @override
  void initState() {
    super.initState();
    // Add an initial DragTarget with booleanValue set to false
    dragTargets.add(DragTargetData(false, 0));
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            for (var target in dragTargets)
              buildDragTarget(target.booleanValue, target.index),
          ],
        ),
      ],
    );
  }

  Widget buildDragTarget(bool booleanValue, int index) {
    return DragTarget<String>(
      builder: (BuildContext context, List<String?> accepted,
          List<dynamic> rejected) {
        return Container(
          width: 200,
          height: 200,
          decoration: BoxDecoration(
            color: booleanValue
                ? Colors.green
                : const Color.fromARGB(90, 171, 168, 164),
            borderRadius:
                BorderRadius.circular(10), // Adjust the radius as needed
          ),
          child: Center(
            child: Container(
              decoration: const BoxDecoration(
                shape: BoxShape.circle,
                color: Color.fromARGB(
                    255, 0, 0, 0), // Set the background color here
              ),
              child: IconButton(
                onPressed: () {
                  // Check if the last DragTarget has booleanValue set to true
                  if (dragTargets.isEmpty || dragTargets.last.booleanValue) {
                    // Add a new DragTarget with a boolean value and index
                    setState(() {
                      int newIndex = dragTargets.length;
                      dragTargets.add(DragTargetData(true, newIndex));
                    });
                  }
                },
                icon: const Icon(
                  Icons.add,
                  size: 40,
                  color: Colors.white,
                ), // Customize the icon and size as needed
              ),
            ),
          ),
        );
      },
      onAccept: (String? data) {
        // Handle the accepted data
        print('Data accepted on DragTarget $index: $data');
        setState(() {
          dragTargets[index].booleanValue = true;
        });
      },
    );
  }
}
