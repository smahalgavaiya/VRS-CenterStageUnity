mergeInto(LibraryManager.library, {
  updateAprilTagDetectionData: function (posX, posY, posZ, pitch, roll, yaw) {
    setAprilTagDetectionData(posX, posY, posZ, pitch, roll, yaw);
  },

  updateAprilTagDetectionData: function (detectionResponse) {
    console.log({ detectionResponse: Pointer_stringify(detectionResponse) });
  },

  updateVisionMessage: function (msg, value) {
    console.log(Pointer_stringify(msg));
    setVisionMessage(Pointer_stringify(msg), 0.1);
  },

  UploadImage: function (
    url,
    formDataName,
    fileName,
    fileData,
    fileDataLength,
    otherData
  ) {
    var boundary =
      "------------------------" + new Date().getTime().toString(16);
    var xhr = new XMLHttpRequest();

    xhr.open("POST", url, true);
    xhr.setRequestHeader(
      "Content-Type",
      "multipart/form-data; boundary=" + boundary
    );

    var body =
      "--" +
      boundary +
      "\r\n" +
      'Content-Disposition: form-data; name="' +
      formDataName +
      '"; filename="' +
      fileName +
      '"\r\n' +
      "Content-Type: image/png\r\n\r\n" +
      arrayBufferToBase64(fileData) +
      "\r\n" +
      "--" +
      boundary +
      "\r\n" +
      'Content-Disposition: form-data; name="otherData"\r\n\r\n' +
      otherData +
      "\r\n" +
      "--" +
      boundary +
      "--\r\n";

    var uint8Array = new Uint8Array(body.length);
    for (var i = 0; i < body.length; i++) {
      uint8Array[i] = body.charCodeAt(i);
    }

    xhr.send(uint8Array);
  },
});

function arrayBufferToBase64(buffer) {
  var binary = "";
  var bytes = new Uint8Array(buffer);
  var len = bytes.byteLength;
  for (var i = 0; i < len; i++) {
    binary += String.fromCharCode(bytes[i]);
  }
  return btoa(binary);
}
