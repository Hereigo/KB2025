/**
 * Promise has three states:
 * 1. Pending
 * 2. Fulfilled
 * 3. Rejected
 */

const myPromise = new Promise((resolve, reject) => {
    setTimeout(() => {
        const success = false;

        if (success) {
            resolve('Promise resolved!');
        } else {
            reject('Promise rejected!');
        }
    }, 3000);
});

/**
 * .then(onfulfilled, onrejected)
 * .catch(onrejected)
 */

myPromise
    .then(result => {
        console.log('then #1');
        console.log('Success:', result);
    }, error => {
        console.error('Error (then #1):', error);
        throw new Error('test error');
    })
    .then(result => {
        console.log('then #2');
    })
    .finally(() => {
        console.log('Executed in any case ');
    })
    .catch(error => {
        console.error('Error:', error);
    });