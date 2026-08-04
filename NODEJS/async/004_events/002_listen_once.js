const EventEmitter = require('events');

const myEmitter = new EventEmitter();

myEmitter.once('greet', () => {
  console.log('Hello, this will only be said once!');
});

myEmitter.emit('greet');
myEmitter.emit('greet');
