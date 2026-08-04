const fs = require('fs');

const FILES_TO_READ = ['file1.txt', 'file2.txt', 'file3.txt'];

function executeParallelFileReading(filesToRead, callback) {
  const results = [];
  let completedCount = 0;

  const handleFileReadOperationResult = (err, data) => {
    if (err) {
      callback(err, null);
      return;
    }

    results.push(data);
    completedCount += 1;

    if (completedCount >= filesToRead.length) {
      callback(null, results);
    }
  }

  filesToRead.forEach(fileToRead => {
    fs.readFile(fileToRead, 'utf8', handleFileReadOperationResult);
  });
}

executeParallelFileReading(FILES_TO_READ, (err, results) => {
  if (err) {
    console.error(err);
  }

  const mergedFilesContent = mergeStrings(...results);

  console.log('Merged Text: ' + mergedFilesContent);
});

function mergeStrings(...strings) {
  return strings.join(' ');
}