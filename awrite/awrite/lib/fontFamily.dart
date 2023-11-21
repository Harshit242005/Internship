import 'package:flutter/material.dart';

class FontFamilyPicker extends StatefulWidget {
  final List<String> fontFamilies;

  FontFamilyPicker({required this.fontFamilies});

  @override
  _FontFamilyPickerState createState() => _FontFamilyPickerState();
}

class _FontFamilyPickerState extends State<FontFamilyPicker> {
  late String selectedFontFamily;

  @override
  void initState() {
    super.initState();
    selectedFontFamily =
        widget.fontFamilies.first; // Select the first font family initially
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: Text('Select Font Family'),
      content: Container(
        width: double.maxFinite,
        child: ListView.builder(
          itemCount: widget.fontFamilies.length,
          itemBuilder: (context, index) {
            final fontFamily = widget.fontFamilies[index];
            return ListTile(
              title: Text(fontFamily),
              tileColor: fontFamily == selectedFontFamily ? Colors.grey : null,
              onTap: () {
                setState(() {
                  selectedFontFamily = fontFamily;
                });
                Navigator.pop(
                    context); // Close the dialog when a font is selected
              },
            );
          },
        ),
      ),
    );
  }
}

void showFontFamilyPicker(BuildContext context, List<String> fontFamilies) {
  showDialog(
    context: context,
    builder: (BuildContext context) {
      return FontFamilyPicker(fontFamilies: fontFamilies);
    },
  );
}
