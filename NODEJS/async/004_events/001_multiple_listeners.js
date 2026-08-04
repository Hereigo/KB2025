const EventEmitter = require('events');

const myEmitter = new EventEmitter();

myEmitter.on('greet', () => {
  console.log('Hello from Listener 1');
});

myEmitter.on('greet', () => {
  console.log('Greetings from Listener 2');
});

myEmitter.emit('greet');
