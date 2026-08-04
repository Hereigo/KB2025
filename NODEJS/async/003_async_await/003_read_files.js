const fs = require('fs/promises');

function mergeStrings(...strings) {
  return strings.join(' ');
}

const readFiles = async () => {
    try {
        const filesData = await Promise.all([
            fs.readFile('file1.txt', 'utf8'),
            fs.readFile('file2.txt', 'utf8'),
            fs.readFile('file3.txt', 'utf8'),
        ]);
    
        const mergedFilesContent = mergeStrings(...filesData);
    
        console.log(mergedFilesContent);
    } catch (error) {
        console.error(error);
    }
};

readFiles();
