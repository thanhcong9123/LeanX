
var buttonCreateMovie = document.getElementById("create");
buttonCreateMovie.onclick = function () {
    var model = new Object();
    model.Title = document.getElementById("Title").value;
    model.Description = document.getElementById("Description").value;
    model.ImgPath = document.getElementById("ImgPath").value;
    var fileSelector = document.getElementById('file1');
    var formData = new FormData();
    formData.append('formFile', $('#file1')[0].files[0]);
    model.IFormFile = formData.append('formFile', $('#file1')[0].files[0]);;

    console.log($('#file1')[0].files[0]);
    $.ajax({
        url: "/EBook/UpLoadFile",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            model.IFormFile = data;

            $.ajax({
                method: 'POST',
                url: '/EBook/Upload',
                data: model,
                ContentType: 'application/json; charset=utf-8',
                // dataType: "text",
                // processData: false,
                success: function (res) {
                  window.location.href = '@Url.Action("Index", "EBook")' ;
                },
                error: function () { alert('Thêm book ko thành công'); }
            });
        },
        error: function () {
            alert('Không nhận được file');

        }
    })
}


function _(el) {
    return document.getElementById(el);
  }
  
  function uploadFile() {
    var file = _("file1").files[0];
    // alert(file.name+" | "+file.size+" | "+file.type);
    var formdata = new FormData();
    formdata.append("file1", file);
    var ajax = new XMLHttpRequest();
    ajax.upload.addEventListener("progress", progressHandler, false);
    ajax.addEventListener("load", completeHandler, false);
    ajax.addEventListener("error", errorHandler, false);
    ajax.addEventListener("abort", abortHandler, false);
    ajax.open("POST", "file_upload_parser.php"); // http://www.developphp.com/video/JavaScript/File-Upload-Progress-Bar-Meter-Tutorial-Ajax-PHP
    //use file_upload_parser.php from above url
    ajax.send(formdata);
  }
  
  function progressHandler(event) {
    _("loaded_n_total").innerHTML = "Uploaded " + event.loaded + " bytes of " + event.total;
    var percent = (event.loaded / event.total) * 100;
    _("progressBar").value = Math.round(percent);
    _("status").innerHTML = Math.round(percent) + "% uploaded... please wait";
  }
  
  function completeHandler(event) {
    _("status").innerHTML = event.target.responseText;
    _("progressBar").value = 0; //wil clear progress bar after successful upload
  }
  
  function errorHandler(event) {
    _("status").innerHTML = "Upload Failed";
  }
  
  function abortHandler(event) {
    _("status").innerHTML = "Upload Aborted";
  }