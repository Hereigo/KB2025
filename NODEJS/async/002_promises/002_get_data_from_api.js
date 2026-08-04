const USER_ID = 1;

fetch(`https://jsonplaceholder.typicode.com/user/${USER_ID}/posts`)
    .then(response => response.json())
    .then(posts => {
        console.log(posts[0]);
        return fetch(`https://jsonplaceholder.typicode.com/posts/${posts[0].id}/comments`);
    })
    .then(response => response.json())
    .then(comments => {
        const myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");

        const raw = JSON.stringify({
            "body": "New text for comment"
        });

        var requestOptions = {
            method: 'PUT',
            headers: myHeaders,
            body: raw,
            redirect: 'follow'
        };

        console.log(comments[0]);

        return fetch(`https://jsonplaceholder.typicode.com/comments/${comments[0].id}`, requestOptions);
    })
    .then(response => response.json())
    .then(comment => {
        console.log(comment);
    })
    .catch(error => {
        console.error(error);
    });
