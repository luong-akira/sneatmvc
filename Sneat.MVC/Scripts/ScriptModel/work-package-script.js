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

})();

function searchWorkPackageProject(projectID) {
    if (!navigator.onLine) {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng kiểm tra kêt nối của bạn!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }
    var key = $("#txt-key-search-task").val().replace(/\s\s+/g, ' ');
    var priority = $('#slPriority').val();

    $.ajax({
        url: '/WorkPackage/SearchWorkPackage',
        data: {
            search: key,
            projectID: projectID,
            priorityType: priority,
        },
        type: 'POST',
        success: function (response) {
            $("#list_work_package_project").html(response);
        },
        error: function (result) {
            console.log(result.responseText);
        }
    });
}

function saveCreateWorkPackage() {
    var subject = $('#add-task-subject').val();
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
    var description = quill.root.innerHTML;

    var assigneeID = $('#add-assignee').val();
    var estTime = $('#add-est-time').val();
    var spentTime = $('#add-real-time').val();
    var remainTime = $('#add-left-time').val();
    var priority = $('#add-priority-point').val();
    var startDate = $('#flatpickr-start-date').val();
    var endDate = $('#flatpickr-end-date').val();
    var projectID = $('#task-add-project-id').val();

    if (subject == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập tiêu đề cho công việc!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (assigneeID == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng chọn người bàn giao công việc!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (priority == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng chọn mức độ ưu tiên!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }
    $.ajax({
        url: "/WorkPackage/CreateWorkPackage",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            Subject: subject,
            Type: 2,
            EstimateTime: estTime,
            SpentTime: spentTime,
            RemainingTime: remainTime,
            PriorityPoint: priority,

            StartDate: startDate,
            FinishDate: endDate,
            Description: description,
            AssigneeID: assigneeID,

            ProjectID: projectID,
        }),
        beforeSend: function () {
            $("#modalLoad").modal("show");
        },
        success: function (response) {
            $("#modalLoad").modal("hide");
           if (response == 1) {
                Swal.fire({
                    title: 'Thành công!',
                    text: 'Tạo nhiệm vụ thành công!',
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
               setTimeout(function () {
                   window.location = "/Projects/ProjectDetail?projectID=" + projectID;
                   searchWorkPackageProject(projectID);
                }, 1000);
            }

            else {
                Swal.fire({
                    title: 'Có lỗi xảy ra!',
                    text: ' Không thể tạo nhiệm vụ!',
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

function updateTaskModal(id) {
    $.ajax({
        url: '/WorkPackage/DetailWorkPackage',
        type: 'GET',
        data: { id: id },
        success: function (res) {
            $("#task-id").val(id);
            $("#project-id").val(res.ProjectID);
            $("#edit-task-subject").val(res.Subject);
            const commentEditor = document.querySelector('.comment-editor-edit');
            let quill;

            if (commentEditor) {
                quill = new Quill(commentEditor, {
                    modules: {
                        toolbar: '.comment-toolbar-edit'
                    },
                    placeholder: 'Nhập mô tả',
                    theme: 'snow'
                });

                quill.clipboard.dangerouslyPasteHTML(res.Description);
            }

            $("#edit-assignee").val(res.AssigneeID).trigger('change'); 
            $("#edit-status").val(res.Status).trigger('change'); 
            $("#edit-est-time").val(res.EstimateTime);
            $("#edit-real-time").val(res.SpentTime);
            $("#edit-left-time").val(res.RemainingTime);
            $("#edit-priority-point").val(res.PriorityPoint).trigger('change'); 
            $("#flatpickr-start-date-edit").val(res.StartDateStr);
            $("#flatpickr-end-date-edit").val(res.FinishDateStr);

            // Open the canvas
            var offcanvasElement = document.getElementById('offcanvasUpdateTask');
            var bsOffcanvas = new bootstrap.Offcanvas(offcanvasElement);
            bsOffcanvas.show();
        }
    })
}

function saveUpdateWorkPackage() {
    var id = $("#task-id").val();
    var projectID = $("#project-id").val();
    var subject = $('#edit-task-subject').val();
    const commentEditor = document.querySelector('.comment-editor-edit');
    let quill;

    if (commentEditor) {
        quill = new Quill(commentEditor, {
            modules: {
                toolbar: '.comment-toolbar-edit'
            },
            placeholder: 'Nhập mô tả',
            theme: 'snow'
        });

    }
    var description = quill.root.innerHTML;

    var assigneeID = $('#edit-assignee').val();
    var estTime = $('#edit-est-time').val();
    var spentTime = $('#edit-real-time').val();
    var remainTime = $('#edit-left-time').val();
    var priority = $('#edit-priority-point').val();
    var startDate = $('#flatpickr-start-date-edit').val();
    var endDate = $('#flatpickr-end-date-edit').val();

    var status = $('#edit-status').val();

    if (subject == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập tiêu đề cho công việc!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (assigneeID == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng chọn người bàn giao công việc!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (priority == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng chọn mức độ ưu tiên!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }
    $.ajax({
        url: "/WorkPackage/UpdateWorkPackage",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            ID: id,
            Subject: subject,
            EstimateTime: estTime,
            SpentTime: spentTime,
            RemainingTime: remainTime,
            PriorityPoint: priority,

            StartDate: startDate,
            FinishDate: endDate,
            Description: description,
            AssigneeID: assigneeID,

            Status: status,
        }),
        beforeSend: function () {
            $("#modalLoad").modal("show");
        },
        success: function (response) {
            $("#modalLoad").modal("hide");
            if (response == 1) {
                Swal.fire({
                    title: 'Thành công!',
                    text: 'Cập nhật nhiệm vụ thành công!',
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
                setTimeout(function () {
                    window.location = "/Projects/ProjectDetail?projectID=" + projectID;
                    searchWorkPackageProject(projectID);
                }, 1000);
            }
            else if (response == -1) {
                Swal.fire({
                    title: 'Thông báo!',
                    text: 'Nhiệm vụ không tồn tại hoặc đã bị xóa!',
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
            else {
                Swal.fire({
                    title: 'Có lỗi xảy ra!',
                    text: ' Không thể tạo nhiệm vụ!',
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