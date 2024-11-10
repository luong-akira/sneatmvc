
'use strict';

(function () {
    // Comment editor
    const commentEditor = document.querySelector('.comment-editor');
    let quill;

    if (commentEditor) {
        quill = new Quill(commentEditor, {
            modules: {
                toolbar: '.comment-toolbar'
            },
            placeholder: 'Nhập mô tả',
            theme: 'snow'
        });

    }

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

    // Function to save the project
    window.saveCreateProject = function () {
        var name = $('#nameCreate').val();
        var description = quill.root.innerHTML; 
        var userIds = $('#slUser').val();

        console.log(description);

        if (name == "") {
            Swal.fire({
                title: 'Thông báo!',
                text: ' Vui lòng nhập tên dự án!',
                icon: 'warning',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false
            });
            return;
        }

         $.ajax({
             url: "/Projects/CreateProject",
             type: "POST",
             contentType: "application/json; charset=utf-8",
             data: JSON.stringify({
                 name: name,
                 description: description,
                 userIds: userIds,
             }),
             beforeSend: function () {
                 $("#modalLoad").modal("show");
             },
             success: function (response) {
                 $("#modalLoad").modal("hide");
                 if (response == -3) {
                     Swal.fire({
                         title: 'Không thể tạo dự án',
                         text: `Đã tồn tại dự án có tên ${name}!`,
                         icon: 'warning',
                         customClass: {
                             confirmButton: 'btn btn-primary'
                         },
                         buttonsStyling: false
                     });
                 }
                 else if (response == 1) {
                     Swal.fire({
                         title: 'Thành công!',
                         text: 'Tạo dự án thành công!',
                         icon: 'success',
                         customClass: {
                             confirmButton: 'btn btn-primary'
                         },
                         buttonsStyling: false
                     });
                     setTimeout(function () {
                         window.location = "/Projects/Index?";
                         searchProject();
                     }, 2000);
                 }
     
                 else {
                     Swal.fire({
                         title: 'Có lỗi xảy ra!',
                         text: ' Không thể tạo dự án!',
                         icon: 'error',
                         customClass: {
                             confirmButton: 'btn btn-primary'
                         },
                         buttonsStyling: false
                     });
                 }
             }
         });
    };
})();

