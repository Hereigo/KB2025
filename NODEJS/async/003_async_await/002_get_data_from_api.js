const USER_ID = 1;

const getUserPosts = async (userId) => {
    const response = await fetch(`https://jsonplaceholder.typicode.com/users/${userId}/posts`);

    return response.json();
};

const getPostComments = async (postId) => {
    const response = await fetch(`https://jsonplaceholder.typicode.com/posts/${postId}/comments`);

    return response.json();
};

const updateCommentBody = async (commentId, newBody) => {
    const myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");

    const raw = JSON.stringify({
        "body": newBody
    });

    var requestOptions = {
        method: 'PUT',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
    };

    const response = await fetch(`https://jsonplaceholder.typicode.com/comments/${commentId}`, requestOptions);

    return response.json();
};

const asyncExecution = async () => {
    try {
        const posts = await getUserPosts(USER_ID);

        const comments = await getPostComments(posts[0].id);

        const updatedComment = await updateCommentBody(comments[0].id, 'New text for comment');

        console.log(updatedComment);
    } catch (error) {
        console.error(error);
    }
};

asyncExecution();
