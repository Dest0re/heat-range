import os

for subdir, filepach, files in os.walk("..\\heat-range\\scripts"):
    for file in files:
        filepath = subdir + os.sep + file

        if filepath.endswith(".cs"):
            with open(filepath, 'r') as f:
                data = f.read()
                if '"' in data:
                    print(filepath)




input("press enter to exit")