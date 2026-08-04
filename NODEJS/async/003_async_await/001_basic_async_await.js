const myPromise = new Promise((resolve, reject) => {
    setTimeout(() => {
        const success = true;

        if (success) {
            resolve('Promise resolved!');
        } else {
            reject('Promise rejected!');
        }
    }, 3000);
});

const asyncExecution = async () => {
    try {
        const result = await myPromise;
        console.log('Success:', result);
    } catch (error) {
        console.error('Error:', error);
    }

};

asyncExecution();
