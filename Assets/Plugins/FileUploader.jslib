var UploaderPlugin = {
  webFileImport: function() {
    if (!document.getElementById('ImageUploaderInput')) {
      var fileInput = document.createElement('input');
      fileInput.setAttribute('type', 'file');
      fileInput.setAttribute('multiple', '');
      fileInput.setAttribute('id', 'UploaderInput');
      fileInput.style.visibility = 'hidden';
      fileInput.onclick = function (event) {
        this.value = null;
      };
      fileInput.onchange = function (event) {
		
			function readMultiFiles(files) {
			var reader = new FileReader();
			function readFile(index) {
			// end of folder
			if (index >= files.length) {
			  UnityInstance.SendMessage("RobotConfigScripts", 'parseFiles');
			  return;
			}
			var file = files[index];
			reader.onload = function (e) {
			  // get file content  
			  var bin = e.target.result;
			  if (file.name != "robotfile.urdf") {
				bin = btoa(e.target.result)
				while (bin.length != 0) {
				  UnityInstance.SendMessage("RobotConfigScripts", "receiveRobotFile", JSON.stringify({ name: file.name, data: bin.slice(0, 300000) }))
				  bin = bin.slice(300000);
				  console.log("File Name: " + file.name + "BIN: " + bin.length);
				}
			  } else {
				UnityInstance.SendMessage("RobotConfigScripts", "receiveRobotFile", JSON.stringify({ name: file.name, data: bin }))
				console.log(file.name + ": " + bin);
			  }
			  readFile(index + 1)
			}
			reader.readAsBinaryString(file);
			}
			readFile(0);
		}
		readMultiFiles(event.target.files);
		
		
      }
      document.body.appendChild(fileInput);
    }
    var OpenFileDialog = function() {
      document.getElementById('UploaderInput').click();
      document.getElementById('unity-canvas').removeEventListener('click', OpenFileDialog);
    };
    document.getElementById('unity-canvas').addEventListener('click', OpenFileDialog, false);
  },
  SyncFiles : function()
  {
    FS.syncfs(false,function (err) {
      // handle callback
    });
  }
};
mergeInto(LibraryManager.library, UploaderPlugin);