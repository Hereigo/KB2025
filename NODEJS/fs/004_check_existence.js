const fs = require('fs');

fs.access('example.txt', fs.constants.F_OK, (err) => {
  if (err) {
    console.error('File does not exist or is not accessible.');
  } else {
    console.log('File exists.');
  }
});

fs.access('example-directory', fs.constants.F_OK, (err) => {
  if (err) {
    console.error('Directory does not exist or is not accessible.');
  } else {
    console.log('Directory exists.');
  }
});
