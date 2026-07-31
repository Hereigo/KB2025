const sum = (a, b) => {
    return a + b;
};

module.exports = sum; // can have fields { ... }

console.log(module);

//  id: '...\\module.js',
//  path: '...',
//  exports: [Function: sum],
//  filename: '...\\module.js',
//  loaded: false,
//  children: [], - imported modules will be here
//  paths: [ ...