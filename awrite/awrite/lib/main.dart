import 'package:awrite/fontFamily.dart';
import 'package:flutter/material.dart';
import 'package:flutter_colorpicker/flutter_colorpicker.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // as this is the statless widget we can define variables here

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        useMaterial3: true,
      ),
      home: const MyHomePage(title: 'Flutter based text editor'),
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
    // defining some variables in the text
    // now we would define a underline bool variable that will listen for the yes or no to the underline

    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.white,
        bottom: PreferredSize(
          preferredSize: const Size.fromHeight(
              1.0), // Adjust the height of the border as needed
          child: Container(
            color: const Color.fromARGB(74, 0, 0, 0), // Color of the border
            height: 1.0, // Height of the border
          ),
        ),
        // leading: Tooltip(
        //   message: 'Menu',
        //   child: ElevatedButton(
        //     onPressed: () {},
        //     style: ElevatedButton.styleFrom(
        //       backgroundColor:
        //           Colors.white, // Adjust the button's background color
        //       shape: RoundedRectangleBorder(
        //         borderRadius: BorderRadius.circular(
        //             0), // Adjust the border radius (0 for a rectangle)
        //       ),
        //     ),
        //     child: Transform.scale(
        //       scale:
        //           4.0, // Adjust the scale factor to increase the size (2.0 doubles the size)
        //       child: Image.asset('assets/Menu.png'),
        //     ),
        //   ),
        // ),
        actions: [
          Row(
            mainAxisAlignment:
                MainAxisAlignment.end, // Align children to the end (right side)
            children: [
              Container(
                margin: const EdgeInsets.only(
                    left: 1), // Adjust the padding as needed
                child: SizedBox(
                    width: 100,
                    height: kToolbarHeight, // Match the height to the AppBar
                    child: Tooltip(
                      message: 'bold text',
                      child: IconButton(
                        onPressed: () {
                          setState(() {
                            // Toggle the underline variable
                            bold = !bold;
                            print(bold);
                          });
                          // Handle the button's onPressed event here
                        },
                        style: TextButton.styleFrom(
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(
                                  0), // Adjust the border radius (0 for a rectangle)
                            ),
                            backgroundColor: bold
                                ? const Color.fromARGB(107, 133, 133, 133)
                                : Colors.white),
                        icon: const Icon(Icons.format_bold),
                      ),
                    )),
              ),
              // icon button for the underlining of the text
              Container(
                margin: const EdgeInsets.only(
                    left: 1), // Adjust the padding as needed
                child: SizedBox(
                    width: 100,
                    height: kToolbarHeight, // Match the height to the AppBar
                    child: Tooltip(
                      message: 'underline text',
                      child: IconButton(
                        onPressed: () {
                          // Handle the button's onPressed event here
                          setState(() {
                            // Toggle the underline variable
                            underline = !underline;
                            print(underline);
                          });
                        },
                        style: TextButton.styleFrom(
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(
                                  0), // Adjust the border radius (0 for a rectangle)
                            ),
                            backgroundColor: underline
                                ? const Color.fromARGB(107, 133, 133, 133)
                                : Colors.white),
                        icon: const Icon(Icons.format_underline),
                      ),
                    )),
              ),
              Container(
                margin: const EdgeInsets.only(
                    left: 1), // Adjust the padding as needed
                child: SizedBox(
                    width: 100,
                    height: kToolbarHeight, // Match the height to the AppBar
                    child: Tooltip(
                      message: 'text color',
                      child: IconButton(
                        onPressed: () {
                          // Handle the button's onPressed event here
                          _openColorPickerDialog();
                        },
                        style: TextButton.styleFrom(
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(
                                0), // Adjust the border radius (0 for a rectangle)
                          ),
                          iconColor: currentColor,
                        ),
                        icon: const Icon(Icons.color_lens),
                      ),
                    )),
              ),
              Container(
                margin: const EdgeInsets.only(left: 1),
                child: SizedBox(
                  width: 100,
                  height: kToolbarHeight,
                  child: Tooltip(
                    message: 'alignment',
                    child: IconButton(
                      onPressed: () {
                        // Handle the button's onPressed event here
                        showAlignmentDialog().then((selectedAlignment) {
                          // ignore: unnecessary_null_comparison
                          if (selectedAlignment != null) {
                            setState(() {
                              alignment = selectedAlignment;
                              print(alignment);
                            });
                          }
                        });
                      },
                      style: TextButton.styleFrom(
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(0),
                        ),
                      ),
                      icon: getAlignmentIcon(alignment),
                    ),
                  ),
                ),
              ),
              Container(
                margin: const EdgeInsets.only(
                    left: 1), // Adjust the padding as needed
                child: SizedBox(
                    width: 100,
                    height: kToolbarHeight, // Match the height to the AppBar
                    child: Tooltip(
                      message: 'text shadow',
                      child: ElevatedButton(
                          onPressed: () {
                            // Handle the button's onPressed event here
                            _showShadowColorPickerDialog();
                          },
                          style: TextButton.styleFrom(
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(
                                    0), // Adjust the border radius (0 for a rectangle)
                              ),
                              backgroundColor: Colors.white),
                          child: Image.asset('assets/shadow.png')),
                    )),
              ),
              Container(
                margin: const EdgeInsets.only(
                    left: 1), // Adjust the padding as needed
                child: SizedBox(
                    width: 100,
                    height: kToolbarHeight, // Match the height to the AppBar
                    child: Tooltip(
                      message: 'size',
                      child: IconButton(
                        onPressed: () {
                          // Handle the button's onPressed event here
                          _showSizeInputDialog();
                        },
                        style: TextButton.styleFrom(
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(
                                0), // Adjust the border radius (0 for a rectangle)
                          ),
                        ),
                        icon: const Icon(Icons.width_wide),
                      ),
                    )),
              ),
              Container(
                margin: const EdgeInsets.only(
                    left: 1), // Adjust the padding as needed
                child: SizedBox(
                    width: 100,
                    height: kToolbarHeight, // Match the height to the AppBar
                    child: Tooltip(
                      message: 'italic text',
                      child: IconButton(
                        onPressed: () {
                          // Handle the button's onPressed event here
                          setState(() {
                            // Toggle the underline variable
                            italic = !italic;
                            print(italic);
                          });
                        },
                        style: TextButton.styleFrom(
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(
                                  0), // Adjust the border radius (0 for a rectangle)
                            ),
                            backgroundColor: italic
                                ? const Color.fromARGB(107, 133, 133, 133)
                                : Colors.white),
                        icon: const Icon(Icons.format_italic),
                      ),
                    )),
              ),
              Container(
                margin: const EdgeInsets.only(
                    left: 1), // Adjust the padding as needed
                child: SizedBox(
                    width: 100,
                    height: kToolbarHeight, // Match the height to the AppBar
                    child: Tooltip(
                      message: 'font family',
                      child: IconButton(
                        onPressed: () {
                          // Handle the button's onPressed event here
                          FontFamilyPicker(
                            fontFamilies: const ['ReadexPro'],
                          );
                        },
                        style: TextButton.styleFrom(
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(
                                0), // Adjust the border radius (0 for a rectangle)
                          ),
                        ),
                        icon: const Icon(Icons.font_download),
                      ),
                    )),
              ),
              Container(
                margin: const EdgeInsets.only(
                    left: 1), // Adjust the padding as needed
                child: SizedBox(
                    width: 100,
                    height: kToolbarHeight, // Match the height to the AppBar
                    child: Tooltip(
                      message: 'line spacing',
                      child: IconButton(
                        onPressed: () {
                          // Handle the button's onPressed event here
                          _showLineHeightInputDialog();
                        },
                        style: TextButton.styleFrom(
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(
                                0), // Adjust the border radius (0 for a rectangle)
                          ),
                        ),
                        icon: const Icon(Icons.format_line_spacing),
                      ),
                    )),
              ),
              Container(
                margin: const EdgeInsets.only(
                    left: 1), // Adjust the padding as needed
                child: SizedBox(
                    width: 100,
                    height: kToolbarHeight, // Match the height to the AppBar
                    child: Tooltip(
                      message: 'letter spacing',
                      child: IconButton(
                          onPressed: () {
                            // Handle the button's onPressed event here
                            _showLetterSpaceInputDialog();
                          },
                          style: TextButton.styleFrom(
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(
                                  0), // Adjust the border radius (0 for a rectangle)
                            ),
                          ),
                          icon: const Icon(Icons.arrow_right_alt)),
                    )),
              ),
              Container(
                width: 200,
                margin: const EdgeInsets.only(
                    left: 1), // Adjust the padding as needed
                child: SizedBox(
                    height: kToolbarHeight, // Match the height to the AppBar
                    child: Tooltip(
                      message: 'save file',
                      child: TextButton(
                        onPressed: () {
                          // Handle the button's onPressed event here
                        },
                        style: TextButton.styleFrom(
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(
                                0), // Adjust the border radius (0 for a rectangle)
                          ),
                        ),
                        child: const Text(
                          'Save',
                          style: TextStyle(
                            fontFamily: 'ReadexPro',
                            color: Colors.black, // Adjust the text color
                            fontSize: 16.0, // Adjust the font size
                          ),
                        ),
                      ),
                    )),
              ),
            ],
          ),
        ],
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Container(
              margin: const EdgeInsets.only(bottom: 0),
              width: 1600, // Set the width of the container
              height: 670,
              padding: const EdgeInsets.all(16), // Add padding
              decoration: BoxDecoration(
                border: Border.all(color: Colors.grey), // Add border
                borderRadius: BorderRadius.circular(0), // Add border radius
              ),
              child: SingleChildScrollView(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: textsWithStyles.map((textWithStyle) {
                    return Container(
                      margin: const EdgeInsets.only(bottom: 8),
                      child: Text(
                        textWithStyle.text,

                        style: textWithStyle.textStyle,
                        textAlign: textWithStyle.alignment,

                        // here we have added some new features for the text alignment
                      ),
                    );
                  }).toList(),
                ),
              ),
            ),

            // here we would add a code to show the new text field list
            Container(
              margin: const EdgeInsets.only(top: 0),
              width: 1600, // Set the width of the container
              padding: const EdgeInsets.all(16), // Add padding
              decoration: BoxDecoration(
                border: Border.all(color: Colors.grey), // Add border
                borderRadius: BorderRadius.circular(0), // Add border radius
              ),
              child: Row(
                children: [
                  Expanded(
                      child: TextField(
                    controller: textController,
                    maxLines: null,
                    // here we would provide the styles for the text we would enter here
                    // means we would change the stuff
                    style: TextStyle(
                      // underline, bold, color, size, transform
                      color: currentColor,
                      shadows: [
                        Shadow(
                            color: shadowColor,
                            offset: Offset(offsetX, offsetY),
                            blurRadius: blurRadius)
                      ],
                      letterSpacing: textLetterSpacing.toDouble(),
                      height: textHeight.toDouble(),
                      fontSize: textFontSize.toDouble(),
                      fontStyle: italic ? FontStyle.italic : FontStyle.normal,
                      fontWeight: bold ? FontWeight.bold : FontWeight.normal,
                      decoration: underline ? TextDecoration.underline : null,
                      decorationColor: underline ? currentColor : null,
                    ),

                    decoration: const InputDecoration(
                      labelText: 'Enter your text',
                      labelStyle: TextStyle(
                        color:
                            Color.fromARGB(90, 0, 0, 0), // Set label text color
                        fontSize: 20.0, // Set label text font size
                        fontFamily: 'ReadexPro',
                      ),
                      focusedBorder: UnderlineInputBorder(
                        borderSide: BorderSide(
                            color: Colors
                                .transparent), // Hide bottom border when focused
                      ),
                      enabledBorder: UnderlineInputBorder(
                        borderSide: BorderSide(
                            color: Colors
                                .transparent), // Hide bottom border when not focused
                      ),
                    ),
                  )),
                  IconButton(
                    icon: const Icon(Icons.arrow_upward),
                    onPressed: () {
                      // Handle arrow up button click
                      setState(() {
                        // Update the text in the first container

                        addedTextStyle = TextStyle(
                          color: currentColor,
                          letterSpacing: textLetterSpacing.toDouble(),
                          height: textHeight.toDouble(),
                          fontSize: textFontSize.toDouble(),
                          shadows: [
                            Shadow(
                                color: shadowColor,
                                offset: Offset(offsetX, offsetY),
                                blurRadius: blurRadius)
                          ],
                          fontWeight:
                              bold ? FontWeight.bold : FontWeight.normal,
                          fontStyle:
                              italic ? FontStyle.italic : FontStyle.normal,
                          decoration:
                              underline ? TextDecoration.underline : null,
                          decorationColor: underline ? currentColor : null,
                        );
                        // this would be called for adding text and their styles
                        addTextAndStyle(textController.text, addedTextStyle);
                        textController
                            .clear(); // Clear the text field in the second container
                      });
                    },
                  ),
                  IconButton(
                    icon: const Icon(Icons.delete),
                    onPressed: () {
                      // Handle delete button click
                      textController.clear(); // Clear the text field
                    },
                  ),
                ],
              ),
            )
          ],
        ),
      ),
    );
  }

  List<TextWithStyle> textsWithStyles = [];
  List<TextEditingController> textControllers = [];
  void addTextAndStyle(String text, TextStyle addedTextStyle) {
    setState(() {
      textsWithStyles.add(TextWithStyle(
          text: text, textStyle: addedTextStyle, alignment: alignment));
      textControllers.add(TextEditingController(text: text));
      print('text align is on $alignment');
    });
  }

  TextEditingController textController = TextEditingController();
  TextAlign alignment = TextAlign.center; // Default alignment
  String firstContainerText = '';
  bool underline = false;
  bool italic = false;
  bool bold = false;
  int textLetterSpacing = 1;
  int textHeight = 1;
  int textFontSize = 14;
  Color currentColor = Colors.black;
  TextStyle addedTextStyle = TextStyle(); // Style for the added text

  // ignore: unused_element
  void _openColorPickerDialog() {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('Pick a color'),
          content: SingleChildScrollView(
            child: ColorPicker(
              pickerColor: currentColor,
              onColorChanged: (Color color) {
                setState(() {
                  currentColor = color;
                });
              },
              pickerAreaHeightPercent: 0.8,
            ),
          ),
          actions: <Widget>[
            TextButton(
              child: const Text('OK'),
              onPressed: () {
                // Handle the selected color (currentColor) here
                Navigator.of(context).pop();
              },
            ),
          ],
        );
      },
    );
  }

  void _showSizeInputDialog() {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('Type font size...'),
          content: TextField(
            keyboardType: TextInputType.number,
            onChanged: (value) {
              // Update the inputValue when the text changes
              textFontSize = int.tryParse(value) ?? 0;
            },
            decoration: const InputDecoration(labelText: 'font size'),
          ),
          actions: [
            TextButton(
              onPressed: () {
                // Close the dialog
                Navigator.of(context).pop();
              },
              child: const Text('Cancel'),
            ),
            TextButton(
              onPressed: () {
                // Process the input value, e.g., use it in your application
                print('Selected Size: $textFontSize');
                // Close the dialog
                Navigator.of(context).pop();
              },
              child: const Text('OK'),
            ),
          ],
        );
      },
    );
  }

  // giving letter spacing
  void _showLetterSpaceInputDialog() {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('Type letter spacing...'),
          content: TextField(
            keyboardType: TextInputType.number,
            onChanged: (value) {
              // Update the inputValue when the text changes
              textLetterSpacing = int.tryParse(value) ?? 0;
            },
            decoration: const InputDecoration(labelText: 'letter spacing'),
          ),
          actions: [
            TextButton(
              onPressed: () {
                // Close the dialog
                Navigator.of(context).pop();
              },
              child: const Text('Cancel'),
            ),
            TextButton(
              onPressed: () {
                // Process the input value, e.g., use it in your application
                print('Selected Size: $textLetterSpacing');
                // Close the dialog
                Navigator.of(context).pop();
              },
              child: const Text('OK'),
            ),
          ],
        );
      },
    );
  }

  // giving the line height
  void _showLineHeightInputDialog() {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('Type line height...'),
          content: TextField(
            keyboardType: TextInputType.number,
            onChanged: (value) {
              // Update the inputValue when the text changes
              textHeight = int.tryParse(value) ?? 0;
            },
            decoration: const InputDecoration(labelText: 'line height'),
          ),
          actions: [
            TextButton(
              onPressed: () {
                // Close the dialog
                Navigator.of(context).pop();
              },
              child: const Text('Cancel'),
            ),
            TextButton(
              onPressed: () {
                // Process the input value, e.g., use it in your application
                print('Selected Size: $textHeight');
                // Close the dialog
                Navigator.of(context).pop();
              },
              child: const Text('OK'),
            ),
          ],
        );
      },
    );
  }

  // here we would add a row based text alignment in the main text field

  Future<TextAlign> showAlignmentDialog() async {
    TextAlign selectedAlignment = await showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('Select Alignment'),
          content: Row(
            mainAxisAlignment: MainAxisAlignment.spaceAround,
            children: [
              IconButton(
                icon: const Icon(Icons.format_align_left),
                onPressed: () {
                  // Handle left alignment
                  Navigator.pop(context, TextAlign.left);
                },
              ),
              IconButton(
                icon: const Icon(Icons.format_align_center),
                onPressed: () {
                  // Handle center alignment
                  Navigator.pop(context, TextAlign.center);
                },
              ),
              IconButton(
                icon: const Icon(Icons.format_align_right),
                onPressed: () {
                  // Handle right alignment
                  Navigator.pop(context, TextAlign.right);
                },
              ),
            ],
          ),
        );
      },
    );

    return selectedAlignment;
  }

  Icon getAlignmentIcon(TextAlign alignment) {
    switch (alignment) {
      case TextAlign.left:
        return const Icon(Icons.format_align_left);
      case TextAlign.center:
        return const Icon(Icons.format_align_center);
      case TextAlign.right:
        return const Icon(Icons.format_align_right);
      default:
        return const Icon(Icons.format_align_left);
    }
  }

  double offsetX = 0;
  double offsetY = 0;
  double blurRadius = 0;
  Color shadowColor = Colors.black;
  TextEditingController offsetXController = TextEditingController();
  TextEditingController offsetYController = TextEditingController();
  TextEditingController blurRadiusController = TextEditingController();
  // applying some text shadow in place of the other stuff
  void _showShadowColorPickerDialog() {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('Pick a Color and Set Shadow'),
          content: Column(
            children: [
              ColorPicker(
                pickerColor: shadowColor,
                onColorChanged: (Color color) {
                  setState(() {
                    shadowColor = color;
                  });
                },
                showLabel: true,
                pickerAreaHeightPercent: 0.8,
              ),
              const SizedBox(height: 20),
              TextField(
                controller: offsetXController,
                keyboardType: TextInputType.number,
                decoration: const InputDecoration(labelText: 'Offset X'),
              ),
              const SizedBox(height: 10),
              TextField(
                controller: offsetYController,
                keyboardType: TextInputType.number,
                decoration: const InputDecoration(labelText: 'Offset Y'),
              ),
              const SizedBox(height: 10),
              TextField(
                controller: blurRadiusController,
                keyboardType: TextInputType.number,
                decoration: const InputDecoration(labelText: 'Blur Radius'),
              ),
            ],
          ),
          actions: [
            ElevatedButton(
              onPressed: () {
                // Handle OK button click
                Navigator.pop(context);
                _applyShadowSettings();
              },
              child: const Text('OK'),
            ),
          ],
        );
      },
    );
  }

  void _applyShadowSettings() {
    setState(() {
      offsetX = double.tryParse(offsetXController.text) ?? 0.0;
      offsetY = double.tryParse(offsetYController.text) ?? 0.0;
      blurRadius = double.tryParse(blurRadiusController.text) ?? 0.0;

      // Use selectedColor, offsetX, offsetY, and blurRadius as needed.
      print('Selected Color: $shadowColor');
      print('Offset X: $offsetX');
      print('Offset Y: $offsetY');
      print('Blur Radius: $blurRadius');
    });
  }
}

class TextWithStyle {
  String text;
  TextStyle textStyle;
  TextAlign alignment;
  TextWithStyle(
      {required this.text, required this.textStyle, required this.alignment});
}
