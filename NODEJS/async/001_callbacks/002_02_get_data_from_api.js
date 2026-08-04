const http = require('http');

const HOSTNAME = 'jsonplaceholder.typicode.com';
const PORT = 80;

const USER_ID = 1;

const genericRequest = (additionalOptions, onEnd = null) => {
  const { body, ...requestOptions } = additionalOptions;

  const options = {
    hostname: HOSTNAME,
    port: PORT,
    headers: {
      'Content-type': 'application/json; charset=UTF-8',
    },
    ...requestOptions
  };

  const req = http.request(options, (res) => {
    let data = '';

    res.on('data', (chunk) => {
      data += chunk;
    });

    res.on('end', () => {
      parsedData = JSON.parse(data);

      onEnd && onEnd(parsedData);
    });
  });

  req.on('error', (error) => {
    console.error(`Error making request: ${error.message}`);
  });

  body && req.write(body);

  req.end();
};

const getUserPosts = (userId, onEnd = null) => {
  const options = {
    path: `/users/${userId}/posts`,
    method: 'GET',
  };

  genericRequest(options, onEnd);
};

const getPostComments = (postId, onEnd = null) => {
  const options = {
    path: `/posts/${postId}/comments`,
    method: 'GET',
  };

  genericRequest(options, onEnd);
};

const updateCommentBody = (commentId, newBody, onEnd = null) => {
  const options = {
    path: `/comments/${commentId}`,
    method: 'PUT',
    body: JSON.stringify({
      body: newBody
    }),
  };

  genericRequest(options, onEnd);
}

function processUpdateCommentResponse(updatedComment) {
  console.log(updatedComment);
}

function processCommentsResponse(comments) {
  console.log(comments[0]);
  updateCommentBody(comments[0].id, 'New text for comment', processUpdateCommentResponse);
}

function processPostsResponse(posts) {
  console.log(posts[0]);
  getPostComments(posts[0].id, processCommentsResponse);
}

getUserPosts(USER_ID, processPostsResponse);
