$(document).ready(function () {
    $("#DepartmentDropdown").change(function () {
        var deptId = $(this).val();
        if (deptId) {
            $.ajax({
                url: '/Employee/GetManagerByDepartment',
                data: { deptId: deptId },
                dataType:"json",
                success: function (data) {
                    console.log(data);
                    if (data) {
                        console.log(data.id);
                        console.log(data.name);
                        $("#ManagerColumn").show();
                        $("#ManagerName").val(data.name).prop("readonly", true);
                        $("#ManagerId").val(data.id);
                    }
                    else {
                        $("#ManagerColumn").hide();
                        $("#ManagerName").val('').prop("readonly", false);
                        $("#ManagerId").val('');
                    }
                },
                error: function () {
                    alert('Error fetching manager');
                }
            });
        }
        else {
            $("#ManagerLabel").hide();
            $("#ManagerName").val('').prop("readonly", false);
            $("#ManagerId").val('');
        }
    });

    function showToast() {
        var toast = new bootstrap.Toast(document.getElementById('toastContainer'));
        toast.show();
    }



    $("#addEmployeeForm").submit(function (e) {
        e.preventDefault();
        var formData = $(this).serialize();
        console.log(formData);
        $.ajax({
            async:true,
            url: '/Employee/Add',
            type: "POST",
            data: formData,
            success: function (response) {
                if (response.success) {
                    setTimeout(() => window.location.href = "/Employee/Index", 2000);
                    toastr.success(response.message, 'Success Alert');
                }
                else {
                    toastr.info(response.message, 'Information Alert');
                }
            },
            error: function () {
                toastr.error("There is some error in execution", 'Error Alert', new { timeOut: 300 });
            }
        });
    });

    $("#editEmployeeForm").submit(function (e) {
        e.preventDefault();
        var formData = $(this).serializeArray();
        console.log(formData);
        $.ajax({
            async: true,
            url: '/Employee/Edit',
            type: "POST",
            data: $.param(formData),
            success: function (response) {
                if (response.success) {
                    setTimeout(() => window.location.href = "/Employee/Index", 2000);
                    toastr.success(response.message, 'Success Alert');
                }
                else {
                    toastr.info(response.message, 'Information Alert');
                }
            },
            error: function () {
                toastr.error("There is some error in execution", 'Error Alert', new { timeOut: 300 });
            }
        });
    });
    

    


    $("#CityId").change(function () {
        var otherCityInput = $("#OtherCity");
        if ($(this).val() == "other") {
            otherCityInput.show();
            otherCityInput.attr("required", "required");
        } else {
            otherCityInput.hide();
            otherCityInput.removeAttr("required");
        }
    });

    if ($("#CityId").val() == "other") {
        $("#OtherCity").show();
    }

    var empToDelete = null;
    $(".delete-Btn").click(function () {
        empToDelete = $(this).data('id');
        console.log(empToDelete);
        $("#confirmDeleteEmpModal").modal('show');
    });
    console.log(empToDelete);

    $("#confirmDeleteEmpButton").on('click', function () {
        if (empToDelete !== null) {
            $.ajax({
                url: '/Employee/DeleteEmployee/' + empToDelete,
                type: 'POST',
                data: { id: empToDelete },
                success: function (response) {
                    if (response.success) {
                        $("#confirmDeleteEmpModal").modal('hide');
                        toastr.success(response.message, "Success");
                        $('tr[data-id="' + empToDelete + '"]').remove();
                    }
                    else {
                        toastr.error(response.message, "Error");
                    }
                },
                error: function (xhr, status, error) {
                    toastr.error("Something went wrong", "Error");
                    console.error("Error", error);
                }
            });
        }
    });
    $("#confirmDeleteModal").modal('hide');
});

function editEmployee(id) {
    window.location.href = '/Employee/Edit/' + id;
}

function deleteEmployee(id) {
    if (confirm("Are you sure want to delete the employee")) {
        $.ajax({
            url: 'Employee/DeleteEmployee',
            type: 'POST',
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    toastr.success("Deleted Successfully");
                    alert("Deleted");
                    location.reload();
                }
                else {
                    alert(response.message);
                }
            }
        });
    }

    
}
