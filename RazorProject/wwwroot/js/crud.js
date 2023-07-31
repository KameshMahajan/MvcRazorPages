
$(document).ready(function () {
    // Load employee data on page load
    LoadEmployees();

    $("#openFormButton").click(function () {
        $("#btnUpdate").hide();
    });

    // Save employee data when "Save" button is clicked in the edit modal
    $("#btnSave").click(function () {
        $("#btnUpdate").hide();
        SaveEmployee();
    });
});

function LoadEmployees() {

    // Send AJAX request to get employee data
    $.ajax({
        type: "GET",
        url: "/CRUD/singlepage?handler=Data",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            // Clear the existing table rows
            $("#IdForAppendingRowsInsideTable").empty();
            console.log("loading employees");
            // Populate the table with the fetched data
            data.forEach(function (employee) {
                var row = '<tr>';
                row += '<td>' + employee.firstName + '</td>';
                row += '<td>' + employee.lastName + '</td>';
                row += '<td>' + employee.department + '</td>';
                row += '<td>' + employee.designation + '</td>';

                // Hidden field to store the employee id using data() method
                row += '<td data-employeeid="' + employee.id + '">';
                row += '<button class="btnEdit btn  btn-outline-primary" id="editMe" onclick="editThisEmployee(' + employee.id + ')"><i class="fa fa-edit"></i></button>';
                row += '<button class="btnDelete btn btn-outline-danger ms-1" onclick="showDeleteConfirmation(' + employee.id + ')"><i class="fa fa-trash-o"></i></button>';
                row += '</td>';
                row += '</tr>';

                $("#IdForAppendingRowsInsideTable").append(row);
            });
        },
        error: function (xhr, status, error) {
            console.log("Error fetching data: " + error);
        }
    });
}


function editThisEmployee(employeeId) {


    console.log("edit me ")
    $.ajax({
        type: "GET",
        url: "/CRUD/singlepage?handler=DataByKey&id=" + employeeId,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            $("#modalContainer").show();
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnUpdate").prop("disabled", false);
            $("#Emp_FirstName").val(data.firstName);
            $("#Emp_LastName").val(data.lastName);
            $("#Emp_Department").val(data.department);
            $("#Emp_Designation").val(data.designation);
            // Unbind any previously bound click event handlers on #btnUpdate
            $("#btnUpdate").off("click");

            // Bind the click event handler to #btnUpdate
            $("#btnUpdate").on("click", function () {
                $("#modalContainer").hide();
                UpdateEmployee(employeeId);
            });

        },
        error: function (xhr, status, error) {
            console.log("Error fetching data: " + error);
        }
    });
}


function SaveEmployee() {
    // Get employee data from the edit modal

    /* var firstName = $("#Emp_FirstName").val();
     var lastName = $("#Emp_LastName").val();
     var department = $("#Emp_Department").val();
     var designation = $("#Emp_Designation").val();
 
     console.log(firstName);
     console.log(lastName);
     console.log(designation);
 
     // Create an object to send as data in the AJAX request
     var employeeData = {
       
         FirstName: firstName,
         LastName: lastName,
         Department: department,
         Designation: designation
     };
 */
    var IsValid = Validate();
    if (!IsValid) {
        return false
    }
    else {
        var form = $('form');
        var frmdata = new FormData($('form').get[0]);
        var other_data = $('form').serializeArray();
        $.each(other_data, function (key, input) {
            frmdata.append(input.name, input.value);

        });


    }

    $.ajax({
        type: "POST",
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        url: "/CRUD/SinglePage?handler=NewData",
        data: frmdata,
        dataType: "json",
        processData: false,
        contentType: false,
        success: function (data) {

            LoadEmployees();

        },
        error: function (error) {
            console.log(error);
        }
    });


}


