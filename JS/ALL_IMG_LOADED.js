function loadAllImages() {
  const images = document.querySelectorAll('img');
  const imagePromises = [];

  images.forEach(image => {
    const imagePromise = new Promise((resolve, reject) => {
      if (image.complete) {
        resolve(image); // Image already loaded
      } else {
        image.onload = () => resolve(image); // Image loaded successfully
        image.onerror = () => reject(new Error(`Failed to load image: ${image.src}`)); // Error handling
      }
    });
    imagePromises.push(imagePromise);
  });

  return Promise.all(imagePromises);
}

loadAllImages()
  .then(() => {
    // All images on page have been loaded
    console.log("All images are fully loaded.");

    // Now can work with any image ...
  })
  .catch(error => {
    console.error("An error occurred while loading images:", error);
  });
