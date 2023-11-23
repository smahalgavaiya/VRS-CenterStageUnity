mergeInto(LibraryManager.library, {
  updateAprilTagDetectionData: function (posX, posY, posZ, pitch, roll, yaw) {
    setAprilTagDetectionData(posX, posY, posZ, pitch, roll, yaw);
  },
});
