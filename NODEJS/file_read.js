const fs = require('fs');

// Read file async.
fs.readFile('file.txt', 'utf8', (err, data) => {
    if(err){
        console.log('Reading error:', err);
    }else{
        console.log('File content:\n', data);
    }
});

console.log('Reading file ...');