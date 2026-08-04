const callback = () => {
    console.log('Callback executed');
};

console.log('Program started');

setTimeout(callback, 3000);

console.log('Just after timeout has been set');