$(document).ready(function () {

    function LoadImgList(GroupId) {
        $('#LostImg').load('DbImg/ImgList?id=' + GroupId, function () {
        })
    }

    LoadImgList(0);


    $('#LostImg').on('click', '.list-action [name = deleteFileLink]', function (e) {
        e.preventDefault();
        var delLink = $(this).attr('href');
        $.post(delLink);
        $(this).closest('tr').remove();
    })
    var ReWriteClone;
    var ReWritedTr

    $('#LostImg').on('click', '.list-action [name = ReWriteFileLink]', function (e) {
        e.preventDefault();
        if (!ReWriteClone) {
            ReWriteClone = $(this).closest('tr').clone();
            ReWritedTr = $(this).closest('tr');
            var link = $(this).attr('href');
            var thisRow = $(this).closest('tr');
            $(this).closest('tr').find('.list-action').remove();
            $(thisRow).append('<td colspan="2" class="ReWriteButton"> <input type="submit" id="SubmitChange" value="Сохранить"/> </td>');
            $(thisRow).append('<td class="ReWriteButton"> <input type="submit" id="CancelChange" value="Отменить"/> </td>');

            var fileName = $(thisRow).find(".FileTitle").text();
            var fileDescr = $(thisRow).find(".FileDescr textarea").text();
            var fileOrder = $(thisRow).find(".FileOrder").text();

            $(thisRow).find(".FileTitle").empty().append('<input type="text" id="FileNameRe" value="' + $.trim(fileName) + '"/>');
            $(thisRow).find(".FileDescr").empty().append('<textarea  id="FileDescrRe">' + $.trim(fileDescr) + '"</textarea>');
            $(thisRow).find(".FileOrder").empty().append('<input type="number" id="FileOrderRe" value="' + $.trim(fileOrder) + '"/>');
        }
    })



    $('#LostImg').on('click', '#CancelChange', function (e) {
        e.preventDefault();
        if (ReWriteClone){
            ClearReWrite();
        }
    })

    function ClearReWrite() {
        $(ReWritedTr).find(".FileTitle").empty().append($(ReWriteClone).find(".FileTitle").text());
        $(ReWritedTr).find(".FileDescr").empty().append('<textarea disabled="disabled" rows="4" style="width:100%">' + $(ReWriteClone).find(".FileDescr").text() + '</textarea>');
        $(ReWritedTr).find(".FileOrder").empty().append($(ReWriteClone).find(".FileOrder").text()); 
        $(ReWritedTr).find('.ReWriteButton').remove();
        $(ReWritedTr).append($(ReWriteClone).find('.actionCell'));
        ReWriteClone = undefined;
    }

    $('#LostImg').on('click', '#SubmitChange', function (e) {
        e.preventDefault();
        if (ReWriteClone) {
            var idPars = $(ReWriteClone).find('.actionCell a').attr('href');
            idPars = idPars.substring(idPars.indexOf('=')+1, idPars.length);
            var ReWrProp = {
               Id: idPars,
               Name: encodeURIComponent($('#FileNameRe').val()),
               Descrip: encodeURIComponent($('#FileDescrRe').val()),
               Order: encodeURIComponent($('#FileOrderRe').val())
            }
            $.post('DbImg/SaveImgChange', ReWrProp, function () {
                $(ReWritedTr).find(".FileTitle").empty().append(decodeURIComponent(ReWrProp.Name));
                $(ReWritedTr).find(".FileDescr").empty().append('<textarea disabled="disabled" rows="4" style="width:100%">' + decodeURIComponent(ReWrProp.Descrip) + '</textarea');
                $(ReWritedTr).find(".FileOrder").empty().append(decodeURIComponent(ReWrProp.Order));
                $(ReWritedTr).find('.ReWriteButton').remove();
                $(ReWritedTr).append($(ReWriteClone).find('.actionCell'));
                ReWriteClone = undefined;
            })


        }
    })



    
    $('#SearchGroupId').change(function () {
        LoadImgList($('#SearchGroupId option:selected').val())
    })


    $.post("DbImg/GetFileGroups", function (data) {
        $(data).each(function (i, val) {
            $('.FileGroupsList')
            .append($("<option>" + val.Description + " </option>")
            .attr("value", val.GroupId)
            );
        })
        //FileGroupsId
    });


    $('#SubmitUpload').on('click', function (e) {
        e.preventDefault();

        var files = document.getElementById('txtUploadFile').files;
        var FileDescription = encodeURIComponent($('#DescriptionFile').val());
        $('#DescriptionFile').val('');
        //var myID = 3; //uncomment this to make sure the ajax URL works
        if (files.length > 0) {
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append("file" + x, files[x]);
                }

                $.ajax({
                    type: "POST",
                    url: '/DbImg/UploadFile?descr=' + FileDescription + '&id=' + $('#FileGroupsIdAdd option:selected').val() + '&oInd=' + $('#OrderId').val(),
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (result) {

                        $('#txtUploadFile').val('');
                        LoadImgList($('#SearchGroupId option:selected').val()); 
                    },
                    error: function (xhr, status, p3, p4) {
                        var err = "Error " + " " + status + " " + p3 + " " + p4;
                        if (xhr.responseText && xhr.responseText[0] == "{")
                            err = JSON.parse(xhr.responseText).Message;
                        console.log(err);
                    }
                });
            } else {
                alert("This browser doesn't support HTML5 file uploads!");
            }
        }
    });
});