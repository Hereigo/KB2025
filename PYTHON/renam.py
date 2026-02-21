import os
from pathlib import Path

def get_all_files(directory="."):
    """
    Returns a list of all files in the specified directory.
    
    Args:
        directory: Path to the directory (default: current directory)
    
    Returns:
        List of file paths
    """
    files = []
    for item in os.listdir(directory):
        path = os.path.join(directory, item)
        if os.path.isfile(path):
            files.append(path)
    return files

# Alternative using pathlib (more modern):
def get_all_files_pathlib(directory="."):
    """Returns a list of all files using pathlib."""
    return list(Path(directory).glob("*"))

# Example usage:
if __name__ == "__main__":
    files = get_all_files(".")
    print("Files in current directory:")
    for file in files:
        print(file)