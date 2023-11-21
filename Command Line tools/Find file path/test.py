#!/usr/bin/env python
import os
import argparse

# Function to search for the file
def find_file(start_dir, target_file):
    for root, _, files in os.walk(start_dir):
        if target_file in files:
            return os.path.join(root, target_file)
    return None

def main():
    # Create a command-line parser
    parser = argparse.ArgumentParser(description="Search for a file in the current directory or its subdirectories.")
    
    # Add an argument for the filename
    parser.add_argument("filename", help="The name of the file to search for.")
    
    # Parse the command-line arguments
    args = parser.parse_args()
    
    # Get the current directory
    current_directory = os.getcwd()
    
    # Search for the file
    result = find_file(current_directory, args.filename)
    
    if result:
        print(f"Found '{args.filename}' at: {result}")
    else:
        print(f"'{args.filename}' not found in the current directory or its subdirectories.")

if __name__ == "__main__":
    main()
