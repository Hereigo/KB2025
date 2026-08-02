const fs = require('fs');

// Read file async.
fs.readFile('file.txt', 'utf8', (err, data) => {
    if (err) {
        console.log('Reading error:', err);
    } else {
        console.log('File content:\n', data);
    }
});

console.log('Reading file ...');

try {
    const data = fs.readFileSync('file.txt', 'utf8');
    console.log('File content (sync):', data);
} catch (err) {
    console.error('Error reading file (sync):', err);
}