/*-------------------Update----------------------------*/
function UpdateEmployee(empId) {

    var IsValid = Validate();
    if (!IsValid) {
        return false
    }
    else {
        var form = $('form');
        var frmdata = new FormData($('form').get[0]);
        var other_data = $('form').serializeArray();
        $.each(other_data, function (key, input) {
            frmdata.append(input.name, input.value);

        });
        frmdata.append('Emp.Id', empId)

    }
    /*frmdata.append('Emp.Id', empId);*/
    console.log(empId, "................");
    $.ajax({
        type: "POST",
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        url: "/CRUD/SinglePage?handler=UpdateData",
        data: frmdata,
        dataType: "json",
        processData: false,
        contentType: false,
        success: function (data) {

            LoadEmployees();

        },
        error: function (error) {
            console.log(error);
        }

    });

}




/*-------------------DELETE----------------------------*/
function showDeleteConfirmation(id) {
    // Show the Bootstrap modal
    $('#deleteConfirmationModal').modal('show');

    // Save the ID of the employee to delete in a data attribute of the "Delete" button
    $('#confirmDeleteBtn').attr('data-employee-id', id);

    // Attach a click event to the "Delete" button inside the modal to perform the deletion
    $('#confirmDeleteBtn').on('click', DeleteEmployee);
}


function DeleteEmployee() {
    var id = $('#confirmDeleteBtn').attr('data-employee-id');

    $.ajax({
        method: "POST",
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        url: "/CRUD/SinglePage?handler=DeleteData",
        data: { Id: id },
        success: function () {
            $('#deleteConfirmationModal').modal('hide')
            LoadEmployees();
        },
        error: function (error) {
            console.log(error);
        }
    });

}


/*jQueryModalPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#viewAll').html(res.html)
                    $('#form-modal').modal('hide');
                    console.log("jQueryModalPost....")
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
*/

/*-----------validate--------------- */
function Validate() {
    var isValid = true;
    requiredId = 0;

    $('.reqiredTextbox').
        not('div.radio.reqiredTextbox').
        not('textarea.ckeditor.reqiredTextbox').
        each(function () {
            if ($.trim($(this).val()) == '') {
                isValid = false;
                $(this).css({
                    "border": "1px solid red",
                    "background": ""
                });
                $(this).not('.popUpDisable,.AutoComplete,.subTime').focus();
                requiredId = ErrorRequiredId;
            }
            else {
                $(this).css({
                    "border": "",
                    "background": ""
                });

            }
        });


    $('.reqiredTextbox.AutoComplete').each(function () {
        var $$thisControl = $(this);
        var $id = $$thisControl.attr('id');
        if ($.trim($(this).val()) == '') {
            isValid = false;
            $(this).css({
                "border": "1px solid red",
                "background": ""
            });
            //$(this).focus();
            requiredId = ErrorRequiredId;
        }
        else {
            $(this).css({
                "border": "",
                "background": ""
            });

        }
    });

    $('select.multipleSelect.reqiredTextbox').each(function () {
        var $$thisControl = $(this).parent('div');
        if ($.trim($(this).val()) == '') {
            isValid = false;
            $$thisControl.css({
                "border": "1px solid red",
                "background": ""
            });
            $$thisControl.focus();
            requiredId = ErrorRequiredId;
        }
        else {
            $$thisControl.css({
                "border": "",
                "background": ""
            });

        }
    });

    $('div.radio.reqiredTextbox').each(function () {
        var radiolength = $(this).find('[type=radio]:checked').length;
        if (radiolength == 0) {
            isValid = false;
            $(this).css({
                "border": "1px solid red",
                "background": ""
            });
            $(this).focus();
            requiredId = ErrorRequiredId;
        }
        else {
            $(this).css({
                "border": "",
                "background": ""
            });
        }
    });

    $('textarea.ckeditor.reqiredTextbox').each(function () {
        var $$this = $(this).siblings('div:eq(0)');
        var editorvalue = $$this.find('iframe').contents().find('body').html();
        if ($.trim(editorvalue) == '' || $.trim(editorvalue) == '<p><br></p>') {
            isValid = false;
            $$this.css({
                "border": "1px solid red",
                "background": ""
            });

            requiredId = ErrorRequiredId;
        }
        else {
            $$this.css({
                "border": "",
                "background": ""
            });

        }
    });

    return isValid;
}
