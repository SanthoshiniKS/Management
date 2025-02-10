$(document).ready(function () {
    $("#generateFields").click(function () {
        let count = $("#numDepts").val();
        let fieldsContainer = $("#deptFields");
        fieldsContainer.html("");
        if (count > 0) {
            for (let i = 1; i <= count; i++) {
                fieldsContainer.append(`
                    <div class="row mb-2 w-50 department-row">
                        <div class="position-relative">
                            <label for="department-name" class="form-label">Department Name: </label>
                            <input type="text" class="form-control deptName" id=department-name required>
                            <span class="remove-field text-danger"><i class="fas fa-times"></i></span>
                        </div>
                        
                    </div>
                `);
            }
            $("#multiDeptForm").show();
        }
    });

    $(document).on("click",".remove-field", function (e) {
        e.preventDefault();
        $(this).closest(".department-row").remove();
    });

    $("#multiDeptForm").submit(function (event) {
        event.preventDefault();
        let deptNames = [];
        $(".deptName").each(function () {
            deptNames.push($(this).val().trim());
        });
        console.log(deptNames);
        let formData = JSON.stringify({ name: deptNames });
        console.log(formData);
        $.ajax({
            url: "/Department/Add",
            type: "POST",
            contentType: "application/json",
            data: formData,
            success: function (response) {
                if (response.success) {
                    $("#multiDeptForm").hide();
                    $("#numDepts").val("");
                    setTimeout(() => window.location.href = "/Department/Index", 2000);
                    toastr.success("Departments added successfully!", "Success Alert");

                } else {
                    toastr.error("Duplicate departments found: " + response.errors.join(", "));
                }
            },
            error: function () {
                toastr.error("Error adding departments", "Error Alert");
            }
        });
    });

    var deptToEdit = null;
    $(".edit-Btn").click(function () {
        deptToEdit = $(this).data('id');
        var deptName = $(this).data('name');

        $("#editDeptId").val(deptToEdit);
        $("#editDeptName").val(deptName);
        $("#editDeptModal").modal('show');
    });

    $("#editDeptButton").click(function (event) {
        event.preventDefault();
        var updatedName = $('#editDeptName').val().trim();
        if (updatedName === ""){
        toastr.error("Department Name cannot be empty", "error");
        return;
        }
        
    $.ajax({
        url: '/Department/Edit',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            id: deptToEdit,
            name: updatedName
        }),
        success: function (response) {
            if (response.success) {
                $('#editDeptModal').modal('hide');
                toastr.success(response.message, "Success");
                $('tr[data-id="' + deptToEdit + '"]').closest('tr').find('.dept-name').text(updatedName);
                
            }
            else {
                $('#editDeptModal').modal('hide');
                toastr.error(response.message, "Error");
            }
        },
        error: function (xhr, status, error) {
            $('#editDeptModal').modal('hide');
            toastr.error("Something went Wrong!", "Error");
            console.error("Error: ", error);
        }
    });

});


    var deptToDelete = null;
    $(".delete-Btn").click(function () {
        deptToDelete = $(this).data('id');
        console.log(deptToDelete);
        $("#confirmDeleteModal").modal('show');
    });
    console.log(deptToDelete);

    $("#confirmDeleteButton").on('click', function () {
        if (deptToDelete !== null) {
            $.ajax({
                url: '/Department/DeleteDepartment/' + deptToDelete,
                type: 'GET',
                success: function (response) {
                    if (response.success) {
                        $("#confirmDeleteModal").modal('hide');
                        toastr.success(response.message, "Success");
                        $('tr[data-id="' + deptToDelete + '"]').remove();
                    }
                    else {
                        $("#confirmDeleteModal").modal('hide');
                        toastr.error(response.message, "Error");
                    }
                },
                error: function (xhr, status, error) {
                    $("#confirmDeleteModal").modal('hide');
                    toastr.error("Something went wrong", "Error");
                    console.error("Error", error);
                }
            });
        }
    });
    $("#confirmDeleteModal").modal('hide');
});