function searchProject() {
    if (!navigator.onLine) {
        Swal.fire({
            title: 'Có lỗi xảy ra!',
            text: ' Kiểm tra kết nối internet!',
            icon: 'error',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }
    var key = $("#txt-key-search").val().replace(/\s\s+/g, ' ');

    $.ajax({
        url: '/Projects/Search',
        data: {
            page: 1,
            search: key
        },
        type: 'POST',
        success: function (response) {
            $("#list_project").html(response);
        },
        error: function (result) {
            console.log(result.responseText);
        }
    });
}

function saveUpdateProject(ID) {
    var name = $('#nameCreate').val();
    const commentEditor = document.querySelector('.comment-editor-edit');
    let quill;

    if (commentEditor) {
        quill = new Quill(commentEditor, {
            modules: {
                toolbar: '.comment-toolbar'
            },
            placeholder: 'Nhập mô tả',
            theme: 'snow'
        });

    }
    var description = quill.root.innerHTML;

    if (name == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập tên dự án!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    $.ajax({
        url: "/Projects/UpdateProject",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            id: ID,
            name: name,
            description: description,
        }),
        beforeSend: function () {
            $("#modalLoad").modal("show");
        },
        success: function (response) {
            $("#modalLoad").modal("hide");
            if (response == -3) {
                Swal.fire({
                    title: 'Không thể cập nhật dự án',
                    text: `Đã tồn tại dự án có tên ${name}!`,
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            } else if (response == -4) {
                Swal.fire({
                    title: 'Không thể cập nhật dự án',
                    text: `Dự án không tồn tại hoặc đã bị xóa!`,
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
            else if (response == 1) {
                Swal.fire({
                    title: 'Thành công!',
                    text: 'Cập nhật dự án thành công!',
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
                setTimeout(function () {
                    window.location = "/Projects/Index?";
                    searchProject();
                }, 2000);
            }

            else {
                Swal.fire({
                    title: 'Có lỗi xảy ra!',
                    text: ' Không thể cập nhật dự án!',
                    icon: 'error',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
        }
    });
}

function deleteProject(id) {
    if (!navigator.onLine) {
        swal({
            title: "Kiểm tra kết nối internet!",
            text: "",
            icon: "warning"
        })
        return;
    }
    Swal.fire({
        title: 'Bạn chắc chắn muốn xóa chứ?',
        text: "Dữ liệu dự án này sẽ bị xóa! Hãy đảm bảo dự án đã đóng và không còn thành viên tham gia",
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Vâng, tôi xác nhận!',
        customClass: {
            confirmButton: 'btn btn-primary me-3',
            cancelButton: 'btn btn-label-secondary'
        },
        buttonsStyling: false
    }).then(function (resultConfirm) {
        console.log(resultConfirm)
        if (resultConfirm.isConfirmed) {
            $.ajax({
                url: '/Projects/RemoveProject',
                data: { id: id },
                type: "POST",
                success: function (result) {
                    if (result == 1) {
                        Swal.fire({
                            title: 'Thành công!',
                            text: 'Xóa dự án thành công!',
                            icon: 'success',
                            customClass: {
                                confirmButton: 'btn btn-primary'
                            },
                            buttonsStyling: false
                        });
                        setTimeout(function () {
                            window.location = "/Projects/Index?";
                            searchProject();
                        }, 2000);
                    } else if (result == -4) {
                        Swal.fire({
                            title: 'Xóa dự án thất bại!',
                            text: 'Dự án hiện không tồn tại hoặc đã bị xóa!',
                            icon: 'warning',
                            customClass: {
                                confirmButton: 'btn btn-primary'
                            },
                            buttonsStyling: false
                        });
                        setTimeout(
                            function () {
                                window.location = "/Projects/Index?";
                                searchProject();
                            }, 1000);

                    }
                    else {
                        Swal.fire({
                            title: 'Có lỗi xảy ra!',
                            text: ' Không thể xóa dự án!',
                            icon: 'error',
                            customClass: {
                                confirmButton: 'btn btn-primary'
                            },
                            buttonsStyling: false
                        });
                    }
                }
            });
        }
    });

}

function searchUserProject() {
    if (!navigator.onLine) {
        Swal.fire({
            title: 'Có lỗi xảy ra!',
            text: ' Kiểm tra kết nối internet!',
            icon: 'error',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }
    var key = $("#txt-key-search").val().replace(/\s\s+/g, ' ');

    $.ajax({
        url: '/Projects/Search',
        data: {
            page: 1,
            search: key
        },
        type: 'POST',
        success: function (response) {
            $("#list_project").html(response);
        },
        error: function (result) {
            console.log(result.responseText);
        }
    });
}

function addUserProject(ID) {
    var userIds = $('#slUser').val();

    if (userIds.length == 0) {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng chọn thành viên!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }
  
    $.ajax({
        url: "/Projects/AddUserProject",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            userIds: userIds,
            projectID: ID,
        }),
        beforeSend: function () {
            $("#modalLoad").modal("show");
        },
        success: function (response) {
            $("#modalLoad").modal("hide");
            if (response == -4) {
                Swal.fire({
                    title: 'Không thể thêm thành viên',
                    text: 'Dự án này không tồn tại hoặc đã bị xóa!',
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
                setTimeout(function () {
                    window.location = "/Projects/Index?";
                    searchProject();
                }, 2000);
            }
           
            else if (response == 1) {
                Swal.fire({
                    title: 'Thành công!',
                    text: 'Thêm thành viên thành công!',
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
                setTimeout(function () {
                    window.location = "/Projects/Update?ID="+ID;
                }, 2000);
            }

            else {
                Swal.fire({
                    title: 'Có lỗi xảy ra!',
                    text: ' Không thể thêm thành viên!',
                    icon: 'error',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
        }
    });
}