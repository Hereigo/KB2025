const fs = require('fs');

fs.readdir('.', (err, files) => {
  if (err) {
    console.error('Error reading directory:', err);
    return;
  }
  console.log('Files in the current directory:', files);
});

try {
  const filesSync = fs.readdirSync('.');
  console.log('Files in the current directory (sync):', filesSync);
} catch (err) {
  console.error('Error reading directory (sync):', err);
}
