const fs = require('fs');

try {
  console.log('File read sync #1');
  const data = fs.readFileSync('example.txt', 'utf8');
  console.log('File content (sync):', data);
} catch (err) {
  console.error('Error reading file (sync):', err);
}

console.log('File read async');
fs.readFile('example.txt', 'utf8', (err, data) => {
  if (err) {
    console.error('Error reading file:', err);
    return;
  }
  console.log('File content (async):', data);
});

try {
  console.log('File read sync #2');
  const data = fs.readFileSync('example.txt', 'utf8');
  console.log('File content (sync):', data);
} catch (err) {
  console.error('Error reading file (sync):', err);
}
