document.addEventListener("DOMContentLoaded", function () {
    new DataTable("#users-table", {
        fixedHeader: !0,
        columnDefs: [
            { orderable:false, targets:[0,8] }
        ]
    })
})


$(document).ready(function () {
    // check/uncheck all checkboxes when the table header checkbox is clicked
    $("#checkAll").on("click", function () {
        $(".checkOne").prop("checked", $(this).prop("checked"));
    });
});



    