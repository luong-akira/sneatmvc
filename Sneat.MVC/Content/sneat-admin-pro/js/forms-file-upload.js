/**
 * File Upload
 */

'use strict';

(function () {
  // previewTemplate: Updated Dropzone default previewTemplate
  // ! Don't change it unless you really know what you are doing
  const previewTemplate = `<div class="dz-preview dz-file-preview">
<div class="dz-details">
  <div class="dz-thumbnail">
    <img data-dz-thumbnail>
    <span class="dz-nopreview">No preview</span>
    <div class="dz-success-mark"></div>
    <div class="dz-error-mark"></div>
    <div class="dz-error-message"><span data-dz-errormessage></span></div>
    <div class="progress">
      <div class="progress-bar progress-bar-primary" role="progressbar" aria-valuemin="0" aria-valuemax="100" data-dz-uploadprogress></div>
    </div>
  </div>
  <div class="dz-filename" data-dz-name></div>
  <div class="dz-size" data-dz-size></div>
</div>
</div>`;

  // ? Start your code from here

  // Basic Dropzone
  // --------------------------------------------------------------------
  const dropzoneBasic = document.querySelector('#dropzone-basic');
  if (dropzoneBasic) {
    const myDropzone = new Dropzone(dropzoneBasic, {
      previewTemplate: previewTemplate,
      parallelUploads: 1,
      maxFilesize: 5,
      addRemoveLinks: true,
      maxFiles: 1,
      
    });

      // Add existing image as a mock file
      const currentImageUrl = document.getElementById('currentImage').value;
      if (currentImageUrl) {
          const mockFile = { name: "Current Image", size: 12345 }; // Mock file name and size
          myDropzone.emit("addedfile", mockFile);
          myDropzone.emit("thumbnail", mockFile, currentImageUrl);
          myDropzone.emit("complete", mockFile);

          // Set the existing file as preview
          myDropzone.files.push(mockFile);
      }

      myDropzone.on('success', function (file) {
          const formData = new FormData();
          formData.append('files', file);

          fetch('/Home/UploadFiles', {
              method: 'POST',
              body: formData
          })
              .then(response => response.json())
              .then(data => {
                  data.forEach(url => {
                      $('#currentImage').val(url);
                      console.log($('#currentImage').val());
                      
                  });
              })
              .catch(error => {
                  console.error('Error uploading files:', error);
              });
      });
   }

  
  // Multiple Dropzone
  // --------------------------------------------------------------------
  /*const dropzoneMulti = document.querySelector('#dropzone-multi');
  if (dropzoneMulti) {
    const myDropzoneMulti = new Dropzone(dropzoneMulti, {
      previewTemplate: previewTemplate,
      parallelUploads: 1,
      maxFilesize: 5,
      addRemoveLinks: true
    });
  }*/

    const dropzoneMulti = document.querySelector('#dropzone-multi');
    if (dropzoneMulti) {
        const myDropzoneMulti = new Dropzone(dropzoneMulti, {
            previewTemplate: previewTemplate,
            parallelUploads: 1,
            maxFilesize: 5,
            addRemoveLinks: true
        });

        // Add existing images as mock files
        const currentImageUrls = document.getElementById('currentMultilImage').value;
        if (currentImageUrls != "") {
            currentImageUrls.split(',').forEach((url, index) => {
                const mockFile = { name: `Existing Image ${index + 1}`, size: 12345 }; // Mock file name and size
                myDropzoneMulti.emit("addedfile", mockFile);
                myDropzoneMulti.emit("thumbnail", mockFile, url);
                myDropzoneMulti.emit("complete", mockFile);

                // Set the existing file as preview
                myDropzoneMulti.files.push(mockFile);
            });
            //console.log(currentImageUrls)
        }

        myDropzoneMulti.on('success', function (file) {
            const formData = new FormData();
            formData.append('files', file);

            fetch('/Home/UploadFiles', {
                method: 'POST',
                body: formData
            })
                .then(response => response.json())
                .then(data => {
                    data.forEach(url => {
                        // Assuming currentImage should store all URLs as a comma-separated string
                        let currentImages = $('#currentMultilImage').val();
                        currentImages = currentImages ? currentImages.split(',') : [];
                        currentImages.push(url);
                        $('#currentMultilImage').val(currentImages.join(','));
                        console.log($('#currentMultilImage').val());
                    });
                })
                .catch(error => {
                    //console.error('Error uploading files:', error);
                });
        });
    }
})();
