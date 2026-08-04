const fs = require('fs/promises');

Promise.all([fs.readFile('file1.txt', 'utf8'), fs.readFile('file2.txt', 'utf8'), fs.readFile('file3.txt', 'utf8')])
    .then(data => {
        console.log(data);
        return mergeStrings(...data);
    })
    .then(mergedFilesContent => {
        console.log(mergedFilesContent);
    })
    .catch(error => {
        console.error(error);
    });

function mergeStrings(...strings) {
  return strings.join(' ');
}