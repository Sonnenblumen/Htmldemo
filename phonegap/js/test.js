document.addEventListener('deviceready', onDeviceReady, false);

function onDeviceReady(){
	
}

function captureImage(){
	navigator.device.capture.captureImage(onSuccess, onFail, {
		limit: 2
	});
}

function captureVideo(){
	navigator.device.capture.captureVideo(onSuccess, onFail, {
		limit: 2
	});
}

function onSuccess(files){
	for (var i = 0, len = files.length; i < len; i++) {
		uploadFile(files[i]);
	}
}

function onFail(err){
	navigator.notification.alert('错误码：' + err.code, null, 'Uh oh!');
}

function uploadFile(file){
	var ft = new FileTransfer(),
		path = file.fullPath,
		name = file.name;
	ft.upload(path, 'http://www.w3hacker.com/test/upload.php', function onSuccess(result){
		alert(result.response + ',' + result.bytesSent);
	}, function onFail(err){
		alert('上传失败' + err.code);
	}, {
		fileName: name
	});
}