const http = require('http');

const options = {
  hostname: 'www.example.com',
  port: 80,
  path: '/',
  method: 'GET',
};

const req = http.request(options, (res) => {
  let data = '';

  res.on('data', (chunk) => {
    data += chunk;
  });

  res.on('end', () => {
    console.log(`Response from server: ${data}`);
  });
});

req.on('error', (error) => {
  console.error(`Error making request: ${error.message}`);
});

req.end();
