const fs = require('fs');

fs.readFile('example.txt', 'utf8', (err, data) => {
  if (err) {
    console.error('Error reading file:', err);
    return;
  }
  console.log('File content:', data);
});

try {
  const data = fs.readFileSync('example.txt', 'utf8');
  console.log('File content (sync):', data);
} catch (err) {
  console.error('Error reading file (sync):', err);
}
